using AutoMarket.Infrastructure;

namespace AutoMarket.API.Applications.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly AutoMarketDbContext _dbContext;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(AutoMarketDbContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = default(TResponse);
            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }
                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);
                    _logger.LogInformation("Begin transaction for {Request}", typeof(TRequest).Name);
                    response = await next();
                    _logger.LogInformation("Commit transaction for {Request}", typeof(TRequest).Name);
                    await _dbContext.CommitTransactionAsync(transaction, cancellationToken);
                });
                return response ?? throw new InvalidOperationException("Response is null.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Transaction for {Request}", typeof(TRequest).Name);
                throw;
            }
        }
    }
};
