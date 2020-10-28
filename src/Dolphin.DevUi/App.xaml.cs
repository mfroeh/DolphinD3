using Dolphin.Service;
using System.Windows;

namespace Dolphin.DevUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var handleService = new HandleService();
            var transformService = new TransformService(handleService);
            var logService = new LogService(new Log());
            var captureScreenService = new CaptureWindowService(handleService, transformService, logService);
            var cropImageService = new CropImageService(transformService, logService);
            var saveImageService = new SaveImageService(cropImageService);

            var window = new MainWindow();
            window.DataContext = new MainViewModel(captureScreenService, saveImageService);
            window.Show();

            handleService.MainLoop();
        }

    }
}
