# Adding a New `IWashServiceStrategy` Implementation

This guide explains how to add a new wash service by implementing the `IWashServiceStrategy` interface. Follow these steps to ensure your implementation is consistent, discoverable, and testable.

---

## 1. Add a new EServiceWash Enum value
- **Location:**  
  Update the `EServiceWash` enum in `CarWashProcessor\Models\CarJob.cs`.
- **Naming Convention:**  
  Use a descriptive name that reflects the new wash service (e.g., `SuperShine`).


---

## 2. Create the New Strategy Class

- **Location:**  
  Place your new class in `CarWashProcessor\Application\Strategies\Wash\`.
- **Naming Convention:**  
  Use a descriptive name ending with `WashService` (e.g., `SuperShineWashService`).


---

## 3. Register the Strategy

- **Attribute:**  
  Decorate your class with the `[WashType("YourTypeName")]` attribute to enable keyed resolution.
- **Dependency Injection:**  
  Ensure your service is registered in the DI container.  
  By adding the attribute, it should be automatically registered if using assembly scanning.


---


## 4. Add or Update Unit Tests

- **Location:**  
  Place tests in `CarWashProcessor.UnitTests\Application\Strategies\Wash\`.
- **Naming Convention:**  
  Name the test class after your service (e.g., `SuperShineWashServiceTests`).
- **Coverage:**  
  - Test all public methods and expected behaviors.
  - Include edge cases and error handling.


---

## 5. Verify Integration

- Ensure your new strategy is discoverable and used where appropriate (e.g., via the resolver or processor service).
- Run all tests to confirm nothing is broken.

---

## Summary Checklist

- [ ] New enum value in `EServiceWash`
- [ ] New class in `Application\Strategies\Wash\`
- [ ] Implements `IWashServiceStrategy`
- [ ] Decorated with `[WashType("...")]`
- [ ] Unit tests in `UnitTests\Application\Strategies\Wash\`
- [ ] Integration verified

---

**Tip:**  
Refer to existing strategies like `BasicWashService`, `AwesomeWashService`, or `ToTheMaxWashService` for examples.
