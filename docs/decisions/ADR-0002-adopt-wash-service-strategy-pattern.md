ADR-0002-adopt-wash-strategy-pattern
---
adr:
  id: 0002
  title: "Adopt Strategy Pattern for Wash Services"
  status: Accepted
  date: 2025-09-02
  revision: 0.1
  tags: [refactoring, .net, testing, mstest, moq, design patterns, strategy]
  related: [ADR-0000]                 # Initial state
  supersedes: []
  superseded_by: null
---

# ADR-0002 — Adopt Strategy Pattern for Wash Services

> Scope: Concise ADR to codify the decision to refactor *WashService.cs to Strategy.  
> Location: store under `docs/decisions/` and list in `ADR-INDEX.md`.  

---

## 1) Initial State (for ADR-000 only)
See [ADR-0000-initial-state.md](ADR-0000-initial-state.md)

## 2) Problem & Goals
- **Problem (one sentence):** Adding a new wash requires touching orchestration and/or DI in multiple places, increasing coupling and violating OCP/DIP.
- **Goals:**
  - Standardize wash handlers behind a **single high-level method** so they are uniform and easy to use.
  - Align with **SOLID** (DIP) and improve testability.
- **Non-Goals:**
  - DI Registration and lifetime (assume Singleton)
  - Service resolution / selection
  - Refactor **add-on** services — will be addressed separately.

## 3) Decision (What & Where)
- **Decision:** Refactor each concrete `*WashService.cs` into a **Strategy** that implements a small interface with a single operation and key property (will be used for resolution / selection).
- **Scope:**
  - Interface: `Services/IWashServiceStrategy.cs`
  - Implementations: `Services/*WashService.cs`
  
## 4) Rationale (Why)
- **Principles:**  
  - **DIP:** CarJobServiceProcessor (orchestrator) should depend on `IWashServiceStrategy` (interface), not concrete classes.  
  - **SRP:** Each strategy encapsulates exactly one wash behavior behind one method.  
- **Evidence:**  
  - Current code violates DIP in CarJobServiceProcessor. This refactoring will allow DIP to be honored when CarJobServiceProcessor is refactored. 

## 5) Refactoring Plan (Small, Reversible Steps)
1. **Introduce interface** `IWashServiceStrategy` with:
   - `EServiceWash Key { get; }`
   - `Task PerformWashAsync(CarJob carJob)`
2. **Rename & implement**: convert each `*WashService.cs` → `*WashServiceStrategy.cs`, implement `IWashServiceStrategy`, adapt existing methods to call `PerformWashAsync`.
3. **Introduce strategy selection**: add `IWashServiceStrategyResolver` interface and implementation.
4. **Orchestrator change**: `CarJobProcessorService` calls:
   - `var strategy = _resolver.Select(carJob.ServiceWash);`
   - `await strategy.PerformWashAsync(carJob);`
5. **Organize files**: move implementations to `Services/Washes/` with namespace `CarWashProcessor.Services.Washes`.
6. **Characterization tests**: capture current behavior; add tests for single-selection and execution path.

- **Risks & Mitigations:**
  - *Behavior drift:* Baseline functional unit tests assert continued expected behavior.
  - *Missing/duplicate keys:* TBD.
  - *Renaming fallout:* IDE-assisted rename; public API remains unchanged.

## 6) Alternatives Considered (Brief)
- **None**. Each type of wash is a strategy for performing that wash. This is a textbook use of the Strategy pattern.

## 7) Validation (How We’ll Know It Worked)
- **Tests:**
  - Each Strategy implements a `Key` property returning the correct enum.
  - Each strategy implements a `PerformWashAsync` function that performs the work originally performed by the `Do\*WashAsync` function.
  - Existing `Do\*WashAsync` TestMethod calls continue to Pass.
- **Checks:**
  - All existing functional behaviors preserved (characterization parity).
- **Manual QA:**
  - Run sample jobs (Basic/Awesome/ToTheMax) and verify identical side effects/logs as before.

## 8) Impact
- **Runtime/Behavior:** No functional change intended; performance unchanged by this ADR (resolution/lifetime unchanged).
- **Breaking Changes:** Internal type names/namespaces change; **no public contract change**.

## 9) Rollout & Backout
- **Rollout:** Single PR focusing only on Strategy refactor.  
- **Backout:** Revert PR; prior concrete wiring remains intact.

## 10) Outcome & Follow-Ups
- **Definition of Done:** Tests passing; strategies in `Services/`; docs updated.

## 11) Links
- **PRs/Commits:** _TBD_  
- **Related ADRs:** ADR-0000 (Initial State)  
