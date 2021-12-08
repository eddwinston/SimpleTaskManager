using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Entities.DomainExceptions;

namespace SimpleTaskManager.Service.UseCases.Stats
{
    public class StatsRequestValidator : AbstractValidator<StatsRequest>
    {
        public StatsRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }

    public class StatsRequest : IRequest<IStatResult>
    {
        public StatsRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class StatsRequestHandler : IRequestHandler<StatsRequest, IStatResult>
    {
        private readonly IEnumerable<IStat> _stats;

        public StatsRequestHandler(IEnumerable<IStat> stats)
        {
            _stats = stats;
        }

        public async Task<IStatResult> Handle(StatsRequest request, CancellationToken cancellationToken)
        {
            var stat = _stats.SingleOrDefault(x => x.GetType().GetCustomAttribute<StatNameAttribute>().Value == request.Name);
            if (stat == null)
            {
                throw new SimpleTaskManagerException($"No stat provider found for {request.Name}");
            }

            return await stat.GetStatResult();
        }
    }

    public class StatNameAttribute : Attribute
    {
        public StatNameAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public interface IStatResult
    {
    }

    public interface IStat
    {
        Task<IStatResult> GetStatResult();
    }

    [StatName("inprogress_task_per_board")]
    public class InProgressTaskPerBoardStat : IStat
    {
        private readonly SimpleTaskManagerContext _context;

        public InProgressTaskPerBoardStat(SimpleTaskManagerContext context)
        {
            _context = context;
        }

        public Task<IStatResult> GetStatResult()
        {
            return Task.FromResult<IStatResult>(new StatResult());
        }

        public class StatResult : IStatResult
        {
            public IDictionary<string, int> Data { get; set; }
        }
    }

    [StatName("inprogress_task_per_user_per_board")]
    public class InProgressTaskPerUserPerBoardStat : IStat
    {
        public Task<IStatResult> GetStatResult()
        {
            throw new NotImplementedException();
        }
    }
}
