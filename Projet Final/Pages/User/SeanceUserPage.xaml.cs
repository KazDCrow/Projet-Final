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

namespace Projet_Final.Pages.User
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SeanceUserPage : Page
    {
        public SeanceUserPage()
        {
            this.InitializeComponent();
            SingletonBD.getInstance().getSeanceForAdherent();
            lv_liste_seances.ItemsSource = SingletonBD.getInstance().ListeSeance;
        }

        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Seance s = (Seance)btn.DataContext;

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.Title = "Attention";
            dialog.PrimaryButtonText = "Oui";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "Voulez-vous vraiment vous désinscrire de cette séance?";

            var resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary) //si on clique sur OUI
            {
                SingletonBD.getInstance().desinscriptionSeance(s.Id);
                ContentDialog dialog2 = new ContentDialog();
                dialog2.XamlRoot = mainGrid.XamlRoot;
                dialog2.Title = "Désinscription réalisé";
                dialog2.IsPrimaryButtonEnabled = false;
                dialog2.CloseButtonText = "Fermer";
                dialog2.DefaultButton = ContentDialogButton.Primary;
                dialog2.Content = "Vous n'êtes plus incrit a cette séance.";

                resultat = await dialog2.ShowAsync();
                SingletonBD.getInstance().getSeanceForAdherent();
            }
        }
    }
}
