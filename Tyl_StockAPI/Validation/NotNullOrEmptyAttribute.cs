using System.ComponentModel.DataAnnotations;

namespace Stock_API.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotNullOrEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not null or empty";
        public NotNullOrEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}
