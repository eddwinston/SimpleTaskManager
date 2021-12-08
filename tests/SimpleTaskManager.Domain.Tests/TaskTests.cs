using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleTaskManager.Domain.Tests
{
    [TestClass]
    public class TaskTests
    {
        private Board _board;
        private TaskItem _task;

        [TestInitialize]
        public void Init()
        {
            _board = new Board();
            _task = _board.CreateNewTask("task");
        }

        [TestMethod]
        public void NewlyCreatedTask_Should_HaveNewStatus()
        {
            _task.Status.Should().Be(TaskStatus.New);
        }

        [TestMethod]
        public void MarkAsInProgress_Should_ChangeStatusToInProgress()
        {
            _task.MarkAsInProgress();

            _task.Status.Should().Be(TaskStatus.InProgress);
        }

        [TestMethod]
        public void MarkAsCompleted_Should_ChangeStatusToCompleted()
        {
            _task.MarkAsComplete();

            _task.Status.Should().Be(TaskStatus.Completed);
        }

        [TestMethod]
        public void Update_Should_BeSuccessful()
        {
            var newTitle = "new title";

            _task.UpdateTitle(newTitle);
            
            _task.Title.Should().Be(newTitle);
            
            _task.UpdateStatus("InProgress");
            _task.Status.Should().Be(TaskStatus.InProgress);
            
            var originalCompletedDate = _task.CompletedAt;
            _task.UpdateStatus("Completed");
            _task.Status.Should().Be(TaskStatus.Completed);
            _task.CompletedAt.Should().NotBe(originalCompletedDate);
        }

        [TestMethod]
        public void CompletedTask_Should_ClearUser()
        {
            var user = User.New("test user", "testuser@email.com");

            _board.GrantUserAccess(user);
            _task.AssignToUser(user);

            _task.UserId.Should().NotBeNull();
            _task.UpdateStatus("Completed");
            _task.UserId.Should().BeNull();
        }

        [TestMethod]
        public void AssignUser_Should_ThrowWhenUserIsNotPartOfBoard()
        {
            var user = User.New("test user", "testuser@email.com");

            Action action = () => _task.AssignToUser(user);

            action.Should().ThrowExactly<UnauthorizedAccessException>();
        }

        [TestMethod]
        public void UnAssignUser_Should_ClearUserFromTask()
        {
            var user = User.New("test user", "testuser@email.com");
            _board.GrantUserAccess(user);

            _task.AssignToUser(user);

            _task.UnAssignUser(user);
            _task.User.Should().BeNull();
        }

        [TestMethod]
        public void UnAssigUser_Should_ThrowWhenUserIsWrongUser()
        {
            var user = User.New("test user", "testuser@email.com");
            _board.GrantUserAccess(user);

            _task.AssignToUser(user);

            var wrongUser = User.New("wrong user", "wronguser@email.com");
            wrongUser.WithId(2);
            
            Action action = () => _task.UnAssignUser(wrongUser);

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
