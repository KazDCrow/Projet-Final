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
        ObservableCollection<Activite> listeActivite;
        ObservableCollection<Seance> listeSeance;
        ObservableCollection<string> listeActiviteForType;
        ObservableCollection<string> listeNomActivites;

        bool connecter;
        string id_utilistaeur = "";
        string nom_utilisateur = "";
        string type_utilisateur = "";

        static SingletonBD instance = null;
        MySqlConnection con;

        internal ObservableCollection<Adherent> ListeAdherent { get => listeAdherent; }
        internal ObservableCollection<Activite> ListeActivite { get => listeActivite; }
        internal ObservableCollection<Seance> ListeSeance { get => listeSeance; }
        internal ObservableCollection<string> ListeActiviteForType { get => listeActiviteForType; }
        internal ObservableCollection<string> ListeNomActivites { get => listeNomActivites; }

        internal bool Connecter { get => connecter; }
        internal string Nom_utilisateur { get => nom_utilisateur; }
        internal string Type_utilisateur { get => type_utilisateur; }

        public SingletonBD()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_gr2_1642618-jacob-rouleau;Uid=1642618;Pwd=1642618");
            listeAdherent = new ObservableCollection<Adherent>();
            listeActivite = new ObservableCollection<Activite>();
            listeSeance = new ObservableCollection<Seance>();
            listeActiviteForType = new ObservableCollection<string>();
            listeNomActivites = new ObservableCollection<string>();
            connecter = false;
            getAdherents();
            getActivites();
            getSeances();
        }

        public static SingletonBD getInstance()
        {
            if (instance == null)
                instance = new SingletonBD();

            return instance;
        }

        public void getAdherents()
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

        public void getActivites()
        {
            listeActivite.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from activite";

            con.Open();
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read()) 
            {
                string nom = reader.GetString("nom");
                string type = reader.GetString ("type");
                double cout_organisation = reader.GetDouble("cout_organisation");
                double prix_vente = reader.GetDouble("prix_vente");

                Activite a = new Activite(nom,type,cout_organisation, prix_vente);
                listeActivite.Add(a);
            }
            reader.Close() ;
            con.Close();
        }

        public Activite getActivite(int id) { return listeActivite[id]; }

        public void getActivitesForType (string _type)
        {
            listeActiviteForType.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "Select nom from activite where type = @type order by nom";
            command.Parameters.AddWithValue("@type", _type);
            con.Open();
            command.Prepare();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string nom = reader.GetString("nom");
                listeActiviteForType.Add(nom);
            }
            reader.Close();
            con.Close();
        }

        public void getNomActivites()
        {
            listeNomActivites.Clear();
            getActivites();

            foreach (Activite a in listeActivite)
            {
                listeNomActivites.Add(a.Nom);
            }
        }

        public void emptyActiviteForType ()
        {
            listeActiviteForType.Clear(); 
        }

        public Seance getSeance(int id) { return listeSeance[id]; }

        public void getSeances()
        {
            listeSeance.Clear();
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "Select * from seance order by nom_activite ASC";
            con.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id_seance = reader.GetInt32("id");
                string nom = reader.GetString("nom_activite");
                string type = reader.GetString("type_activite");
                DateTime date = reader.GetDateTime("date");
                string heure = reader.GetString("heure");
                int nb_place = reader.GetInt32("nb_place");
                Double appeciation_general;
                try
                {
                    appeciation_general = reader.GetDouble("appreciation_general");
                }
                catch (Exception)
                {
                    appeciation_general = 0;
                }
                Seance s = new Seance(id_seance, type, nom, date, heure, nb_place, appeciation_general);
                listeSeance.Add(s);
            }
            reader.Close();
            con.Close();
        }

        public void getSeanceAtivite(int id)
        {
            Activite a = getActivite(id);

            if (a != null)
            {
                listeSeance.Clear();
                MySqlCommand command = new MySqlCommand();
                command.Connection = con;
                command.CommandText = "select * from seance s left join adherent_seance a_s on s.id = a_s.id_seance and a_s.id_adherent = @id where nom_activite = @nom and type_activite = @type and id_adherent is null";
                command.Parameters.AddWithValue("@nom", a.Nom);
                command.Parameters.AddWithValue("@type", a.Type);
                command.Parameters.AddWithValue("@id", id_utilistaeur);
                con.Open();
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id_seance = reader.GetInt32("id");
                    string nom = reader.GetString("nom_activite");
                    string type = reader.GetString("type_activite");
                    DateTime date = reader.GetDateTime("date");
                    string heure = reader.GetString("heure");
                    int nb_place = reader.GetInt32("nb_place");
                    Double appeciation_general;
                    try
                    {
                        appeciation_general = reader.GetDouble("appreciation_general");
                    }
                    catch (Exception)
                    {
                        appeciation_general = 0;
                    }
                    Seance s = new Seance(id_seance, type, nom, date, heure, nb_place, appeciation_general);
                    listeSeance.Add(s);
                }

                reader.Close();
                con.Close();
            }
        }

        public void getSeanceForAdherent()
        {
            ListeSeance.Clear();

            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "select * from seance s inner join adherent_seance a_s on id_seance = id where id_adherent = @id";
            command.Parameters.AddWithValue("@id", id_utilistaeur);

            con.Open();
            command.Prepare();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id_seance = reader.GetInt32("id");
                string nom = reader.GetString("nom_activite");
                string type = reader.GetString("type_activite");
                DateTime date = reader.GetDateTime("date");
                string heure = reader.GetString("heure");
                int nb_place = reader.GetInt32("nb_place");
                Double appeciation_general;
                try
                {
                    appeciation_general = reader.GetDouble("appreciation_general");
                }
                catch (Exception)
                {
                    appeciation_general = 0;
                }
                Seance s = new Seance(id_seance, type, nom, date, heure, nb_place, appeciation_general);
                listeSeance.Add(s);
            }
            reader.Close();
            con.Close() ;          
        }

        public bool connection(string id, string mot_passe)
        {
            foreach (Adherent a in listeAdherent)
            {
                if (a.Id == id && a.Mot_passe == mot_passe)
                {
                    connecter = true;
                    id_utilistaeur = a.Id;
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
            id_utilistaeur = string.Empty;
        }

        public void inscriptionSeance(int id_seance)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "insert into adherent_seance (id_adherent, id_seance) value (@adherent, @seance)";
            command.Parameters.AddWithValue("@adherent", id_utilistaeur);
            command.Parameters.AddWithValue("@seance", id_seance);

            con.Open();
            command.Prepare();
            command.ExecuteNonQuery();
            con.Close();
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

            getAdherents();
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

        public void ajouterActivite(Activite a)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Insert into activite (nom,type,cout_organisation, prix_vente) value (@nom, @type, @cout_organisation, @prix_vente)";
            commande.Parameters.AddWithValue("@nom", a.Nom);
            commande.Parameters.AddWithValue("@type", a.Type);
            commande.Parameters.AddWithValue("@cout_organisation", a.Cout_organisation);
            commande.Parameters.AddWithValue("@prix_vente", a.Prix_vente);

            con.Open();
            commande.Prepare();
            commande.ExecuteNonQuery();
            con.Close();

            getActivites();
        }

        public void ajouterSeance(Seance s)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Insert into seance (nom_activite,type_activite,date, heure, nb_place) values (@nom, @type, @date, @heure, @nb_place)";
            commande.Parameters.AddWithValue("@nom", s.Nom_activite);
            commande.Parameters.AddWithValue("@type", s.Type_activite);
            commande.Parameters.AddWithValue("@heure", s.Heure);
            commande.Parameters.AddWithValue("@date", s.Date);
            commande.Parameters.AddWithValue("@nb_place", s.Nb_place);

            con.Open();
            commande.Prepare();
            commande.ExecuteNonQuery();
            con.Close();

            getSeances();
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
            getAdherents();
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

        public void supprimerActivite(string _nom, string _type)
        {
            //Suppression des association adherent et seance
            List<Seance> listeSeances = getListSeanceOfActivite(_nom, _type);
            foreach (Seance s in listeSeances)
            {
                con.Open();
                MySqlCommand commandeDeleteAdherent_seance = new MySqlCommand();
                commandeDeleteAdherent_seance.Connection = con;
                commandeDeleteAdherent_seance.CommandText = "delete from adherent_seance where id_seance = @id";
                commandeDeleteAdherent_seance.Parameters.AddWithValue("@id", s.Id);
                commandeDeleteAdherent_seance.Prepare();
                commandeDeleteAdherent_seance.ExecuteNonQuery();
                con.Close();
            }
            //Suppresion des séances
            MySqlCommand commandeDeleteSeance = new MySqlCommand();
            commandeDeleteSeance.Connection = con;
            commandeDeleteSeance.CommandText = "delete from seance where nom_activite = @nom AND type_activite = @type";
            commandeDeleteSeance.Parameters.AddWithValue("@nom", _nom);
            commandeDeleteSeance.Parameters.AddWithValue("@type", _type);
            con.Open();
            commandeDeleteSeance.Prepare();
            commandeDeleteSeance.ExecuteNonQuery();
            con.Close();

            //Suppression de l'activité
            MySqlCommand commandDeleteActivite = new MySqlCommand();
            commandDeleteActivite.Connection = con;
            commandDeleteActivite.CommandText = "delete from activite where nom = @nom AND type = @type";
            commandDeleteActivite.Parameters.AddWithValue("@nom", _nom);
            commandDeleteActivite.Parameters.AddWithValue("@type", _type);
            con.Open();
            commandDeleteActivite.Prepare();
            commandDeleteActivite.ExecuteNonQuery();
            con.Close();
            getActivites();
        }

        public void supprimerSeance(int _id)
        {
            con.Open();
            MySqlCommand commandeDeleteAdherent_seance = new MySqlCommand();
            commandeDeleteAdherent_seance.Connection = con;
            commandeDeleteAdherent_seance.CommandText = "delete from adherent_seance where id_seance = @id";
            commandeDeleteAdherent_seance.Parameters.AddWithValue("@id", _id);
            commandeDeleteAdherent_seance.Prepare();
            commandeDeleteAdherent_seance.ExecuteNonQuery();
            con.Close();

            MySqlCommand commandeDeleteSeance = new MySqlCommand();
            commandeDeleteSeance.Connection = con;
            commandeDeleteSeance.CommandText = "delete from seance where id = @id";
            commandeDeleteSeance.Parameters.AddWithValue("@id", _id);
            con.Open();
            commandeDeleteSeance.Prepare();
            commandeDeleteSeance.ExecuteNonQuery();
            con.Close();
            getSeances();
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
            getAdherents();
        }

        public void modifierActivite(Activite a, string _oldNomm, string _oldType)
        {
            //Suppression des association adherent et seance
            List<Seance> listeSeances = getListSeanceOfActivite(_oldNomm, _oldType);
            foreach (Seance s in listeSeances)
            {
                con.Open();
                MySqlCommand commandeDeleteAdherent_seance = new MySqlCommand();
                commandeDeleteAdherent_seance.Connection = con;
                commandeDeleteAdherent_seance.CommandText = "delete from adherent_seance where id_seance = @id";
                commandeDeleteAdherent_seance.Parameters.AddWithValue("@id", s.Id);
                commandeDeleteAdherent_seance.Prepare();
                commandeDeleteAdherent_seance.ExecuteNonQuery();
                con.Close();
            }

            //Suppresion des séances
            MySqlCommand commandeDeleteSeance = new MySqlCommand();
            commandeDeleteSeance.Connection = con;
            commandeDeleteSeance.CommandText = "delete from seance where nom_activite = @nom AND type_activite = @type";
            commandeDeleteSeance.Parameters.AddWithValue("@nom", _oldNomm);
            commandeDeleteSeance.Parameters.AddWithValue("@type", _oldType);

            //supression de l'ancienne activité
            MySqlCommand commandDeleteActivite = new MySqlCommand();
            commandDeleteActivite.Connection = con;
            commandDeleteActivite.CommandText = "delete from activite where nom = @nom AND type = @type";
            commandDeleteActivite.Parameters.AddWithValue("@nom", _oldNomm);
            commandDeleteActivite.Parameters.AddWithValue("@type", _oldType);

            //Ajout de l'activité modifié
            MySqlCommand commandeAddModActivite = new MySqlCommand();
            commandeAddModActivite.Connection = con;
            commandeAddModActivite.CommandText = "Insert into activite (nom,type,cout_organisation, prix_vente) value (@nom, @type, @cout_organisation, @prix_vente)";
            commandeAddModActivite.Parameters.AddWithValue("@nom", a.Nom);
            commandeAddModActivite.Parameters.AddWithValue("@type", a.Type);
            commandeAddModActivite.Parameters.AddWithValue("@cout_organisation", a.Cout_organisation);
            commandeAddModActivite.Parameters.AddWithValue("@prix_vente", a.Prix_vente);

            con.Open();
            commandeDeleteSeance.Prepare();
            commandeDeleteSeance.ExecuteNonQuery();
            con.Close();

            con.Open();
            commandDeleteActivite.Prepare();
            commandDeleteActivite.ExecuteNonQuery();
            con.Close();

            con.Open();
            commandeAddModActivite.Prepare();
            commandeAddModActivite.ExecuteNonQuery();
            con.Close();
            getActivites();
        }

        public void modifierSeance(Seance s)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "update seance set type_activite = @type, nom_activite = @nom, date = @date, heure = @heure, nb_place = @place where id = @id";
            command.Parameters.AddWithValue("@nom", s.Nom_activite);
            command.Parameters.AddWithValue("@type", s.Type_activite);
            command.Parameters.AddWithValue("@date", s.Date);
            command.Parameters.AddWithValue("@heure", s.Heure);
            command.Parameters.AddWithValue("@place", s.Nb_place);
            command.Parameters.AddWithValue("@id", s.Id);
            con.Open();
            command.Prepare();
            command.ExecuteNonQuery();
            con.Close();
            getSeances();
        }

        private List<Seance> getListSeanceOfActivite(string _nom, string _type) 
        {
            //On stock les séances
            List<Seance> listeSeances = new List<Seance>();
            MySqlCommand commande1 = new MySqlCommand();
            commande1.Connection = con;
            commande1.CommandText = "select * from seance where nom_activite = @nom AND type_activite = @type";
            commande1.Parameters.AddWithValue("@nom", _nom);
            commande1.Parameters.AddWithValue("@type", _type);

            con.Open();
            commande1.Prepare();
            MySqlDataReader reader = commande1.ExecuteReader();
            while (reader.Read())
            {
                int id_seance = reader.GetInt32("id");
                DateTime date = reader.GetDateTime("date");
                string heure = reader.GetString("heure");
                int nb_place = reader.GetInt32("nb_place");
                Double appeciation_general = reader.GetDouble("appreciation_general");
                Seance s = new Seance(id_seance, _type, _nom, date, heure, nb_place, appeciation_general);
                listeSeances.Add(s);
            }
            reader.Close();
            con.Close();
            return listeSeances;
        }

        public int getTotalParticipant()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "call p_get_total_participant()";
            con.Open();
            MySqlDataReader reader = command.ExecuteReader();
            int total = 0;
            while (reader.Read())
            {
                total = reader.GetInt32("total_participant");
            }
            reader.Close() ;
            con.Close();
            return total;
        }

        public int getTotalActivite()
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = con;
            command.CommandText = "select count(*) as total from activite";
            con.Open();
            MySqlDataReader reader = command.ExecuteReader();
            int total = 0;
            while (reader.Read())
            {
                total = reader.GetInt32("total");
            }
            reader.Close();
            con.Close();
            return total;
        }

        public List<Activite> getTotalAdherentByActivite()
        {
            List<Activite> activites = new List<Activite>();
            MySqlCommand command = new MySqlCommand(); 
            command.Connection = con;
            command.CommandText = "select a.nom, a.type, count(a_s.id_adherent) as nb_adherent, count(a_s.id_seance) as nb_seance, round((sum(a_s.appreciation)/count(a_s.appreciation)),2) as note from activite a left join seance s on nom = nom_activite and type = type_activite left join adherent_seance a_s on s.id = a_s.id_seance group by a.nom, a.type order by a.nom,a.type";
            con.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string nom = reader.GetString("nom");
                string type = reader.GetString("type");
                int nb_adherent = reader.GetInt32("nb_adherent");
                int nb_seance = reader.GetInt32("nb_seance");
                double appreciation;
                if (!reader.IsDBNull(reader.GetOrdinal("note")))
                {
                    appreciation = reader.GetDouble("note");
                }
                else
                {
                    appreciation = 0;    
                }
                Activite a = new Activite(nom,type,nb_adherent,nb_seance,appreciation);
                activites.Add(a);
            }
            reader.Close();
            con.Close();
            return activites;
        }

        public void desinscriptionSeance(int id_seance)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection= con;
            command.CommandText = "delete from adherent_seance where id_seance = @id_seance and id_adherent = @id_adherent";
            command.Parameters.AddWithValue("@id_seance", id_seance);
            command.Parameters.AddWithValue("@id_adherent", id_utilistaeur);

            con.Open();
            command.Prepare();
            command.ExecuteNonQuery();
            con.Close();
        }
    }
}
