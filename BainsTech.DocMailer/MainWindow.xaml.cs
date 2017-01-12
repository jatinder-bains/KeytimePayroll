using System.Windows;
using Autofac;
using BainsTech.DocMailer.Components;
using BainsTech.DocMailer.ViewModels;

namespace BainsTech.DocMailer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IContainer Container { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            

            var builder = new ContainerBuilder();
            
            builder.RegisterType<DocumentHandler>().As<IDocumentHandler>();
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>();
            builder.RegisterType<ConfigurationSettings>().As<IConfigurationSettings>();
            builder.RegisterType<MailerDocumentsViewModel>().As<IMailerDocumentsViewModel>();

            Container = builder.Build();

            var mwvm = Container.Resolve<IMainWindowViewModel>();
            DataContext = mwvm;
        }
    }
}