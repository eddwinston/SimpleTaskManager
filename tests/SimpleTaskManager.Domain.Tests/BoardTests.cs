using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Entities.DomainExceptions;
using System;
using System.Data;

namespace SimpleTaskManager.Domain.Tests
{
    [TestClass]
    public class BoardTests
    {
        private Board _board;

        [TestInitialize]
        public void Init()
        {
            _board = new Board();
        }

        [TestMethod]
        public void CreateNewTask_Should_ReturnValidTask()
        {
            var taskTitle = "Sample task";
            _board.WithId(1);

            var task = _board.CreateNewTask(taskTitle);
            task.Should().NotBeNull();

            task.Title.Should().Be(taskTitle);
            task.BoardId.Should().Be(_board.Id);

            _board.Tasks.Should().NotBeEmpty();
        }

        [TestMethod]
        public void CreateNewTask_Should_ThrowWhenSameTitleExist()
        {
            var title = "Existing title";
            var originalTask = _board.CreateNewTask(title);

            Action action = () => _board.CreateNewTask(title);
            action.Should().ThrowExactly<DuplicateNameException>();
        }

        [TestMethod]
        public void GrantUserAccess_Should_AddBoardUsers()
        {
            var user = User.New("Adam Smith", "adamsmith@email.com");

            _board.GrantUserAccess(user);

            _board.BoardUsers.Should().ContainSingle(x => x.User.Name == user.Name);
        }

        [TestMethod]
        public void GrantUserAccess_Should_NotAddMoreThanOneUserWhenCalledMultipleTimes()
        {
            var user = User.New("Adam Smith", "adamsmith@email.com");

            _board.GrantUserAccess(user);
            _board.GrantUserAccess(user);

            _board.BoardUsers.Should().ContainSingle(x => x.User.Name == user.Name);
        }

        [TestMethod]
        public void RevokeAccess_Should_RemoveBoardUsers()
        {
            var user = User.New("Adam Smith", "adamsmith@email.com");

            _board.GrantUserAccess(user);

            _board.RevokeUserAccess(user);

            _board.BoardUsers.Should().NotContain(x => x.User.Name == user.Name);
        }

        [TestMethod]
        public void CanBeAccessedBy_Should_BeTrueForUser()
        {
            var user = User.New("Adam Smith", "adamsmith@email.com");

            _board.GrantUserAccess(user);

            _board.CanBeAccessedBy(user).Should().BeTrue();
        }

        [TestMethod]
        public void CanBeAccessedBy_Should_BeFalseForUser()
        {
            var user = User.New("Adam Smith", "adamsmith@email.com");

            _board.CanBeAccessedBy(user).Should().BeFalse();
        }
    }
}
