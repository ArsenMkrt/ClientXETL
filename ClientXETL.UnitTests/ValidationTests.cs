using ClientXETL.Services.Validation;
using Microsoft.Extensions.Logging;
using Moq;

// Combined test suite for all classes

namespace ClientXETL.Tests
{
    public class ValidationTests
    {
        // 2. Tests for GreaterThanValidationRule
        [Fact]
        public void GreaterThanValidationRule_Validate_ValidValue_NoErrors()
        {
            // Arrange
            var rule = new GreaterThanValidationRule<TestModel, int>(m => m.Value, 10);
            var model = new TestModel { Value = 15 };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void GreaterThanValidationRule_Validate_InvalidValue_ReturnsError()
        {
            // Arrange
            var rule = new GreaterThanValidationRule<TestModel, int>(m => m.Value, 10);
            var model = new TestModel { Value = 5 };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Single(errors);
            Assert.Contains("Property must be greater than 10", errors.First().Message);
        }

        // 3. Tests for NotNullOrEmptyValidationRule
        [Fact]
        public void NotNullOrEmptyValidationRule_Validate_ValidValue_NoErrors()
        {
            // Arrange
            var rule = new NotNullOrEmptyValidationRule<TestModel>(m => m.Name);
            var model = new TestModel { Name = "Valid Name" };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void NotNullOrEmptyValidationRule_Validate_InvalidValue_ReturnsError()
        {
            // Arrange
            var rule = new NotNullOrEmptyValidationRule<TestModel>(m => m.Name);
            var model = new TestModel { Name = "" };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Single(errors);
            Assert.Contains("Property cannot be null or empty", errors.First().Message);
        }

        // 4. Tests for RangeValidationRule
        [Fact]
        public void RangeValidationRule_Validate_ValidValue_NoErrors()
        {
            // Arrange
            var rule = new RangeValidationRule<TestModel, int>(m => m.Value, 5, 15);
            var model = new TestModel { Value = 10 };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Empty(errors);
        }

        [Theory]
        [InlineData(3)]  // Below range
        [InlineData(20)] // Above range
        public void RangeValidationRule_Validate_InvalidValue_ReturnsError(int value)
        {
            // Arrange
            var rule = new RangeValidationRule<TestModel, int>(m => m.Value, 5, 15);
            var model = new TestModel { Value = value };

            // Act
            var errors = rule.Validate(model).ToList();

            // Assert
            Assert.Single(errors);
            Assert.Contains($"TestModel value {value} is out of range [5, 15]", errors.First().Message);
        }

        // Helper Test Model
        private class TestModel
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }
    }
}