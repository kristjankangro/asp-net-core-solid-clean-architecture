// BAD EXAMPLE - Violates KISS (Keep It Simple, Stupid)
// This class is overcomplicated for a simple task of adding two numbers.
// Problems:
// - Unnecessary dependencies (logger, validator, cache)
// - Over-engineered solution for a trivial operation
// - Harder to test, maintain, and understand
public class NumberAdder
{
    private readonly ILogger _logger;
    private readonly IValidator _validator;
    private readonly ICache _cache;

    public NumberAdder(ILogger logger, IValidator validator, ICache cache)
    {
        _logger = logger;
        _validator = validator;
        _cache = cache;
    }

    public int Add(int a, int b)
    {
        _logger.Log($"Adding {a} and {b}");
        _validator.ValidateNumbers(a, b);

        var cacheKey = $"{a}_{b}";
        if (_cache.TryGet(cacheKey, out int result))
            return result;

        result = a + b;
        _cache.Set(cacheKey, result);
        _logger.Log($"Result: {result}");

        return result;
    }
}

// GOOD EXAMPLE - Follows KISS principle
// Simple, direct, easy to understand and test
public class SimpleAdder
{
    public int Add(int a, int b) => a + b;
}
