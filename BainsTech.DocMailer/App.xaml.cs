using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BainsTech.DocMailer.Components;
using ConfigurationSettings = BainsTech.DocMailer.Components.ConfigurationSettings;

namespace BainsTech.DocMailer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>  
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var config = new ConfigurationSettings();
            if (!config.NoTrump)
            {
                var screen = new SplashScreen("Resources/splash.jpg");
                screen.Show(false, true);
                Task.Delay(5000).Wait();
                screen.Close(TimeSpan.FromSeconds(3));
            }
        }
    }
}
