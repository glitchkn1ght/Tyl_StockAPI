using Stock_API.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Stock_API.Validation
{
    public interface IModelStateValidator
    {
        ResponseStatus MapModelStateErrors(ModelStateDictionary modelState);
    }

    public class ModelStateValidator : IModelStateValidator
    {
        public ResponseStatus MapModelStateErrors(ModelStateDictionary modelState)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            
            IEnumerable<ModelError> allErrors = modelState.Values.SelectMany(v => v.Errors);
            responseStatus.Code = -101;
            responseStatus.Message = string.Join(", ", allErrors.Select(x => x.ErrorMessage));

            return responseStatus;
        }
    }
}
