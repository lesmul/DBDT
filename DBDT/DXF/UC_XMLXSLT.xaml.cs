using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using Saxon.Api;
using System.Xml.Xsl;
using System.Xml;
using Mvp.Xml;
using Mvp.Xml.Common.Xsl;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using QName = Saxon.Api.QName;
using System.Xml.XPath;
using System.Data.SqlClient;
using DBDT.SQL.SQL_SELECT;
using DBDT.USTAWIENIA_PROGRAMU;
using System.Data;

namespace DBDT.DXF
{
    /// <summary>
    /// Logika interakcji dla klasy UC_RYS_DXF.xaml
    /// </summary>
    public partial class UC_XMLXSLT : UserControl
    {

        public UC_XMLXSLT()
        {
            InitializeComponent();

            Parameters = new ObservableCollection<Parameter>();
            Instance = this;

        }

        public static UC_XMLXSLT Instance { get; private set; }
        private string XmlFilePath { get; set; }
        private string XsltFilePath { get; set; }

        public ObservableCollection<Parameter> Parameters { get; set; }

        private void OpenXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Pliki XML (*.xml)|*.xml",
                Title = "Wybierz plik XML"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                XmlFilePath = openFileDialog.FileName;
                xmlFilePathTextBox.Text = XmlFilePath;
              
            }
        }
        private void SaveXmlFile_Click(object sender, RoutedEventArgs e)
        {
            if (txt_numerSerii.Text.Trim() == "") return;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XML (*.xml)|*.xml",
                FileName = txt_numerSerii.Text.Trim() + "_P_PROD.XML",
                Title = "Wybierz lokalizację zapisu pliku XML"
            };


            if (saveFileDialog.ShowDialog() == true)
            {
                XmlFilePath = saveFileDialog.FileName;
                xmlFilePathTextBox.Text = XmlFilePath;

                DataTable dt = new DataTable();

                dt = _PUBLIC_SqlLite.SelectQuery("SELECT id, serwer, nazwa_bazy FROM ParametryPalaczenia WHERE nazwa_bazy <> '' order by id desc");

                if (dt.Rows.Count == 0)
                {
                    return;
                }

                try
                {

                string connectionString =  @"Server=" + dt.Rows[0][1].ToString() + ";Database=" + dt.Rows[0][2].ToString() + ";Trusted_Connection=True";

                string seriaProdukcyjna = txt_numerSerii.Text.Trim(); // Zmień to na właściwą wartość

                DataTable dtsql = new DataTable();

                dtsql = _PUBLIC_SqlLite.SelectQuery("SELECT pole11 FROM ParametryPalaczenia WHERE pole9 LIKE 'POLE_KONFIGURACJI_INDEKSOW'");

                if (dtsql.Rows.Count == 0) return;

                string query = dtsql.Rows[0]["pole11"].ToString();

                if (query.ToLower().IndexOf("@seriaprodukcyjna") < 0) return;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //string query = "SELECT zlib.UnzipXml(xmlBlob) FROM tabelaXML WHERE seria_produkcyjna = @SeriaProdukcyjna";
                 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SeriaProdukcyjna", seriaProdukcyjna);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string xmlString = reader.GetString(0);
                                // Konwertuj dane z postaci tekstu na bajty
                                byte[] xmlData = Encoding.UTF8.GetBytes(xmlString);

                                string sciezkaDoPliku = XmlFilePath; // Zmień na właściwą ścieżkę
                                File.WriteAllBytes(sciezkaDoPliku, xmlData);
                            }
                        }
                    }

                    connection.Close();
                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Błąd!!!");
                }
            }
        }

        private void OpenXsltFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Pliki XSLT (*.xslt)|*.xslt|Pliki XSL (*.xsl)|*.xsl",
                Title = "Wybierz plik XSLT"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                XsltFilePath = openFileDialog.FileName;
                xsltFilePathTextBox.Text = XsltFilePath;
            

            }
        }
        private async void XsltFile20_Click(object sender, RoutedEventArgs e)
        {
            if (XmlFilePath == null || XsltFilePath == null) return;

            PROGRESX.Visibility = Visibility.Visible;
            await Task.Delay(1);
            if (Instance.Parameters.Count > 0)
            {
                xmlTextBox.Text = ConvertXmlToHtmlWithVBScriptParametry(XmlFilePath, XsltFilePath);
                PROGRESX.Visibility = Visibility.Hidden;
            }
            else
            {
                xmlTextBox.Text = ConvertXmlToHtmlWithVBScript(XmlFilePath, XsltFilePath);
                PROGRESX.Visibility = Visibility.Hidden;
            }

            TC_DANE.SelectedIndex = 0;
        }
        private async void XsltFile10_Click(object sender, RoutedEventArgs e)
        {
            //var processor = new MvpXslTransform();

            //processor.SupportedFunctions = Mvp.Xml.Exslt.ExsltFunctionNamespace.GdnDynamic;

            //processor.Load(XsltFilePath);

            //processor.Transform(new XmlInput(XmlFilePath), null, new XmlOutput(Console.Out));
            if (XmlFilePath == null || XsltFilePath == null) return;

            if(Instance.Parameters.Count > 0)
            {
                PROGRESX.Visibility = Visibility.Visible;
                await Task.Delay(1);

                xmlTextBox.Text = ConvertXmlToHtmlWithVBScript10ZParametrami(XmlFilePath, XsltFilePath);
                PROGRESX.Visibility = Visibility.Hidden;

            }
            else
            {
                PROGRESX.Visibility = Visibility.Visible;
                await Task.Delay(1);

                xmlTextBox.Text = ConvertXmlToHtmlWithVBScript10(XmlFilePath, XsltFilePath);
                PROGRESX.Visibility = Visibility.Hidden;
            }

            TC_DANE.SelectedIndex = 0;

        }

        public static string ConvertXmlToHtmlWithVBScript10ZParametrami(string xmlFilePath, string xsltFilePath)
        {
            try
            {
                // Utwórz instancję MvpXslTransform
                var xslt = new MvpXslTransform();

                // Create the XsltSettings object with script enabled.
                XsltSettings settings = new XsltSettings(true, true);

                xslt.Load(xsltFilePath, settings, new XmlUrlResolver());
                // Opcjonalnie ustaw wyjście jako XHTML
                xslt.EnforceXHTMLOutput = true;
  
                // Przykładowe argumenty (opcjonalne)
                var arguments = new XsltArgumentList();

                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }
                string tmpar = "";

                foreach (var parameter in Instance.Parameters)
                {
                    if (arguments.GetParam(parameter.Name, "") == null)
                    {
                        arguments.AddParam(parameter.Name, "", parameter.Value);
                        tmpar += $"{parameter.Name}, {parameter.Value}" + "\r\n";
                    }
                   
                }

                Instance.ParaMetry.Content = tmpar;
                //arguments.AddParam("param1", "", "value1");
                //arguments.AddParam("param2", "", "value2");

                // Utwórz wynik transformacji jako ciąg znaków
                var resultString = TransformXmlToString(xslt, xmlFilePath, arguments);

                return resultString;
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                return $"Błąd konwersji XML: {ex.Message}";
            }
        }

        static string TransformXmlToString(MvpXslTransform transformer, string xmlFilePath, XsltArgumentList arguments)
        {

            using (var inputReader = new System.IO.StreamReader(xmlFilePath))
            {
                var inputXml = new XmlInput(inputReader);

                using (var outputWriter = new System.IO.StringWriter())
                {
                    var outputXml = new XmlOutput(outputWriter);

                    transformer.Transform(inputXml, arguments, outputXml);

                    // Pobierz wynik transformacji jako ciąg znaków
                    var resultString = outputWriter.ToString();

                    return resultString;
                }
            }

        }

        public static string ConvertXmlToHtmlWithVBScript10(string xmlFilePath, string xsltFilePath)
        {
            try
            {
                var processor = new MvpXslTransform();
                processor.SupportedFunctions = Mvp.Xml.Exslt.ExsltFunctionNamespace.GdnDynamic;

                // Create the XsltSettings object with script enabled.
                XsltSettings settings = new XsltSettings(true, true);

                processor.Load(xsltFilePath, settings, new XmlUrlResolver());

                using (var inputReader = new StreamReader(xmlFilePath))
                {
                    var inputXml = new XmlInput(inputReader);

                    using (var outputWriter = new StringWriter())
                    {
                        var outputXml = new XmlOutput(outputWriter);
                        processor.Transform(inputXml, null, outputXml);

                        // Pobierz wynik transformacji jako ciąg znaków
                        string resultString = outputWriter.ToString();
                        return resultString;
                    }
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                return $"Błąd konwersji XML: {ex.Message}";
            }
        }

        public static string ConvertXmlToHtmlWithVBScript(string xmlFilePath, string xsltFilePath)
        {
            try
            {
                // Utwórz procesor Saxon
                Processor processor = new Processor();

                // Wczytaj plik XML
                XdmNode input = processor.NewDocumentBuilder().Build(new Uri(xmlFilePath));

                // Wczytaj plik XSLT
                XsltTransformer transformer = processor.NewXsltCompiler().Compile(new Uri(xsltFilePath)).Load();

                // Wykonaj transformację
                transformer.InputXmlResolver = new XmlUrlResolver();
                transformer.InitialContextNode = input;

                XdmDestination result = new XdmDestination();
                transformer.Run(result);

                // Zwróć wynik jako ciąg znaków
                string htmlResult = result.XdmNode.OuterXml;

                return htmlResult;
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                return $"Błąd konwersji XML: {ex.Message}";
            }
        }

        public static string ConvertXmlToHtmlWithVBScriptParametry(string xmlFilePath, string xsltFilePath)
        {
            try
            {
                // Utwórz procesor Saxon
                Processor processor = new Processor();

                // Wczytaj plik XML
                XdmNode input = processor.NewDocumentBuilder().Build(new Uri(xmlFilePath));

                // Przykładowe argumenty (opcjonalne)
                var arguments = new XsltArgumentList();

                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }
                string tmpar = "";

                // Wczytaj plik XSLT
                XsltTransformer transformer = processor.NewXsltCompiler().Compile(new Uri(xsltFilePath)).Load();

                if (arguments != null)
                {
                    foreach (var arg in Instance.Parameters)
                    {
                        // Przekonwertuj ciąg znaków na XdmValue
                        XdmValue argValue = new XdmAtomicValue(arg.Value);

                        // Użyj QName z przestrzeni nazw Saxon.Api
                        QName paramName = new QName(arg.Name);

                        transformer.SetParameter(paramName, argValue);
                        tmpar += $"{arg.Name}, {arg.Value}" + "\r\n";
                    }
                }

                Instance.ParaMetry.Content = tmpar;

                // Wykonaj transformację
                transformer.InputXmlResolver = new XmlUrlResolver();
                transformer.InitialContextNode = input;

                XdmDestination result = new XdmDestination();
                transformer.Run(result);

                // Zwróć wynik jako ciąg znaków
                string htmlResult = result.XdmNode.OuterXml;

                return htmlResult;
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                return $"Błąd konwersji XML: {ex.Message}";
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;

            if (tabControl.SelectedItem is TabItem selectedItem && selectedItem.Header.ToString() == "Dane HTML")
            {
                // Po wybraniu zakładki "Dane HTML" przekaż zawartość xmlTextBox do kontrolki WebBrowser
                string xmlText = xmlTextBox.Text;
                if (xmlText == null || xmlText == "")
                {
                    webBrowser.NavigateToString("<html lang=\"pl\"></html>");
                    return;
                }
                // Ustaw zawartość WebBrowser jako HTML
                webBrowser.NavigateToString(xmlText);
            }
            else if (tabControl.SelectedItem is TabItem selectedItem2 && selectedItem2.Header.ToString() == "Dane XML")
            {
                string xmlText = xmlTextBox.Text;
                xmlEditor.Text = xmlText;
            }
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            // Tworzenie okna dialogowego do wyboru lokalizacji zapisu pliku
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki CSV (*.csv)|*.csv|Pliki XML (*.xml)|*.xml|Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Zapisz zawartość TextBox do pliku
                    File.WriteAllText(filePath, xmlTextBox.Text);
                    MessageBox.Show("Dane zostały zapisane do pliku.", "Zapisano", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas zapisywania danych do pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddParameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Add(new Parameter { Name = "NazwaParametru", Value = "WartośćParametru" });
        }
        private void DelParameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Clear();
        }
        private void ExampleParameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Add(new Parameter { Name = "machineId", Value = "1" });
            Parameters.Add(new Parameter { Name = "Set", Value = "1" });
        }

        private void Example2Parameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Add(new Parameter { Name = "machine", Value = "1" });
            Parameters.Add(new Parameter { Name = "opr", Value = "1" });
        }
        private void LoadXSLTParameter_Click(object sender, RoutedEventArgs e)
        {
            Parameters.Clear();
            Instance.ParaMetry.Content = "Załadowano parametry: " + PobierzParametryZPlikuXSLT(Instance.XsltFilePath);
        }

        public static string PobierzParametryZPlikuXSLT(string xsltFilePath)
        {
            try
            {

                var xsltContent = File.ReadAllText(xsltFilePath);
                var xsltDocument = new XPathDocument(new StringReader(xsltContent));
                var xsltNamespaceManager = new XmlNamespaceManager(new NameTable());
                xsltNamespaceManager.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                var xsltNavigator = xsltDocument.CreateNavigator();
               // xsltNavigator.Namespaces = false; // Wyłącz automatyczną obsługę przestrzeni nazw

                var paramQuery = xsltNavigator.Compile("//xsl:param");
                paramQuery.SetContext(xsltNamespaceManager); // Przypisz przestrzeń nazw

                var paramIterator = xsltNavigator.Select(paramQuery);

                while (paramIterator.MoveNext())
                {
                    var paramNode = paramIterator.Current;

                    // Pobierz nazwę parametru
                    var paramName = paramNode.GetAttribute("name", "");

                    // Pobierz wartość domyślną parametru (jeśli istnieje)
                    var defaultValue = paramNode.GetAttribute("select", "");

                    // Dodaj parametr do listy
 
                    Instance.Parameters.Add(new Parameter { Name = paramName, Value = defaultValue });
                }

                // Zwróć listę parametrów jako tekst
                return Instance.Parameters.ToString();
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                return $"Błąd odczytu parametrów z pliku XSLT: {ex.Message}";
            }
        }

    }
}
