
namespace SleepingFish
{
    public class DelimitedStringDataValidatorOptions
    {
        public char Delimiter { get; set; }
        public char TextQualifier { get; set; }
        public char EscapeCharacter { get; set; }
        public bool IncludeFailureDetails { get; set; }
    }
}
