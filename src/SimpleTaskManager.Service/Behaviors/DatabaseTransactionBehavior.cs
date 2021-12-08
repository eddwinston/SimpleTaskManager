using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTaskManager.Service.Behaviors
{
    public class DatabaseTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly SimpleTaskManagerContext _context;

        public DatabaseTransactionBehavior(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var response = await next();

                await transaction.CommitAsync();
                
                return response;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                throw ex switch
                {
                    DbUpdateConcurrencyException concurrencyException => new SimpleTaskManagerException(concurrencyException.Message),
                    DbUpdateException dbUpdateException => new SimpleTaskManagerException(dbUpdateException.Message),
                    ValidationException validationException => new InputValidationException(validationException.Message),
                    _ => ex,
                };
            }
        }
    }
}
