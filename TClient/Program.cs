using System;
using System.Diagnostics.CodeAnalysis;
using TModel;

namespace TClient
{
    [SuppressMessage("ReSharper", "UnusedVariable")]
    internal class Program
    {
        private static void Main()
        {
            var result = ServiceFactory.CallMethod<LoginOutputs>("LoginService", "Login", new LoginInputs
            {
                Username = "Arman",
                Password = "332332"
            });

            ServiceConfigs.SetToken(result.Token);

            Test0();
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
            Test6();
            Test7();
        }

        private static void Test0()
        {
            ServiceFactory.CallMethod("AccountService", "Do");
        }

        private static void Test1()
        {
            var result = ServiceFactory.CallMethod<decimal>("AccountService", "GetBalance", 1, "69/0/164194");
        }

        private static void Test2()
        {
            ServiceFactory.CallMethod("AccountService", "Debit", 1000);
        }

        private static void Test3()
        {
            var result2 = ServiceFactory.CallMethod<string>("AccountService", "CreateAccount", new AccountDto
            {
                Balance = 26000,
                BranchId = 1
            });
        }

        private static void Test4()
        {
            var result2d = ServiceFactory.CallMethod<AccountDto>("AccountService", "CreateAccount2", new AccountDto
            {
                Balance = 26000,
                BranchId = 1
            });
        }

        private static void Test5()
        {
            try
            {
                ServiceFactory.CallMethod("AccountService", "ThrowException");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

        private static void Test6()
        {

        }

        private static void Test7()
        {

        }
    }
}
