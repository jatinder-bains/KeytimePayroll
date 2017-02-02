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
using BainsTech.DocMailer.Adapters;
using BainsTech.DocMailer.Factories;
using BainsTech.DocMailer.Infrastructure;
using BainsTech.DocMailer.Repositories;

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
            builder.RegisterType<DocumentMailer>().As<IDocumentMailer>();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            //builder.RegisterType<MailMessageAdapter>().As<IMailMessageAdapter>().SingleInstance();
            builder.RegisterType<MailMessageAdapterFactory>().As<IMailMessageAdapterFactory>().SingleInstance();
            builder.RegisterType<FileSystemAdapter>().As<IFileSystemAdapter>().SingleInstance();
            builder.RegisterType<FailedMovedDocumentsRepository>().As<IFailedMovedDocumentsRepository>().SingleInstance();
            
            Container = builder.Build();

            var mwvm = Container.Resolve<IMainWindowViewModel>();
            DataContext = mwvm;
        }
       
        private void SetPasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var passwordBox = (PasswordBox)sender;

            var en = PasswordBox.Password.Encrypt();
            MainWindowViewModel.PasswordConfigViewModel.SetSenderEmailAccountPassword(en);

            MessageBox.Show("Password Set");
            MainWindowViewModel.PasswordConfigViewModel.IsEmailPasswordNeeded = false;

        }
    }
}