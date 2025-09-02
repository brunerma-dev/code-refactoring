# ADR Index

> Canonical list of Architecture Decision Records (ADRs).  
> Keep this file updated with every ADR addition or status change.

---

## Maintenance Rules
- **One row per ADR**; keep the table **sorted by ID** (ascending).
- File naming: `ADR-####-short-title.md` (e.g., `ADR-0007-adopt-opentelemetry.md`).
- Allowed **Status** values: `Proposed`, `Accepted`, `Rejected`, `Deprecated`, `Superseded`.

## Status Legend
- **Proposed** — Draft under review
- **Accepted** — Decision approved and in effect
- **Rejected** — Considered but not chosen
- **Deprecated** — Being phased out
- **Superseded** — Replaced by another ADR

---

## Table

| ID     | Title                                    | Status   | Date         | Supersedes       | Superseded By     | Link                                                        | Tags                      |
|--------|------------------------------------------|----------|--------------|------------------|-------------------|-------------------------------------------------------------|-------------------------|
| 0000   | Initial State — Coding Challenge Kickoff | Accepted | 2025-08-30   | –                | –                 | [ADR-0000](ADR-0000-initial-state.md)                       | refactoring, kickoff    |
| 0001   | Add Unit Test Project Before Refactoring | Accepted | 2025-09-02   | -                | -                 | [ADR-0001](ADR-0001-unit-test-project.md)                   | refactoring, .net, testing, mstest, moq  |
| 0002   | Adopt Strategy Pattern for Wash Services | Accepted | 2025-09-02   | -                | -                 | [ADR-0002](ADR-0002-adopt-wash-service-strategy-pattern.md) | refactoring, .net, testing, mstest, moq, design patterns, strategy  |
| <####> | <short-title>                            | <Status> | <YYYY-MM-DD> | <ADR-#### \>     | <ADR-#### \>      | [ADR-####](ADR-####-short-title.md)                         | <comma,separated,tags>  |

---

## How to Add a New ADR
1. Copy the [ADR template](ADR-0000_template.md) to a new file: `ADR-####_short-title.md` (use next sequential ID).
2. Fill in the template and open a PR with `status: Proposed`.
3. After review/approval, set `status: Accepted` (or `Rejected`) and update this index row.
4. If an ADR replaces another, set **Supersedes** on the new ADR and **Superseded By** on the old ADR.
