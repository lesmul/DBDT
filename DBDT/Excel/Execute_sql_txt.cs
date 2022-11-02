using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DBDT.Excel
{
    internal class Execute_sql_txt
    {
        public static string data_exec_function(DataTable dt_data, DataTable dt_data_replace, string nazwa_f, string idv, string procedura_tlumaczen,
            bool tlumacz, string str_numerProjektu)
        {

            string[] strN = nazwa_f.Split('¶');

            for (int k = 0; k < strN.Length; k++)
            {

                nazwa_f = strN[k];

                try
                {
                    if (nazwa_f.IndexOf(" ") < 0 || nazwa_f.IndexOf("@") < 0) return "Brak danych! Skonfiguruj SQL np: nazwa_procedury @nazwa itp";

                    string[] str_x = nazwa_f.Substring(nazwa_f.IndexOf("@"), nazwa_f.Length - nazwa_f.IndexOf("@")).Split(',');
                    string str_tylko_nazwa = nazwa_f.Substring(0, nazwa_f.IndexOf("@")) + " ";
                    string str_r = "";

                    DataView dvx = new DataView();
                    dvx.Table = dt_data_replace.Copy();

                    DataView dvt = new DataView(dt_data);
                    //pole1 = NazwaZakladki --> TypParametru
                    //pole2 = TypDanych
                    //pole3 = TypParametru --> NazwaParametru
                    //pole4 = PolePowiazane
                    //pole5 = DodatkowOpcje

                    if (dt_data.Columns.Contains("TypParametru") == false)
                    {
                        return "Tabela musi zawierać kolumne z Typem Parametru!";
                    }             

                    var distinctRows_new_row = (from r in dt_data.AsEnumerable()
                    .Where(myRow => myRow.Field<string>("TypParametru").Trim().ToUpper() == "@")
                                                select r["id"]).Distinct().ToList();

                    if (idv == "") return "Brak danych! Brak powiązanego ID wpliku XML projektu";
                    if (nazwa_f.IndexOf('@') < 0) continue;

                    dvx.RowFilter = "id = " + idv;

                    for (int i = 0; i < str_x.Length; i++)
                    {
                        string str = str_x[i].ToString().Trim();

                        if (str != "")
                        {
                            dvt.RowFilter = "TypParametru = '" + str.Substring(0, 1) + "' AND NazwaParametru = '" + str.Substring(1, str.Length - 1) + "'";

                            if (dvt.Count > 0)
                            {
                                if (dvt[0]["TypParametru"].ToString() == "@" && dvx.Count > 0)
                                {
                                    if (dvt[0]["PolePowiazane"].ToString().Trim() == "Wartość z dodatkowych opcji")
                                    {
                                        str_x[i] = "'" + dvt[0]["DodatkowOpcje"].ToString().Trim() + "'";
                                    }
                                    else
                                    {
                                        if (dvx.Table.Columns.Contains(dvt[0]["PolePowiazane"].ToString().Trim()))
                                        {
                                            if (dvx.Table.Columns[dvt[0]["PolePowiazane"].ToString().Trim()].DataType.ToString() == "System.String")
                                            {
                                                str_x[i] = "N'" + dvx[0][dvt[0]["PolePowiazane"].ToString().Trim()] + "'";
                                            }
                                            else
                                            {
                                                str_x[i] = "" + dvx[0][dvt[0]["PolePowiazane"].ToString().Trim()] + "";
                                            }      
                                        }
                                        else
                                        {
                                            str_x[i] = "";
                                        }
                                    }
                                }
                            }
                        }

                    }

                    var distinctRowsCount = (from r in dt_data.AsEnumerable().Where(myRow => myRow.Field<string>("TypParametru").Trim().ToUpper() == "@")
                                             select r["id"]).Distinct().ToList();

                    if (distinctRowsCount.Count > 0)
                    {

                        str_r = str_tylko_nazwa;

                        for (int i = 0; i < str_x.Length; i++)
                        {
                            str_r += str_x[i] + ", ";
                        }
                        str_r = str_r.Trim();
                        str_r = str_r.TrimEnd(',');
                        str_r = str_r.Trim().TrimEnd(',') + ";";
                    }

                    str_r += "\r\n";

                    string szt_translate = "";

                    if (distinctRows_new_row.Count > 0)
                    {
                        for (int j = 0; j < distinctRows_new_row.Count; j++) // id = distinctRows_new_row - id wiersza z konfiguratora nazwa pola powiązan z nawą kolumny z xml
                        {

                            dvt.RowFilter = "id = " + distinctRows_new_row[j].ToString(); //Wyszukiwanie wiersz może mieć wartość 0 - brak lub 1

                            if (dvx.Table.Columns.Contains(dvt[0]["PolePowiazane"].ToString()) && tlumacz == true) //dvx - wiersz z danymy z pliku xml - dvx.nazwa kulumny = nazwa kolumny z PolePowiazane
                            {

                                DataColumn dtc = new DataColumn();
                                dtc = dvx.Table.Columns[dvt[0]["PolePowiazane"].ToString()];

                                List<Ncode> lista_transl = new List<Ncode>();
                                lista_transl.AddRange(data_exec_nr_kod_jezyka(dtc.Namespace, tlumacz));

                                szt_translate = zamien_zmienna_na_wartosc(dvt.Table, lista_transl, dvx, "@", 0, procedura_tlumaczen, tlumacz, str_numerProjektu);

                            }

                            str_r += szt_translate + "\r\n";

                            break;//może trzeba usunąć for.... po testach powiązne z chek w kolumnach

                        }

                    }
                    str_r = str_r.Trim();
                    str_r = str_r.Trim().TrimEnd(',').TrimEnd();
                    return str_r.Trim().TrimEnd(',').TrimEnd(';').TrimEnd(';').TrimEnd(';') + ";";

                }
                catch (Exception e)
                {
                    if (System.Environment.UserName == "Leszek Mularski")
                    {
                        return e.StackTrace;
                    }
                    else
                    {
                        return e.Message;
                    }

                }

            }

            return "Sprawdź dane!!!!";
        }


        public static string data_exec_procedura(DataTable dt_data, DataTable dt_data_replace, string nazwa_f, string idv, string procedura_tlumaczen,
            bool tlumacz, string numer_projetu)
        {
            string[] strN = nazwa_f.Split('¶');

            for (int k = 0; k < strN.Length; k++)
            {
                nazwa_f = strN[k].Trim().Replace("  ", " ").Replace("\"", "");

                try
                {
                    if (nazwa_f.IndexOf(" ") < 0 || nazwa_f.IndexOf("=") < 0) continue; //return "Brak danych! Skonfiguruj procedurę CommandText = dbo.up_dbdt_AddNewBaseCostam, lub brak końca danych ¶";

                    string stmp = nazwa_f.Substring(nazwa_f.IndexOf("="), nazwa_f.Length - nazwa_f.IndexOf("="));
                    stmp = stmp.Trim();
                    stmp = stmp.Substring(stmp.IndexOf(" ", 2), stmp.Length - stmp.IndexOf(" ", 2));

                    string[] str_x = stmp.Split(',');

                    int start = nazwa_f.IndexOf("=") + 1;
                    int stop = stmp.IndexOf(" ", start + 2);

                    string str_tylko_nazwa = nazwa_f.Substring(start, start + stop + 2) + "\r\n";

                    string str_r = "";

                    DataView dvx = new DataView();
                    dvx.Table = dt_data_replace.Copy();

                    DataView dvt = new DataView(dt_data);


                    var distinctRows_new_row = (from r in dt_data.AsEnumerable()
                   .Where(myRow => (myRow.Field<string>("TypParametru") == "adParamInput" || myRow.Field<string>("TypParametru") == "adParamOutput") &&
                   myRow.Field<string>("TypParametru").Trim().ToUpper() != "@")
                                                select r["id"]).Distinct().ToList();

                    if (idv == "") return "Brak danych! Brak powiązanego ID wpliku XML projektru";

                    dvx.RowFilter = "id = " + idv;

                    str_r = "";

                    for (int i = 0; i < str_x.Length; i++)
                    {
                        string str = str_x[i].ToString().Trim();

                        if (str != "")
                        {
                            dvt.RowFilter = "NazwaParametru = '" + str + "' and TypParametru not like '@%'";

                            if (dvt.Count > 0)
                            {
                                if (dvx.Count > 0)
                                {
                                    if (dvt[0]["PolePowiazane"].ToString().Trim() == "Wartość z dodatkowych opcji")
                                    {
                                        str_x[i] = dvt[0]["TypDanych"].ToString().Trim() + ", " + dvt[0]["TypParametru"].ToString().Trim() + ", " + str_x[i] + " = " + dvt[0]["DodatkowOpcje"].ToString().Trim() + "";
                                    }
                                    else
                                    {
                                        if (dvx.Table.Columns.Contains(dvt[0]["PolePowiazane"].ToString().Trim()))
                                        {
                                            str_x[i] = dvt[0]["TypDanych"].ToString().Trim() + ", " + dvt[0]["TypParametru"].ToString().Trim() + ", " + str_x[i] + " = " + (string)dvx[0][dvt[0]["PolePowiazane"].ToString().Trim()] + "";
                                        }
                                        else
                                        {
                                            if (dvt[0]["TypParametru"].ToString().Trim() == "adParamOutput")
                                            {
                                                str_x[i] = dvt[0]["TypDanych"].ToString().Trim() + ", " + dvt[0]["TypParametru"].ToString().Trim() + " = " + dvt[0]["NazwaParametru"].ToString().Trim();
                                            }
                                            else
                                            {
                                                str_x[i] = dvt[0]["TypDanych"].ToString().Trim() + ", " + dvt[0]["TypParametru"].ToString().Trim() + ", " + str_x[i] + " = [BRAK POWIAZANIA!!!!]";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                str_x[i] = "Popraw wiersz: " + i.ToString() + " nazwa pola [" + str_x[i].Trim() + "] Brak powiązanego pola" + " ! Sprawdź procedurę !  = " + nazwa_f;
                            }
                        }

                    }

                    for (int i = 0; i < str_x.Length; i++)
                    {
                        str_r += str_x[i] + "\r\n";
                    }

                    return str_tylko_nazwa.Trim() + "\r\n" + str_r.Trim().TrimEnd(',');
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

            return "Sprawdź dane!!!! Popraw wywołanie procedury...";
        }

        private static string zamien_zmienna_na_wartosc(DataTable dt, List<Ncode> lista_transl, DataView xmldata, string starchar, int id_rec_xml, string procedura_tlumaczen,
            bool tlumacz, string str_numerProjektu)
        {

            string onlyfun = procedura_tlumaczen.Trim();

            DataView dtv = new DataView(dt);

            dtv.RowFilter = "TypParametru = '@'";

            ArrayList arrayList = new ArrayList();

            for (int i = 0; i < dtv.Count; i++)
            {
                string str = dtv[i]["PolePowiazane"].ToString().Trim();

                if (xmldata.Table.Columns.Contains(str))
                {
                    if (dtv[i]["TypDanych"].ToString() == "Tłumacz")
                    {
                        if(xmldata.Table.Columns[str].DataType.ToString() == "System.String")
                        {
                            arrayList.Add("N'" + str_numerProjektu + " " + (string)xmldata[0][str] + "'");
                        }
                        else
                        {
                            if (xmldata.Table.Columns[str].DataType.ToString() == "System.String")
                            {
                               arrayList.Add("'" + str_numerProjektu + " " + (string)xmldata[0][str] + "'");
                            }
                            else
                            {
                               arrayList.Add("'" + str_numerProjektu + " " + (string)xmldata[0][str] + "'");
                            }  
                        }
                    }
                    else if (dtv[i]["TypDanych"].ToString() == "Tłumacz bez nr projektu")
                    {
                        if (xmldata.Table.Columns[str].DataType.ToString() == "System.String")
                        {
                          arrayList.Add("N'" + (string)xmldata[0][str] + "'");
                        }
                        else
                        {
                          arrayList.Add("'" + (string)xmldata[0][str] + "'");
                        }   
                    }
                }
                else if (dtv[i]["TypDanych"].ToString() == "Kod języka")
                {
                    arrayList.Add("@KOD$");
                }

            }

            string s1 = string.Join(",", arrayList.ToArray());
            string strzw = "";

            var filteredtransl = lista_transl.Where(lista => new[] { true }.Any(s => s == lista.UseCode));

            foreach (Ncode item in filteredtransl)
            {
                strzw += onlyfun + " " + s1.Trim().Replace("@KOD$", "" + item.CodeNamber + "") + ";" + "\r\n";
            }

            return strzw;
        }

        public static DataTable data_procedure_ext(string str_proc)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TypDanych", typeof(string));
            dt.Columns.Add("NazwaParametru", typeof(string));
            dt.Columns.Add("TypParametru", typeof(string));
            dt.Columns.Add("WartoscParametru", typeof(string));
            dt.Columns.Add("NazwaProcedury", typeof(string));

            string[] stringSeparators = new string[] { "\r\n" };
            string[] strdata = str_proc.Split(stringSeparators, StringSplitOptions.None);

            for (int i = 1; i < strdata.Length; i++)
            {
                string[] strdataline = strdata[i].ToString().Split(',');

                DataRow dr = dt.NewRow();

                if (strdataline[1].ToString().Trim() == "adParamInput")
                {
                    dr["TypDanych"] = strdataline[0].ToString().Trim();
                    dr["NazwaParametru"] = strdataline[2].ToString().Trim().Substring(0, strdataline[2].IndexOf("=") - 2).Trim();
                    dr["TypParametru"] = strdataline[1].ToString().Trim();
                    dr["WartoscParametru"] = strdataline[2].ToString().Trim().Substring(strdataline[2].IndexOf("=")).Trim();
                    dr["NazwaProcedury"] = strdata[0].ToString().Trim();
                }
                else if (strdataline[1].ToString().Trim().StartsWith("adParamOutput"))
                {
                    dr["TypDanych"] = strdataline[0].ToString().Trim();
                    dr["NazwaParametru"] = strdataline[1].ToString().Trim().Substring(strdataline[1].IndexOf("=")).Trim();
                    dr["TypParametru"] = strdataline[1].ToString().Trim().Substring(0, strdataline[1].IndexOf("=") - 2).Trim();
                    dr["WartoscParametru"] = "";
                    dr["NazwaProcedury"] = strdata[0].ToString().Trim();
                }
  
                dt.Rows.Add(dr);
            }

            return dt;
        }
        private static Ncode[] data_exec_nr_kod_jezyka(string namespece, bool nietlumacz)
        {

            string[] strWerJ = namespece.Split(';');

            List<Ncode> parts = new List<Ncode>();

            if (strWerJ.Length > 15)
            {

                if (strWerJ[0].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 0, CodeNamber = "1045", UseCode = true, CodeFull = "Polski", CodeShotr = "PL" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 0, CodeNamber = "1045", UseCode = false, CodeFull = "Polski", CodeShotr = "PL" });
                }

                if (strWerJ[1].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 1, CodeNamber = "1031", UseCode = true, CodeFull = "Niemiecki", CodeShotr = "DE" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 1, CodeNamber = "1031", UseCode = false, CodeFull = "Niemiecki", CodeShotr = "DE" });
                }

                if (strWerJ[2].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 2, CodeNamber = "1040", UseCode = true, CodeFull = "Włoski", CodeShotr = "IT" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 2, CodeNamber = "1040", UseCode = false, CodeFull = "Włoski", CodeShotr = "IT" });
                }

                if (strWerJ[3].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 3, CodeNamber = "1036", UseCode = true, CodeFull = "Francuski", CodeShotr = "FR" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 3, CodeNamber = "1036", UseCode = false, CodeFull = "Francuski", CodeShotr = "FR" });
                }

                if (strWerJ[4].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 4, CodeNamber = "3082", UseCode = true, CodeFull = "Hiszpański", CodeShotr = "ES" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 4, CodeNamber = "3082", UseCode = false, CodeFull = "Hiszpański", CodeShotr = "ES" });
                }

                if (strWerJ[5].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 5, CodeNamber = "1029", UseCode = true, CodeFull = "Czeski", CodeShotr = "CZ" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 5, CodeNamber = "1029", UseCode = false, CodeFull = "Czeski", CodeShotr = "CZ" });
                }

                if (strWerJ[6].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 6, CodeNamber = "1051", UseCode = true, CodeFull = "Słowacki", CodeShotr = "SK" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 6, CodeNamber = "1051", UseCode = false, CodeFull = "Słowacki", CodeShotr = "SK" });
                }

                if (strWerJ[7].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 7, CodeNamber = "1060", UseCode = true, CodeFull = "Słoweński", CodeShotr = "SI" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 7, CodeNamber = "1060", UseCode = false, CodeFull = "Słoweński", CodeShotr = "SI" });
                }

                if (strWerJ[8].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 8, CodeNamber = "1030", UseCode = true, CodeFull = "Duński", CodeShotr = "DK" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 8, CodeNamber = "1030", UseCode = false, CodeFull = "Duński", CodeShotr = "DK" });
                }

                if (strWerJ[9].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 9, CodeNamber = "1043", UseCode = true, CodeFull = "Holdenderski", CodeShotr = "NL" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 9, CodeNamber = "1043", UseCode = false, CodeFull = "Holdenderski", CodeShotr = "NL" });
                }

                if (strWerJ[10].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 10, CodeNamber = "1033", UseCode = true, CodeFull = "USA", CodeShotr = "US" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 10, CodeNamber = "1033", UseCode = false, CodeFull = "USA", CodeShotr = "US" });
                }

                if (strWerJ[11].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 11, CodeNamber = "1038", UseCode = true, CodeFull = "Węgierski", CodeShotr = "HU" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 11, CodeNamber = "1038", UseCode = false, CodeFull = "Węgierski", CodeShotr = "HU" });
                }

                if (strWerJ[12].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 12, CodeNamber = "1044", UseCode = true, CodeFull = "Norweski", CodeShotr = "NO" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 12, CodeNamber = "1044", UseCode = false, CodeFull = "Norweski", CodeShotr = "NO" });
                }

                if (strWerJ[13].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 13, CodeNamber = "2070", UseCode = true, CodeFull = "Portugalski", CodeShotr = "PT" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 13, CodeNamber = "2070", UseCode = false, CodeFull = "Portugalski", CodeShotr = "PT" });
                }

                if (strWerJ[14].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 14, CodeNamber = "2055", UseCode = true, CodeFull = "Szwajcarski/Niemiecki", CodeShotr = "CH" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 14, CodeNamber = "2055", UseCode = false, CodeFull = "Szwajcarski", CodeShotr = "CH" });
                }

                if (strWerJ[15].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 15, CodeNamber = "1053", UseCode = true, CodeFull = "Szwedzki", CodeShotr = "SE" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 15, CodeNamber = "1053", UseCode = false, CodeFull = "Szwedzki", CodeShotr = "SE" });
                }

                if (strWerJ[16].ToLower() == "true")
                {
                    parts.Add(new Ncode() { Id = 16, CodeNamber = "1055", UseCode = true, CodeFull = "Turecki", CodeShotr = "TR" });
                }
                else
                {
                    parts.Add(new Ncode() { Id = 16, CodeNamber = "1055", UseCode = false, CodeFull = "Turecki", CodeShotr = "TR" });
                }

            }
            //dr["PL"] = strWerJ[1]; 0
            //dr["DE"] = strWerJ[1]; 1
            //dr["IT"] = strWerJ[2]; 2
            //dr["FR"] = strWerJ[3]; 3
            //dr["ES"] = strWerJ[4]; 4
            //dr["CZ"] = strWerJ[5]; 5
            //dr["SK"] = strWerJ[6]; 6
            //dr["SI"] = strWerJ[7]; 7
            //dr["DK"] = strWerJ[8]; 8
            //dr["NL"] = strWerJ[9]; 9
            //dr["US"] = strWerJ[10]; 10
            //dr["HU"] = strWerJ[11]; 11
            //dr["NO"] = strWerJ[12]; 12
            //dr["PT"] = strWerJ[13]; 13
            //dr["CH"] = strWerJ[14]; 14
            //dr["SE"] = strWerJ[15]; 15
            //dr["TR"] = strWerJ[16]; 16


            return parts.ToArray();
        }

        public class Ncode : IEquatable<Ncode>
        {
            public int Id { get; set; }
            public bool UseCode { get; set; }
            public string CodeNamber { get; set; }
            public string CodeShotr { get; set; }
            public string CodeFull { get; set; }

            public override string ToString()
            {
                return UseCode + ":" + CodeNamber;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Ncode objAsPart = obj as Ncode;
                if (objAsPart == null) return false;
                else return Equals(objAsPart);
            }
            public override int GetHashCode()
            {
                return Id;
            }
            public bool Equals(Ncode other)
            {
                if (other == null) return false;
                return (this.Id.Equals(other.Id));
            }

        }

    }
}
