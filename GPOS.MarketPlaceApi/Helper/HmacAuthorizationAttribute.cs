using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GPOS.MarketPlaceApi.Helper
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class HmacAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var value = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(value))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "UnAuthorized"
                };
                _logger.Error(context.Result + "Authorization Is Null Or Empty");
                return;
            }
            var result = HmacAuthorize.HmacAuthorization(value);
            if (!result)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "UnAuthorized"
                };
                _logger.Error(context.Result + "The authorization model did not Match the Header");
                return;
            }
            await next();
        }
    }
}