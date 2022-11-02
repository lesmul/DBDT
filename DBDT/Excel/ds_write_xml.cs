using DBDT.Excel.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDT.Excel
{
    internal class ds_write_xml
    {
        public static bool write_ds_xml(string nazwa_plik_xml, DataSetXML ds_d)
        {
            if (nazwa_plik_xml == "") nazwa_plik_xml = "_auto";

            nazwa_plik_xml = nazwa_plik_xml.Replace("/", "_");

            string ScieszkaProgramu;

            ScieszkaProgramu = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (ScieszkaProgramu.Trim().EndsWith(@"\") == false)
            {
                ScieszkaProgramu += @"\";
            }

            ScieszkaProgramu += @"\xml\";
            ds_d.WriteXmlSchema(ScieszkaProgramu + nazwa_plik_xml + ".xsd");
            ds_d.WriteXml(ScieszkaProgramu + nazwa_plik_xml + ".xml");

            return true;
        }
    }
}
