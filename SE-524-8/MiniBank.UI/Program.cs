using Microsoft.Extensions.DependencyInjection;
using MiniBank.Repository;
using MiniBank.Repository.Interfaces;
using MiniBank.Service;
using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // უკვე რეგისტრირებული ფორმის მიღება DI კონტეინერიდან
            var mainForm = ServiceProvider.GetRequiredService<Form1>();

            Application.Run(mainForm);
        }



        //კოდი სადაც უშუალოდ მოხდევა სერვისების რეგისტრაცია და ინიციალიზაცია.
        private static void ConfigureServices(IServiceCollection services)
        {
            #region რეპოზიტორის რეგისტრაცია

            const string customerFilePath = @"../../../../MiniBank.Data/Customers.csv";
            const string accountFilePath = @"../../../../MiniBank.Data/Accounts.json";


            services.AddSingleton<ICustomerRepository>(options =>
            {
                return CustomerRepository
                    .CreateAsync(customerFilePath)
                    .GetAwaiter()
                    .GetResult();
            });

            services.AddSingleton<IAccountRepository>(options =>
            {
                return AccountRepository
                    .CreateAsync(accountFilePath)
                    .GetAwaiter()
                    .GetResult();
            });



            #endregion


            #region სერვისების რეგისტრაცია

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<Form1>();
            services.AddTransient<Form2>();

            #endregion
        }


    }
}