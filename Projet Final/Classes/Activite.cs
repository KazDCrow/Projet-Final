using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class Activite
    {
        private string nom;
        private string type;
        private double cout_organisation;
        private double prix_vente;

        public Activite(string nom, string type, double cout_organisation, double prix_vente)
        {
            this.nom = nom;
            this.type = type;
            this.cout_organisation = cout_organisation;
            this.prix_vente = prix_vente;
        }

        public string Nom { get { return nom; } set { nom = value; } }
        public string Type { get { return type; } set { type = value; } }
        public double Cout_organisation { get { return cout_organisation; } set { cout_organisation = value; } }
        public string Cout_organisation_str { get { return cout_organisation + " $"; } }
        public double Prix_vente { get { return prix_vente; } set { prix_vente = value; } }
        public string Prix_vente_str { get { return prix_vente + " $"; } }

    }
}
