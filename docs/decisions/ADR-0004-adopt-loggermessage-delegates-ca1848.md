ADR-0004-adopt-loggermessage-delegates-ca1848.md
---
adr:
  id: 0004
  title: "Adopt LoggerMessage delegates for high-performance logging (CA1848)"
  status: Accepted
  date: 2025-09-04
  supersedes: []
  superseded_by: []
  related:
    - ADR-0000-initial-state
  owners:
    - Engineering Leads, Platform & Observability
  reviewers:
    - Performance WG
    - SRE
  tags:
    - logging
    - performance
    - reliability
    - analyzers
    - .net

Context
-------
This codebase is a demo, but it intentionally models patterns suitable for an enterprise application serving millions of users. Logging runs on or near the hot path for most requests (telemetry, diagnostics, support investigations). Using ILogger extension methods such as LogInformation(string, params object[]) generates avoidable allocations (message template parsing, params array creation, and boxing of value types), plus additional CPU work. At high RPS, these per-call costs aggregate into measurable throughput and latency impact.

Microsoft analyzer CA1848 flags such calls and recommends the LoggerMessage pattern—implemented either via compile-time source generation using [LoggerMessage] on static partial methods or via predeclared, cached delegates using LoggerMessage.Define*. Both approaches remove repeated formatting work and reduce allocations while preserving structured logging fields.

Decision
--------
1) Adopt the LoggerMessage pattern for all logging on or near hot paths.
   - Preferred (NET 6+): Source-generated logging via [LoggerMessage] on static partial methods.
   - Fallback: Delegate-based LoggerMessage.Define* for cases where source generation is unavailable or a delegate is explicitly required.

2) Enforce structured logging with named placeholders (e.g., {CustomerId}, {ArticleId}) to preserve semantic properties through sinks and telemetry pipelines.

3) Require EventId usage (reserve numeric ranges per subsystem/feature) to support filtering, analytics, and incident correlation.

4) Analyzer policy: Enable CA1848 as warning in Debug and escalate to error in Release, to enforce performance without degrading the inner-loop developer experience.

5) Prohibit string interpolation or concatenation on hot paths; use templated messages with typed parameters.

Rationale
---------
- Performance: The LoggerMessage pattern (including source generation) avoids params array allocations, repeated format parsing, and value-type boxing, reducing both allocation pressure and CPU usage.
- Safety & clarity: Generated methods provide compile-time checks that placeholders and parameter lists match.
- Observability quality: Named placeholders reliably propagate as structured fields to logs/OTel/JSON, improving queryability and correlation.
- Compliance: Proactively addresses CA1848 across the codebase and codifies a maintainable, consistent logging API.

Implementation
--------------
1) Conventions
   - Create a Logging folder per assembly with a static partial class named {Feature}Log (internal visibility).
   - Reserve EventId ranges per feature (e.g., 1000–1099 Search, 1100–1199 Articles).
   - Use clear, PascalCase placeholder names mapped to parameter meanings: {CustomerId}, {ArticleId}, {DurationMs}.
   - Avoid string interpolation ($"...") and concatenation on hot paths.

2) Source-generated pattern (preferred)
   Example: /<FeatureOrLayer>/Logging/FeatureLog.cs

     using Microsoft.Extensions.Logging;

     namespace Contoso.Feature;

     internal static partial class FeatureLog
     {
         [LoggerMessage(
             EventId = 1001,
             Level = LogLevel.Information,
             Message = "Handled request {Route} in {DurationMs} ms for {CustomerId}")]
         public static partial void RequestHandled(ILogger logger, string route, long durationMs, string customerId);

         [LoggerMessage(
             EventId = 1002,
             Level = LogLevel.Warning,
             Message = "Cache miss for key {CacheKey}")]
         public static partial void CacheMiss(ILogger logger, string cacheKey);

         [LoggerMessage(
             EventId = 1003,
             Level = LogLevel.Error,
             Message = "Unhandled exception processing article {ArticleId}")]
         public static partial void ArticleProcessingError(ILogger logger, string articleId, Exception exception);
     }

     // Usage
     FeatureLog.RequestHandled(_logger, route, stopwatch.ElapsedMilliseconds, customerId);

   Notes:
   - The generator emits efficient code (including IsEnabled checks) that minimizes per-call overhead.
   - The first parameter is always the ILogger; subsequent parameters map positionally to placeholders.

