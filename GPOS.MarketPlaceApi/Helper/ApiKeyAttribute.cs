using GPOS.Core.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(validOn: AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyName = "Api-Key";
    private const string ClientId = "Client-Id";
    private readonly string _clientName;
    public ApiKeyAttribute(string clientName)
    {
        _clientName = clientName;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey)
                || !context.HttpContext.Request.Headers.TryGetValue(ClientId, out var extractedClientId))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "UnAuthorized"
            };
            return;
        }

        var apiKey = context.HttpContext.RequestServices.GetService<List<ApiKeyModel>>().FirstOrDefault(c => c.ClientId == extractedClientId && c.Key == extractedApiKey && c.ClientName == _clientName);

        if (apiKey == null)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "UnAuthorized"
            };
            return;
        }

        await next();
    }
}

