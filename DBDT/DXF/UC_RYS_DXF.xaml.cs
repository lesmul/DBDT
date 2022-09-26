using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private void laduj_uc(object sender, RoutedEventArgs e)
        {
            return;

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

        public class Unit : Control
        {
            static Unit()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Unit), new FrameworkPropertyMetadata
                    (typeof(Unit)));
            }

            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();

                ContentControl CC = base.GetTemplateChild("NewCC") as ContentControl;
                CC.Style = (Style)FindResource("ItemStyle");
                Selector.SetIsSelected(CC, true);
            }
        }

        public class MainViewModel
        {
            public UC_RYS_DXF ChildView { get; set; }

            public MainViewModel()
            {
                ChildView = new UC_RYS_DXF();
            }
        }

        void AddItemsToCanvas()
        {
           // FillCircle();
            //nPolygon();
           PathGeometry();

           DesignerCanvas.UpdateLayout();
        }

        private void FillCircle() 
        {
            Style s = this.FindResource("DesignerItemStyle") as Style;

            //add Ellipse
            var contentControl = new ContentControl();
            contentControl.Width = 130;
            contentControl.Height = 130;
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, 50);
            Canvas.SetLeft(contentControl, 100);

            contentControl.Style = s;
            var ellipse = new Ellipse { Fill = Brushes.Blue, Stretch = Stretch.Fill, IsHitTestVisible = false };
            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = ellipse;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);
        }

        private void PathGeometry()
        {
            Style s = this.FindResource("DesignerItemStyle") as Style;

            //add Ellipse
            var contentControl = new ContentControl();
            contentControl.Width = 100;
            contentControl.Height = 200;
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, 100);
            Canvas.SetLeft(contentControl, 200);

            contentControl.Style = s;

            var g = new StreamGeometry();

            using (var gc = g.Open())
            {
                gc.BeginFigure(
                    startPoint: new Point(0, 0),
                    isFilled: false,
                    isClosed: false);

                gc.ArcTo(
                    point: new Point(100, 100),
                    size: new Size(100, 100),
                    rotationAngle: 0d,
                    isLargeArc: false,
                    sweepDirection: SweepDirection.Clockwise,
                    isStroked: true,
                    isSmoothJoin: false);
              gc.LineTo(
                    point: new Point(100, 200),
                    isStroked: true,
                    isSmoothJoin: false
                    );
              gc.LineTo(
                    point: new Point(0, 200),
                    isStroked: true,
                    isSmoothJoin: false
                    );
              gc.LineTo(
                  point: new Point(0, 0),
                  isStroked: true,
                  isSmoothJoin: false
                   );

            }

            var path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                Data = g,
                Stretch = Stretch.Fill
            };

            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = path;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);

        }

            private void nPolygon()
        {
            Style s = this.FindResource("DesignerItemStyle") as Style;

            //add Ellipse
            var contentControl = new ContentControl();
            contentControl.Width = 130;
            contentControl.Height = 130;
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, 50);
            Canvas.SetLeft(contentControl, 100);

            contentControl.Style = s;
        
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(0, 0));
            myPointCollection.Add(new Point(0, 1));
            myPointCollection.Add(new Point(1, 1));
            myPointCollection.Add(new Point(1, 0));

            Polygon myPolygon = new Polygon();
            myPolygon.Points = myPointCollection;
            //myPolygon.Fill = Brushes.Blue;
            //myPolygon.Width = 100;
            //myPolygon.Height = 100;
            myPolygon.Stretch = Stretch.Fill;
            myPolygon.Stroke = Brushes.White;
            myPolygon.StrokeThickness = 2;

            //System.Windows.UIElement

            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = myPolygon;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);
        }

        private void BClick(object sender, RoutedEventArgs e)
        {
           AddItemsToCanvas();

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
        private void BClick_SaveDXF(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SaveFileDialog_DXF = new SaveFileDialog();
            SaveFileDialog_DXF.Filter = "plik DXF (*.dxf)|*.dxf";
            SaveFileDialog_DXF.FileName = "dbdtdxf";

            if (SaveFileDialog_DXF.ShowDialog() == true)
            {
                System.IO.FileInfo fiA = new System.IO.FileInfo(SaveFileDialog_DXF.FileName);

                if (fiA.Exists == true)
                    fiA.Delete();

                System.IO.FileStream FsA = new System.IO.FileStream(fiA.FullName, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write, System.IO.FileShare.None);
                System.IO.StreamWriter SwFromFileStreamDefaultEncA = new System.IO.StreamWriter(FsA, System.Text.Encoding.Default);

                SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfInit());
                SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfText(0, 0, 12, 0, 4, "Wygenerowano z programu OKNOPLAST - LM : " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute));
                // SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfLine(C_OB_SZYBA.Item(i).X, C_OB_SZYBA.Item(i).Y, C_OB_SZYBA.Item(i + 1).X, C_OB_SZYBA.Item(i + 1).y, 1, ""));

               // var aa = DesignerCanvas.Children[0].GetType;

                SwFromFileStreamDefaultEncA.WriteLine(DXF.DxfEnd());

                SwFromFileStreamDefaultEncA.Flush();
                SwFromFileStreamDefaultEncA.Close();
            }
        }

    }
}

