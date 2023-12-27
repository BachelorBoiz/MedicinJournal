using MedicinJournal.Security;
using MedicinJournal.Security.Models;
using System.Security.Claims;
using System.Text;

namespace MedicinJournal.API.Middleware
{
    public class AuditLogMiddleware
    {
        private const string ControllerKey = "controller";
        private const string IdKey = "id";

        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, SecurityDbContext dbContext)
        {
            await _next(context);

            var request = context.Request;

            if (request.Path.StartsWithSegments("/api"))
            {
                var userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

                Console.WriteLine(userName);

                if (request.Path.StartsWithSegments("/api/Auth/login"))
                    userName = "Login";

                request.RouteValues.TryGetValue(ControllerKey, out var controllerValue);
                var controllerName = (string)(controllerValue ?? string.Empty);

                var changedValue = await GetChangedValues(request).ConfigureAwait(false);

                var auditLog = new AuditLog
                {
                    EntityName = controllerName,
                    UserName = userName,
                    Action = request.Method,
                    Timestamp = DateTime.UtcNow,
                    Changes = changedValue
                };

                dbContext.AuditLogs.Add(auditLog);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task<string> GetChangedValues(HttpRequest request)
        {
            var changedValue = string.Empty;

            switch (request.Method)
            {
                case "POST":
                case "PUT":
                    changedValue = await ReadRequestBody(request, Encoding.UTF8).ConfigureAwait(false);
                    break;

                case "DELETE":
                    request.RouteValues.TryGetValue(IdKey, out var idValueObj);
                    changedValue = (string?)idValueObj ?? string.Empty;
                    break;

                default:
                    break;
            }

            return changedValue;
        }

        private static async Task<string> ReadRequestBody(HttpRequest request, Encoding? encoding = null)
        {
            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;

            return requestBody;
        }
    }
}
