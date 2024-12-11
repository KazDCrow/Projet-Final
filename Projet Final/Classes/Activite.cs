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
        private int nb_adherent;
        private int nb_seance;
        private double appreciation;

        public Activite(string nom, string type, double cout_organisation, double prix_vente)
        {
            this.nom = nom;
            this.type = type;
            this.cout_organisation = cout_organisation;
            this.prix_vente = prix_vente;
        }

        public Activite(string nom, string type, int nb_adherent, int nb_seance, double appreciation)
        {
            this.nom = nom;
            this.type = type;
            this.nb_adherent = nb_adherent;
            this.nb_seance = nb_seance;
            this.appreciation = appreciation;
        }

        public string Nom { get { return nom; } set { nom = value; } }
        public string Type { get { return type; } set { type = value; } }
        public double Cout_organisation { get { return cout_organisation; } set { cout_organisation = value; } }
        public string Cout_organisation_str { get { return cout_organisation + " $"; } }
        public double Prix_vente { get { return prix_vente; } set { prix_vente = value; } }
        public string Prix_vente_str { get { return prix_vente + " $"; } }
        public int Nb_adherent { get { return nb_adherent; } set { nb_adherent = value; } }
        public int Nb_seance { get { return nb_seance; } set { nb_seance = value; } }
        public double Appreciation { get { return appreciation; } set { appreciation = value; } }

        public override string? ToString()
        {
            return nom + ";" + type + ";" + cout_organisation + ";" + prix_vente;
        }
    }
}
