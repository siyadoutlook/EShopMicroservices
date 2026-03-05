using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("[START] Handling {RequestName} - Request Data = {@RequestData}",
                typeof(TRequest).Name, request);
        }

        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();

        if (timer.Elapsed.TotalSeconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] {RequestName} took {TimeTaken:0.000} seconds",
                typeof(TRequest).Name, timer.Elapsed.TotalSeconds);
        }

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("[END] Handled {RequestName} in {ElapsedMs}ms",
                typeof(TRequest).Name, timer.ElapsedMilliseconds);
        }

        return response;
    }
}
