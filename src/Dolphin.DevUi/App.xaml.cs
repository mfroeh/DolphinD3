using Dolphin.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            var captureScreenService = new CaptureWindowService(handleService, transformService, null);
            var cropImageService = new CropImageService(transformService, null);

            var saveImageService = new SaveImageService(cropImageService);

            var window = new MainWindow();
            window.DataContext = new MainViewModel(captureScreenService, saveImageService);
            window.Show();

            handleService.MainLoop();
        }

    }
}
