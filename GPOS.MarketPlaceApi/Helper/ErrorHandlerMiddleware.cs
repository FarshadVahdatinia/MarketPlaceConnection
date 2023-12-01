using GPOS.Core.Helper;
using GPOS.Infrastructure.UOW;
using Microsoft.Data.SqlClient;
using System.Net;

namespace GPOS.MarketPlaceApi.Helper
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnitOfWork> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<UnitOfWork> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                var errorTitle = string.Empty;
                try
                {
                    errorTitle = ex?.StackTrace.Split("at")[1];
                }
                catch
                {
                    errorTitle = "Error";
                }
                _logger.LogWarning(ex, errorTitle);
                await HandleUnauthorizeExceptionAsync(context, ex);
            }
            catch (CatchedByMeException ex)
            {
                var errorTitle = string.Empty;
                try
                {
                    errorTitle = ex?.StackTrace.Split("at")[1];
                }
                catch
                {
                    errorTitle = "Error";
                }
                _logger.LogWarning(ex, errorTitle);
                await HandleExceptionAsync(context, ex);
            }
            catch (SqlException ex)
            {
                string errorTitle = string.Empty;
                try
                {
                    errorTitle = ex.SQLErrorConvert();
                }
                catch
                {
                    errorTitle = "Error";
                }
                _logger.LogWarning(ex, errorTitle);
                await HandleExceptionAsync(context, new Exception(errorTitle));
            }
            catch (Exception ex)
            {
                var errorTitle = string.Empty;
                var errorName = "خطای غیر منتظره";
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(SqlException))
                {
                    var sqlException = (SqlException)ex.InnerException;
                    if (sqlException.Number == 2601)
                    {
                        errorName = "اطلاع تکراری";
                    }
                }
                try
                {
                    errorTitle = ex?.StackTrace.Split("at")[1];
                }
                catch
                {
                    errorTitle = "Error";
                }

                _logger.LogError(ex, errorTitle);
                await HandleExceptionAsync(context, new Exception(errorName));
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }

        private async Task HandleUnauthorizeExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}
