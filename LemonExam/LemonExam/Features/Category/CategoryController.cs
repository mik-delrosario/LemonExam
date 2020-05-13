using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LemonExam.Infrastructure;
using LemonExam.Infrastructure.ActionFilters;
using LemonExam.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LemonExam.Features.Category
{
    [Route("api/v1/category")]
    [ServiceFilter(typeof(ValidateJWTAttribute))]
    public class CategoryController : BaseController
    {
        #region
        private readonly IHttpContextAccessor _accessor;
        private HttpResponse _response => _accessor.HttpContext.Response;
        IHeaderDictionary _headers => _accessor.HttpContext.Request.Headers;
        JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
        #endregion

        public CategoryController(IMediator mediator, IHttpContextAccessor accessor) : base(mediator)
        {
            _accessor = accessor;
        }

        [HttpPost("create")]
        public async Task create([FromBody]JObject inputValue)
        {
            string responseBody = null;
            //_ipaddress = this.HttpContext.Request.Host.Value;
            var response = _mediator.Send<CategoryResponse>(new CategoryParam { JsonLog = inputValue.ToString(), action = "create" });
            int statusCode = response.Data.statusCode;
            if (response.HasException())
                responseBody = DefaultApiResponse.Create(null, response.Data.statusCode, response.Data.message);    //Return 400 - bad request
            else
            {
                responseBody = DefaultApiResponse.Create(response.Data.isSuccess, statusCode, response.Data.message);    //Return 201                 
            }

            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            _response.ContentLength = responseBody.Length;
            await _response.WriteAsync(responseBody, Encoding.UTF8);
        }

        [HttpPost("update")]
        public async Task update([FromBody]JObject inputValue)
        {
            string responseBody = null;
            //_ipaddress = this.HttpContext.Request.Host.Value;
            var response = _mediator.Send<CategoryResponse>(new CategoryParam { JsonLog = inputValue.ToString(), action = "update" });
            int statusCode = response.Data.statusCode;
            if (response.HasException())
                responseBody = DefaultApiResponse.Create(null, response.Data.statusCode, response.Data.message);    //Return 400 - bad request
            else
            {
                responseBody = DefaultApiResponse.Create(response.Data.isSuccess, statusCode, response.Data.message);    //Return 201                 
            }

            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            _response.ContentLength = responseBody.Length;
            await _response.WriteAsync(responseBody, Encoding.UTF8);
        }

        [HttpGet("delete/{id?}")]
        public async Task delete(string ID)
        {
            string responseBody = null;
            //_ipaddress = this.HttpContext.Request.Host.Value;
            var response = _mediator.Send<CategoryResponse>(new CategoryParam { ID = ID, action = "delete" });
            int statusCode = response.Data.statusCode;
            if (response.HasException())
                responseBody = DefaultApiResponse.Create(null, response.Data.statusCode, response.Data.message);    //Return 400 - bad request
            else
            {
                responseBody = DefaultApiResponse.Create(response.Data.isSuccess, statusCode, response.Data.message);    //Return 201                 
            }

            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            _response.ContentLength = responseBody.Length;
            await _response.WriteAsync(responseBody, Encoding.UTF8);
        }

        [HttpGet("read/{id?}")]
        public async Task read(string ID)
        {
            string responseBody = null;
            //_ipaddress = this.HttpContext.Request.Host.Value;
            var response = _mediator.Send<CategoryResponse>(new CategoryParam { ID = ID, action = "read" });
            int statusCode = response.Data.statusCode;
            if (response.HasException())
                responseBody = DefaultApiResponse.Create(null, response.Data.statusCode, response.Data.message);    //Return 400 - bad request
            else
            {
                responseBody = DefaultApiResponse.Create(response.Data.data, statusCode, response.Data.message);    //Return 201                 
            }

            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            _response.ContentLength = responseBody.Length;
            await _response.WriteAsync(responseBody, Encoding.UTF8);
        }

    }
}