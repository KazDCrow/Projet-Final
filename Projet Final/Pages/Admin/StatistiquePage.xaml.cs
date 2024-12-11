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

namespace Projet_Final.Pages.Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatistiquePage : Page
    {
        public StatistiquePage()
        {
            this.InitializeComponent();
            tbx_nb_adherent.Text = Convert.ToString(SingletonBD.getInstance().getTotalParticipant());
            tbx_nb_activite.Text = Convert.ToString(SingletonBD.getInstance().getTotalActivite());
            SingletonBD.getInstance().getActivites();
            lv_activite.ItemsSource = SingletonBD.getInstance().getTotalAdherentByActivite();
        }
    }
}
