using Stock_API.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Stock_API.Interfaces
{
    public interface IModelStateErrorMapper
    {
        ResponseStatus MapModelStateErrors(ModelStateDictionary modelState);
    }
}
