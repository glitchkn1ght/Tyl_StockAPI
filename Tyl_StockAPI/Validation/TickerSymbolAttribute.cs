using Stock_API.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Stock_API.Validation
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class TickerSymbolAttribute : ValidationAttribute
    {
        //In order to be most performant this would be cached from an external service at an appropriate interval.
        List<string> validSymbols = new List<string> { "AAPL", "AMZN", "MSFT", "ATVI" };

        public TickerSymbolAttribute() : base() { }

        public override bool IsValid(object value)
        {
            ResponseStatus response = new ResponseStatus();

            string requestedSymbols = (string)value;
            List<string> requestedSymbolList = requestedSymbols.Split(',').ToList();

            IEnumerable<string> invalidSymbols = requestedSymbolList.Except(validSymbols);

            if (invalidSymbols.Any())
            {
                string validationErrors = $"The following ticker symbols are not valid: {String.Join(", ", invalidSymbols)}";

                ErrorMessage = validationErrors;

                return false;
            }

            return true;
        }
    }
}
