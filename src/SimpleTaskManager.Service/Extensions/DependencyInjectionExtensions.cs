using AutoMapper;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleTaskManager.Service.Behaviors;
using SimpleTaskManager.Service.Dtos;
using SimpleTaskManager.Service.UseCases.Boards.CreateBoard;
using SimpleTaskManager.Service.UseCases.Stats;
using System.Linq;
using System.Reflection;

namespace SimpleTaskManager.Service.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSimpleTaskManagerService(this IServiceCollection services)
        {
            var statType = typeof(IStat);
            var implementingTypes = statType
                .Assembly
                .GetTypes()
                .Where(x => statType.IsAssignableFrom(x))
                .Where(x => !x.IsAbstract)
                .ToList();

            implementingTypes.ForEach(implementingType => services.AddScoped(statType, implementingType));

            services
                .AddMediatR(typeof(CreateBoardRequestHandler))
                .AddAutoMapper(typeof(UserDto));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DatabaseTransactionBehavior<,>));

            services.AddFluentValidation(new[] { typeof(CreateBoardRequestHandler).GetTypeInfo().Assembly });

            return services;
        }
    }
}
