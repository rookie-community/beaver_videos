using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaverVideos.WPF.ViewModel
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ConfigureServices();
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            // services.AddSingleton<IContactsService, ContactsService>();
            // services.AddSingleton<IPhoneService, PhoneService>();

            // Viewmodels
            services.AddTransient<MainViewModel>();

            var serviceProvider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider);

            return serviceProvider;
        }

        public MainViewModel? MainVM => Ioc.Default.GetService<MainViewModel>();
    }
}
