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

        public Adherent getAdherent(int id) {  return listeAdherent[id]; }

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

        public string ajouterAdherentReturnID(Adherent a)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select f_insert_adherent_return_id(@prenom, @nom, @adresse, @date_naissance, @mot_passe)";
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
            string id = (string) commande.ExecuteScalar();
            con.Close();
            return id;
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

        public void supprimerAdherentWithoutGet(string _id)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select f_delete_adherent(@id)";
            commande.Parameters.AddWithValue("@id", _id);

            con.Open();
            commande.Prepare();
            commande.ExecuteNonQuery();
            con.Close();
        }

        public void modifierAdherent(string _id, Adherent a)
        {
            //On stock les séances
            List<Adherent_Seance> seances = new List<Adherent_Seance>();
            MySqlCommand commande1 = new MySqlCommand();
            commande1.Connection = con;
            commande1.CommandText = "select * from adherent_seance where id_adherent = @id";
            commande1.Parameters.AddWithValue("@id", _id);
            con.Open();
            commande1.Prepare();
            MySqlDataReader reader = commande1.ExecuteReader();
            while (reader.Read())
            {
                int id_seance = reader.GetInt32("id_seance");
                Double appeciation = reader.GetDouble("appreciation");
                Adherent_Seance s = new Adherent_Seance(_id,id_seance, appeciation);
                seances.Add(s);
            }
            reader.Close();
            con.Close();

            //On efface les séances en liens avec l'usager
            MySqlCommand commandeDelete = new MySqlCommand();
            commandeDelete.Connection = con;
            commandeDelete.CommandText = "delete from adherent_seance where id_adherent = @id";
            commandeDelete.Parameters.AddWithValue("@id", _id);
            con.Open();
            commandeDelete.Prepare();
            commandeDelete.ExecuteNonQuery();
            con.Close();

            //On efface l'usager
            supprimerAdherentWithoutGet(_id);

            //On insère l'usager modifier
            var newId = ajouterAdherentReturnID(a);

            //On remet les seances auquel participe l'usager
            foreach (Adherent_Seance s in seances)
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "insert into adherent_seance (id_adherent, id_seance, appreciation) value (@id_a, @id_s, @appreciation)";
                commande.Parameters.AddWithValue("@id_a", newId);
                commande.Parameters.AddWithValue("@id_s", s.Id_seance);
                commande.Parameters.AddWithValue("@appreciation", s.Appreciation);
                con.Open() ;
                commande.Prepare();
                commande.ExecuteNonQuery();
                con.Close();
            }
            getAdherent();
        }
    }
}
