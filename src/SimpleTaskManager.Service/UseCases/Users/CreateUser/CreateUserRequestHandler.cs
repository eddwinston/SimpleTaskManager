using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Service.Dtos;

namespace SimpleTaskManager.Service.UseCases.Users.CreateUser
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Email).NotEmpty();
        }
    }

    public class CreateUserBatchRequestValidator : AbstractValidator<CreateUserBatchRequest>
    {
        public CreateUserBatchRequestValidator()
        {
            RuleFor(r => r.Users).NotEmpty();
        }
    }

    public class CreateUserRequest : IRequest<UserDto>
    {
        public CreateUserRequest(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserBatchRequest : IRequest<IEnumerable<UserDto>>
    {
        public CreateUserBatchRequest(IEnumerable<CreateUserRequest> users)
        {
            Users = users;
        }

        public IEnumerable<CreateUserRequest> Users { get; set; }
    }

    public class CreateUserRequestHandler : 
        IRequestHandler<CreateUserRequest, UserDto>,
        IRequestHandler<CreateUserBatchRequest, IEnumerable<UserDto>>
    {
        private readonly SimpleTaskManagerContext _context;
        private readonly IMapper _mapper;

        public CreateUserRequestHandler(SimpleTaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User(request.Name, request.Email);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> Handle(CreateUserBatchRequest request, CancellationToken cancellationToken)
        {
            var users = request.Users.Select(x => new User(x.Name, x.Email));

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
