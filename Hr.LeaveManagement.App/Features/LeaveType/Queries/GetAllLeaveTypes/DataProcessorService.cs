using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Hr.LeaveManagement.Domain.ComplexCode
{
    public class DataProcessorService
    {
        private readonly Dictionary<string, Func<string, string>> _transformations;
        private readonly HttpClient _httpClient;
        private const int MAX_RETRY_ATTEMPTS = 3;
        private const int BUFFER_SIZE = 4096;
        private static readonly string[] SUPPORTED_FORMATS = new[] { "json", "xml", "csv", "yaml" };
        
        public DataProcessorService()
        {
            _httpClient = new HttpClient();
            _transformations = new Dictionary<string, Func<string, string>>
            {
                { "uppercase", s => s.ToUpper() },
                { "lowercase", s => s.ToLower() },
                { "reverse", s => new string(s.Reverse().ToArray()) },
                { "hash", ComputeHash }
            };
        }

        /// <summary>
        /// Processes data from multiple sources, applies various transformations,
        /// validates the results, and returns a combined result set.
        /// </summary>
        /// <param name="sources">Collection of data sources to process</param>
        /// <param name="transformationChain">Sequence of transformations to apply</param>
        /// <param name="validationRules">Optional validation rules for processed data</param>
        /// <param name="outputFormat">Format of the output (json, xml, csv, yaml)</param>
        /// <param name="compressionLevel">Level of compression for the output (0-9)</param>
        /// <param name="enableCache">Whether to cache intermediate results</param>
        /// <param name="timeout">Operation timeout in milliseconds</param>
        /// <returns>Processed data results with metadata</returns>
        public async Task<ProcessingResult> ProcessComplexDataAsync(
            List<DataSource> sources,
            string[] transformationChain,
            Dictionary<string, Predicate<string>> validationRules = null,
            string outputFormat = "json",
            int compressionLevel = 5,
            bool enableCache = true,
            int timeout = 30000)
        {
            if (sources == null || !sources.Any())
                throw new ArgumentException("At least one data source must be provided", nameof(sources));
            
            if (transformationChain == null || !transformationChain.Any())
                throw new ArgumentException("At least one transformation must be specified", nameof(transformationChain));
            
            if (!SUPPORTED_FORMATS.Contains(outputFormat.ToLower()))
                throw new ArgumentException($"Output format must be one of: {string.Join(", ", SUPPORTED_FORMATS)}", nameof(outputFormat));
            
            if (compressionLevel < 0 || compressionLevel > 9)
                throw new ArgumentOutOfRangeException(nameof(compressionLevel), "Compression level must be between 0 and 9");

            var cancellationTokenSource = new System.Threading.CancellationTokenSource(timeout);
            var cancellationToken = cancellationTokenSource.Token;
            
            var processingStartTime = DateTime.UtcNow;
            var processingLog = new StringBuilder();
            var cache = enableCache ? new Dictionary<string, string>() : null;
            var results = new Dictionary<string, string>();
            var successCount = 0;
            var failureCount = 0;
            
            try
            {
                foreach (var source in sources)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    
                    string rawData;
                    processingLog.AppendLine($"Processing source: {source.Name} ({source.Type})");
                    
                    try
                    {
                        // Fetch data from various source types
                        rawData = await FetchDataFromSourceAsync(source, cancellationToken);
                        
                        if (string.IsNullOrEmpty(rawData))
                        {
                            processingLog.AppendLine($"Warning: Empty data received from {source.Name}");
                            failureCount++;
                            continue;
                        }
                        
                        // Apply transformations sequentially
                        var processedData = rawData;
                        foreach (var transformation in transformationChain)
                        {
                            var cacheKey = $"{source.Name}_{transformation}_{processedData.GetHashCode()}";
                            
                            if (enableCache && cache.TryGetValue(cacheKey, out var cachedResult))
                            {
                                processedData = cachedResult;
                                processingLog.AppendLine($"Cache hit for transformation: {transformation}");
                            }
                            else
                            {
                                if (!_transformations.TryGetValue(transformation.ToLower(), out var transformFunc))
                                {
                                    processingLog.AppendLine($"Error: Unknown transformation '{transformation}'");
                                    throw new InvalidOperationException($"Transformation '{transformation}' is not supported");
                                }
                                
                                // Apply transformation with retry logic for resilience
                                for (int attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++)
                                {
                                    try
                                    {
                                        processedData = transformFunc(processedData);
                                        
                                        if (enableCache)
                                            cache[cacheKey] = processedData;
                                        
                                        break;
                                    }
                                    catch (Exception ex) when (attempt < MAX_RETRY_ATTEMPTS)
                                    {
                                        processingLog.AppendLine($"Retry {attempt}/{MAX_RETRY_ATTEMPTS} for transformation '{transformation}' due to: {ex.Message}");
                                        await Task.Delay(100 * attempt, cancellationToken); // Exponential backoff
                                    }
                                }
                                
                                processingLog.AppendLine($"Applied transformation: {transformation}");
                            }
                            
                            // Check for cancellation after each transformation
                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        
                        // Apply validation if rules provided
                        if (validationRules != null && validationRules.Count > 0)
                        {
                            bool isValid = true;
                            foreach (var rule in validationRules)
                            {
                                if (!rule.Value(processedData))
                                {
                                    processingLog.AppendLine($"Validation failed for rule: {rule.Key}");
                                    isValid = false;
                                    break;
                                }
                            }
                            
                            if (!isValid)
                            {
                                failureCount++;
                                continue;
                            }
                            
                            processingLog.AppendLine("All validation rules passed");
                        }
                        
                        // Process data differently based on source type
                        string normalizedData = source.Type switch
                        {
                            DataSourceType.File => NormalizeFileData(processedData, source.Path),
                            DataSourceType.Database => NormalizeDatabaseData(processedData, source.Query),
                            DataSourceType.Api => NormalizeApiData(processedData, source.Url),
                            DataSourceType.Stream => NormalizeStreamData(processedData),
                            _ => throw new NotSupportedException($"Unsupported data source type: {source.Type}")
                        };
                        
                        // Apply compression if level > 0
                        if (compressionLevel > 0)
                        {
                            normalizedData = CompressData(normalizedData, compressionLevel);
                            processingLog.AppendLine($"Compressed data with level {compressionLevel}");
                        }
                        
                        // Store the processed result
                        results.Add(source.Name, normalizedData);
                        successCount++;
                        processingLog.AppendLine($"Successfully processed source: {source.Name}");
                    }
                    catch (OperationCanceledException)
                    {
                        processingLog.AppendLine($"Processing of {source.Name} was cancelled due to timeout");
                        failureCount++;
                    }
                    catch (Exception ex)
                    {
                        processingLog.AppendLine($"Error processing {source.Name}: {ex.Message}");
                        // In a real system we'd log the full exception details
                        failureCount++;
                    }
                }
                
                // Format final output according to requested format
                var combinedResult = FormatOutput(results, outputFormat);
                
                return new ProcessingResult
                {
                    ProcessedData = combinedResult,
                    SuccessCount = successCount,
                    FailureCount = failureCount,
                    ProcessingTimeMs = (int)(DateTime.UtcNow - processingStartTime).TotalMilliseconds,
                    Log = processingLog.ToString(),
                    Format = outputFormat,
                    CompressionLevel = compressionLevel,
                    CacheEnabled = enableCache,
                    CacheHitCount = enableCache ? cache.Count : 0
                };
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        private async Task<string> FetchDataFromSourceAsync(DataSource source, System.Threading.CancellationToken cancellationToken)
        {
            switch (source.Type)
            {
                case DataSourceType.File:
                    if (string.IsNullOrEmpty(source.Path))
                        throw new ArgumentException("File path is required for file data sources", nameof(source));
                    
                    return await File.ReadAllTextAsync(source.Path, cancellationToken);
                
                case DataSourceType.Database:
                    // Simplified for example purposes
                    if (string.IsNullOrEmpty(source.Query))
                        throw new ArgumentException("SQL query is required for database data sources", nameof(source));
                    
                    // In a real implementation, this would use a database connection
                    // For this example, we'll just return the query as the data
                    await Task.Delay(100, cancellationToken); // Simulate DB operation
                    return $"{{\"results\": [{Guid.NewGuid()}, {Guid.NewGuid()}], \"query\": \"{source.Query}\"}}";
                
                case DataSourceType.Api:
                    if (string.IsNullOrEmpty(source.Url))
                        throw new ArgumentException("URL is required for API data sources", nameof(source));
                    
                    var response = await _httpClient.GetAsync(source.Url, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                
                case DataSourceType.Stream:
                {
	                // Simulated stream processing
	                var buffer = new byte[BUFFER_SIZE];
	                using var memoryStream = new MemoryStream();
                    
	                // Simulate reading from a stream
	                var random = new Random();
	                random.NextBytes(buffer);
                    
	                await memoryStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
	                memoryStream.Position = 0;
                    
	                using var reader = new StreamReader(memoryStream);
	                return await reader.ReadToEndAsync();
                }

                default:
                    throw new NotSupportedException($"Unsupported data source type: {source.Type}");
            }
        }

        private string NormalizeFileData(string data, string filePath)
        {
            var extension = Path.GetExtension(filePath)?.ToLowerInvariant();
            
            // Apply different normalizations based on file type
            return extension switch
            {
                ".json" => NormalizeJsonData(data),
                ".xml" => NormalizeXmlData(data),
                ".csv" => NormalizeCsvData(data),
                ".txt" => data.Trim().Replace("\r\n", "\n"),
                _ => data
            };
        }

        private string NormalizeJsonData(string data)
        {
            // This would use a real JSON parser in production code
            return data.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        }

        private string NormalizeXmlData(string data)
        {
            // This would use a real XML parser in production code
            return Regex.Replace(data, @">\s+<", "><");
        }

        private string NormalizeCsvData(string data)
        {
            // Normalize CSV line endings and trim whitespace
            return string.Join("\n", 
                data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                    .Select(line => string.Join(",", 
                        line.Split(',').Select(field => field.Trim()))));
        }

        private string NormalizeDatabaseData(string data, string query)
        {
            // In a real system, this would parse and normalize database-specific data
            return data;
        }

        private string NormalizeApiData(string data, string url)
        {
            // Apply different normalizations based on the API endpoint
            if (url.Contains("/json"))
                return NormalizeJsonData(data);
            else if (url.Contains("/xml"))
                return NormalizeXmlData(data);
            
            return data;
        }

        private string NormalizeStreamData(string data)
        {
            // Remove non-printable characters and normalize whitespace
            return Regex.Replace(data, @"[\x00-\x1F]", "").Trim();
        }

        private string FormatOutput(Dictionary<string, string> results, string format)
        {
            switch (format.ToLower())
            {
                case "json":
                    return $"{{\"results\": {{{string.Join(",", results.Select(r => $"\"{r.Key}\": \"{EscapeJsonString(r.Value)}\""))}}}}}";
                
                case "xml":
                    return $"<Results>{string.Join("", results.Select(r => $"<Result><Name>{r.Key}</Name><Value>{System.Security.SecurityElement.Escape(r.Value)}</Value></Result>"))}</Results>";
                
                case "csv":
                    return $"Name,Value\n{string.Join("\n", results.Select(r => $"{r.Key},\"{r.Value.Replace("\"", "\"\"")}"))}";
                
                case "yaml":
                    return $"results:\n{string.Join("\n", results.Select(r => $"  {r.Key}: \"{EscapeYamlString(r.Value)}\""))}";
                
                default:
                    throw new NotSupportedException($"Unsupported output format: {format}");
            }
        }

        private string EscapeJsonString(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private string EscapeYamlString(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        private string CompressData(string data, int level)
        {
            // Simulate compression with different levels
            if (level <= 3)
                return data.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            else if (level <= 6)
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
            else
                return ComputeHash(data);
        }
    }

    public class ProcessingResult
    {
        public string ProcessedData { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public int ProcessingTimeMs { get; set; }
        public string Log { get; set; }
        public string Format { get; set; }
        public int CompressionLevel { get; set; }
        public bool CacheEnabled { get; set; }
        public int CacheHitCount { get; set; }
    }

    public class DataSource
    {
        public string Name { get; set; }
        public DataSourceType Type { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string Url { get; set; }
    }

    public enum DataSourceType
    {
        File,
        Database,
        Api,
        Stream
    }
}