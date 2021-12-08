using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Service.UseCases.Boards.FetchBoard;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTaskManager.Service.Tests.UseCases.Boards.FetchBoard
{
    [TestClass]
    public class FetchBoardRequestHandlerTests : RequestHandlerTestsBase
    {
        private FetchBoardRequestHandler _handler;

        [TestInitialize]
        public void Init()
        {
            _handler = new FetchBoardRequestHandler(Context, Mapper);
        }

        [TestMethod]
        public async Task Handle_Should_ReturnNullWhenNoBoardExist()
        {
            var request = new FetchBoardRequest(1);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Handle_Should_FetchBoard()
        {
            var boards = new[] { new Board("b1"), new Board("b2"), new Board("b3"), new Board("b4") };

            Context.Boards.AddRange(boards);
            await Context.SaveChangesAsync();

            var request = new FetchBoardRequest(1);

            var result = await _handler.Handle(request, CancellationToken.None);
            result.Should().NotBeNull();
            result.Name.Should().Be("b1");
        }

        [TestMethod]
        public async Task Handle_Should_EagerLoadTaskAndUsers()
        {
            var board = new Board("b1");

            Enumerable.Range(0, 2)
                .ToList()
                .ForEach(index => board.CreateNewTask($"Task{index}"));

            Enumerable.Range(0, 3)
                .ToList()
                .ForEach(index => board.GrantUserAccess(User.New($"user{index}", $"user{index}@email.com")));

            Context.Boards.Add(board);
            await Context.SaveChangesAsync();

            var request = new FetchBoardRequest(1);

            var persistedBoard = await _handler.Handle(request, CancellationToken.None);
            persistedBoard.Should().NotBeNull();
            persistedBoard.Tasks.Count().Should().Be(2);
            persistedBoard.Users.Count().Should().Be(3);
        }
    }
}
