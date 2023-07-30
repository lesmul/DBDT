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
    public static class zamien_wybrane_excel
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
                        row[colx.DisplayIndex] = MyRow[colx.DisplayIndex].ToString().Trim();
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
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[colx.Header.ToString()] = (str_x.StartsWith("_") ? "" : str_x) + value;
                            }
                            else
                            {
                                MyRow[colx.Header.ToString()] = (str_x.StartsWith("_") ? "" : str_x + "_") + value;
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
                              }
                            else
                            {
                                if (value.StartsWith("_"))
                                {
                                    MyRow[colx.Header.ToString()] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + value;
                                }
                                else
                                {
                                    MyRow[colx.Header.ToString()] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + "_" + value;
                                }
                            }
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[colx.Header.ToString()] = str_x + value;
                            }
                            else
                            {
                                MyRow[colx.Header.ToString()] = str_x + "_" + value;
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
                        var colx = DG.SelectedCells as DataGridColumn;

                        DataRow row = dt_d.NewRow();
                        row[colx.DisplayIndex] = MyRow[colx.DisplayIndex].ToString().Trim();
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
                           }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[3] = (str_x.StartsWith("_") ? "" : str_x) + value;
                            }
                            else
                            {
                                MyRow[3] = (str_x.StartsWith("_") ? "" : str_x + "_") + value;
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
                            }
                            else
                            {
                                if (value.StartsWith("_"))
                                {
                                    MyRow[3] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + value;
                                }
                                else
                                {
                                    MyRow[3] = (str_x.EndsWith("_") ? str_x.TrimEnd('_') : str_x) + "_" + value;
                                }
                            }
                        }
                        else
                        {
                            if (value.StartsWith("_"))
                            {
                                MyRow[3] = str_x + value;
                            }
                            else
                            {
                                MyRow[3] = str_x + "_" + value;
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
