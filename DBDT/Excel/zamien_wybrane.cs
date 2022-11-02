using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DBDT.Excel
{
    public static class zamien_wybrane
    {
        public static DataTable Zamien_wybrane_txt(string str_x, bool zamien, bool zastap, DataGrid DG, DataTable dt_d)
        {

            if (str_x.Trim() == "") return null;

            IList items = DG.SelectedItems;

            if (items.Count == 0)
            {
                foreach (DataGridCellInfo item in DG.SelectedCells)
                {
                    var colx = item.Column as DataGridColumn;
                    var MyRow = item.Item as DataRowView;
                    //row.Row[col.Header.ToString()].ToString().Trim()
                    // var rowy = rowx.Row[colx.Header.ToString()];
                    // DataRowView MyRow = (DataRowView)rowy;

                    if (zamien == false)
                    {
                        DataRow row = dt_d.NewRow();
                        row["Objekt"] = MyRow["Objekt"].ToString();
                        row[colx.DisplayIndex] = MyRow[colx.DisplayIndex].ToString().Trim();
                        row["Ilość_Znaków"] = MyRow[colx.DisplayIndex].ToString().Length;
                        row[colx.DisplayIndex + 1] = MyRow[colx.DisplayIndex + 1].ToString();
                        row[colx.DisplayIndex + 2] = MyRow[colx.DisplayIndex + 2].ToString();
                        dt_d.Rows.Add(row);
                    }

                    MyRow.BeginEdit();

                    string value = MyRow[colx.Header.ToString()].ToString();
                    int ch_s = value.ToString().IndexOf("_");

                    if (zamien == true)
                    {
                        if (ch_s > -1)
                        {
                            MyRow[colx.Header.ToString()] = (str_x.StartsWith("_") ? "" : str_x) + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                            MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[colx.Header.ToString()] = (str_x.StartsWith("_") ? "" : str_x) + value;
                                MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                            }
                            else
                            {
                                MyRow[colx.Header.ToString()] = (str_x.StartsWith("_") ? "" : str_x + "_") + value;
                                MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                            }
                        }
                    }
                    else
                    {
                        if (zastap == true)
                        {
                            if (ch_s > -1)
                            {
                                MyRow[colx.Header.ToString()] = (str_x.EndsWith("_") ? str_x : str_x + "_") + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                                MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                            }
                            else
                            {
                                if (value.StartsWith("_"))
                                {
                                    MyRow[colx.Header.ToString()] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + value;
                                    MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                                }
                                else
                                {
                                    MyRow[colx.Header.ToString()] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + "_" + value;
                                    MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                                }
                            }
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[colx.Header.ToString()] = str_x + value;
                                MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                            }
                            else
                            {
                                MyRow[colx.Header.ToString()] = str_x + "_" + value;
                                MyRow["Ilość_Znaków"] = MyRow[colx.Header.ToString()].ToString().Length;
                            }
                        }

                    }

                    MyRow.EndEdit();

                }
            }
            else
            {
                foreach (object item in items)
                {

                    DataRowView MyRow = (DataRowView)item;

                    if (zamien == false)
                    {
                        DataRow row = dt_d.NewRow();
                        row["Objekt"] = MyRow["Objekt"].ToString();
                        row[3] = MyRow[3].ToString().Trim();
                        row["Ilość_Znaków"] = MyRow[3].ToString().Length;
                        row[4] = MyRow[4].ToString();
                        row[5] = MyRow[5].ToString();
                        dt_d.Rows.Add(row);
                    }

                    MyRow.BeginEdit();

                    string value = MyRow[3].ToString();
                    int ch_s = value.ToString().IndexOf("_");

                    if (zamien == true)
                    {
                        if (ch_s > -1)
                        {
                            MyRow[3] = (str_x.StartsWith("_") ? "" : str_x) + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                            MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[3] = (str_x.StartsWith("_") ? "" : str_x) + value;
                                MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                            }
                            else
                            {
                                MyRow[3] = (str_x.StartsWith("_") ? "" : str_x + "_") + value;
                                MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                            }
                        }
                    }
                    else
                    {
                        if (zastap == true)
                        {
                            if (ch_s > -1)
                            {
                                MyRow[3] = (str_x.EndsWith("_") ? str_x : str_x + "_") + value.Substring(ch_s + 1, value.Length - ch_s - 1);
                                MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                            }
                            else
                            {
                                if (value.StartsWith("_"))
                                {
                                    MyRow[3] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + value;
                                    MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                                }
                                else
                                {
                                    MyRow[3] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + "_" + value;
                                    MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                                }
                            }
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[3] = str_x + value;
                                MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                            }
                            else
                            {
                                MyRow[3] = str_x + "_" + value;
                                MyRow["Ilość_Znaków"] = MyRow[3].ToString().Length;
                            }
                        }

                    }

                    MyRow.EndEdit();

                }
            }

            return dt_d;

        }
    }
}
