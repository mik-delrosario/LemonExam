using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LemonExam.Infrastructure;
using System;


namespace LemonExam.Shared
{
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;

        #region Constructor

        protected BaseController(IMediator mediator) {
            _mediator = mediator;
        }

        #endregion

        //protected ActionResult SendQuery<TResponse>(IRequest<TResponse> query) {
        //    return SendQuery(query, null);
        //}

        //protected ActionResult SendQuery<TResponse>(IRequest<TResponse> query, Func<Response<TResponse>, ActionResult> redirect) {
        //    var result = _mediator.Request<TResponse>(query);
        //    if (result.HasException())
        //        throw result.Exception;
        //    if (redirect != null)
        //        return redirect.Invoke(result);
        //    return View(result.Data);
        //}

        //protected JsonResult SendQueryToJson<TResponse>(IRequest<TResponse> query) {
        //    var result = _mediator.Request<TResponse>(query);
        //    if (result.HasException())
        //        throw result.Exception;
        //    else
        //        return Json(result.Data);
        //}

        protected ActionResult SendCommand(ICommand command) {
            return SendCommand(command);
        }

        protected ActionResult SendCommand(ICommand command, Func<ActionResult> redirect) {
            var result = _mediator.Send(command);
            if (result.HasException()) {
                throw result.Exception;
            }
            if (redirect != null) {
                return redirect.Invoke();
            }

            return RedirectToAction("Index");
        }

    }
}
