using midTerm.Models.Tests.Internal;
using midTerm.Models.Profiles;
using System;
using Xunit;

namespace midTerm.Models.Tests
{
    public class MappingConfigurationValidation
    {
        [Fact]
        public void IsValid()
        {
            var configuration = AutoMapperModule.CreateMapperConfiguration<QuestionProfile>();

            configuration.AssertConfigurationIsValid();

        }
    }
}
