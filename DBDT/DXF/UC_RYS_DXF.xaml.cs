using DBDT.Konfiguracja;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace DBDT.DXF
{
    /// <summary>
    /// Logika interakcji dla klasy UC_RYS_DXF.xaml
    /// </summary>
    public partial class UC_RYS_DXF : UserControl
    {

        public UC_RYS_DXF()
        {
            InitializeComponent();
            //AddItemsToCanvas();
            //this.DataContext = new MainViewModel();
        }

        //List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

        int is_select = -1;
        ArrayList ALL_DXF_OBJ = new ArrayList();

        private void laduj_uc(object sender, RoutedEventArgs e)
        {
            //FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            //// MoveFocus takes a TraveralReqest as its argument.
            //TraversalRequest request = new TraversalRequest(focusDirection);
            //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            //if (elementWithFocus != null)
            //{
            //    elementWithFocus.MoveFocus(request);
            //}
 
        }

        private void OnClick(object sender, RoutedEventArgs args)
        {
            if (DesignerCanvas.Children.Count > 0)
            {
                if (DesignerCanvas.Children[0] is System.Windows.Controls.Control)
                {
                    CheckBox selectionCheckBox = sender as CheckBox;
                    if (selectionCheckBox != null && selectionCheckBox.IsChecked == true)
                    {
                        foreach (Control child in DesignerCanvas.Children)
                        {
                            Selector.SetIsSelected(child, true);
                        }
                    }
                    else
                    {
                        foreach (Control child in DesignerCanvas.Children)
                        {
                            Selector.SetIsSelected(child, false);
                        }
                    }
                }
            }

        }

        private void PathGeometryFDXF(string filename, bool edycja)
        {
            Style s = this.FindResource("DesignerItemStyle") as Style;
       
            var contentControl2 = new ContentControl();

            LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

            int id_wezel = 1;

            if (edycja == false)
            {
                myObjDxfAll = load_save_dxf.LOAD(filename, DesignerCanvas.Children.Count);

                if (myObjDxfAll== null)
                {
                    return ;
                }

                ALL_DXF_OBJ.Add(myObjDxfAll);

            }
            else
            {

                DesignerCanvas.Children.RemoveAt(is_select);

                myObjDxfAll = (List<CDxfOBJ>)ALL_DXF_OBJ[is_select];

                for (int j = 0; j < myObjDxfAll.Count; j++)
                {
                    myObjDxfAll[j].Juz_dodany = false;
                }

                ALL_DXF_OBJ.RemoveAt(is_select);

                ALL_DXF_OBJ.Add(myObjDxfAll);

            }
           
            //double wys_canvas = DesignerCanvas.Height;

            //add Ellipse
            var contentControl = new ContentControl();

            var maxX = myObjDxfAll.Max(row => row.MaxX);
            var maxY = myObjDxfAll.Max(row => row.MaxY);

            contentControl.Width = maxX;
            contentControl.Height = maxY;
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, 50);
            Canvas.SetLeft(contentControl, 50);

            contentControl.Style = s;

            var geoDXF = new System.Text.StringBuilder();
            int i = 0;
            if(myObjDxfAll.Count > 0)
            {
                var find_i_min = from u in myObjDxfAll
                                 where u.Juz_dodany == false
                                 select u.Id;
                i = (int)find_i_min.Min();
            }

            double X = 0;
            double Y = 0;
            //for (int i = 0; i < myObjDxfAll.Count; i++)
            var query = from u in myObjDxfAll
                        where u.Juz_dodany == false
                        select u;
            // przykład: M100,100 l0,100 M100,100 a25,25 0 0 1 25,-25 M125,75 l125,0 M250,75 a25,25 0 0 1 25,25 M275,100 l0,100
            while (query.Count() != 0) // condition
            {

                myObjDxfAll[i].IntWezel = id_wezel;

                switch (myObjDxfAll[i].Typ)
                {
                    case "L":

                        geoDXF.AppendLine(" M" + Convert.ToString(myObjDxfAll[i].X1).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Y1).Replace(",", "."));
                        //M100,100 l0,100
                        geoDXF.AppendLine(" L" + Convert.ToString(myObjDxfAll[i].X2).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Y2).Replace(",", "."));

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "A":

                        double maxWidth = Math.Round(myObjDxfAll[i].Radius * 2, 3);
                        double maxHeight = Math.Round(myObjDxfAll[i].Radius * 2, 3);

                        double xStart = Math.Round((maxWidth / 2.0) * Math.Cos(myObjDxfAll[i].StartA * (Math.PI / 180.0)), 3);
                        double yStart = Math.Round((maxHeight / 2.0) * Math.Sin(myObjDxfAll[i].StartA * (Math.PI / 180.0)), 3);

                        double xEnd = Math.Round((maxWidth / 2.0) * Math.Cos(myObjDxfAll[i].EndA * (Math.PI / 180.0)), 3);
                        double yEnd = Math.Round((maxHeight / 2.0) * Math.Sin(myObjDxfAll[i].EndA * (Math.PI / 180.0)), 3);

                       //M100,100 a25,25 0 0 1 25,-25
                        geoDXF.AppendLine(" M" + Convert.ToString(myObjDxfAll[i].X1).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Y1).Replace(",", "."));

       
                        //" 0 0 0" - zgodnie z ruchem wsk. zeg.
                        //" 0 0 1" - przeciwnie z ruchem wsk. zeg.
                        double sX = xStart + xEnd;
                        double sY = yStart + yEnd;

                        if (sY > 0)
                        {
                            sY = -sY;
                        }
                        else
                        {
                            sY = Math.Abs(sY);
                        }

                        geoDXF.AppendLine(" A" + Convert.ToString(myObjDxfAll[i].Radius).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Radius).Replace(",", ".") + " 0 0 " + Convert.ToString(Convert.ToInt16(myObjDxfAll[i].KierZegara)).Replace(",", ".") + " "
                        + Convert.ToString(myObjDxfAll[i].X2).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Y2).Replace(",", "."));
                        //geoDXF.AppendLine(" a" + Convert.ToString(myObjDxfAll[i].Radius).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Radius).Replace(",", ".") + " 0 0 0 "
                        //+ Convert.ToString(sX).Replace(",", ".") + "," + Convert.ToString(sY).Replace(",", "."));

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "E":

                        //geoDXF.AppendLine(" M" + Convert.ToString(myObjDxfAll[i].X1).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].Y1).Replace(",", "."));
                        //geoDXF.AppendLine(" a" + Convert.ToString(myObjDxfAll[i].MajorA).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].MinorA).Replace(",", ".") + " 0 1 0 0.00001,0");

                        geoDXF.AppendLine(" M" + Convert.ToString(myObjDxfAll[i].CenterX).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].CenterY).Replace(",", "."));
                        geoDXF.AppendLine(" a" + Convert.ToString(myObjDxfAll[i].MajorA / 2).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].MinorA / 2).Replace(",", ".") + " 0 1 0 0.00001,0");

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "C":
     
                        geoDXF.AppendLine(" M" + Convert.ToString(myObjDxfAll[i].CenterX).Replace(",", ".") + "," + Convert.ToString(myObjDxfAll[i].CenterY).Replace(",", "."));
                        geoDXF.AppendLine(" a" + Convert.ToString(myObjDxfAll[i].Radius).Replace(",",".") + "," + Convert.ToString(myObjDxfAll[i].Radius).Replace(",", ".") + " 0 1 0 0.00001,0");

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    default:
                        myObjDxfAll[i].Juz_dodany = true;
                        myObjDxfAll[i].IntWezel = -99;
                        break;
                }

              //  myPathGeometry.Figures.Add(myPathFigure);

                //if (MessageBox.Show("Wyjdź z pętli?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                //{
                //    //goto EndWhile;
                //}
                //else
                //{
                //    goto EndWhile;
                //}

                //query = from u in myObjDxfAll
                //        where u.Juz_dodany == false
                //        select u;

                var find_i = from u in myObjDxfAll
                          where u.Juz_dodany == false
                            && u.X1 >= X-0.01 && u.X1 <= X + 0.01 && u.Y1 >= Y - 0.01 && u.Y1 <= Y + 0.01
                          select u.Id;
                if (find_i.Count() > 0)
                {
                    i = (int)find_i.Min();
                    id_wezel = i;
                }
                else
                {
                    //i= myObjDxfAll.Min()
                    var find_i_min = from u in myObjDxfAll
                                 where u.Juz_dodany == false
                                 select u.Id;
                    if (find_i_min.Count() > 0)
                    {
                        i = (int)find_i_min.Min();
                        myObjDxfAll[i].IntWezel = 0;
                    }
                    else
                    {
                        myObjDxfAll[i].IntWezel = -1;
                        i++;   
                    }
                }

                if (myObjDxfAll.Count - 1 < i)
                {
                    goto EndWhile;
                }

            }

        EndWhile:


            var path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                //Data = g,
                //Data = myPathGeometry,
                Data = Geometry.Parse(geoDXF.ToString()),
                Stretch = Stretch.Fill
            };
            path.Tag = DesignerCanvas.Children.Count;

            contentControl.Tag = DesignerCanvas.Children.Count;
       
            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = path;
            border.Tag = DesignerCanvas.Children.Count;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);
  

        }


        private Path GetPath(string aColor)
        {
            Path imagePath = new Path();
            SolidColorBrush colorBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(aColor);
            imagePath.Stroke = colorBrush;
            imagePath.StrokeThickness = 2;
            imagePath.Fill = colorBrush;

            return imagePath;
        }

        public Point GetFinalPoint(Point startPoint, Point centerPoint, double arcAngle)
        {
            double radius = Math.Sqrt(Math.Pow((startPoint.X - centerPoint.X), 2) + Math.Pow((startPoint.Y - centerPoint.Y), 2));
            double alpha = Math.Abs(startPoint.Y - centerPoint.Y) == radius ? 90.0 : Math.Asin(Math.Abs(startPoint.Y - centerPoint.Y) / radius);
            arcAngle = alpha - arcAngle;
            //Calculate the final point
            return new Point
            {
                X = Math.Cos(Math.PI * arcAngle / 180.0) * radius + centerPoint.X,
                Y = Math.Abs(startPoint.Y - centerPoint.Y) - Math.Sin(Math.PI * arcAngle / 180.0) * radius + startPoint.Y
            };
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private void btnGetChildren_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in DesignerCanvas.Children)
            {
                if (element is StackPanel)
                {
                    StackPanel sp = element as StackPanel;

                    foreach (UIElement spElement in sp.Children)
                    {
                        if (spElement is Button)
                        {
                            Button b = spElement as Button;
                            MessageBox.Show(b.Content.ToString());
                        }
                    }

                }
            }
        }


        private void canvas_MouseSelect(object sender, MouseEventArgs e)
        {

          is_select = Convert.ToInt32(((System.Windows.FrameworkElement)e.Source).Tag);

            if (is_select < 0) return;

            foreach (Control child in DesignerCanvas.Children)
            {
                Selector.SetIsSelected(child, false);
            }

            foreach (Control child in DesignerCanvas.Children)
            {
                Selector.SetIsSelected(DesignerCanvas.Children[is_select], true);
            }

        }

        private void BClickEditData(object sender, RoutedEventArgs e)
        {

            foreach (Control child in DesignerCanvas.Children)
            {
                Selector.SetIsSelected(child, false);
            }

             if (DesignerCanvas.Children.Count == 0)
            {
                return;
            }

            W_EDIT_DATA_DXF FRM = new W_EDIT_DATA_DXF();

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();
       
            myObjDxfAll = (List<CDxfOBJ>)ALL_DXF_OBJ[is_select];

            FRM.DG_DXF_OBJ.ItemsSource = myObjDxfAll;

           // FRM.Canvas_TXT.Text = DesignerCanvas.Children[is_select].ToString();


            foreach (UIElement element in DesignerCanvas.Children)
            {
                var contentControl = new ContentControl();
                contentControl = ((System.Windows.Controls.ContentControl)element);
                var content = contentControl.Content;

                if (Convert.ToString(((System.Windows.FrameworkElement)content).Tag) == Convert.ToString(is_select))
                   {
                    var chil = ((System.Windows.Controls.Decorator)content).Child;
                    var dat = ((System.Windows.Shapes.Path)chil).Data;
                    FRM.Canvas_TXT.Text = dat.ToString().Replace(",",".").Replace(";", " ");
                }


            }

            if (FRM.ShowDialog() == true)
            {

                chOdbChek.IsChecked = false;

                PathGeometryFDXF("", true);

                if (DesignerCanvas.Children.Count > 0)
                {
                    if (DesignerCanvas.Children[0] is System.Windows.Controls.Control)
                    {
                        foreach (Control child in DesignerCanvas.Children)
                        {
                            Selector.SetIsSelected(child, false);
                        }
                    }
                }

            };

        }

        private void BClickDellData(object sender, RoutedEventArgs e)
        {
            if (is_select == -1) return;

                DesignerCanvas.Children.RemoveAt(is_select);
                ALL_DXF_OBJ.RemoveAt(is_select);
                is_select = -1;
            int i = 0;
            foreach (Control child in DesignerCanvas.Children)
            {
                child.Tag = i;
                i++;
            }
        }

        private void BClick_LoadDXF(object sender, RoutedEventArgs e)
        {
            //****************************************************************************************************************************************
            //****************************************************************************************************************************************

            LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

            OpenFileDialog OpenFileDialog_DXF = new OpenFileDialog();
            OpenFileDialog_DXF.Filter = "plik DXF (*.dxf)|*.dxf";
            OpenFileDialog_DXF.FileName = "dbdt_file";

            if (OpenFileDialog_DXF.ShowDialog() == true)
            {

                //  PathGeometryFromDXF(OpenFileDialog_DXF.FileName);
                PathGeometryFDXF(OpenFileDialog_DXF.FileName,false);
                
                chOdbChek.IsChecked = false;

                if (DesignerCanvas.Children.Count > 0)
                {
                    if (DesignerCanvas.Children[0] is System.Windows.Controls.Control)
                    {
                        foreach (Control child in DesignerCanvas.Children)
                        {
                            Selector.SetIsSelected(child, false);
                        }
                    }
                }
            }


            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
        }
        private void BClick_SaveDXF(object sender, RoutedEventArgs e)
        {

            // return;

            //string savedCanvasString = System.Windows.Markup.XamlWriter.Save(((System.Windows.Shapes.Path)abcd).Data);

            SaveFileDialog SaveFileDialog_DXF = new SaveFileDialog();
            SaveFileDialog_DXF.Filter = "plik DXF (*.dxf)|*.dxf";
            SaveFileDialog_DXF.FileName = "dbdt_file";

            if (SaveFileDialog_DXF.ShowDialog() == true)
            {
                System.IO.FileInfo fiA = new System.IO.FileInfo(SaveFileDialog_DXF.FileName);

                if (fiA.Exists == true)
                    fiA.Delete();

                //****************************************************************************************************************************************
                //****************************************************************************************************************************************
                //DxfDocument dxf = new DxfDocument();
                //Angular2LineDimension d = null;
                //DxfDocument doc = DxfDocument.Load("C:\\Users\\Leszek Mularski\\Documents\\SOLID_WORKS\\WYDRUKI_3D\\Część1.DXF\");
                //foreach (Dimension dim in doc.Entities.Dimensions)
                //{
                //    dim.Block = DimensionBlock.Build(dim);
                //    if (dim.DimensionType == DimensionType.Angular)
                //        d = (Angular2LineDimension)dim;
                //}

                //****************************************************************************************************************************************
                //****************************************************************************************************************************************



                //   System.IO.FileStream FsA = new System.IO.FileStream(fiA.FullName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None);
                //   System.IO.StreamWriter SwFromFileStreamDefaultEncA = new System.IO.StreamWriter(FsA, System.Text.Encoding.Default);

                //    SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfInit());
                //     SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfText(0, 0, 12, 0, 4, "Wygenerowano z programu OKNOPLAST - LM : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute));
                // SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfLine(C_OB_SZYBA.Item(i).X, C_OB_SZYBA.Item(i).Y, C_OB_SZYBA.Item(i + 1).X, C_OB_SZYBA.Item(i + 1).y, 1, ""));

                //M = przenieś do
                //L = linia do
                //H = linia pozioma do
                //V = linia pionowa do
                //C = krzywa do
                //S = gładka krzywa do
                //Q = kwadratowa krzywa Béziera
                //T = gładka kwadratowa krzywa Bézierato
                //A = łuk eliptyczny
                //Z = ścieżka zamnknij

                ArrayList linieDXF = new ArrayList();

                foreach (UIElement element in DesignerCanvas.Children)
                {
                    var contentControl = new ContentControl();
                    contentControl = ((System.Windows.Controls.ContentControl)element);
                    var content = contentControl.Content;
                    var chil = ((System.Windows.Controls.Decorator)content).Child;
                    var dat = ((System.Windows.Shapes.Path)chil).Data;

                    string[] splitLine;
                    splitLine = (Convert.ToString(dat).Split(new char[] { ' ', '\t' }));

                    string ostatniakomenda = "!";

                    for (int i = 0; i < splitLine.Length; i++)
                    {
                        string splitLn;
                        splitLn = ostatniakomenda + splitLine[i].Trim();
                        var bytes = Encoding.UTF8.GetBytes(Convert.ToString(splitLn));

                        bool generuj_linieDXF = true;
                        bool generuj_linie_konic_liniDXF = false;
                        string str = "";

                        int j = 0;

                        foreach (byte pojz in bytes)
                        {
                            j++;

                            switch (pojz)
                            {
                                case 65:
                                    //A
                                    str += "A";
                                    generuj_linieDXF = true;
                                    ostatniakomenda = "A";
                                    break;
                                case 97:
                                    //a
                                    str += "a";
                                    generuj_linieDXF = true;
                                    ostatniakomenda = "a";
                                    break;
                                case 59:
                                    //;
                                    str += ";";
                                    break;
                                case 76:
                                    //L
                                    str += "L";
                                    generuj_linieDXF = true;
                                    ostatniakomenda = "L";
                                    break;
                                case 77:
                                    //M
                                    str += "M";
                                    generuj_linieDXF = true;
                                    ostatniakomenda = "M";
                                    break;
                                case 48:
                                    //0
                                    str += "0";
                                    break;
                                case 49:
                                    //1
                                    str += "1";
                                    break;
                                case 50:
                                    //2
                                    str += "2";
                                    break;
                                case 51:
                                    //3
                                    str += "3";
                                    break;
                                case 52:
                                    //4
                                    str += "4";
                                    break;
                                case 53:
                                    //5
                                    str += "5";
                                    break;
                                case 54:
                                    //6
                                    str += "6";
                                    break;
                                case 55:
                                    //7
                                    str += "7";
                                    break;
                                case 56:
                                    //8
                                    str += "8";
                                    break;
                                case 57:
                                    //9
                                    str += "9";
                                    break;
                                case 46:
                                    //.
                                    str += ".";
                                    break;
                                case 44:
                                    //,
                                    str += ",";
                                    break;
                                case 33:
                                    //,
                                    str += "|";
                                    break;
                                default:
                                    str += "?";
                                    break;
                            }

                            if (generuj_linieDXF == true || generuj_linie_konic_liniDXF == true)
                            {
                                if (str.Length > 1)
                                {
                                    string stmp = str.Substring(str.Length - 1, 1);
                                    if (stmp == "A" || stmp == "a" || stmp == "L" || stmp == "M" || generuj_linie_konic_liniDXF == true)
                                    {
                                        string ostznak = stmp;
                                        if (generuj_linie_konic_liniDXF == true)
                                        {
                                            linieDXF.Add(str);
                                        }
                                        else
                                        {
                                            linieDXF.Add(str.Substring(0, str.Length - 1));
                                        }

                                        str = ostznak;
                                        generuj_linieDXF = false;
                                        generuj_linie_konic_liniDXF = false;
                                    }

                                    //string ostznak = str.Substring(str.Length - 1, 1);
                                    //sb.AppendLine(str.Substring(0, str.Length - 1));
                                    //str = ostznak;
                                    //generuj_linieDXF = false;
                                }
                                else if (linieDXF.Count == 0)
                                {
                                    generuj_linieDXF = false;
                                }
                                //
                            }

                            if (j == bytes.Length - 1)
                            {
                                generuj_linie_konic_liniDXF = true;
                            }

                        }

                        //  SwFromFileStreamDefaultEncA.WriteLine("----------------????????" + sb.ToString());

                        //linia DXF
                        //  var svg = Encoding.UTF8.GetString(bytes);
                    }

                    //{M0;0A100;100;0;0;1;100;100L100;200 0;200 0;0}
                    //{M0;0A50;50;0;0;1;100;100  L100;200 0;200 0;0}
                    //{M0;0
                    //->A100;100;0;0;1;100;100
                    //->L100;200
                    //->0;200
                    //->0;0}

                }

                CoverterSVG coverterSVG = new CoverterSVG();
                linieDXF = coverterSVG.ConvertSVGDXF(linieDXF);

                //   SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfEnd());
                //  SwFromFileStreamDefaultEncA.Flush();
                //  SwFromFileStreamDefaultEncA.Close();

                linieDXF.Clear();

            }
        }

    }
}

