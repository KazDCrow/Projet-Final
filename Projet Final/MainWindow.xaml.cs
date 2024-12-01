using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Projet_Final.Pages;
using Projet_Final.Classes;
using Projet_Final.Pages.Admin;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Projet_Final
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            SingletonBD.getInstance();
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item;

            try
            {
                item = (NavigationViewItem)args.SelectedItem;
            }
            catch (Exception)
            {
                item = new NavigationViewItem();
            }

            switch (item.Name)
            {
                case "adminNav_iAdherent":
                    mainFrame.Navigate(typeof(AdherentPage));
                    break;
                case "adminNav_iSeance":
                    mainFrame.Navigate(typeof(TestAdherentPage));
                    break;

                case "adminNav_iActivite":
                    mainFrame.Navigate(typeof(TestAdherentPage));
                    break;

                case "adminNav_iStatitistique":
                    mainFrame.Navigate(typeof(TestAdherentPage));
                    break;

                default:
                    break;
            }
        }

        private void btn_connection_Click(object sender, RoutedEventArgs e)
        {
            error_id.Text = string.Empty;
            if (tb_id.Text == "")
            {
                error_id.Text = "L'identifiant ne peut être vide";
            }
            else
            {
                bool connecter = SingletonBD.getInstance().connection(tb_id.Text, tb_pass.Text);
                if (connecter)
                {
                    navView.Visibility = Visibility.Visible;
                    adminNav_header.Visibility = Visibility.Visible;
                    adminNav_iActivite.Visibility = Visibility.Visible;
                    adminNav_iAdherent.Visibility = Visibility.Visible;
                    adminNav_iSeance.Visibility = Visibility.Visible;
                    adminNav_iStatitistique.Visibility = Visibility.Visible;
                    tb_userName.Text = SingletonBD.getInstance().Nom_utilisateur;
                    mainFrame.Navigate(typeof(AdherentPage));
                    adminNav_iAdherent.IsSelected = true;
                    login_menu.Visibility = Visibility.Collapsed;
                }
            }

        }
    }
}
