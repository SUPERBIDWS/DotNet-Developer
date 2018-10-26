using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.S4Pay.Controller
{
    public class BaseController : ApiController
    {
        protected new HttpResponseMessage ResponseMessage;

        public Task<HttpResponseMessage> CreateResponse<TResponse>(TResponse response)
        {
            ResponseMessage = Request.CreateResponse(new
            {
                Status = "Success",
                Response = response
            });
            ResponseMessage.StatusCode = HttpStatusCode.OK;
            return Task.FromResult(ResponseMessage);
        }

        public Task<HttpResponseMessage> CreateErrorResponse(string erroMessage)
        {
            ResponseMessage = Request.CreateResponse(
                new
                {
                    Status = "Failure",
                    ErrorMessage = erroMessage
                });
            ResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
            return Task.FromResult(ResponseMessage);
        }
    }
}