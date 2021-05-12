using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using midTerm.Controllers;
using midTerm.Models.Models.Question;
using midTerm.Services.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Controller.Test
{
    public class QuestionsControllerTest
    {

        private readonly Mock<IQuestionService> _mockService;
        private readonly QuestionsController _controller;

        public QuestionsControllerTest()
        {
            _mockService = new Mock<IQuestionService>();
            _controller = new QuestionsController(_mockService.Object);
        }


        [Fact]
        public async Task ReturnNoContentWhenHasNoData()
        {
            // Arrange
            int expectedId = 1;
            var question = new Faker<QuestionModelExtended>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(question.Find(x => x.Id == expectedId));
            // Act
            var result = await _controller.GetById(expectedId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnExtendedQuestionByIdWhenHasData()
        {
            // Arrange
            int expectedId = 1;
            var question = new Faker<QuestionModelExtended>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(question.Find(x => x.Id == expectedId));
            // Act
            var result = await _controller.GetById(expectedId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<QuestionModelExtended>().Subject.Id.Should().Be(expectedId);
        }

        [Fact]
        public async Task ReturnQuestionWhenHasData()
        {
            // Arrange
            int expectedId = 10;
            var question = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(10);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question);
            // Act
            var result = await _controller.Getall();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<QuestionModelBase>>().Subject.Count().Should().Be(expectedId);
        }

        [Fact]
        public async Task ReturnEmptyListWhenNoData()
        {
            // Arrange

            var question = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate(0);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(question);
            // Act
            var result = await _controller.Getall();

            // Assert
            result.Should().BeOfType<NoContentResult>();



        }

        [Fact]
        public async Task ReturnOkWhenDeletedData()
        {
            // Arrange
            int id = 1;
            bool expected = true;
            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }

        [Fact]
        public async Task ReturnCreatedQuestionOnCreateWhenCorrectModel()
        {
            // Arrange

            var question = new Faker<QuestionCreateModel>()
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<CreatedResult>();

            var model = result as CreatedResult;
            model?.Value.Should().Be(1);
            model?.Location.Should().Be("/api/Questions/1");
        }

        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionCreateModel>()
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<BadRequestResult>();


        }

        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Put(question.Id, question);

            // Assert
            result.Should().BeOfType<BadRequestResult>();


        }

        [Fact]
        public async Task ReturnQuestionOnUpdateWhenCorrectModel()
        {
            // Arrange
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Put(question.Id, question);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }

        [Fact]
        public async Task ReturnNoContentOnUpdateWhenRepositoryError()
        {
            // Arrange
            var question = new Faker<QuestionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            var expected = new Faker<QuestionModelBase>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<QuestionUpdateModel>()))
                .ReturnsAsync(() => null);
            // Act
            var result = await _controller.Put(question.Id, question);

            // Assert
            result.Should().BeOfType<NoContentResult>();



        }

        [Fact]
        public async Task ReturnConflictOnCreateWhenRepositoryError()
        {
            // Arrange

            var question = new Faker<QuestionCreateModel>()
                .RuleFor(p => p.Text, v => v.Music.Genre())
                .RuleFor(p => p.Description, v => v.Address.FullAddress())
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<QuestionCreateModel>()))
                .ReturnsAsync(() => null);
            // Act
            var result = await _controller.Post(question);

            // Assert
            result.Should().BeOfType<ConflictResult>();


        }

        [Fact]
        public async Task ReturnOkFalseWhenNoDataToDelete()
        {
            // Arrange
            int id = 1;
            bool expected = false;
            _mockService.Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }
        [Fact]
        public async Task ReturnBadRequestWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<BadRequestResult>();

            var model = result as BadRequestResult;


        }


    }
}
