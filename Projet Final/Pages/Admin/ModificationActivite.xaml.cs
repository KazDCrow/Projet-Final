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
        public ModificationActivite()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not null)
            {
                SingletonNavigation.getInstance().NavigationView.SelectedItem = null;

                var index = (int)e.Parameter;
                Activite a = SingletonBD.getInstance().getActivite(index);
                tbl_nom.Text = a.Nom;
                tbx_nom.Text = a.Nom;
                tbx_type.Text = a.Type;
                tbx_cout.Text = Convert.ToString(a.Cout_organisation);
                tbx_vente.Text = Convert.ToString(a.Prix_vente);
            }
        }

        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
