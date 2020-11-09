using AdonisUI.Controls;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Dolphin.Ui.Dialog
{
    /// <summary>
    /// Interaction logic for ChangeSkillCastProfileDialog.xaml
    /// </summary>
    public partial class ChangeSkillCastProfileDialog : AdonisWindow
    {
        public ChangeSkillCastProfileDialog()
        {
            InitializeComponent();
        }

        internal void Initialize(SkillCastConfiguration skillCastProfile)
        {
            throw new NotImplementedException();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}