3) Delegate-based fallback (LoggerMessage.Define*)
   Use when source generation cannot be applied (legacy constraints, specialized delegate customization).

     using Microsoft.Extensions.Logging;

     internal static class LegacyFeatureLog
     {
         private static readonly Action<ILogger, string, long, string, Exception?> _requestHandled =
             LoggerMessage.Define<string, long, string>(
                 LogLevel.Information,
                 new EventId(1001, nameof(RequestHandled)),
                 "Handled request {Route} in {DurationMs} ms for {CustomerId}");

         public static void RequestHandled(ILogger logger, string route, long durationMs, string customerId) =>
             _requestHandled(logger, route, durationMs, customerId, null);
     }

4) Analyzer configuration (.editorconfig)
   Set CA1848 severity and optionally escalate in Release builds.

     # CA1848: Use the LoggerMessage delegates
     dotnet_diagnostic.CA1848.severity = warning

   Example escalation pattern via Directory.Build.props or a Release.ruleset is acceptable for CI enforcement.

5) Event ID governance
   - Maintain an event ID map in /docs/observability/event-ids.md.
   - Ensure uniqueness in PR review.
   - Prefer nameof(Method) as EventName for readability when constructing EventId.

6) Inspecting the source-generated logging code
   To view code emitted by the logging source generator ([LoggerMessage]), configure the project to emit compiler-generated files. This is useful for debugging, code reviews, and ensuring placeholders/parameters align.

   Note: $(BaseIntermediateOutputPath) typically resolves to "obj/". Do not commit generated files; "obj/" is ignored by default in .gitignore.

   Add to the CSPROJ (project file):

     <Project Sdk="Microsoft.NET.Sdk">
       <PropertyGroup Label="Generated code configuration">
         <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
         <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
       </PropertyGroup>
     </Project>

   After building, inspect files under:
   - <project-root>/obj/generated/
   You’ll see subfolders by generator (e.g., Microsoft.Extensions.Logging.Generators).

   Optional (Debug-only emission):

     <PropertyGroup Condition="'$(Configuration)'=='Debug'" Label="Generated code configuration (Debug only)">
       <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
       <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)generated</CompilerGeneratedFilesOutputPath>
     </PropertyGroup>

Alternatives Considered
-----------------------
- Keep ILogger.Log* extension methods:
  - Pros: Minimal refactor.
  - Cons: Continues allocations and extra CPU per call on hot paths; not compliant with CA1848; weaker structured logging guarantees.
- Wrap logging in custom abstractions:
  - Pros: Centralized API.
  - Cons: If built atop Log* extension methods, still incurs overhead; adds indirection without addressing analyzer guidance.

Consequences
------------
Positive
- Lower allocation and CPU overhead in high-RPS paths.
- Safer, clearer logging APIs with compile-time checks.
- Stronger structured telemetry quality and CA1848 compliance.

Trade-offs
- Slight increase in boilerplate (central log classes and event ID governance).
- Developer learning curve for source-generated pattern and conventions.

Rollout Plan
------------
1) Introduce {Feature}Log partial classes and reserve EventId ranges.
2) Convert hot-path logs first (controllers, middleware, core services).
3) Enable CA1848 as warning solution-wide; track remaining violations with issues.
4) After stabilization, escalate CA1848 to error for Release/CI.
5) Validate improvements with perf runs (allocation counts and throughput), and verify structured fields in telemetry.

Testing & Verification
----------------------
- Unit tests: Assert EventId, LogLevel, and structured fields using a test ILoggerProvider or in-memory sink.
- Perf checks: BenchmarkDotNet microbenchmarks and integration-level load tests to confirm reduction in allocations and CPU.
- Telemetry validation: Ensure placeholders appear as structured properties end-to-end.

Non-Goals
---------
- Changing logging providers or sinks.
- Introducing PII; existing redaction policies remain.

Security & Privacy
------------------
- Maintain PII/secret redaction and data-handling policies.
- Avoid logging sensitive fields; use approved identifiers only.

References
----------
- CA1848: Use the LoggerMessage delegates (Microsoft code analysis rule).
- .NET logging source generation with [LoggerMessage] (compile-time logging).
- LoggerMessage.Define* APIs for delegate-based logging.
- High-performance logging guidance in .NET (reducing allocations and overhead).
