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

        private void PathGeometryFDXF(string filename, byte edycja, double lokX = 50, double lokY = 50)
        {

            Style s = this.FindResource("DesignerItemStyle") as Style;
       
            var contentControl2 = new ContentControl();

            LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

            int id_wezel = 1;

            if (edycja == 0)
            {
                myObjDxfAll = load_save_dxf.LOAD(filename, DesignerCanvas.Children.Count);

                if (myObjDxfAll== null)
                {
                    return ;
                }

                ALL_DXF_OBJ.Add(myObjDxfAll);

            }
            else if (edycja == 1)
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
            else if (edycja == 2)
            {
                myObjDxfAll = (List<CDxfOBJ>)ALL_DXF_OBJ[is_select];

               // ALL_DXF_OBJ.Add(myObjDxfAll);

            }

            if (myObjDxfAll == null) return;

            if (myObjDxfAll.Count == 0) return;
            //double wys_canvas = DesignerCanvas.Height;

            //add Ellipse
            var contentControl = new ContentControl();

            var maxX = myObjDxfAll.Max(row => row.MaxX);
            var maxY = myObjDxfAll.Max(row => row.MaxY);

            contentControl.Width = Math.Abs(maxX);
            contentControl.Height = Math.Abs(maxY);
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, lokX);
            Canvas.SetLeft(contentControl, lokY);

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

            if (edycja != 2)
            {
                path.Tag = DesignerCanvas.Children.Count;
                contentControl.Tag = DesignerCanvas.Children.Count;
            }
            else
            {
                path.Tag = DesignerCanvas.Children.Count;
                contentControl.Tag = DesignerCanvas.Children.Count;
            }

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

        private void canvas_Right_MouseSelect(object sender, MouseButtonEventArgs e)
        {

            if (is_select < 0) return;

            var menuItem = sender as MenuItem;
            var contextMenu = menuItem?.Parent as ContextMenu;
            var ellipse = contextMenu?.PlacementTarget as Ellipse;
            DesignerCanvas.Children.Remove(ellipse);

            foreach (Control child in DesignerCanvas.Children)
            {
                Selector.SetIsSelected(DesignerCanvas.Children[is_select], true);
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

             if (is_select < 0) return;

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

                var aktAngle =  contentControl.RenderTransform;

                if (Convert.ToString(((System.Windows.FrameworkElement)content).Tag) == Convert.ToString(is_select))
                   {
                    var chil = ((System.Windows.Controls.Decorator)content).Child;
                    var dat = ((System.Windows.Shapes.Path)chil).Data;
                    FRM.Canvas_TXT.Text = dat.ToString().Replace(",",".").Replace(";", " ");
       
                    var tt = new ToolTip();

                    if (aktAngle.IsSealed == false)
                    {
                        tt.Content = "Kąt obrotu: " + ((System.Windows.Media.RotateTransform)aktAngle).Angle;
                    }
                    else
                    {
                        tt.Content = "Kąt obrotu: 0";
                    }

                    FRM.Canvas_TXT.ToolTip = tt;
                  }

            }

            if (FRM.ShowDialog() == true)
            {

                chOdbChek.IsChecked = false;

                PathGeometryFDXF("", 1);

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
        private void BClickCopyData(object sender, RoutedEventArgs e)
        {
            if (is_select == -1) return;

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();
            myObjDxfAll = (List<CDxfOBJ>)ALL_DXF_OBJ[is_select];

            List<CDxfOBJ> myObjDxfAllC = new List<CDxfOBJ>();

            if (ALL_DXF_OBJ == null) return;

            int id_s = ALL_DXF_OBJ.Count;

            for (int j = 0; j < myObjDxfAll.Count; j++)
            {
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_s;
                myObjDxf.Juz_dodany = false;
                myObjDxf.X1 = myObjDxfAll[j].X1;
                myObjDxf.Y1 = myObjDxfAll[j].Y1;
                myObjDxf.X2 = myObjDxfAll[j].X2;
                myObjDxf.Y2 = myObjDxfAll[j].Y2;
                myObjDxf.Typ = myObjDxfAll[j].Typ;
                myObjDxf.Radius = myObjDxfAll[j].Radius;
                myObjDxf.CenterX = myObjDxfAll[j].CenterX;
                myObjDxf.CenterY = myObjDxfAll[j].CenterY;
                myObjDxf.EndA = myObjDxfAll[j].EndA;
                myObjDxf.StartA = myObjDxfAll[j].StartA;
                myObjDxf.MajorA = myObjDxfAll[j].MajorA;
                myObjDxf.MinorA = myObjDxfAll[j].MinorA;
                myObjDxf.Id = myObjDxfAll[j].Id;
                myObjDxf.IntHandle = myObjDxfAll[j].IntHandle;
                myObjDxf.IntWezel = myObjDxfAll[j].IntWezel;
                myObjDxf.KierZegara = myObjDxfAll[j].KierZegara;
                myObjDxf.MaxX = myObjDxfAll[j].MaxX;
                myObjDxf.MaxY = myObjDxfAll[j].MaxY;

                myObjDxfAllC.Add(myObjDxf);
            }

            ALL_DXF_OBJ.Add(myObjDxfAllC);

            is_select = id_s;

            PathGeometryFDXF("", 2, 99 + is_select, 88 + is_select);

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
                PathGeometryFDXF(OpenFileDialog_DXF.FileName, 0);

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
            if (is_select == -1) return;

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

                int katObr = 0;

                foreach (UIElement element in DesignerCanvas.Children)
                {
                    var contentControl = new ContentControl();
                    contentControl = ((System.Windows.Controls.ContentControl)element);
                    var content = contentControl.Content;

                    var aktAngle = contentControl.RenderTransform;

                    if (Convert.ToString(((System.Windows.FrameworkElement)content).Tag) == Convert.ToString(is_select))
                    {
       
                        if (aktAngle.IsSealed == false)
                        {
                            katObr = (int)((RotateTransform)aktAngle).Angle;
                        }
                        else
                        {
                            katObr = 0;
                        }
                    }

                }

                LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();
                load_save_dxf.SAVE(SaveFileDialog_DXF.FileName, ALL_DXF_OBJ, is_select, katObr);

                //****************************************************************************************************************************************
                //****************************************************************************************************************************************

            }
        }

        private void BClick_SaveDXFALL(object sender, RoutedEventArgs e)
        {
            if (is_select == -1) return;

            //string savedCanvasString = System.Windows.Markup.XamlWriter.Save(((System.Windows.Shapes.Path)abcd).Data);

            SaveFileDialog SaveFileDialog_DXF = new SaveFileDialog();
            SaveFileDialog_DXF.Filter = "plik DXF (*.dxf)|*.dxf";
            SaveFileDialog_DXF.FileName = "dbdt_file_all";

            if (SaveFileDialog_DXF.ShowDialog() == true)
            {
                System.IO.FileInfo fiA = new System.IO.FileInfo(SaveFileDialog_DXF.FileName);

                if (fiA.Exists == true)
                    fiA.Delete();

                LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

                load_save_dxf.SAVE_ALL(SaveFileDialog_DXF.FileName, ALL_DXF_OBJ, DesignerCanvas);

                //****************************************************************************************************************************************
                //****************************************************************************************************************************************

            }
        }

    }
}

