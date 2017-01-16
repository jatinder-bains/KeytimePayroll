//using System.ComponentModel;

using System;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.ViewModels;
using System.Security.Cryptography;
using BainsTech.DocMailer.Infrastructure;

namespace BainsTech.DocMailer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IContainer Container { get; set; }

        private IMainWindowViewModel MainWindowViewModel => (IMainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            

            var builder = new ContainerBuilder();
            
            builder.RegisterType<DocumentHandler>().As<IDocumentHandler>();
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>();
            builder.RegisterType<ConfigurationSettings>().As<IConfigurationSettings>();
            builder.RegisterType<MailerDocumentsViewModel>().As<IMailerDocumentsViewModel>();
            builder.RegisterType<PasswordConfigViewModel>().As<IPasswordConfigViewModel>();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            
            Container = builder.Build();

            var mwvm = Container.Resolve<IMainWindowViewModel>();
            DataContext = mwvm;
        }
       
        private void SetPasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var passwordBox = (PasswordBox)sender;

            var en = PasswordBox.Password.Encrypt();
            MainWindowViewModel.PasswordConfigViewModel.SetSenderEmailAccountPassword(en); 
        }
    }
}