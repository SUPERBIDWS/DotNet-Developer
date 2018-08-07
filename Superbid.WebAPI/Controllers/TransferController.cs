using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Superbid.Domain.DomainModels;
using Superbid.Domain.ServicesInterfaces;
using Superbid.WebAPI.models;

namespace Superbid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private IAccountService accountService;

        public TransferController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

  
        // GET api/transfer/xxxx
        [HttpGet("{accountId}")]
        public ActionResult<string> Get(string accountId)
        {
            return Ok(accountService.GetAccountById(accountId));

            return "value";
        }

        // POST api/transfer
        [HttpPost]
        public void Post([FromBody] TransferModel transferModel)
        {
            accountService.TransferAmmountBetweenAccounts(transferModel.DebitAccountId, transferModel.CreditAccountId, transferModel.Ammount);
            Ok("Amount has transfered sucessufy");
        }

        // PUT api/transfer/xxxx
        [HttpPut("{transactionId}")]
        public ActionResult<IList<Account>> Put(string transactionId)
        {
            var accounts = accountService.ConfirmTransaction(transactionId);
            return Ok(accounts);

        }
    }
}