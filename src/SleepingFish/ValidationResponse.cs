
using System.Collections.Generic;
using System.Text;

namespace SleepingFish
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public string[] ValidationErrors { get; set; }

        public ValidationResponse(bool isValid, string[] validationErrors)
        {
            IsValid = isValid;
            ValidationErrors = validationErrors;
        }

        public ValidationResponse(bool isValid, string validationError) : this(isValid, validationError != null ? new[] { validationError } : null) { }
        public ValidationResponse(bool isValid) : this(isValid, (string[])null) { }

        public static ValidationResponse FailData(string reason, string value, int column)
        {
            return new ValidationResponse(false, string.Format("{0}, Value \"{1}\" in Column {2}.", reason, value, column));
        }

        public static ValidationResponse FailFormat(string reason, int characterNumber, Queue<char> last10, bool includeFailureDetails)
        {
            string error = null;

            if (includeFailureDetails)
            {
                var errorBuilder = new StringBuilder();

                errorBuilder.Append(reason);

                if (last10 != null)
                {
                    errorBuilder.Append(", in ");
                    while (last10.Count > 0)
                    {
                        var isLast = (last10.Count == 1);
                        if (isLast)
                            errorBuilder.Append("-->");

                        errorBuilder.Append(last10.Dequeue());

                        if (isLast)
                            errorBuilder.Append("<--");
                    }
                }

                errorBuilder.Append(string.Format(", at position {0}(first character is 1)", characterNumber));

                error = errorBuilder.ToString();
            }

            return new ValidationResponse(false, error);
        }

        public static ValidationResponse Success
        {
            get
            {
                return new ValidationResponse(true);
            }
        }
    }
}
