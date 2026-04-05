using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockKeeperMail.Database.Data;
using StockKeeperMail.Desktop.Stores;
using StockKeeperMail.Desktop.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;

namespace StockKeeperMail.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly AuthenticationStore _authenticationStore;

        public App()
        {
            SplashScreen splashScreen = new SplashScreen("Assets/SplashScreen.png");
            splashScreen.Show(true);

            _navigationStore = new NavigationStore();
            _authenticationStore = new AuthenticationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplyBrandTheme();

            using (var db = new InventoryManagementContext())
            {
                db.Database.Migrate();

                var connection = db.Database.GetDbConnection();
                System.Diagnostics.Debug.WriteLine("=== RUNTIME DATABASE === " + connection.Database);
                System.Diagnostics.Debug.WriteLine("=== RUNTIME CONNECTION === " + connection.ConnectionString);
            }

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _authenticationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private static void ApplyBrandTheme()
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#0B3BDE"));
            theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString("#AA1845"));

            paletteHelper.SetTheme(theme);
        }

        IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            return services.BuildServiceProvider();
        }
    }

}
