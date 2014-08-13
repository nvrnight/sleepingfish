using System;
using System.Collections.Generic;
using System.Text;

namespace SleepingFish
{
    public static class DelimitedStringValidator
    {
        public static ValidationResponse ValidateFormat(
            int columnCount, 
            string value, 
            TextQualifierOption textQualifierOption = TextQualifierOption.Optional,
            char delimiter = ',',
            char textQualifier = '"',
            char escapeCharacter = '"',
            bool ignoreEmptyLines = false,
            bool includeFailureDetails = true
            )
        {
            var columns = new List<TextQualifierOption>();
            for (var i = 0; i < columnCount; i++)
                columns.Add(textQualifierOption);

            return ValidateFormat(columns.ToArray(), value, delimiter, textQualifier, escapeCharacter, ignoreEmptyLines, includeFailureDetails);
        }

        public static ValidationResponse ValidateFormat(TextQualifierOption[] columns, string value, DelimitedStringFormatValidatorOptions options)
        {
            if(columns == null)
                throw new ArgumentNullException("columns");

            if (columns.Length < 1)
                throw new ArgumentOutOfRangeException("columns");

            if (string.IsNullOrEmpty(value))
            {
                if(options.IgnoreEmptyLines)
                {
                    return ValidationResponse.Success;
                }
                else
                {
                    return ValidationResponse.FailFormat("Unexpected empty line", 1, null, options.IncludeFailureDetails);
                }
            }

            var insideText = false;
            var lastCharacterEndedTextField = false;
            var lastCharacterIsEscapeCharacter = false;
            var lastCharacterIsDelimiter = false;
            var fieldsFound = 1;
            var fieldsExpected = columns.Length;
            var lastIndex = value.Length - 1;
            var currentFieldOption = columns[0];

            var last10 = new Queue<char>();

            for (var i = 0; i < value.Length; i++)
            {
                if (i >= 10)
                    last10.Dequeue();

                last10.Enqueue(value[i]);

                if (lastCharacterIsEscapeCharacter && value[i] != options.EscapeCharacter && value[i] != options.TextQualifier)
                    return ValidationResponse.FailFormat(string.Format("Expected {0} or {1}", options.EscapeCharacter, options.TextQualifier), i, last10, options.IncludeFailureDetails);

                if(lastCharacterIsDelimiter)
                {
                    if (currentFieldOption == TextQualifierOption.Required && value[i] != options.TextQualifier)
                        return ValidationResponse.FailFormat("Expected text qualifier", i, last10, options.IncludeFailureDetails);

                    if (currentFieldOption == TextQualifierOption.Excluded && value[i] == options.TextQualifier)
                        return ValidationResponse.FailFormat("Unexpected text qualifier", i, last10, options.IncludeFailureDetails);
                }

                if(lastCharacterEndedTextField)
                {
                    if (value[i] != options.Delimiter)
                        return ValidationResponse.FailFormat("Expected delimiter", i, last10, options.IncludeFailureDetails);

                    lastCharacterEndedTextField = false;
                }

                if (insideText)
                {
                    if (i == lastIndex && value[i] != options.TextQualifier)
                        return ValidationResponse.FailFormat("Expected text qualifier", i, last10, options.IncludeFailureDetails);
                    else
                    {
                        if(!lastCharacterIsEscapeCharacter)
                        {
                            if(value[i] == options.TextQualifier)
                            {
                                if(i == lastIndex)
                                {
                                    lastCharacterEndedTextField = true;
                                    insideText = false;
                                }
                                else
                                {
                                    if (value[i + 1] == options.Delimiter)
                                    {
                                        lastCharacterEndedTextField = true;
                                        insideText = false;
                                    }
                                    else if (value[i] == options.EscapeCharacter)
                                    {
                                        lastCharacterIsEscapeCharacter = true;
                                    }
                                }
                            }
                            else if(value[i] == options.EscapeCharacter)
                            {
                                lastCharacterIsEscapeCharacter = true;
                            }                            
                        }
                        else
                        {
                            if(options.TextQualifier == options.EscapeCharacter)
                                lastCharacterIsEscapeCharacter = false;
                        }
                    }
                }
                else
                {

                    if (value[i] == options.Delimiter)
                    {
                        lastCharacterIsDelimiter = true;
                        fieldsFound++;
                        
                        if (fieldsFound > fieldsExpected)
                            return ValidationResponse.FailFormat("Too many fields", i, null, options.IncludeFailureDetails);

                        currentFieldOption = columns[fieldsFound - 1];
                    }
                    else
                    {
                        lastCharacterIsDelimiter = false;

                        if (value[i] == options.TextQualifier)
                        {
                            insideText = true;
                        }
                        else
                        {
                            lastCharacterIsEscapeCharacter = value[i] == options.EscapeCharacter;
                        }
                    }
                }
            }

            if (fieldsFound < fieldsExpected)
                return ValidationResponse.FailFormat("Not enough fields", value.Length, null, options.IncludeFailureDetails);

            if (insideText)
                return ValidationResponse.FailFormat("Unterminated text qualifier", value.Length, null, options.IncludeFailureDetails);

            return ValidationResponse.Success;
        }

        public static ValidationResponse ValidateFormat(TextQualifierOption[] columns,
            string value,
            char delimiter = ',',
            char textQualifier = '"',
            char escapeCharacter = '"',
            bool ignoreEmptyLines = false,
            bool includeFailureDetails = true
            )
        {
            return ValidateFormat(columns, value, new DelimitedStringFormatValidatorOptions(delimiter, textQualifier, escapeCharacter, ignoreEmptyLines, includeFailureDetails));
        }
    }
}
