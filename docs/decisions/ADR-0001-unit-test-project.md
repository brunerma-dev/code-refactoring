ADR-0001-add-unit-test-project
---
adr:
  id: 0001
  title: "Add Unit Test Project Before Refactoring"
  status: Accepted
  date: 2025-09-02
  revision: 0.1
  tags: [refactoring, .net, testing, mstest, moq]
  related: [ADR-0000]
  supersedes: []
  superseded_by: null
---

# ADR-0001 — Add Unit Test Project Before Refactoring

> Scope: Concise ADR template used to codify decisions.  
> Location: store under `docs/decisions/` and list in `ADR-INDEX.md`.

---

## 2) Problem & Goals
- **Problem**: The initial coding challenge solution prototype did not include unit tests, leaving no safety net for validating existing functionality during refactoring.  
- **Goals**:
  - Establish a functional baseline / safety net before refactoring.
  - Use baseline unit tests to expose code smells and guide SOLID design improvements.
  - Enable a TDD-style workflow for subsequent refactoring efforts.
- **Non-Goals**:
  - Performance benchmarks, integration tests, end-to-end automation, and bug fixes will not be addressed at this stage.

## 3) Decision (What & Where)
- **Decision**: Introduce a dedicated unit test project to the solution, targeting all refactorable files prior to beginning code refactoring.  
- **Scope**: `src/CarWashProcessor/Worker.cs`, `src/CarWashProcessor/CarJobService.cs`, `src/CarWashProcessor/CarJob.cs`.  
  - Excluded: `Program.cs` (complex to test, expected to change significantly) and any files explicitly marked as “do not modify.”

## 4) Rationale (Why)
- **Principles**: Stability before change, SRP, TDD readiness, YAGNI (only tests for areas expected to change).  
- **Evidence**:
  - Challenge statement emphasizes “organization, stability, and longevity.”
  - Unit tests directly contribute to stability and help ensure longevity of the codebase.
  - MSTest + Moq chosen for alignment with standard .NET practices and mocking capabilities.

## 5) Refactoring Plan (Small, Reversible Steps)
1. Scaffold new `CarWashProcessor.UnitTests` project with MSTest and Moq references.  
2. Add initial unit tests covering critical paths of `Worker.cs`, `CarJobService.cs`, and `CarJob.cs`.  
3. Validate ≥ 80% coverage of code paths expected to be refactored.  
- **Risks & Mitigations**:  
  - Risk: Behavior drift during refactoring → Mitigation: execute unit tests locally before commits and PRs.  
  - Risk: Overfitting tests to fragile code → Mitigation: treat tests as characterization, refactor alongside production code.  

## 6) Alternatives Considered
- **Alt A**: Begin refactoring immediately without a test project → rejected due to high regression risk.  
- **Alt B**: Add integration/end-to-end tests instead → rejected, scope is limited to unit-level guarantees for now.  

## 7) Validation (How We’ll Know It Worked)
- **Tests**: All added unit tests pass locally.  
- **Checks**: ≥ 80% coverage on files within scope.  
- **Manual QA**: Not required at this stage; developer experience validation only.  

## 8) Impact
- **Developer Experience**: Provides immediate safety net, enabling confidence during refactoring.  
- **Runtime/Behavior**: No runtime or functional changes.  
- **Breaking Changes**: None.  

## 9) Rollout & Backout
- **Rollout**: Direct merge into `main`.  
- **Backout**: Simple revert commit/PR; no runtime changes at risk.  

## 10) Outcome & Follow-Ups
- **Definition of Done**: Unit test project added, tests passing, ≥ 80% coverage for files in scope.  
- **Follow-Ups**:
  - ADR-0002: Refactor `CarJobProcessorService` with strategy pattern.  
  - ADR-0003: Introduce configuration-driven add-on ordering (if required).  

## 11) Links
- **PRs/Commits**: _(to be added once merged)_  
- **Related ADRs**: [ADR-0000](ADR-0000-initial-state.md) Initial State.  
- **README Anchor**: `CarWashProcessor/README.txt` (challenge requirements: stability & longevity).  
