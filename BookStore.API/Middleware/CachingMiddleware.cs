using BookStore.Api.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace BookStore.Api.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;

        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDistributedCache cache)
        {
            var endpoint = context.GetEndpoint();
            var cacheAttribute = endpoint?.Metadata.GetMetadata<CachedAttribute>();

            // Nếu không có attribute hoặc là GET thì bỏ qua
            if (cacheAttribute is null || context.Request.Method != HttpMethods.Get)
            {
                await _next(context);
                return;
            }

            var cacheKey = GenerateCacheKeyFromRequest(context.Request);
            var cachedResponse = await cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse);
                return;
            }

            // Nếu không có cache thì tiếp tục
            var originalBodyStream = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            memStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memStream).ReadToEndAsync();
            memStream.Seek(0, SeekOrigin.Begin);

            // Nếu phương thức trả về 200 thì lưu vào cache
            if (context.Response.StatusCode == 200)
            {
                await cache.SetStringAsync(cacheKey, responseBody, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttribute.DurationInSeconds)
                });
            }

            await memStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var path = request.Path.ToString();
            var query = request.Query
                .OrderBy(x => x.Key)
                .Select(x => $"{x.Key}-{x.Value}")
                .Aggregate("", (acc, val) => acc + val);

            return $"response_cache:{path}|{query}";
        }
    }

}
