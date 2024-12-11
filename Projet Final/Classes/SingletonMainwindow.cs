using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    class SingletonMainwindow
    {
        static SingletonMainwindow instance = null;
        public MainWindow mainWindow { get; set; }

        public SingletonMainwindow()
        {

        }

        public static SingletonMainwindow getInstance()
        {
            if (instance == null)
                instance = new SingletonMainwindow();

            return instance;
        }
    }
}
