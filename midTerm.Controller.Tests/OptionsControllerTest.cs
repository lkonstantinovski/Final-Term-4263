using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using midTerm.Controllers;
using midTerm.Models.Models.Option;
using midTerm.Services.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Controller.Test
{
    public class OptionsControllerTest
    {
        private readonly Mock<IOptionService> _mockService;
        private readonly OptionsController _controller;

        public OptionsControllerTest()
        {
            _mockService = new Mock<IOptionService>();
            _controller = new OptionsController(_mockService.Object);
        }

        [Fact]
        public async Task ReturnExtendedOptionByIdWhenHasData()
        {
            // Arrange
            int expectedId = 1;
            var Option = new Faker<OptionModelExtended>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(Option.Find(x => x.Id == expectedId));
            // Act
            var result = await _controller.GetById(expectedId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<OptionModelExtended>().Subject.Id.Should().Be(expectedId);
        }

        

        [Fact]
        public async Task ReturnOptionWhenHasData()
        {
            // Arrange
            int expectedId = 10;
            var option = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(10);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(option);
            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().BeOfType<List<OptionBaseModel>>().Subject.Count().Should().Be(expectedId);
        }


        [Fact]
        public async Task ReturnEmptyListWhenNoData()
        {
            // Arrange

            var option = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(0);

            _mockService.Setup(x => x.Get())
                .ReturnsAsync(option);
            // Act
            var result = await _controller.Get();

            // Assert
            result.Should().BeOfType<NoContentResult>();



        }

        [Fact]
        public async Task ReturnNoContentWhenHasNoData()
        {
            // Arrange
            int expectedId = 1;
            var Option = new Faker<OptionModelExtended>()
                .RuleFor(p => p.Id, v => v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate(6);

            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(Option.Find(x => x.Id == expectedId));
            // Act
            var result = await _controller.GetById(expectedId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ReturnConflictOnCreateWhenRepositoryError()
        {
            // Arrange
            var option = new Faker<OptionCreateModel>()
               .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(() => null);
            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<ConflictResult>();


        }

        [Fact]
        public async Task ReturnCreatedOptionOnCreateWhenCorrectModel()
        {
            // Arrange

            var option = new Faker<OptionCreateModel>()
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, v => ++v.IndexVariable)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<CreatedResult>();

            var model = result as CreatedResult;
            model?.Value.Should().Be(1);
            model?.Location.Should().Be("/api/Options/1");
        }

        [Fact]
        public async Task ReturnBadRequestOnCreateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var option = new Faker<OptionCreateModel>()
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Insert(It.IsAny<OptionCreateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Post(option);

            // Assert
            result.Should().BeOfType<BadRequestResult>();


        }


        [Fact]
        public async Task ReturnBadRequestOnUpdateWhenModelNotValid()
        {
            // Arrange
            _controller.ModelState.AddModelError("someFakeError", "fakeError message");
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Put(option.Id, option);

            // Assert
            result.Should().BeOfType<BadRequestResult>();


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

        [Fact]
        public async Task ReturnOptionOnUpdateWhenCorrectModel()
        {
            // Arrange
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(expected);
            // Act
            var result = await _controller.Put(option.Id, option);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var model = result as OkObjectResult;
            model?.Value.Should().Be(expected);

        }

        [Fact]
        public async Task ReturnNoContentOnUpdateWhenRepositoryError()
        {
            // Arrange
            var option = new Faker<OptionUpdateModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            var expected = new Faker<OptionBaseModel>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.Text, v => v.Name.FirstName())
                .RuleFor(p => p.Order, v => ++v.IndexVariable)
                .RuleFor(p => p.QuestionId, v => ++v.IndexVariable)
                .Generate();

            _mockService.Setup(x => x.Update(It.IsAny<OptionUpdateModel>()))
                .ReturnsAsync(() => null);
            // Act
            var result = await _controller.Put(option.Id, option);

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

        
    }
}
