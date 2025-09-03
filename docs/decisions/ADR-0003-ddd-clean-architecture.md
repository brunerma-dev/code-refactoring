# ADR-0003 — Adopt DDD & Clean Architecture

**Status:** Accepted  
**Date:** 2025-09-03

## Context
The README emphasizes: (1) **easy & safe extensibility** for washes, (2) **minimal files touched** per new feature, (3) **high developer discoverability** and **low barrier to entry**, (4) **clear, well-designed architecture** that remains flexible, (5) **reasonable performance**, (6) preparedness for **scale**, and (7) a focus on **code longevity** (not cloud plumbing).  
**Constraints:** Do **not** modify `Worker.cs`, `CarJob.cs`, `CarJobService.cs`. `Program.cs` may change **except** the `Main` method body/signature. Work begins at `CarJobProcessorService.cs`.

## Decision
Adopt a **Clean Architecture overlay guided by DDD** within the current project, using stable domain contracts and externalized composition. Concretely:

- **Domain (pure contracts)**
  - Introduce `IWashServiceStrategy` to express wash behavior (`PerformWashAsync(CarJob, CancellationToken)`) without DI or logging concerns.
- **Application (use cases & extensibility)**
  - Keep `CarJobProcessorService` in its **original location/namespace** (preserves constraints) but refactor it to depend on `IWashServiceStrategy` via a small resolver abstraction `IServiceResolver<TKey,TService>`.  
  - Use attribute-driven extension points for discoverability and “drop-a-class-here” DX:
    - `IKeyedRegistration<TKey>` (marker contract with `Key`)
    - `[WashType(EServiceWash.X)]` on each wash implementation
- **Infrastructure (composition)**
  - Generic convention registrar `AddKeyedImplementationsByConvention<TService,TKey,TAttr>()` scans once at startup, **fails fast** on duplicate keys or multiple attributes, and registers implementations using **.NET 8 keyed DI**.
  - `KeyedServiceResolver<TKey,TService>` implements `IServiceResolver<TKey,TService>` and resolves services in **O(1)** by enum key.
- **Program wiring**
  - All registration happens inside `Program._RegisterServices(IServiceCollection)`; **`Main` remains unchanged**.

> Result: adding a new wash requires **one class + one attribute**; no edits to the orchestrator or central switches. Performance remains predictable (startup scan only; runtime lookups are O(1)).

## Rationale (why this fits the README)
- **Easy & safe extensibility / minimal files touched:** new wash = one class + one attribute; no central code churn.
- **Discoverable & intuitive:** folders and attributes make the extension point obvious.
- **Clear & flexible architecture:** domain contracts are framework-free; composition is externalized; internals are transparent (simple attributes + registrar + keyed DI).
- **Performance:** single startup scan with fail-fast validation; runtime is O(1) keyed resolution and one pass over requested work.
- **Longevity & scale:** stable seams mean many more washes can be added without reworking the orchestrator or DI.

## Constraints today, and recommended relocations later
**Must not change now:**  
- `Worker.cs` — Host/process concern.  
- `CarJob.cs` — Domain model.  
- `CarJobService.cs` — Use-case/integration concern.  
- `Program.Main` — Host entry; only `_RegisterServices` is used.

**Why consider moving later (Phase-2, no behavior change):**  
- Move `CarJob.cs` to **Domain/Entities** to co-locate the model with domain contracts.  
- Move `CarJobService.cs` to **Application** (use case) or **Infrastructure** (integration) based on responsibility.  
- Move `Worker.cs` and `Program.Main` into a dedicated **Host** project.  
- Split solution: `*.Domain` (no deps) → `*.Application` (ref Domain) → `*.Infrastructure` (ref App/Domain) → `*.Host` (ref Infra/App).

These relocations clarify ownership and enforce compile-time boundaries while preserving the same public seams established now.

## Consequences
**Positive:**  
- High DX; minimal blast radius; predictable performance; fail-fast misconfiguration; easy unit testing (orchestrator uses interfaces).  
**Trade-offs:**  
- Small, bounded assembly scan at startup; introduces two simple concepts (attributes + keyed DI) that are easy to learn.

## Implementation (concise)
1. **Domain**: add `IWashServiceStrategy` (`PerformWashAsync(...)`).  
2. **Application**:  
   - Add `IKeyedRegistration<TKey>` and `[WashType]` attribute.  
   - Add `IServiceResolver<TKey,TService>`; refactor `CarJobProcessorService` to use `IServiceResolver<EServiceWash,IWashServiceStrategy>`.  
3. **Infrastructure**:  
   - Add `AddKeyedImplementationsByConvention<TService,TKey,TAttr>()` (fail-fast) and `KeyedServiceResolver<TKey,TService>`.  
4. **Program.cs**: implement `_RegisterServices(IServiceCollection)` only; **do not modify `Main`**.

## Notes
- Details about add-ons are intentionally omitted here and will be addressed separately, following the same principles and naming conventions (`IAddonServiceStrategy`, `PerformAddonAsync`, etc.).
