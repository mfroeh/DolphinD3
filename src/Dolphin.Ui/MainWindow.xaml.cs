using Dolphin.Ui.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Dolphin.Ui.View
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((Hyperlink)sender).NavigateUri.OriginalString);

            e.Handled = true;
        }

        private void TabItem_Selected(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).Children.First(x => x is HotkeyTabViewModel).RaisePropertyChanged(nameof(HotkeyTabViewModel.SkillCastProfiles));
        }
    }
}