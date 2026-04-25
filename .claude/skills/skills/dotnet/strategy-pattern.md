# Strategy Pattern Skill

Goal:
Apply Strategy Pattern when the system needs interchangeable algorithms at runtime.

Context:
The dark kitchen system has delivery types and pricing rules that may change over time.
This is a good candidate for Strategy when multiple calculation rules exist.

When to use:

- Different shipping cost calculations
- Different pricing rules
- Different discount application rules
- Different delivery behaviors selected at runtime

Structure:

- Define a strategy interface
- Create one implementation per algorithm
- Inject or select the correct strategy at runtime
- Keep the client code independent from concrete implementations

Example:
interface IShippingCostStrategy
{
decimal Calculate(decimal subtotal);
}

class ExpressShippingStrategy : IShippingCostStrategy
{
public decimal Calculate(decimal subtotal) => 150;
}

class TwentyFourHoursShippingStrategy : IShippingCostStrategy
{
public decimal Calculate(decimal subtotal) => 80;
}

Rules:

- Do not use large if/else or switch when strategies can encapsulate behavior
- Keep each strategy focused on one calculation rule
- Client should depend on abstraction, not concrete classes
- Strategy selection can be done by factory or resolver if needed

Benefits:

- Supports Open/Closed Principle
- Reduces conditional complexity
- Makes pricing rules easier to test
- Improves maintainability

Testing:

- Test each strategy independently
- Test the selector/resolver separately
- Include happy path and invalid type cases
