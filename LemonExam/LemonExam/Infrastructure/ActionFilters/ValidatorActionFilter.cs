using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure.ActionFilters
{
    public class ValidatorActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.Controller.ViewData.Modeltate.IsValid) {
            //    if (filterContext.HttpContext.Request.HttpMethod == "GET") {
            //        var result = new StatusCodes.HttpStatusCode.BadRequest);
            //        filterContext.Result = result;
            //    }
            //    else {
            //        var result = new ContentResult();
            //        string content = JsonConvert.SerializeObject(filterContext.Controller.ViewData.ModelState,
            //            new JsonSerializerSettings {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //        result.Content = content;
            //        result.ContentType = "application/json";

            //        filterContext.HttpContext.Response.StatusCode = 400;
            //        filterContext.Result = result;
            //    }
            //}
        }
        public void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext filterContext) { }
    }
}
