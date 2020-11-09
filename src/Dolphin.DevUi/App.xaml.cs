using Dolphin.Image;
using Dolphin.Service;
using System.Diagnostics;
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
            
            var logService = new LogService(new Log());
            var handleService = new HandleService(logService);
            var transformService = new TransformService(handleService);
            var captureScreenService = new CaptureWindowService(handleService, logService);
            var cropImageService = new CropImageService(transformService, logService);
            var saveImageService = new SaveImageService(cropImageService);

            handleService.MainLoop("Diablo III64");

            var window = new MainWindow
            {
                DataContext = new MainViewModel(captureScreenService, saveImageService)
            };
            window.Show();
        }
    }
}