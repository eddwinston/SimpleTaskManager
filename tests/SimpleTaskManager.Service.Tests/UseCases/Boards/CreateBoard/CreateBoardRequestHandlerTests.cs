using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Service.Dtos;
using SimpleTaskManager.Service.UseCases.Boards.CreateBoard;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleTaskManager.Service.Tests.UseCases.Boards.CreateBoard
{
    [TestClass]
    public class CreateBoardRequestHandlerTests : RequestHandlerTestsBase
    {
        private CreateBoardRequestHandler _handler;

        [TestInitialize]
        public void Init()
        {
            _handler = new CreateBoardRequestHandler(Context, Mapper);
        }

        [TestMethod]
        public async Task Handler_Should_CreateNewBoard()
        {
            var request = new CreateBoardRequest("Board name");

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();

            var persistedBoard = await Context.Boards.SingleOrDefaultAsync(b => b.Name == request.Name);
            persistedBoard.Should().NotBeNull();
        }

        [TestMethod]
        public async Task Handler_Should_ThrowOnDuplicateBoardName()
        {
            var board = new Board("Board");

            Context.Boards.Add(board);            
            await Context.SaveChangesAsync();

            Func<Task<BoardDto>> operation = async () => 
                await _handler.Handle(new CreateBoardRequest(board.Name), CancellationToken.None);
            
            await operation.Should().ThrowAsync<DuplicateNameException>();
        }
    }
}
