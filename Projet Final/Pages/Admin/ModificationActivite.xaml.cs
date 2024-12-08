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
    public sealed partial class ModificationActivite : Page
    {
        bool valide;
        string oldNom = "";
        string oldType = "";
        public ModificationActivite()
        {
            this.InitializeComponent();
            cbx_type.ItemsSource = SingletonTypeActivite.getInstance().ListeTypes;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {
                SingletonNavigation.getInstance().NavigationView.SelectedItem = null;

                var index = (int)e.Parameter;
                Activite a = SingletonBD.getInstance().getActivite(index);
                tbl_nom.Text = a.Nom;
                oldNom = a.Nom;
                oldType = a.Type;
                tbx_nom.Text = a.Nom;
                cbx_type.SelectedItem = a.Type;
                tbx_cout.Text = Convert.ToString(a.Cout_organisation);
                tbx_vente.Text = Convert.ToString(a.Prix_vente);
            }
        }

        private void resetErreur()
        {
            tblErreurNom.Text = string.Empty;
            tblErreurType.Text = string.Empty;
            tblErreurCout.Text = string.Empty;
            tblErreurPrix.Text = string.Empty;
        }

        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {
            resetErreur();
            valide = true;

            if (!ValidationAdherent.validatePrenomNom(tbx_nom.Text))
            {
                valide = false;
                tblErreurNom.Text = "Le nom doit contenir entre 3 et 50 lettres";
            }

            if (cbx_type.SelectedItem == null)
            {
                valide = false;
                tblErreurType.Text = "Veuillez choisir un type pour l'activité.";
            }

            if (tbx_cout.Text == "")
            {
                valide = false;
                tblErreurCout.Text = "Le coût d'organisation ne peut pas être null.";
            }

            if (tbx_vente.Text == "")
            {
                valide = false;
                tblErreurPrix.Text = "Le prix de vente ne peut pas être null.";
            }

            if (valide)
            {
                Activite a = new Activite(tbx_nom.Text, cbx_type.SelectedItem.ToString(), Convert.ToDouble(tbx_cout.Text), Convert.ToDouble(tbx_vente.Text));
                SingletonBD.getInstance().modifierActivite(a,oldNom,oldType);
                this.Frame.Navigate(typeof(ActiviteAdminPage));
            }
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
                tblErreurPrix.Text = "Vous devez entrer un nombre valide (ne pas utiliser le point pour les nombres à virgules)";
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
                tblErreurCout.Text = "Vous devez entrer un nombre valide (ne pas utiliser le point pour les nombres à virgules)";
                var positionCurseur = sender.SelectionStart;
                if (sender.Text.Length != 0)
                {
                    sender.Text = sender.Text.Remove(sender.Text.Length - 1);
                }
                sender.SelectionStart = Math.Min(positionCurseur, sender.Text.Length);
            }
        }

        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ActiviteAdminPage));
        }
    }
}
