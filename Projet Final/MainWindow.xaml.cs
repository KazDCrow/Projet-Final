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
using Projet_Final.Pages.User;

namespace Projet_Final
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            SingletonBD.getInstance();
            SingletonNavigation.getInstance().NavigationView = navView;
            SingletonMainwindow.getInstance().mainWindow = this;
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

            switch (item?.Name)
            {
                case "adminNav_iAdherent":
                    mainFrame.Navigate(typeof(AdherentPage));
                    break;
                case "adminNav_iSeance":
                    mainFrame.Navigate(typeof(SeanceAdminPage));
                    break;

                case "adminNav_iActivite":
                    mainFrame.Navigate(typeof(ActiviteAdminPage));
                    break;

                case "adminNav_iStatitistique":
                    mainFrame.Navigate(typeof(StatistiquePage));
                    break;

                case "userNav_activite":
                    mainFrame.Navigate(typeof(ActiviteUserPage));
                    break;

                case "userNav_seance":
                    mainFrame.Navigate(typeof(SeanceUserPage));
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
                string pass = pb_pass.Password;
                bool connecter = SingletonBD.getInstance().connection(tb_id.Text, pb_pass.Password);
                
                if (connecter)
                {
                    tb_id.Text = string.Empty;
                    pb_pass.Password = string.Empty;
                    login_menu.Visibility = Visibility.Collapsed;
                    navView.Visibility = Visibility.Visible;
                    tb_userName.Text = SingletonBD.getInstance().Nom_utilisateur;
                    if (SingletonBD.getInstance().Type_utilisateur == "admin")
                    {
                        adminNav_header.Visibility = Visibility.Visible;
                        adminNav_iActivite.Visibility = Visibility.Visible;
                        adminNav_iAdherent.Visibility = Visibility.Visible;
                        adminNav_iSeance.Visibility = Visibility.Visible;
                        adminNav_iStatitistique.Visibility = Visibility.Visible;
                        mainFrame.Navigate(typeof(AdherentPage));
                        adminNav_iAdherent.IsSelected = true;
                    }
                    else
                    {
                        userNav_activite.Visibility = Visibility.Visible;
                        userNav_seance.Visibility = Visibility.Visible;
                        userNav_header.Visibility = Visibility.Visible;
                        mainFrame.Navigate(typeof(ActiviteUserPage));
                        userNav_activite.IsSelected = true;
                    }
                }
                else
                {
                    error_id.Text = "Identifiant ou mot de passe invalide";
                }
            }

        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            if (SingletonBD.getInstance().Type_utilisateur == "admin")
            {
                adminNav_header.Visibility = Visibility.Collapsed;
                adminNav_iActivite.Visibility = Visibility.Collapsed;
                adminNav_iAdherent.Visibility = Visibility.Collapsed;
                adminNav_iSeance.Visibility = Visibility.Collapsed;
                adminNav_iStatitistique.Visibility = Visibility.Collapsed;
            }
            else
            {
                userNav_activite.Visibility = Visibility.Collapsed;
                userNav_seance.Visibility= Visibility.Collapsed;
                userNav_header.Visibility = Visibility.Collapsed;
            }

            navView.Visibility = Visibility.Collapsed;
            tb_userName.Text = string.Empty;
            login_menu.Visibility = Visibility.Visible;
        }
    }
}
