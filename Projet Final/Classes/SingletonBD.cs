using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    internal class SingletonBD
    {
        ObservableCollection<Adherent> listeAdherent;
        //ObservableCollection<Activite> listeActivite;
        //ObservableCollection<Seance> listeSeance;
        //ObservableCollection<Adherent_Seance> listeAdherentSeance;
        bool connecter;
        string nom_utilisateur = "";
        string type_utilisateur = "";

        static SingletonBD instance = null;
        MySqlConnection con;

        internal ObservableCollection<Adherent> ListeAdherent { get => listeAdherent; }
        internal bool Connecter { get => connecter; }
        internal string Nom_utilisateur { get => nom_utilisateur; }
        internal string Type_utilisateur { get => type_utilisateur; }

        public SingletonBD()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_gr2_1642618-jacob-rouleau;Uid=1642618;Pwd=1642618");
            listeAdherent = new ObservableCollection<Adherent>();
            connecter = false;
            getAdherent();
        }

        public static SingletonBD getInstance()
        {
            if (instance == null)
                instance = new SingletonBD();

            return instance;
        }

        public void getAdherent()
        {
            listeAdherent.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from adherent";

            con.Open();
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string id = reader.GetString("id");
                string nom = reader.GetString("nom");
                string prenom = reader.GetString("prenom");
                string adresse = reader.GetString("adresse");
                DateTime date_naissance = reader.GetDateTime("date_naissance");
                int age = reader.GetInt32("age");
                string mot_passe = reader.IsDBNull(reader.GetOrdinal("mot_passe")) ? "" : reader.GetString("mot_passe");

                Adherent a = new Adherent(id, nom, prenom, adresse, date_naissance, age, mot_passe);
                listeAdherent.Add(a);
            }
            reader.Close();
            con.Close();
        }

        public bool connection(string id, string mot_passe)
        {
            foreach (Adherent a in listeAdherent)
            {
                if (a.Id == id && a.Mot_passe == mot_passe)
                {
                    connecter = true;
                    nom_utilisateur = a.Prenom + " " + a.Nom;
                    if (a.Mot_passe != "" && a.Mot_passe != null)
                    {
                        type_utilisateur = "admin";
                    }
                    else
                    {
                        type_utilisateur = "user";
                    }
                    break;
                }
            }
            return connecter;
        }

        public void deconnection()
        {
            connecter = false;
            nom_utilisateur = string.Empty;
            type_utilisateur = string.Empty;
        }

        public void ajouterAdherent(Adherent a)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Insert into adherent (nom,prenom,adresse, date_naissance, mot_passe) value (@nom, @prenom, @adresse, @date_naissance, @mot_passe)";
            commande.Parameters.AddWithValue("@nom", a.Nom);
            commande.Parameters.AddWithValue("@prenom", a.Prenom);
            commande.Parameters.AddWithValue("@adresse", a.Adresse);
            commande.Parameters.AddWithValue("@date_naissance", a.Date_naissance);
            if (a.Mot_passe != "")
            {
                commande.Parameters.AddWithValue("@mot_passe", a.Mot_passe);
            }
            else 
            {
                commande.Parameters.AddWithValue("@mot_passe", null);
            }
            
            con.Open();
            commande.Prepare();
            commande.ExecuteNonQuery();
            con.Close();

            getAdherent();
        }

        public void supprimerAdherent(string _id)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select f_delete_adherent(@id)";
            commande.Parameters.AddWithValue("@id", _id);
            
            con.Open(); 
            commande.Prepare();
            commande.ExecuteNonQuery();
            con.Close();
            getAdherent();
        }
    }
}
