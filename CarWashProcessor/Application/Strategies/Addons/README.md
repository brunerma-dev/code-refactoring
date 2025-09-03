# Adding a New `IAddonServiceStrategy` Implementation

This guide explains how to add a new addon service by implementing the `IAddonServiceStrategy` interface. Follow these steps to ensure your implementation is consistent, discoverable, and testable.

---

## 1. Add a new `EServiceAddon` Enum Value

- **Location:**  
  Update the `EServiceAddon` enum in `CarWashProcessor\Models\CarJob.cs`.
- **Naming Convention:**  
  Use a descriptive name that reflects the new addon service (e.g., `PetHairRemoval`).

---

## 2. Create the New Strategy Class

- **Location:**  
  Place your new class in `CarWashProcessor\Application\Strategies\Addons\`.
- **Naming Convention:**  
  Use a descriptive name ending with `Service` (e.g., `PetHairRemovalService`).

---

## 3. Register the Strategy

- **Attribute:**  
  Decorate your class with the `[AddonType("YourTypeName")]` attribute to enable keyed resolution.
- **Dependency Injection:**  
  Ensure your service is registered in the DI container.  
  By adding the attribute, it should be automatically registered if using assembly scanning.

---

## 4. Add or Update Unit Tests

- **Location:**  
  Place tests in `CarWashProcessor.UnitTests\Application\Strategies\Addons\`.
- **Naming Convention:**  
  Name the test class after your service (e.g., `PetHairRemovalServiceTests`).
- **Coverage:**  
  - Test all public methods and expected behaviors.
  - Include edge cases and error handling.

---

## 5. Verify Integration

- Ensure your new strategy is discoverable and used where appropriate (e.g., via the resolver or processor service).
- Run all tests to confirm nothing is broken.

---

## Summary Checklist

- [ ] New enum value in `EServiceAddon`
- [ ] New class in `Application\Strategies\Addons\`
- [ ] Implements `IAddonServiceStrategy`
- [ ] Decorated with `[AddonType("...")]`
- [ ] Unit tests in `UnitTests\Application\Strategies\Addons\`
- [ ] Integration verified

---

**Tip:**  
Refer to existing strategies like `InteriorCleanService` or `TireShineService` for examples.
