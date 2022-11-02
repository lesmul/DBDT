using DBDT.Excel.DS;
using DBDT.SQL.Indeksy;
using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace DBDT.Excel
{
    internal class add_boom
    {
        public static string AddBoom(DataGrid resultGrid, IndeksTable spc, string str_Tag, string str_indesk)
        {
            if (resultGrid.SelectedCells.Count == 0) return "";

            DataGridCellInfo cell = resultGrid.SelectedCells[0];

            //string str_indesk;

            if (cell.Item == null)
            {
                return "";
            }

            string sql = "SELECT id ,nazwa_obrobki ,opis ,pole1 ,pole2 ,pole3 ,pole4 ,pole5 ,pole6 ,pole7 ,pole8 ,pole9 ,pole10 ,pole11 ,poleint1" +
                         ", poleint2 FROM obrobki WHERE poleint1 = 1 ORDER BY nazwa_obrobki";

            DataTable dt = new DataTable();

            //poleint2 - 1 kolor / 2 bez / 3 sztuka / 4 pomin

            dt = _PUBLIC_SqlLite.SelectQuery(sql);

            if (dt == null)
            {
                MessageBox.Show("Brak danych w tabeli konfiguracja", "Błąd konfiguracji!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return ""; 
            }

            string[] strOp = dt.Rows[0]["opis"].ToString().Split(',');

            if (strOp[0].ToString() == "" || strOp[1].ToString() == "" || strOp.Length<5) return "";

            DataTable dts = new DataTable();

            dts = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT * FROM " + strOp[0].ToString() + " WHERE " + strOp[1].ToString() + " = '" + str_indesk + "'");

            if(str_Tag == "LIKE" && dts.Rows.Count == 0)
            {
                if (str_indesk.IndexOf("-") > 0)
                {
                   dts = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT top 1000 * FROM " + strOp[0].ToString() + " WHERE " + strOp[1].ToString() + " like '" 
                       + str_indesk.Substring(0, str_indesk.LastIndexOf("-") - 1) + "%'");
                }
                else
                {
                   dts = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT top 1000 * FROM " + strOp[0].ToString() + " WHERE " + strOp[1].ToString() + " like '" + str_indesk + "%'");
                }
                
                str_Tag = "OK";
            }

            if (dts.Rows.Count == 0) 
            {
                MessageBox.Show("Brak danych w tabeli " + strOp[0].ToString() + " dla indeksu: " + str_indesk + " musi być równy " + strOp[1].ToString(), "Błąd konfiguracji!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return ""; 
            }

            if (dt.Columns.Contains("nazwa_obrobki") == false) 
            {
                MessageBox.Show("Tabela nie zawiera pasującej kolumny", "Błąd konfiguracji!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return ""; 
            }

            string typs = "";

            if (strOp.Length > 4 && strOp[4].ToString() != "" && strOp[0].ToString() != "")
            {

                DataTable dti = new DataTable();
                dti = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT TOP 1 " + strOp[4].ToString() + " FROM " + strOp[0].ToString() +
                    " WHERE " + strOp[1].ToString() + " like '" + str_indesk.Trim() + "%'" + " ORDER BY " + strOp[1].ToString());

                if (dti == null) 
                {
                    MessageBox.Show("Brak danych w tabeli: " + strOp[0].ToString() + " nie znaleziono pasujących danych w kolumnie: " + strOp[1].ToString(), "Błąd konfiguracji!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return ""; 
                }

                if (dti.Rows.Count > 0)
                {
                    typs = dti.Rows[0][0].ToString().ToString().Trim();
                }

            }

            DataView dv = new DataView(dt);

            switch ((int)dt.Rows[0]["poleint2"])
            {
                case 1:
                    dv.RowFilter = "opis like'%," + typs + ",%' AND poleint2 = 1";
                    break;
                case 2:
                    dv.RowFilter = "opis like'%," + typs + ",%' AND poleint2 = 2";
                    break;
                case 3:
                    dv.RowFilter = "opis like'%," + typs + ",%' AND poleint2 = 3";
                    break;
                case 4:
                    dv.RowFilter = "opis like'%," + typs + ",%'";
                    break;
            }

            string str_fun = "";

            if (dv.Count > 0)
            {
                string licz_kolor = "";

                for (int i = 0; i < dv.Count; i++)
                {
                    str_fun = dv[i]["nazwa_obrobki"].ToString().Trim() + "("
                 + '\u0022' + str_indesk.Trim() + '\u0022' + ","
                 + (dv[i]["pole1"].ToString().Trim() != "" ? dv[i]["pole1"].ToString() + "," : "")
                 + (dv[i]["pole3"].ToString().Trim() != "" ? dv[i]["pole3"].ToString() + "," : "")
                 + (dv[i]["pole5"].ToString().Trim() != "" ? dv[i]["pole5"].ToString() + "," : "")
                 + (dv[i]["pole7"].ToString().Trim() != "" ? dv[i]["pole7"].ToString() + "," : "")
                 + (dv[i]["pole9"].ToString().Trim() != "" ? dv[i]["pole9"].ToString() + "," : "")
                 + (dv[i]["pole11"].ToString().Trim() != "" ? dv[i]["pole11"].ToString() + "," : "");

                    if (str_Tag == "OK")
                    {
                        str_fun = str_fun.TrimEnd(',');
                        str_fun += ");";
                    }
                    else
                    {
                        str_fun = str_fun.TrimEnd(',');
                        str_fun += ");" + "%" + dv[i]["pole2"].ToString();

                        if (strOp[5].ToString() != "" && strOp[3].ToString() != "" && str_indesk.Trim() != "")
                        {
                            if (licz_kolor == "")
                            {
                                DataTable dtic = new DataTable();
                                dtic = _PUBLIC_SQL.ZWROC_WARTOSC_MSSQL("SELECT Count(*) FROM " + strOp[3].ToString() +
                                    " WHERE " + strOp[5].ToString() + " = '" + str_indesk.Trim() + "'");

                                if(dtic == null)
                                {
                                    MessageBox.Show("Brak danych w tabeli: " + strOp[3].ToString(), "Błąd w tabeli z kolorami konfiguracji!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    return "";
                                }

                                str_fun += " / Ilość danych w tabeli: " + strOp[3].ToString() + " = " + dtic.Rows[0][0].ToString().Trim();

                                licz_kolor = " / Ilość danych w tabeli: " + strOp[3].ToString() + " = " + dtic.Rows[0][0].ToString().Trim();

                            }
                            else
                            {
                                str_fun += licz_kolor;
                            }

                        }
                        else
                        {
                            str_fun += " / Brak danych";
                        }

                    }

                    spc.itemsIndeksy.Add(str_fun.TrimEnd(','));
                }

            }
            else
            {
                str_fun = dt.Rows[0]["nazwa_obrobki"].ToString().Trim() + "("
                 + '\u0022' + str_indesk.Trim() + '\u0022' + ","
                 + (dt.Rows[0]["pole1"].ToString().Trim() != "" ? dt.Rows[0]["pole1"].ToString() + "," : "")
                 + (dt.Rows[0]["pole3"].ToString().Trim() != "" ? dt.Rows[0]["pole3"].ToString() + "," : "")
                 + (dt.Rows[0]["pole5"].ToString().Trim() != "" ? dt.Rows[0]["pole5"].ToString() + "," : "")
                 + (dt.Rows[0]["pole7"].ToString().Trim() != "" ? dt.Rows[0]["pole7"].ToString() + "," : "")
                 + (dt.Rows[0]["pole9"].ToString().Trim() != "" ? dt.Rows[0]["pole9"].ToString() + "," : "")
                 + (dt.Rows[0]["pole11"].ToString().Trim() != "" ? dt.Rows[0]["pole11"].ToString() + "," : "");

                if (str_Tag == "OK")
                {
                    str_fun = str_fun.TrimEnd(',');
                    str_fun += "); %Brak typu w konfiguracji!!!";
                }
                else
                {
                    str_fun = str_fun.TrimEnd(',');
                    str_fun += ");" + "%" + dt.Rows[0]["pole2"].ToString() + " Brak typu w konfiguracji!!!";

                }

                spc.itemsIndeksy.Add(str_fun.TrimEnd(','));
            }

            if (spc.Tag == null)
            {

                spc.itContr.ItemsSource = spc.itemsIndeksy;
                spc.Show();

            }
            else if (spc.Tag.ToString() == "NOK")
            {

                List<object> itemsIndeksyTMP = new List<object>();
                itemsIndeksyTMP = spc.itemsIndeksy;

                spc = new IndeksTable();

                spc.itContr.ItemsSource = null;
                spc.itContr.ItemsSource = itemsIndeksyTMP;
                spc.TXT_FILTR.Text = "";
                spc.Show();
            }
            else
            {

                spc.itContr.ItemsSource = null;
                spc.itContr.ItemsSource = spc.itemsIndeksy;
                spc.TXT_FILTR.Text = "";
            }

            return spc.public_str_copy;

        }
    }
}
