// Violates KISS - overcomplicated for a simple task
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

// KISS version - simple and direct
public class SimpleAdder
{
    public int Add(int a, int b) => a + b;
}
