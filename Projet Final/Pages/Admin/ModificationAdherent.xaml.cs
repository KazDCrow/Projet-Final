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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModificationAdherent : Page
    {
        private bool valide;
        private string id;
        public ModificationAdherent()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {
                SingletonNavigation.getInstance().NavigationView.SelectedItem = null;

                var index = (int)e.Parameter;
                Adherent a = SingletonBD.getInstance().getAdherent(index);
                id = a.Id;
                tbl_nom.Text = a.Id;
                tbx_nom.Text = a.Nom;
                tbx_prenom.Text = a.Prenom;
                tbx_adresse.Text = a.Adresse;
                calendar_date.Date = a.Date_naissance;
                tbxMotPassAjout.Text = a.Mot_passe;
                if (a.Mot_passe != "")
                {
                    sp_pass.Visibility = Visibility.Visible;
                    chk_admin.IsChecked = true;
                }
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

        private void resetErreur()
        {
            tblErreurPrenom.Text = string.Empty;
            tblErreurNom.Text = string.Empty;
            tblErreurAdresse.Text = string.Empty;
            tblErreurDate.Text = string.Empty;
            tblErreurPass.Text = string.Empty;
        }

        private void btn_modifierClick(object sender, RoutedEventArgs e)
        {
            resetErreur();
            valide = true;

            if (!ValidationAdherent.validatePrenomNom(tbx_prenom.Text))
            {
                valide = false;
                tblErreurNom.Text = "Le nom doit contenir entre 3 et 50 lettres";
            }

            if (!ValidationAdherent.validatePrenomNom(tbx_nom.Text))
            {
                valide = false;
                tblErreurPrenom.Text = "Le prénom doit contenir entre 3 et 50 lettres";
            }

            if (!ValidationAdherent.validateAdresse(tbx_adresse.Text))
            {
                valide = false;
                tblErreurAdresse.Text = "L'adresse doit contenir entre 12 et 255 lettres";
            }

            if (!ValidationAdherent.validateDate(calendar_date.Date))
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
                Adherent a = new Adherent(tbx_nom.Text, tbx_prenom.Text, tbx_adresse.Text, calendar_date.Date.Value.DateTime, tbxMotPassAjout.Text);
                SingletonBD.getInstance().modifierAdherent(id, a);
                this.Frame.Navigate(typeof(AdherentPage));
            }
        }

        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdherentPage));
        }
    }
}
