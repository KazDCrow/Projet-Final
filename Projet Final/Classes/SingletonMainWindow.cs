using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class SingletonMainWindow
    {
        static SingletonMainWindow instance = null;
        Frame mainFrame;
        internal Frame MainFrame { get => mainFrame; set=> mainFrame = value; }

        public SingletonMainWindow()
        {

        }

        public static SingletonMainWindow getInstance()
        {
            if (instance == null)
                instance = new SingletonMainWindow();

            return instance;
        }

    }
}
