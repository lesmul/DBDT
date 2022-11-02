using DBDT.USTAWIENIA_PROGRAMU;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DBDT.Excel
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_EDYCJA_KOLUMN.xaml
    /// </summary>
    public partial class WPF_EDYCJA_KOLUMN : Window
    {

        ObservableCollection<string> listTypyDanych = null;
        ObservableCollection<string> listPoleWymagane = null;

        DataTable dtk = new DataTable();
        public DataTable dt = new DataTable();
        public bool BoolUsunDane = false;
        public WPF_EDYCJA_KOLUMN(DataTable dtksurce)
        {
            InitializeComponent();

            dtk = dtksurce;

            listTypyDanych = new ObservableCollection<string>();
            listPoleWymagane = new ObservableCollection<string>();

            if (dtk != null) // table is a DataTable
            {
                DataColumn wCol1 = dt.Columns.Add("id", typeof(Int32));
                wCol1.AllowDBNull = false;
                wCol1.Unique = true;
                wCol1.AutoIncrement = true;
                wCol1.AutoIncrementSeed = 1;

                DataColumn wCol3 = dt.Columns.Add("Nazwa", typeof(string));
                wCol3.DefaultValue = "Nazwa?";
                wCol3.Unique= true;

                DataColumn wCol2 = dt.Columns.Add("Typ", typeof(string));
                wCol2.DefaultValue= "System.String";

                DataColumn wCol4 = dt.Columns.Add("PoleWymagane", typeof(string));
                wCol4.DefaultValue = "True";

                DataColumn wCol5 = dt.Columns.Add("DefaultValue", typeof(string));
                wCol5.DefaultValue = "";

                DataColumn wCol6 = dt.Columns.Add("MaxLength", typeof(Int32));
                wCol6.DefaultValue = -1;

                //kataloński(Hiszpania)  ca_CA   1027
                //czeski(Czechy)     cs_CZ   1029
                //duński(Dania)  da_DK   1030
                //holenderski(Holandia)  nl_NL   1043
                //angielski(Stany Zjednoczone)   en_US   1033
                //fiński(Finlandia)  fi_FI   1035
                //francuski(Francja)     fr_FR   1036
                //niemiecki(Niemcy)  de_DE   1031
                //węgierski(Węgry)   hu_HU   1038
                //włoski(Włochy)     it_IT   1040
                //japoński(Japonia)  ja_JP   1041
                //norweski(bokmål)   no_NO   1044
                //polski(Polska)     pl_PL   1045
                //portugalski(Brazylia)  pt_BR   1046
                //portugalski(Portugalia)    pt_PT   2070
                //rosyjski(Rosja)    ru_RU   1049
                //słoweński(Słowenia)    sl_SI   1060
                //hiszpański(Hiszpania)  es_ES   1034
                //szwedzki(Szwecja)  sv_SE   1053
                //turecki (Turcja) 	tr_TR 	1055
                DataColumn wColJEZYK1 = dt.Columns.Add("PL", typeof(bool));
                wColJEZYK1.DefaultValue = true;
                DataColumn wColJEZYK2 = dt.Columns.Add("DE", typeof(bool));
                wColJEZYK2.DefaultValue = false;
                DataColumn wColJEZYK3 = dt.Columns.Add("IT", typeof(bool));
                wColJEZYK3.DefaultValue = false;
                DataColumn wColJEZYK4 = dt.Columns.Add("FR", typeof(bool));
                wColJEZYK4.DefaultValue = false;
                DataColumn wColJEZYK5 = dt.Columns.Add("ES", typeof(bool));
                wColJEZYK5.DefaultValue = false;
                DataColumn wColJEZYK6 = dt.Columns.Add("CZ", typeof(bool));
                wColJEZYK6.DefaultValue = false;
                DataColumn wColJEZYK7 = dt.Columns.Add("SK", typeof(bool));
                wColJEZYK7.DefaultValue = false;
                DataColumn wColJEZYK8 = dt.Columns.Add("SI", typeof(bool));
                wColJEZYK8.DefaultValue = false;
                DataColumn wColJEZYK9 = dt.Columns.Add("DK", typeof(bool));
                wColJEZYK9.DefaultValue = false;
                DataColumn wColJEZYK10 = dt.Columns.Add("NL", typeof(bool));
                wColJEZYK10.DefaultValue = false;
                DataColumn wColJEZYK11 = dt.Columns.Add("US", typeof(bool));
                wColJEZYK11.DefaultValue = false;
                DataColumn wColJEZYK12 = dt.Columns.Add("HU", typeof(bool));
                wColJEZYK12.DefaultValue = false;
                DataColumn wColJEZYK13 = dt.Columns.Add("NO", typeof(bool));
                wColJEZYK13.DefaultValue = false;
                DataColumn wColJEZYK14 = dt.Columns.Add("PT", typeof(bool));
                wColJEZYK14.DefaultValue = false;
                DataColumn wColJEZYK15 = dt.Columns.Add("CH", typeof(bool));
                wColJEZYK15.DefaultValue = false;
                DataColumn wColJEZYK16 = dt.Columns.Add("SE", typeof(bool));
                wColJEZYK16.DefaultValue = false;
                DataColumn wColJEZYK17 = dt.Columns.Add("TR", typeof(bool));
                wColJEZYK17.DefaultValue = false;
                DataColumn wColJEZYK18 = dt.Columns.Add("PoleListy", typeof(String));
                wColJEZYK18.DefaultValue = "";

                listTypyDanych.Add("System.String");
                listTypyDanych.Add("System.Int32");
                listTypyDanych.Add("System.Boolean");

                listPoleWymagane.Add("True");
                listPoleWymagane.Add("False");

                foreach (DataColumn col in dtk.Columns)
                {
                    DataRow dr = dt.NewRow();
                    dr["Nazwa"] = col.ColumnName;
                    dr["Typ"] = col.DataType.FullName.ToString();
                    dr["PoleWymagane"] = col.AllowDBNull.ToString();
                    dr["DefaultValue"] = col.DefaultValue.ToString();
                    dr["MaxLength"] = col.MaxLength;
                    string[] strWerJ = col.Namespace.Split(';');
                    if (strWerJ.Length > 17)
                    {
                        dr["PL"] = strWerJ[0];
                        dr["DE"] = strWerJ[1];
                        dr["IT"] = strWerJ[2];
                        dr["FR"] = strWerJ[3];
                        dr["ES"] = strWerJ[4];
                        dr["CZ"] = strWerJ[5];
                        dr["SK"] = strWerJ[6];
                        dr["SI"] = strWerJ[7];
                        dr["DK"] = strWerJ[8];
                        dr["NL"] = strWerJ[9];
                        dr["US"] = strWerJ[10];
                        dr["HU"] = strWerJ[11];
                        dr["NO"] = strWerJ[12];
                        dr["PT"] = strWerJ[13];
                        dr["CH"] = strWerJ[14];
                        dr["SE"] = strWerJ[15];
                        dr["TR"] = strWerJ[16];
                        dr["PoleListy"] = strWerJ[17];
                    }

                    dt.Rows.Add(dr);
                }

                gtypy_kolumn.ItemsSource = listTypyDanych;
                gtypy_pole_wymagane.ItemsSource= listPoleWymagane;
                DG_XML_KOLUMNY.ItemsSource = dt.DefaultView;
            }
        }

        private void state_changed(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            dt.RejectChanges();
            this.Close();
        }

        private void B_OK_Click(object sender, RoutedEventArgs e)
        {
            sprawdz_dane();

            DataView dataView= new DataView(dt);
            dataView.RowFilter = "Nazwa = 'id' or Nazwa = 'ID' or Nazwa = 'Id' or Nazwa = 'iD'";
            if (dataView.Count > 1 ) 
            {
                MessageBox.Show("Pole ID może wystapić tylko raz!!!", "Ważna informacja!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DialogResult = true;
            dt.AcceptChanges();
            this.Close();
        }

        public class Info
        {
            public string TYPY_KOLUMN { get; set; }
            public string TYPY_WART_WYMAGANA { get; set; }
        }

        private void MyList_BeginningEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            var items = ((System.Data.DataRowView)e.Row.Item).Row;
            if ((int)items.ItemArray[0] < 4)
            {
                e.Cancel = true;
            }
        }

        private void MyList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            var items = ((System.Data.DataRowView)e.Row.Item).Row;
            if ((int)items.ItemArray[0] < 4)
            {
                e.Cancel = true;
            }
        }

        private void prev_key_up(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Delete) return;
            try
            {

                var currentRowIndex = DG_XML_KOLUMNY.Items.IndexOf(DG_XML_KOLUMNY.CurrentItem);

                if (currentRowIndex < 5) return;

                DataRowView MyRow = (DataRowView)DG_XML_KOLUMNY.Items[currentRowIndex];

                MyRow.BeginEdit();
                MyRow.Delete();
                MyRow.EndEdit();
                dt.AcceptChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DEL_Click(object sender, RoutedEventArgs e)
        {
            BoolUsunDane = true;
            B_USUN_DANE.IsEnabled = false;
            B_USUN_DANE.Content = "OK = Usuń";
        }

        private void sprawdz_dane()
        {

            foreach (DataRow rowx in dt.Rows)
            {
                if (rowx["PoleWymagane"].ToString() == "False")
                {
                    if (rowx["Typ"].ToString() == "System.String" && rowx["DefaultValue"].ToString() == "")
                    {
                        rowx["DefaultValue"] = " ";
                    }
                    else if (rowx["Typ"].ToString() == "System.Int32" && rowx["DefaultValue"].ToString() == "")
                    {
                        rowx["DefaultValue"] = "0";
                    }
                    else if (rowx["Typ"].ToString() == "System.Boolean" && rowx["DefaultValue"].ToString() == "")
                    {
                        rowx["DefaultValue"] = "False";
                    }
                }
            }

        }
    }
}
