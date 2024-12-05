using Google.Protobuf.WellKnownTypes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogueAjoutActivite : ContentDialog
    {
        bool valide;
        public DialogueAjoutActivite()
        {
            this.InitializeComponent();
            cbx_type.ItemsSource = SingletonTypeActivite.getInstance().ListeTypes;
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                if (valide == false)
                    args.Cancel = true;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private bool isNumber(string text)
        {
            try
            {
                Convert.ToDouble(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void tbx_prix_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            tblErreurPrix.Text = string.Empty;
            if (!isNumber(sender.Text))
            {
                tblErreurPrix.Text = "Vous devez entrer un nombre valide (ne pas utiliser le point pour les nombres � virgules)";
                var positionCurseur = sender.SelectionStart;
                if (sender.Text.Length != 0)
                {
                    sender.Text = sender.Text.Remove(sender.Text.Length - 1);
                }
                sender.SelectionStart = Math.Min(positionCurseur, sender.Text.Length);
            }
        }

        private void tbx_cout_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            tblErreurCout.Text = string.Empty;
            if (!isNumber(sender.Text))
            {
                tblErreurCout.Text = "Vous devez entrer un nombre valide (ne pas utiliser le point pour les nombres � virgules)";
                var positionCurseur = sender.SelectionStart;
                if (sender.Text.Length != 0)
                {
                    sender.Text = sender.Text.Remove(sender.Text.Length - 1);
                }
                sender.SelectionStart = Math.Min(positionCurseur, sender.Text.Length);
            }
        }
    }
}
