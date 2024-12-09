using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Final.Classes
{
    class ValidationAdherent
    {
        public static bool validatePrenomNom(string nom)
        {
            if (!string.IsNullOrEmpty(nom.Trim()) && nom.Trim().Length <= 50 && nom.Trim().Length >=3)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public static bool validateAdresse(string adresse)
        {
            if (!string.IsNullOrEmpty(adresse.Trim()) && adresse.Trim().Length <= 255 && adresse.Trim().Length >= 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool validatePass(string pass) 
        {
            if (!string.IsNullOrEmpty(pass.Trim()) && pass.Trim().Length >= 4 && pass.Trim().Length < 20)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public static bool validateDate(DateTimeOffset? date)
        {
            if (!string.IsNullOrEmpty(date.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isNumber(char text)
        {
            try
            {
                int i = Convert.ToInt32(text);
                if (i < 0 || i == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool validateHeureSeance(string heure)
        {
            var liste = heure.ToCharArray();
            if (liste.Length < 5) { return false; }
            for (int i = 0; i < liste.Length; i++)
            {
                if (i == 2 && (liste[i] != 'h' && liste[i] != 'H'))
                {
                    return false;
                }
                else
                {
                    if (!isNumber(liste[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
