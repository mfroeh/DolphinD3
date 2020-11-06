using System;
using System.Windows;

namespace Dolphin.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisUI.Controls.AdonisWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ((App)Application.Current).WindowPlace.Register(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}