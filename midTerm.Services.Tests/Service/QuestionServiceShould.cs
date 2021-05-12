using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using midTerm.Models.Models.Question;
using midTerm.Services.Services;
using midTerm.Services.Tests.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace midTerm.Service.Test.Service
{
    public class QuestionServiceShould
    : SqliteContext
    {
        private readonly IMapper _mapper;
        private readonly QuestionService _service;

        public QuestionServiceShould()
        : base(true)
        {
            if (_mapper == null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(QuestionService));
                }).CreateMapper();
                _mapper = mapper;
            }
            _service = new QuestionService(DbContext, _mapper);
        }

        [Fact]
        public async Task GetQuestionById()
        {
            // Arrange
            var expected = 1;

            // Act
            var result = await _service.GetById(expected);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelExtended>();
            result.Id.Should().Be(expected);
        }

        [Fact]
        public async Task GetQuestions()
        {
            // Arrange
            var expected = 3;

            // Act
            var result = await _service.Get();

            // Assert
            result.Should().NotBeEmpty().And.HaveCount(expected);
            result.Should().BeAssignableTo<IEnumerable<QuestionModelBase>>();
        }

        [Fact]
        public async Task InsertNewQuestion()
        {
            
            var question = new QuestionCreateModel
            {

                Description = "VisualStudio 2019"
            };

            
            var result = await _service.Insert(question);

            
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task UpdateQuestion()
        {
            
            var question = new QuestionUpdateModel
            {
                Id = 1,
                Description = "VisualStudio 2019"
            };

            
            var result = await _service.Update(question);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<QuestionModelBase>();
            result.Id.Should().Be(question.Id);
            result.Description.Should().Be(question.Description);
            result.Description.Should().Be(question.Description);

        }

        [Fact]
        public async Task ThrowExceptionOnUpdateQuestion()
        {
            
            var question = new QuestionUpdateModel
            {
                Id = 10,
                Description = "VisualStudio 2019"
            };

            
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.Update(question));
            Assert.Equal("Question not found", ex.Message);

        }

        [Fact]
        public async Task DeleteQuestion()
        {
            
            var expected = 1;

            
            var result = await _service.Delete(expected);
            var question = await _service.GetById(expected);

            
            result.Should().Be(true);
            question.Should().BeNull();
        }
    }
}

