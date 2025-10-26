using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Pedidos.WebApi.Filters;

public class ModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.HttpContext.Items["ModelStateErrors"] = new SerializableError(context.ModelState);
            context.Result = new BadRequestResult(); 
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}