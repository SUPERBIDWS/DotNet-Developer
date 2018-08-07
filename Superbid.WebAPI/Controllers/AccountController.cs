using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Superbid.Domain.DomainModels;
using Superbid.Domain.ServicesInterfaces;
using Superbid.WebAPI.models;

namespace Superbid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        
        // POST api/account
        [HttpPost]
        public ActionResult<Account> Post([FromBody] AccountModel accountModel)
        {
            var account = this._accountService.CreateAccount(accountModel.InitialAmmount);
            return Ok(account);
        }
        
        // GET api/transfer
        [HttpGet]
        public ActionResult<IList<Account>> Get()
        {
            Account account = null;
    
            return Ok(_accountService.GetAccounts());

        }

    }
}