ADR-0000-short-title
---
adr:
  id: 0000                           # sequential integer with leading zeros
  title: "<Short decision title>"
  status: Proposed                   # Proposed | Accepted | Rejected | Superseded | Deprecated
  date: 2025-08-30                   # YYYY-MM-DD
  revision: 0.1                      # bump on edits to this ADR
  tags: [refactoring, .net, carwashprocessor]
  related: []                        # e.g., [ADR-0001, ADR-0002]
  supersedes: []                     # IDs this ADR replaces
  superseded_by: null                # ID that replaces this ADR (when applicable)
---

# ADR-<####> — <Short decision title>

> Scope: Concise ADR template used to codify decisions.  
> Location: store under `docs/decisions/` and list in `ADR-INDEX.md`.

---

## 1) Initial State (for ADR-000 only)
See [ADR-0000-initial-state.md](ADR-0000-initial-state.md)

## 2) Problem & Goals
- **Problem (one sentence)**: _what friction are we removing right now?_
- **Goals (3–5 bullets)**: _e.g., testability, SOLID alignment, reduce duplication_
- **Non-Goals**: _what we’re explicitly not changing in this step_

## 3) Decision (What & Where)
- **Decision**: _one sentence describing the change being made now_
- **Scope**: _files/projects impacted (globs ok, e.g., `src/CarWashProcessor/**/*.cs`)_

## 4) Rationale (Why)
- **Principles**: _e.g., SRP, composition over inheritance, YAGNI_
- **Evidence**: _before/after examples, spike notes, constraints from README_

## 5) Refactoring Plan (Small, Reversible Steps)
1. _step 1 (≤ ~150 LOC)_  
2. _step 2_  
3. _step 3_  
- **Risks & Mitigations**: _e.g., behavior drift → add characterization tests first_

## 6) Alternatives Considered (Brief)
- _Alt A_: _one-line summary + why not now_
- _Alt B_: _one-line summary + why not now_

## 7) Validation (How We’ll Know It Worked)
- **Tests**: _unit/contract updates (MSTest/Moq)_
- **Checks**: _before/after complexity, lines removed, p95 latency (if relevant)_
- **Manual QA (if any)**: _brief steps_

## 8) Impact
- **Developer Experience**: _easier to add a new wash/add-on handler, etc._
- **Runtime/Behavior**: _no functional change expected | minor changes (list)_
- **Breaking Changes**: _none | list with migration note_

## 9) Rollout & Backout
- **Rollout**: _feature flag, separate branch, or straight merge (demo repo)_
- **Backout**: _simple revert commit/PR; keep old path behind a flag if used_

## 10) Outcome & Follow-Ups
- **Definition of Done**: _tests passing, coverage ≥ X, docs updated_
- **Follow-Ups**: _next ADR(s) to tackle adjacent issues_

## 11) Links
- **PRs/Commits**: _URLs/SHAs_
- **Related ADRs**: _IDs/titles_
- **README Anchor**: `CarWashProcessor/README.txt` (relevant section)
