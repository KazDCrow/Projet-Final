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

namespace Projet_Final.Pages.User
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ActiviteUserPage : Page
    {
        public ActiviteUserPage()
        {
            this.InitializeComponent();
            lv_seances.Visibility = Visibility.Collapsed;
            SingletonBD.getInstance().getNomActivites();
            cbx_nom_activite.ItemsSource = SingletonBD.getInstance().ListeNomActivites;
            lv_seances.ItemsSource = SingletonBD.getInstance().ListeSeance;
        }

        private async void btn_inscription_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Seance s = (Seance)btn.DataContext;

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.Title = "Attention";
            dialog.PrimaryButtonText = "Oui";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "Voulez-vous vous inscrire à cette activité?";

            var resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary) //si on clique sur OUI
            {
                SingletonBD.getInstance().inscriptionSeance(s.Id);
                ContentDialog dialog2 = new ContentDialog();
                dialog2.XamlRoot = mainGrid.XamlRoot;
                dialog2.Title = "Inscription réalisé";
                dialog2.IsPrimaryButtonEnabled = false;
                dialog2.CloseButtonText = "Fermer";
                dialog2.DefaultButton = ContentDialogButton.Primary;
                dialog2.Content = "L'inscription a été effectué.";
                resultat = await dialog2.ShowAsync();
            }
            int index = cbx_nom_activite.SelectedIndex;
            SingletonBD.getInstance().getSeanceAtivite(index);

        }

        private void cbx_nom_activite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbx_nom_activite.SelectedIndex;

            SingletonBD.getInstance().getSeanceAtivite(index);
            lv_seances.Visibility = Visibility.Visible;
        }
    }
}
