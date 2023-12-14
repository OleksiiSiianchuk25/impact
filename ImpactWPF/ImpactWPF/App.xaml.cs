using NLog;
using System.Windows;

namespace ImpactWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
        }

        private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            logger.Error($"Виникла неперехоплена помилка: {e.Exception.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}
