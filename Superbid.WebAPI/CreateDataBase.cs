using Superbid.Domain.ServicesInterfaces;

namespace Superbid.WebAPI
{
    public class CreateDataBase
    {
        private readonly IAccountService _accountService;
        public CreateDataBase(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public void CreateDataBaseAndPopulate()
        {
            _accountService.CreateAccount(10000.00m);
            _accountService.CreateAccount(100000.00m);
        }
    }
}