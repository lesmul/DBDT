using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDT.USTAWIENIA_PROGRAMU
{
    internal class LadujIni
    {

        public void LADUJ_USTAWIENIA_INI()
        {
            string ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }


            System.IO.FileInfo fiA = new System.IO.FileInfo(ScieszkaProgramu + "ustawienia.ini");

            if (fiA.Exists == true)
            {
                System.IO.StreamReader srA = new System.IO.StreamReader(fiA.FullName, System.Text.Encoding.Default);
                string STR_UZYTKOWNIK = srA.ReadLine();

                srA.Close();
                srA.Dispose();
            }
        }

        public void Laduj_SQLLite()
        {
            if (_PUBLIC_SqlLite.Existsdb("") == false)
            {
                MessageBox.Show("Utworzono nową bazę danych", "Informacja");
            }

            DataTable td = new DataTable();
            td = _PUBLIC_SqlLite.SelectQuery("SELECT * FROM `ParametryPalaczenia`");
            if (td == null)
            {
                return;
            }
        }

    };
}
