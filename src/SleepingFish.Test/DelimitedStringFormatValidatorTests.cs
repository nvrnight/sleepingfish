using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SleepingFish.Test
{
    [TestClass]
    public class DelimitedStringFormatValidatorTests
    {
        [TestMethod]
        public void BasicNumbers()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "1,2,3").IsValid);
        }

        [TestMethod]
        public void BasicStrings()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(5, "adipisicing,elit,sed,do,eiusmod").IsValid);
        }

        [TestMethod]
        public void QuotedStrings()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "Lorem,\"ipsum, dolor, sit\",amet").IsValid);
        }

        [TestMethod]
        public void EmptyField()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(4, "1,2,,3").IsValid);
        }

        [TestMethod]
        public void TrailingEmptyField()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(4, "1,2,3,").IsValid);
        }

        [TestMethod]
        public void LeadingEmptyField()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(4, ",1,2,3").IsValid);
        }

        [TestMethod]
        public void EmptyRecordIgnored()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(4, "", ignoreEmptyLines: true).IsValid);
        }

        [TestMethod]
        public void EmptyRecordNotIngored()
        {
            Assert.IsFalse(DelimitedStringValidator.ValidateFormat(4, "").IsValid);
        }

        [TestMethod]
        public void EscapedQuote()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, @"1,""2""""3"",4").IsValid);
        }

        [TestMethod]
        public void FieldStartsWithEscapedDoubleQuote()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(1, @"""""""--""").IsValid);
        }

        [TestMethod]
        public void FieldEndsWithEscapedDoubleQuote()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(1, @"""--""""""").IsValid);
        }

        [TestMethod]
        public void OneTwoOrThreeEscapedDoubleQuotesInStrings()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "\"\"\"\",\"\"\"\"\"\",\"\"\"\"\"\"\"\"").IsValid);
        }

        [TestMethod]
        public void SpecifiedDelimiter()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "1;2;3", delimiter: ';').IsValid);
        }

        [TestMethod]
        public void SpecifiedQuoteChar()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "1,`2,3`,4", textQualifier: '`').IsValid);
        }

        [TestMethod]
        public void LineBreakInField()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(3, "1,\"2\n3\",4").IsValid);
        }

        [TestMethod]
        public void SpacesInFields()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(4, ", 1 ,\" 2 \",").IsValid);
        }

        [TestMethod]
        public void TwoBlankFields()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(2, ",").IsValid);
        }

        [TestMethod]
        public void FieldBeginsWithComma()
        {
            Assert.IsTrue(DelimitedStringValidator.ValidateFormat(1, "\",a\"").IsValid);
        }
    }
}
