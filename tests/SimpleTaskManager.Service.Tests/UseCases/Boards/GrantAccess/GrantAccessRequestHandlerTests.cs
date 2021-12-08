using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Entities.DomainExceptions;
using SimpleTaskManager.Service.UseCases.Boards.GrantAccess;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTaskManager.Service.Tests.UseCases.Boards.GrantAccess
{
    [TestClass]
    public class GrantAccessRequestHandlerTests : RequestHandlerTestsBase
    {
        private GrantAccessRequestHandler _handler;

        [TestInitialize]
        public void Init()
        {
            _handler = new GrantAccessRequestHandler(Context);
        }

        [TestMethod]
        public async Task Handle_Should_ThrowWhenBoardNotFound()
        {
            var request = new GrantAccessRequest(1, 1);

            Func<Task<Unit>> operation = async () => await _handler.Handle(request, CancellationToken.None);
            await operation.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [TestMethod]
        public async Task Handle_Should_ThrowWhenUserNotFound()
        {
            var board = new Board("Board1");
            Context.Boards.Add(board);
            await Context.SaveChangesAsync();

            var request = new GrantAccessRequest(1, 1);

            Func<Task<Unit>> operation = async () => await _handler.Handle(request, CancellationToken.None);
            await operation.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [TestMethod]
        public async Task Handle_Should_NotPersistWhenUserAlreadyHasAccessToBoard()
        {
            var user = new User("user1", "user1@email.com");
            var board = new Board("Board1");
            board.GrantUserAccess(user);
            Context.Boards.Add(board);
            await Context.SaveChangesAsync();

            var request = new GrantAccessRequest(board.Id, user.Id);
            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().Be(Unit.Value);
            board.BoardUsers.SingleOrDefault(x => x.User.Email == user.Email).Should().NotBeNull();
        }

        [TestMethod]
        public async Task Handle_Should_AddUserToBoardUsers()
        {
            var board = new Board("Board1");
            Context.Boards.Add(board);
            await Context.SaveChangesAsync();

            board.BoardUsers.Any().Should().BeFalse();

            var user = new User("user", "user@email.com");
            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            var request = new GrantAccessRequest(board.Id, user.Id);
            var result = await _handler.Handle(request, CancellationToken.None);

            board.BoardUsers.Any().Should().BeTrue();
            board.BoardUsers.Count().Should().Be(1);
            board.BoardUsers.Any(x => x.User.Email == user.Email).Should().BeTrue();
        }
    }
}
