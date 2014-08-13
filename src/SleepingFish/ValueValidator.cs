
using System;
namespace SleepingFish
{
    public static class ValueValidator
    {
        public static ValidationResponse ValidateInt16(string value, int column, bool allowProceedingSpaces = false, bool allowTrailingSpaces = false, bool allowEmptyValue = false, Int16? minValue = null, Int16? maxValue = null)
        {
            return Validate(value, column, 
                (v, c) => 
                {
                    short t;

                    if (Int16.TryParse(v.Trim(), out t))
                    {
                        if (minValue != null && t < minValue.Value)
                            return ValidationResponse.FailData(string.Format("Minimum value {0} not met", minValue.Value), v, c);

                        if (maxValue != null && t > maxValue.Value)
                            return ValidationResponse.FailData(string.Format("Maximum value {0} exceeded", maxValue.Value), v, c);

                        return ValidationResponse.Success;
                    }

                    return ValidationResponse.FailData("Not Int16", v, c);
                }, 
                allowProceedingSpaces: allowProceedingSpaces,
                allowTrailingSpaces: allowTrailingSpaces,
                allowEmptyValue: allowEmptyValue);
        }

        public static ValidationResponse Validate(string value, int column, Func<string, int, ValidationResponse> validateDataFunc, bool allowProceedingSpaces = false, bool allowTrailingSpaces = false, bool allowEmptyValue = false, int? minLength = null, int? maxLength = null)
        {
            if(string.IsNullOrEmpty(value))
            {
                if(allowEmptyValue)
                    return ValidationResponse.Success;
                else
                    return ValidationResponse.FailData("Unexpected empty value", value, column);
            }

            if (!allowProceedingSpaces && value[0] == ' ')
                return ValidationResponse.FailData("Unexpected proceeding space", value, column);

            if (!allowTrailingSpaces && value[value.Length - 1] == ' ')
                return ValidationResponse.FailData("Unexpected trailing space", value, column);

            if (minLength != null && value.Length < minLength.Value)
                return ValidationResponse.FailData(string.Format("Minimum length {0} not met", minLength.Value), value, column);

            if(maxLength != null && value.Length > maxLength.Value)
                return ValidationResponse.FailData(string.Format("Maximum length {0} exceeded", maxLength.Value), value, column);

            return validateDataFunc(value, column);
        }
    }
}
