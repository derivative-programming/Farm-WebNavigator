using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GENVALRootName.Base.Objects
{
    public class ValidationRule
    {
        public ValidationRule()
        {
            _ForUIOnly = false;
            _PropertyName = string.Empty;
            _ValidationRuleType = Objects.ValidationRuleType.Unknown;
            _RuleValue = string.Empty;
            _RuleValueIsBoolean = false;
            _RuleValueIsInteger = false;
            _ValidationErrorSeverity = Objects.ValidationErrorSeverity.Critical;
            _ErrorMessage = string.Empty;
            
        }

        protected bool _ForUIOnly;
        protected string _PropertyName;
        protected ValidationRuleType _ValidationRuleType;
        protected string _RuleValue;
        protected bool _RuleValueIsBoolean;
        protected bool _RuleValueIsInteger;
        protected ValidationErrorSeverity _ValidationErrorSeverity;
        protected string _ErrorMessage;

        public bool ForUIOnly { get { return _ForUIOnly; } }
        public string PropertyName { get { return _PropertyName; } }
        public ValidationRuleType ValidationRuleType { get { return _ValidationRuleType; } }
        public string RuleValue { get { return _RuleValue; } }
        public ValidationErrorSeverity ValidationErrorSeverity { get { return _ValidationErrorSeverity; } }
        public string ErrorMessage { get { return _ErrorMessage; } }
    }

    public class ValidationError
    {
        public ValidationError()
        {
            this.ValidationErrorSeverity = Objects.ValidationErrorSeverity.Critical;
            this.PropertyName = string.Empty;
            this.Message = string.Empty;
        }

        public ValidationErrorSeverity ValidationErrorSeverity { get; set; }
        public string PropertyName { get; set; }
        public string Message { get; set; } 
    }

    public enum ValidationRuleType
    {
        Unknown,
        IsRequired,
        MinStringLength,
        MaxStringLength,
        MinNumeric,
        MaxNumeric,
        IsNotEditableByUser,
        IsNotVisibleToUser,
        IsEmail,
        AllowMultipleEmail,
        IsNumeric
    }

    public enum ValidationRuleTypeBooleanValue
    {
        IsRequired, 
        IsNotEditableByUser,
        IsNotVisibleToUser,
        IsEmail,
        AllowMultipleEmail,
        IsNumeric
    }

    public enum ValidationRuleTypeIntegerValue
    { 
        MinStringLength,
        MaxStringLength,
        MinNumeric,
        MaxNumeric
    }

    public enum ValidationErrorSeverity
    {
        Critical,
        Warning,
        Info,
        Tip
    }
}
 
