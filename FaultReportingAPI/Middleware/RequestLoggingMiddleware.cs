namespace FaultReportingAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.UtcNow;

            try
            {
                await _next(context);
            }
            finally
            {
                var elapsed = DateTime.UtcNow - start;

                _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms Response : {Body}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    elapsed.TotalMilliseconds,
                    context.Response.Body);
            }
        }
    }
}
