
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SleepingFish.Test
{
    [TestClass]
    public class ValueValidatorTests
    {
        [TestMethod]
        public void ValidateEmptyValueAndItsAllowed()
        {
            var result1 = ValueValidator.Validate("", 1, (x, y) => { return ValidationResponse.Success; }, allowEmptyValue: true);
            var result2 = ValueValidator.Validate(null, 1, (x, y) => { return ValidationResponse.Success; }, allowEmptyValue: true);
            Assert.IsTrue(result1.IsValid);
            Assert.IsTrue(result2.IsValid);
        }

        [TestMethod]
        public void ValidateEmptyValueAndItsNotAllowed()
        {
            var result1 = ValueValidator.Validate("", 1, (x, y) => { return ValidationResponse.Success; });
            var result2 = ValueValidator.Validate(null, 1, (x, y) => { return ValidationResponse.Success; });
            Assert.IsFalse(result1.IsValid);
            Assert.IsFalse(result2.IsValid);
            Assert.AreEqual("Unexpected empty value, Value \"\" in Column 1.", result1.ValidationErrors[0]);
            Assert.AreEqual("Unexpected empty value, Value \"\" in Column 1.", result2.ValidationErrors[0]);
        }

        [TestMethod]
        public void HasProceedingSpacesAndItsAllowed()
        {
            var result = ValueValidator.Validate("  myvalue", 1, (x, y) => { return ValidationResponse.Success; }, allowProceedingSpaces: true);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void HasProceedingSpacesAndItsNotAllowed()
        {
            var result = ValueValidator.Validate("  myvalue", 1, (x, y) => { return ValidationResponse.Success; });
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unexpected proceeding space, Value \"  myvalue\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void HasTrailingSpacesAndItsAllowed()
        {
            var result = ValueValidator.Validate("myvalue  ", 1, (x, y) => { return ValidationResponse.Success; }, allowTrailingSpaces: true);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void HasTrailingSpacesAndItsNotAllowed()
        {
            var result = ValueValidator.Validate("myvalue  ", 1, (x, y) => { return ValidationResponse.Success; });
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unexpected trailing space, Value \"myvalue  \" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void MinimumLengthMet()
        {
            var result = ValueValidator.Validate("123", 1, (x, y) => { return ValidationResponse.Success; }, minLength: 3);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MinimumLengthNotMet()
        {
            var result = ValueValidator.Validate("123", 1, (x, y) => { return ValidationResponse.Success; }, minLength: 4);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Minimum length 4 not met, Value \"123\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void MaximumLengthNotExceeded()
        {
            var result = ValueValidator.Validate("123", 1, (x, y) => { return ValidationResponse.Success; }, maxLength: 3);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MaximumLengthExceeded()
        {
            var result = ValueValidator.Validate("123", 1, (x, y) => { return ValidationResponse.Success; }, maxLength: 2);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Maximum length 2 exceeded, Value \"123\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void Int16InvalidValue()
        {
            var result = ValueValidator.ValidateInt16("ffdsfdsgds", 1);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Not Int16, Value \"ffdsfdsgds\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void Int16DoesntMeetMinimumValue()
        {
            var result = ValueValidator.ValidateInt16("5", 1, minValue: 6);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Minimum value 6 not met, Value \"5\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void Int16MaximumValueExceeded()
        {
            var result = ValueValidator.ValidateInt16("5", 1, maxValue: 4);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Maximum value 4 exceeded, Value \"5\" in Column 1.", result.ValidationErrors[0]);
        }

        [TestMethod]
        public void Int16ValueIsValid()
        {
            var result = ValueValidator.ValidateInt16("5", 1, minValue: 5, maxValue: 5);
            Assert.IsTrue(result.IsValid);
        }
    }
}
