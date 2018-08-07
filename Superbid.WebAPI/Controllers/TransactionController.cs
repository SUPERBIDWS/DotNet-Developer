using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Superbid.Domain.DomainModels;
using Superbid.Domain.ServicesInterfaces;

namespace Superbid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController: ControllerBase
    {
        private IAccountService _accountService;
        public TransactionController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        
        // GET api/transaction
        [HttpGet]
        public ActionResult<IList<Transaction>> Get()
        {
            var pendingTransactions = _accountService.GetPendingTransactions();
            return Ok(pendingTransactions);
        }
    }
}