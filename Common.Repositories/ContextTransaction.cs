using Common.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Repositories
{
    public class ContextTransaction : IContextTransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;
        public ContextTransaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContextTransaction.DisposeAsync();
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await _dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }
}
