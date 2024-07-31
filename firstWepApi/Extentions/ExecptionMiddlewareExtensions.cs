using Entities.ErrorModel;
using Entities.Exeptions;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace firstWepApi.Extentions
{
    public static class ExecptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appErr => {
                appErr.Run(async context => {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature is not null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };
    
                        logger.LogError($"Someting went wrong : {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString()) ;
                    }
                });
            });
        }
    }
}
