---
adr:
  id: 000
  title: "Initial State — Coding Challenge Kickoff"
  status: Accepted
  date: 2025-08-30
  revision: 1.0
  tags: [refactoring, .net, carwashprocessor]
  related: []
  supersedes: []
  superseded_by: null
---

# ADR-000 — Initial State (Coding Challenge Kickoff)

> **Scope**
> This ADR is associated with the initial state of the solution and focuses on the coding challenge details in `CarWashProcessor/README.txt` to set the context for the refactoring effort.

## 1) Challenge Framing
Refactor the code for this system so that:
- Once refactored, adding new washes and add-ons should be as easy and safe as possible
  - Ideally, other developers should not have to have advanced knowledge of the inner workings
  - Ideally, adding new washes and add-ons should not require adding/editing many files
  - Ideally, adding to and using the system is intuitive and self-explanatory through architecture to other developers
- Once refactored, the underlying architecture is well-designed and clear
  - Ideally, the underlying architecture should be able to be changed without fundamentally changing the already implemented washes and add-ons
  - Ideally, other developers with advanced knowledge should be able to understand the inner workings
- The system is reasonably performant

_**TIP:**_ _This challenge is about code organization, stability, and longevity. It's not about cloud architecture (databases, queues, etc.)._

## 2) Functional Requirements
- _Adding new washes and add-ons **should be as easy and safe as possible**.__
- _Adding new washes and add-ons **should not require adding/editing many files**._
- _The **underlying architecture should be able to be changed without fundamentally changing the already implemented washes and add-ons**.__
- _The system is **reasonably performant**._

## 3) Non-Functional Goals
- _The underlying architecture is **well-designed and clear**._
- _Other **developers should not have to have advanced knowledge of the inner workings**._
- _Adding to and using **the system is intuitive and self-explanatory through architecture** to other developers._
- _Other **developers with advanced knowledge should be able to understand the inner workings**._

## 4) Out-of-Scope / Explicit Non-Goals
- _Performance optimization beyond reasonable levels._
- _Cloud architecture (databases, queues, etc.)._
- _Repository focus (CI/CD, GitHub Actions, etc.)._

## 5) Constraints & Assumptions
- **Runtime/Template**: `Microsoft.NET.Sdk.Worker`
- **Testing**: _MSTest/Moq_
- **Language/Framework**: _C#_	

## 6) Key Entities & Terms
- _`CarJob`:_ _Represents a record containing the customer id, make of their vehicle, and the types of car wash and add-ons they have requested._
	- _Mapping it to the real/physical world, this is a 'manifest' of what a customer has purchased from a car wash._
- _`CarJobService`:_ _Called by the **`Worker`** and responsible generating **`CarJob`** records._
	- _This is responsible for simulating the flow of traffic throughout the day at a car wash. _
- _`CarJobProcessorService`:_ _Called by **`Worker`** and processes **`CarJob`** records serving as a pipeline orchestrator, first identifying the type of car wash service requested, handing off the car job to the specific wash service for processing, and, finally, individually checks for each possible add-on and if the car job includes the add-on, then hands the car job to the specific add-on service for processing._
	- _This is where the real work occurs and the starting point of the challenge._ 
- _`Worker`:_ _The main entry point for the application, which runs continuously generating and processing car jobs until stopped._
- _`*WashService`:_ _The current car wash services available_
	- _These are largely a no-op, with a delay to simulate any work performed by that specific car wash type._
- _`(remaining) Service(s)`:_ _The current add-on services available_
	- _These are largely a no-op, with a delay to simulate any work performed by that specific add-on type._

## 7) Repository/Solution Baseline
- **Repository**: [https://github.com/brunerma-dev/code-refactoring/](https://github.com/brunerma-dev/code-refactoring/)
- **Solution**: `./CarWashProcessor.sln`
- **Projects**: `./CarWashProcessor/CarWashProcessor.csproj`

## 8) Validation Expectations
- _If successful, I should be able to demonstrate and defend how I have addressed each of the **functional** (code) and **non-functional** (organization, documentation, etc.) requirements to facilitate better code organization, stability, and longevity in a maturing code base._

## 9) Initial Risks / Caveats
- _You may not modify:_
	- _`Worker.cs`_
	- _`CarJob.cs`_
	- _`CarJobService.cs`_
	- _`Program.cs`: `Main(string[] args)`_

---

## 10) Next Steps (process guidance)
1. Perform an initial assessment, adding comments to the existing code to map out my observations, opinions, etc ("refactoring opportunities").
2. Introduce a simple unit test project, it's the best way for me to get to know the code base and introduce guard rails (unit tests that pass with the initial state of the code base), to ensure the code functions as expected. 
3. Create Architectual Decision Records (ADR) documents for major design decisions.
3. Keep changes **scoped**; update ADRs and `ADR-INDEX.md` with each step.

---

## Change Log
- **v1.0 (2025-08-30)** — Initial state; code provided as is.
