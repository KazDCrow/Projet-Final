 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class Adherent
    {
        private string? id;
        private string nom;
        private string prenom;
        private string adresse;
        private DateTime date_naissance;
        private int age;
        private string mot_passe;

        public Adherent(string id, string nom, string prenom, string adresse, DateTime date_naissance, int age, string mot_passe)
        {
            Console.WriteLine("l'id est :");
            Console.WriteLine(id);
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.date_naissance = date_naissance;
            this.age = age;
            this.mot_passe = mot_passe;
        }

        public Adherent(string nom, string prenom, string adresse, DateTime date_naissance, string mot_passe)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.date_naissance = date_naissance ;
            this.mot_passe=mot_passe;
        }

        public string Id { get { return id??""; } set { id = value; } }
        public string Nom { get { return nom; } set { nom = value; } }
        public string Prenom { get {return prenom; } set { prenom = value; } }
        public string Adresse { get { return adresse; } set { adresse = value; } }
        public string Date_naissanceStr { get { return date_naissance.ToString("yyyy-MM-dd"); }}
        public DateTime Date_naissance { get { return date_naissance; } set { date_naissance = value; } }
        public int Age { get { return age; } set { age = value; } }
        public string AgeStr {  get { return age + " ans"; } }
        public string Mot_passe { get {return mot_passe; } set { mot_passe = value; } }

        public override string? ToString()
        {
            return id + ";" + nom + ";" + prenom + ";" + adresse + ";" + date_naissance.ToString("yyyy-MM-dd") + ";" + age + ";" + mot_passe;
        }
    }
}
