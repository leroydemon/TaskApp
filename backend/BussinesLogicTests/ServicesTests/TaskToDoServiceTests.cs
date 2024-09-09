using AutoMapper;
using BussinesLogic.EntityDtos;
using BussinesLogic.Services;
using Domain.Entities;
using Domain.Filters;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTests
{
    public class TaskToDoServiceTests
    {
        private readonly Mock<IRepository<TaskToDo>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TaskToDoService>> _loggerMock;
        private readonly TaskToDoService _taskService;

        public TaskToDoServiceTests()
        {
            _repositoryMock = new Mock<IRepository<TaskToDo>>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TaskToDoService>>();
            _taskService = new TaskToDoService(_repositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_TaskExists_ReturnsTaskDto()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskToDo { Id = taskId, Title = "Test Task" };
            var taskDto = new TaskToDoDto { Id = taskId, Title = "Test Task" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskToDoDto>(task)).Returns(taskDto);

            // Act
            var result = await _taskService.GetByIdAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(taskDto);
        }

        [Fact]
        public async Task AddAsync_ValidTask_ReturnsTaskDto()
        {
            // Arrange
            var taskDto = new TaskToDoDto { Title = "New Task" };
            var task = new TaskToDo { Title = "New Task" };
            var addedTask = new TaskToDo { Id = Guid.NewGuid(), Title = "New Task" };
            var addedTaskDto = new TaskToDoDto { Id = addedTask.Id, Title = "New Task" };

            _mapperMock.Setup(m => m.Map<TaskToDo>(taskDto)).Returns(task);
            _repositoryMock.Setup(repo => repo.AddAsync(task)).ReturnsAsync(addedTask);
            _mapperMock.Setup(m => m.Map<TaskToDoDto>(addedTask)).Returns(addedTaskDto);

            // Act
            var result = await _taskService.AddAsync(taskDto);

            // Assert
            result.Should().BeEquivalentTo(addedTaskDto);
        }

        [Fact]
        public async Task RemoveAsync_TaskExists_RemovesTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskToDo { Id = taskId };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            // Act
            await _taskService.RemoveAsync(taskId);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidTask_ReturnsUpdatedTaskDto()
        {
            // Arrange
            var taskDto = new TaskToDoDto { Id = Guid.NewGuid(), Title = "Updated Task" };
            var task = new TaskToDo { Id = taskDto.Id, Title = "Updated Task" };
            var updatedTask = new TaskToDo { Id = taskDto.Id, Title = "Updated Task" };

            _mapperMock.Setup(m => m.Map<TaskToDo>(taskDto)).Returns(task);
            _repositoryMock.Setup(repo => repo.UpdateAsync(task)).ReturnsAsync(updatedTask);
            _mapperMock.Setup(m => m.Map<TaskToDoDto>(updatedTask)).Returns(taskDto);

            // Act
            var result = await _taskService.UpdateAsync(taskDto);

            // Assert
            result.Should().BeEquivalentTo(taskDto);
        }

        [Fact]
        public async Task SearchAsync_ValidFilter_ReturnsMatchingTasks()
        {
            // Arrange
            var filter = new TaskToDoFilter { Title = "Test" };
            var tasks = new List<TaskToDo>
            {
            new TaskToDo { Id = Guid.NewGuid(), Title = "Test Task 1" },
            new TaskToDo { Id = Guid.NewGuid(), Title = "Test Task 2" }
            };
            var taskDtos = tasks.Select(t => new TaskToDoDto { Id = t.Id, Title = t.Title }).ToList();

            _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<TaskToDo>>()))
                .ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<List<TaskToDoDto>>(tasks)).Returns(taskDtos);

            // Act
            var result = await _taskService.SearchAsync(filter);

            // Assert
            result.Should().BeEquivalentTo(taskDtos);
        }
    }

}
