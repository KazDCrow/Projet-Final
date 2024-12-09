using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class SingletonTypeActivite
    {
        ObservableCollection<string> listeTypes = new ObservableCollection<string>{ "Sport", "Bien-être", "Musique", "Culture", "Loisirs", "Art", "Spectacle", "Aventure", "Gastronomie", "Tourisme", "Technologie", "Éducation", "Écologie", "Photographie", "Mode", "Santé", "Jeux", "Architecture", "Histoire", "Innovation" };
        
        public ObservableCollection<string> ListeTypes { get { return listeTypes; } }

        static SingletonTypeActivite instance = null;

        public static SingletonTypeActivite getInstance()
        {
            if (instance == null)
                instance = new SingletonTypeActivite();

            return instance;
        }

        public int getIndexOf(string str)
        {
           return listeTypes.IndexOf(str);
        }
    }
}
