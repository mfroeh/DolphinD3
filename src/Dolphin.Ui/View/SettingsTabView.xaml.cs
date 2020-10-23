using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Dolphin.Ui.View
{
    /// <summary>
    /// Interaction logic for SettingsTabView.xaml
    /// </summary>
    public partial class SettingsTabView : UserControl
    {
        public SettingsTabView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}