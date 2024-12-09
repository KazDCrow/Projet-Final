using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class Seance
    {
        private int id;
        private string type_activite;
        private string nom_activite;
        private DateTime date;
        private string heure;
        private int nb_place;
        private double appreciation_general;

        public Seance(int id, string type_activite, string nom_activite, DateTime date, string heure, int nb_place, double appreciation_general)
        {
            this.id = id;
            this.type_activite = type_activite;
            this.nom_activite = nom_activite;
            this.date = date;
            this.heure = heure;
            this.nb_place = nb_place;
            this.appreciation_general = appreciation_general;
        }

        public Seance(string type_activite, string nom_activite, DateTime date, string heure, int nb_place)
        {
            this.type_activite = type_activite;
            this.nom_activite = nom_activite;
            this.date = date;
            this.heure = heure;
            this.nb_place = nb_place;
        }

        public Seance(int id, string type_activite, string nom_activite, DateTime date, string heure, int nb_place)
        {
            this.id = id;
            this.type_activite = type_activite;
            this.nom_activite = nom_activite;
            this.date = date;
            this.heure = heure;
            this.nb_place = nb_place;
        }

        public int Id {  get { return id; } }
        public string Type_activite { get { return type_activite; } set { type_activite = value; } }
        public string Nom_activite { get { return nom_activite; } set { nom_activite = value; } }
        public DateTime Date { get { return date; } set { date = value; } }
        public string DateStr { get { return date.ToString("yyyy-mm-dd"); }}
        public string Heure { get { return heure; } set { heure = value; } }
        public int Nb_place { get { return nb_place; } set { nb_place = value; } }
        public double Appreciation_general { get { return appreciation_general; } set { appreciation_general = value; } }
    }
}
