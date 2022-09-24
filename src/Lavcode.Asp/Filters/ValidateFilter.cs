using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Lavcode.Asp.Filters
{
    public class ValidateFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (modelState.IsValid)
            {
                return;
            }

            var state = modelState
                .Select(item => item.Value)
                .FirstOrDefault(item => item != null && item.Errors.Count > 0);
            if (state == default)
            {
                return;
            }

            context.Result = new BadRequestObjectResult(new
            {
                message = state.Errors[0].ErrorMessage
            });
        }
    }
}
