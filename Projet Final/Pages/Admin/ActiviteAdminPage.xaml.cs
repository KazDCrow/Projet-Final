using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Projet_Final.Classes;
using Projet_Final.Dialogs;
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
    public sealed partial class ActiviteAdminPage : Page
    {
        public ActiviteAdminPage()
        {
            this.InitializeComponent();
            SingletonBD.getInstance().getActivites();
            gv_liste_activites.ItemsSource = SingletonBD.getInstance().ListeActivite;
        }

        private async void btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Activite a = (Activite)btn.DataContext;

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = mainGrid.XamlRoot;
            dialog.Title = "Attention";
            dialog.PrimaryButtonText = "Oui";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "Voulez-vous supprimer cet activité?";

            var resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary) //si on clique sur OUI
            {
                SingletonBD.getInstance().supprimerActivite(a.Nom, a.Type);
                ContentDialog dialog2 = new ContentDialog();
                dialog2.XamlRoot = mainGrid.XamlRoot;
                dialog2.Title = "Suppression réalisé";
                dialog2.IsPrimaryButtonEnabled = false;
                dialog2.CloseButtonText = "Fermer";
                dialog2.DefaultButton = ContentDialogButton.Primary;
                dialog2.Content = "La suppression a été effectué.";
                resultat = await dialog2.ShowAsync();
            }
        }

        private void gv_liste_activites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gv_liste_activites.SelectedIndex >= 0)
            {
                this.Frame.Navigate(typeof(ModificationActivite), gv_liste_activites.SelectedIndex);
            }
        }

        private async void btn_ajouter_Click(object sender, RoutedEventArgs e)
        {
            DialogueAjoutActivite dialogue = new DialogueAjoutActivite();
            dialogue.XamlRoot = mainGrid.XamlRoot;
            dialogue.Title = "Nouvelle activité";
            dialogue.PrimaryButtonText = "Ajouter";
            dialogue.CloseButtonText = "Annuler";
            dialogue.DefaultButton = ContentDialogButton.Close;

            var resultat = await dialogue.ShowAsync();
        }

        private async void btn_exporter_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(SingletonMainwindow.getInstance().mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "activites";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            //écrit dans le fichier chacune des lignes du tableau
            //une boucle sera faite sur la collection et prendra chacun des objets de celle-ci
            await Windows.Storage.FileIO.WriteLinesAsync(monFichier, SingletonBD.getInstance().ListeActivite.ToList().ConvertAll(x => x.ToString()));

        }
    }
}
