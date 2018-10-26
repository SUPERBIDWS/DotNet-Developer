using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Api.S4Pay.Model;
using S4Pay.Domain.Account;
using S4Pay.Factory.Service;

namespace Api.S4Pay.Controller
{
    [RoutePrefix("transaction")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost, Route("save")]
        public Task<HttpResponseMessage> Save(TransactionModel transaction)
        {
            try
            {
                var saved = _transactionService.Save(new Transaction
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    Value = transaction.Value,
                    AnotherUserId = transaction.AnotherUserId

                });
                return CreateResponse(saved);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

        [HttpPost, Route("approve/{transactionId}")]
        public Task<HttpResponseMessage> Approve(Guid transactionId)
        {
            try
            {
                var approved = _transactionService.Approve(transactionId);
                return CreateResponse(approved);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

        [HttpGet, Route("pending/{userId}")]
        public Task<HttpResponseMessage> Get(Guid userId)
        {
            try
            {
                var transactionPending = _transactionService.GetTransactionPending(userId);
                return CreateResponse(transactionPending);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

        [HttpGet, Route("balance/{userId}")]
        public Task<HttpResponseMessage> Balance(Guid userId)
        {
            try
            {
                var balance = _transactionService.GetBalance(userId);
                return CreateResponse(balance);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

        [HttpGet, Route("extracts/{userId}")]
        public Task<HttpResponseMessage> Extract(Guid userId)
        {
            try
            {
                var extracts = _transactionService.GetTransactionApproved(userId);
                return CreateResponse(extracts);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

        [HttpPost, Route("disapprove/{transactionId}")]
        public Task<HttpResponseMessage> Disapprove(Guid transactionId)
        {
            try
            {
                var extracts = _transactionService.Disapprove(transactionId);
                return CreateResponse(extracts);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e.Message);
            }
        }

    }
}