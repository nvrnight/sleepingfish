
namespace SleepingFish
{
    public class DelimitedStringFormatValidatorOptions
    {
        public char Delimiter { get; set; }
        public char TextQualifier { get; set; }
        public char EscapeCharacter { get; set; }
        public bool IgnoreEmptyLines { get; set; }
        public bool IncludeFailureDetails { get; set; }

        public DelimitedStringFormatValidatorOptions(char delimiter, char textQualifier, char escapeCharacter, bool ignoreEmptyLines, bool includeFailureDetails)
        {
            IgnoreEmptyLines = ignoreEmptyLines;
            Delimiter = delimiter;
            TextQualifier = textQualifier;
            EscapeCharacter = escapeCharacter;
            IncludeFailureDetails = includeFailureDetails;
        }
    }
}
