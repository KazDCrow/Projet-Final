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
using Google.Protobuf.WellKnownTypes;
using Projet_Final.Classes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModificationSeancePage : Page
    {
        bool valide;
        DateTime MinDateNaissance { get; set; }
        public ModificationSeancePage()
        {
            this.InitializeComponent();
            cbx_type.ItemsSource = SingletonTypeActivite.getInstance().ListeTypes;
            cbx_nom.ItemsSource = SingletonBD.getInstance().ListeActiviteForType;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {
                SingletonNavigation.getInstance().NavigationView.SelectedItem = null;

                var index = (int)e.Parameter;
                Seance s = SingletonBD.getInstance().getSeance(index);
                

                tbx_id.Text = Convert.ToString(s.Id);
                tbx_place.Text = Convert.ToString(s.Nb_place);
                tbx_heure.Text = s.Heure;
                calendar_date.Date = s.Date;

                cbx_type.SelectedItem = s.Type_activite;
                cbx_nom.SelectedItem = s.Nom_activite;
                MinDateNaissance = DateTime.Now;
            }
        }

        private void btn_modifierClick(object sender, RoutedEventArgs e)
        {
            valide = true;
            resetErreur();
            if (cbx_nom.SelectedItem == null)
            {
                valide = false;
                tblErreurNom.Text = "Veuillez choisir une activité";
            }

            if (cbx_type.SelectedItem == null)
            {
                tblErreurType.Text = "Veuillez choisir un type d'activité";
            }

            if (!ValidationAdherent.validateDate(calendar_date.Date))
            {
                valide = false;
                tblErreurDate.Text = "Veuillez choisir une date pour l'activité";
            }

            if (!ValidationAdherent.validateHeureSeance(tbx_heure.Text))
            {
                valide = false;
                tblErreurHeure.Text = "L'heure doit être de la forme: 11h20, 01h02 ou encore 12h59";
            }

            if (tbx_place.Text == "")
            {
                valide = false;
                tblErreurPlace.Text = "Veuillez inscrire le nombre de place";
            }

            if (valide)
            {
                Seance s = new Seance(Convert.ToInt32(tbx_id.Text),cbx_type.SelectedItem.ToString(), cbx_nom.SelectedItem.ToString(), calendar_date.Date.Value.DateTime, tbx_heure.Text, Convert.ToInt32(tbx_place.Text));
                SingletonBD.getInstance().modifierSeance(s);
                this.Frame.Navigate(typeof(SeanceAdminPage));
            }
        }

        private void resetErreur()
        {
            tblErreurNom.Text = string.Empty;
            tblErreurType.Text = string.Empty;
            tblErreurPlace.Text = string.Empty;
            tblErreurHeure.Text = string.Empty;
            tblErreurDate.Text = string.Empty;
        }

        private void cbx_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = cbx_type.SelectedItem.ToString();
            SingletonBD.getInstance().getActivitesForType(type);
            if (SingletonBD.getInstance().ListeActiviteForType.Count > 0)
            {
                cbx_nom.PlaceholderText = "";
            }
            else
            {
                cbx_nom.PlaceholderText = "Aucune activité de ce type";
            }
        }

        private bool isNumber(string text)
        {
            try
            {
                int i = Convert.ToInt32(text);
                if (i < 0 || i == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void tbx_place_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            tblErreurPlace.Text = string.Empty;
            if (!isNumber(sender.Text))
            {
                tblErreurPlace.Text = "Vous devez entrer un nombre entier positif valide";
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
            this.Frame.Navigate(typeof(SeanceAdminPage));
        }
    }
}
