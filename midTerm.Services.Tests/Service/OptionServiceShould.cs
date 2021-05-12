using AutoMapper;
using FluentAssertions;
using midTerm.Models.Models.Option;
using midTerm.Models.Profiles;
using midTerm.Services.Services;
using midTerm.Services.Tests.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Services.Tests
{
    public class OptionServiceShould
        : SqliteContext
    {
        private readonly IMapper _mapper;
        private readonly OptionService _service;

        public OptionServiceShould()
            : base(withData: true)
        {
            if (_mapper == null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(OptionProfile));
                }).CreateMapper();
                _mapper = mapper;
            }

            _service = new OptionService(DbContext, _mapper);

        }

        [Fact]
        public async Task GetOptionById()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.GetById(expected);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionModelExtended>();
            result.Id.Should().Be(expected);
        }

        [Fact]
        public async Task GetOptionByQuestionId()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.GetByQuestionId(expected);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<OptionModelExtended>>();

        }

        [Fact]
        public async Task GetOptions()
        {
            // Arrange
            var expected = 3;

            // Act
            var result = await _service.Get();

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<OptionBaseModel>>();

        }

        [Fact]
        public async Task InsertNewOption()
        {
            // Arrange
            var option = new OptionCreateModel
            {
                Text = "Mock",
                Order = 4,
                QuestionId = 3
            };
            // Act
            var result = await _service.Insert(option);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionBaseModel>();
            result.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task UpdateOption()
        {
            // Arrange
            var option = new OptionUpdateModel
            {
                Id = 1,
                Text = "Mock",
                Order = 2,
                QuestionId = 7

            };
            // Act
            var result = await _service.Update(option);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OptionBaseModel>();
            result.Id.Should().Be(option.Id);
            result.Order.Should().Be(option.Order);
            result.QuestionId.Should().Be(option.QuestionId);
        }

        [Fact]
        public async Task ThrowExceptionOnUpdateOption()
        {
            // Arrange
            var option = new OptionUpdateModel
            {
                Id = 10,
                Text = "Mock",
                Order = 1,
                QuestionId = 9
            };
            // Act & Assert
            var result = await _service.Update(option);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Update(option));
            Assert.Equal("Option not found", ex.Message);
        }

        [Fact]
        public async Task DeleteOption()
        {
            // Arrange
            var expected = 1;
            // Act
            var result = await _service.Delete(expected);
            var option = await _service.GetById(expected);


            // Assert
            result.Should().Be(true);
            option.Should().BeNull();

        }

    }
}
