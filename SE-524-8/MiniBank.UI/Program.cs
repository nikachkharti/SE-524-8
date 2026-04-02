using Microsoft.Extensions.DependencyInjection;
using MiniBank.Repository;
using MiniBank.Repository.Interfaces;
using MiniBank.Service;
using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainForm = ServiceProvider.GetRequiredService<Form1>();
            Application.Run(mainForm);
        }


        private static void ConfigureServices(IServiceCollection services)
        {
            string customerFilePath = @"../../../../MiniBank.Data/Customers.csv";

            // Repository (Singleton because of in-memory state)
            services.AddSingleton<ICustomerRepository>(provider =>
            {
                return CustomerRepository
                    .CreateAsync(customerFilePath)
                    .GetAwaiter()
                    .GetResult();
            });

            // Service
            services.AddTransient<ICustomerService, CustomerService>();

            // Forms
            services.AddTransient<Form1>();
        }


    }
}