using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class Adherent_Seance
    {
        private string id_adherent;
        private int id_seance;
        private double appreciation;

        public Adherent_Seance(string id_adherent, int id_seance, double appreciation)
        {
            this.id_adherent = id_adherent;
            this.id_seance = id_seance;
            this.appreciation = appreciation;
        }

        public string Id_adherent { get { return id_adherent; } set { id_adherent = value; } }
        public int Id_seance { get { return id_seance; } set {id_seance = value; } }
        public double Appreciation { get { return appreciation; } set { appreciation = value; } }
    }
}
