using Microsoft.AspNetCore.Mvc;

namespace DingFood.API.Middleware;

/// <summary>
/// Captura exceções não tratadas e devolve um ProblemDetails genérico —
/// evita vazar stack trace/detalhes internos para o cliente em produção.
/// </summary>
public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DingFood.Domain.Exceptions.TenantAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new ProblemDetails { Status = 403, Title = "Tenant.Forbidden", Detail = "O recurso não pertence à empresa ativa." }, context.RequestAborted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            if (context.Response.HasStarted)
                throw;

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = "Erro interno",
                Status = StatusCodes.Status500InternalServerError,
                Detail = environment.IsDevelopment()
                    ? ex.ToString()
                    : "Ocorreu um erro inesperado. Tente novamente ou contate o suporte."
            };

            try
            {
                await context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                // Cliente desconectou antes da resposta de erro ser escrita.
            }
        }
    }
}
