using System;
using TModel;
using TServer.Api.Helper;

namespace TApplication
{
    public class AccountService
    {
        public void Do()
        {
            var payload = ApiHelper.GetTokenPayload();
        }

        public decimal GetBalance(int branchId, string accountCode)
        {
            return 2000000;
        }

        public void Debit(decimal amount)
        {
            amount = 1000000 - amount;
        }

        public string CreateAccount(AccountDto accountDto)
        {
            return accountDto.Balance + "|" + accountDto.Balance;
        }

        public AccountDto CreateAccount2(AccountDto accountDto)
        {
            return accountDto;
        }

        public void ThrowException()
        {
            throw new Exception("تست");
        }
    }
}