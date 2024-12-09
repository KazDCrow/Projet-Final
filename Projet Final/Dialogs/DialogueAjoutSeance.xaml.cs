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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DialogueAjoutSeance : ContentDialog
    {
        bool valide;
        DateTime MinDateNaissance { get; set; }
        public DialogueAjoutSeance()
        {
            this.InitializeComponent();
            cbx_type.ItemsSource = SingletonTypeActivite.getInstance().ListeTypes;
            SingletonBD.getInstance().emptyActiviteForType();
            cbx_nom.ItemsSource = SingletonBD.getInstance().ListeActiviteForType;
            MinDateNaissance = DateTime.Now;
        }

        private void resetErreur()
        {
            tblErreurNom.Text = string.Empty;
            tblErreurType.Text = string.Empty;
            tblErreurPlace.Text = string.Empty;
            tblErreurHeure.Text = string.Empty;
            tblErreurDate.Text = string.Empty;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
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
                Seance s = new Seance(cbx_type.SelectedItem.ToString(), cbx_nom.SelectedItem.ToString(), calendar_date.Date.Value.DateTime, tbx_heure.Text, Convert.ToInt32(tbx_place.Text));
                SingletonBD.getInstance().ajouterSeance(s);
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
    }
}
