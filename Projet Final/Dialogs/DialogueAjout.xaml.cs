using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Classes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Dialogs
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogueAjout : ContentDialog
    {
        bool valide;
        DateTime MinDateNaissance {  get; set; }
        DateTime MaxDateNaissance { get; set;}

        public DialogueAjout()
        {
            this.InitializeComponent();
            MaxDateNaissance = DateTime.Now.AddYears(-18);
            MinDateNaissance = DateTime.Now.AddYears(-100);
        }

        private void resetErreur()
        {
            tblErreurPrenom.Text = string.Empty;
            tblErreurNom.Text = string.Empty;
            tblErreurAdresse.Text = string.Empty;
            tblErreurDate.Text = string.Empty;
            tblErreurPass.Text = string.Empty;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            resetErreur();
            valide = true;

            if (!ValidationAdherent.validatePrenomNom(tbxNomAjout.Text))
            {
                valide = false;
                tblErreurNom.Text = "Le nom doit contenir entre 3 et 50 lettres";
            }

            if (!ValidationAdherent.validatePrenomNom(tbxPrenomAjout.Text))
            {
                valide = false;
                tblErreurPrenom.Text = "Le prénom doit contenir entre 3 et 50 lettres";
            }

            if (!ValidationAdherent.validateAdresse(tbxAdresseAjout.Text))
            {
                valide = false;
                tblErreurAdresse.Text = "L'adresse doit contenir entre 12 et 255 lettres";
            }

            if (!ValidationAdherent.validateDate(calendar_dateAjout.Date))
            {
                valide = false;
                tblErreurDate.Text = "Veuillez entrer la date de naissance";
            }

            if (chk_admin.IsChecked == true)
            {
                if (!ValidationAdherent.validatePass(tbxMotPassAjout.Text))
                {
                    valide = false;
                    tblErreurPass.Text = "Le mot de passe doit contenir entre 4 et 20 caractères";
                }
            }

            if (valide)
            {
                var pass = tbxMotPassAjout.Text;
                Adherent a = new Adherent(tbxNomAjout.Text, tbxPrenomAjout.Text, tbxAdresseAjout.Text,calendar_dateAjout.Date.Value.DateTime, tbxMotPassAjout.Text);
                SingletonBD.getInstance().ajouterAdherent(a);
            }
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                if (valide == false)
                    args.Cancel = true;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sp_pass.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            sp_pass.Visibility = Visibility.Collapsed;
            tbxMotPassAjout.Text = string.Empty;
        }
    }
}
