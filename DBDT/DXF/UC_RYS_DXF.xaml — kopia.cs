using DBDT.Konfiguracja;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
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

        List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

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
            contentControl.Height = 100;
            contentControl.MinHeight = 5;
            contentControl.MinWidth = 5;
            contentControl.Padding = new Thickness(1.0);

            Canvas.SetTop(contentControl, 100);
            Canvas.SetLeft(contentControl, 100);

            contentControl.Style = s;

            var contentControl2 = new ContentControl();

            //"M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2C6.47,2 2,6.47 2,12C2,17.53 6.47,22 12,22C17.53,22 22,17.53 22,12C22,6.47 17.53,2 12,2M14.59,8L12,10.59L9.41,8L8,9.41L10.59,12L8,14.59L9.41,16L12,13.41L14.59,16L16,14.59L13.41,12L16,9.41L14.59,8Z"
            var g = new StreamGeometry();



            // Utwórz łuk. Narysuj łuk od punktu początkowego do 200 100 z określonymi parametrami.
            //  ctx.ArcTo(new Point(200, 100), new Size(100, 50), 45 /* kąt obrotu */, true /* to duży łuk */,
            //          SweepDirection.Odwrotnie do ruchu wskazówek zegara, prawda /* obrysowana */, fałsz /* to płynne łączenie */);



            //List<Point> pointList = new List<Point>();
            //pointList.Add(new Point(0, 0));
            //pointList.Add(new Point(100, 50));
            //pointList.Add(new Point(100, 0));

            // End point for second quadratic Bezier curve.
            // pointList.Add(new Point(400, 100));
            //gc.BeginFigure(new Point(10, 100), true /* is filled */, false /* is closed */);

            //gc.ArcTo(new Point(200, 100), new Size(100, 50), 45 /* rotation angle */, true /* is large arc */,
            //          SweepDirection.Counterclockwise, true /* is stroked */, false /* is smooth join */);

            // using (var gc = g.Open())
            //{
            StreamGeometryContext gc = g.Open();
            gc.BeginFigure(
                    startPoint: new Point(0, 0),
                    isFilled: false,
                    isClosed: false);

            gc.ArcTo(
                point: new Point(100, 50),//punkt końcowy x=100; y=50
                size: new Size(100, 50),
                rotationAngle: 45d,
                isLargeArc: false,
                sweepDirection: SweepDirection.Clockwise,
                isStroked: true,
                isSmoothJoin: true);

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

            gc.Close();

            //  gc.set(DefiningGeometryArc(2, 0, 355.99));

            // }

            //g = DefiningGeometryArc(2, 0, 355.99);

            // ((System.Windows.Shapes.Path)((System.Windows.Controls.Decorator)contentControl.Content).Child).Data




            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(10, 50);
            myPathFigure.Segments.Add(
                new BezierSegment(
                    new Point(100, 0),
                    new Point(200, 200),
                    new Point(300, 100),
                    true /* IsStroked */  ));
            myPathFigure.Segments.Add(
                new LineSegment(
                    new Point(400, 100),
                    true /* IsStroked */ ));
            myPathFigure.Segments.Add(
                new ArcSegment(
                    new Point(200, 100),
                    new Size(50, 50),
                    45,
                    true, /* IsLargeArc */
                    SweepDirection.Clockwise,
                    true /* IsStroked */ ));




            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures.Add(myPathFigure);



            var path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                //Data = g,
                Data = myPathGeometry,
                Stretch = Stretch.Fill
            };

            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = path;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);

        }

        private void PathGeometryFDXF(string filename)
        {
            Style s = this.FindResource("DesignerItemStyle") as Style;
       
            var contentControl2 = new ContentControl();

            LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

            // List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

            myObjDxfAll = load_save_dxf.LOAD(filename);

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

            // myObjDxfAll =  load_save_dxf.LOAD("test.dxf");
            //myObjDxfAll = load_save_dxf.LOAD(filename);

            PathGeometry myPathGeometry = new PathGeometry();

            //  StreamGeometryContext myStreamGeometryContext;

            PathFigure myPathFigure = new PathFigure();

            goto EndWhile;

            int i = 0;
            double X = 0;
            double Y = 0;
            //for (int i = 0; i < myObjDxfAll.Count; i++)
            var query = from u in myObjDxfAll
                        where u.Juz_dodany == false
                        select u;
            while (query.Count() != 0) // condition
            {

                switch (myObjDxfAll[i].Typ)
                {
                    case "L":
                        //myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        //myPathFigure.Segments.Add(
                        //new LineSegment(
                        //    new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1),
                        //    true /* IsStroked */ ));
                       // if (myPathGeometry.Figures.Count == 0)
                        //{
                            myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        //}

                        myPathFigure.Segments.Add(
                            new LineSegment(
                                new Point(myObjDxfAll[i].X2, myObjDxfAll[i].Y2),
                                true /* IsStroked */ ));
                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "A":

                        // if (myPathGeometry.Figures.Count == 0)
                        // {
                        //    myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        //}

                        //var cas = DefiningGeometryArc(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA);

                        //myPathFigure.Segments.Add(
                        //    new ArcSegment(
                        //        new Point(myObjDxfAll[i].Y1, myObjDxfAll[i].Y1),
                        //        new Size(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius),
                        //        myObjDxfAll[i].EndA,
                        //        true, /* IsLargeArc */
                        //        SweepDirection.Clockwise,
                        //        true /* IsStroked */ ));
                        ////  myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        ///

                        Point pS = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        Point pC = new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY);
                        int ka = (int)(myObjDxfAll[i].EndA);

                        double szukX = myObjDxfAll[i].X1 - myObjDxfAll[i].CenterX - myObjDxfAll[i].Radius;
                        double szukY = myObjDxfAll[i].Y1 - myObjDxfAll[i].CenterY - myObjDxfAll[i].Radius;

                        double maxWidth = Math.Max(0.0, RenderSize.Width - myObjDxfAll[i].Radius);
                        double maxHeight = Math.Max(0.0, RenderSize.Height - myObjDxfAll[i].Radius);

                        //Console.WriteLine(string.Format("* maxWidth={0}, maxHeight={1}", maxWidth, maxHeight));

                        double xStart = maxWidth / 2.0 * Math.Cos(myObjDxfAll[i].StartA * Math.PI / 180.0);
                        double yStart = maxHeight / 2.0 * Math.Sin(myObjDxfAll[i].StartA * Math.PI / 180.0);

                        double xEnd = maxWidth / 2.0 * Math.Cos(myObjDxfAll[i].EndA * Math.PI / 180.0);
                        double yEnd = maxHeight / 2.0 * Math.Sin(myObjDxfAll[i].EndA * Math.PI / 180.0);

                        Point pX = GetFinalPoint(pS, pC, ka);

                        myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        myPathFigure.Segments.Add(
                                    new ArcSegment(
                                        new Point(-25, 0),
                                        new Size(25, 25),
                                        myObjDxfAll[i].StartA,
                                        false, /* IsLargeArc */
                                        SweepDirection.Clockwise,
                                        true /* IsStroked */ ));

                    
                        //Point pS = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        //Point pC = new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY);
                        //int ka = (int)(myObjDxfAll[i].EndA - myObjDxfAll[i].StartA);

                        //myPathFigure = DefiningGeometryArc(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA, true,
                        //pS, pC, ka);

                        //myPathFigure = DefiningGeometryArc(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA,true);
                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "E":
                        //myPathFigure.Segments.Add(
                        //    new ArcSegment(
                        //        new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY),
                        //        new Size(myObjDxfAll[i].MajorA, myObjDxfAll[i].MinorA),
                        //        myObjDxfAll[i].EndA,
                        //        true, /* IsLargeArc */
                        //        SweepDirection.Clockwise,
                        //        true /* IsStroked */ ));
                        //myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        if (myPathGeometry.Figures.Count == 0)
                        {
                             myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        }
                        PathFigure myPathFigureE = new PathFigure();
                        Point pS1 = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        Point pC1 = new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY);
                        int ka1 = (int)(myObjDxfAll[i].EndA - myObjDxfAll[i].StartA);
                        myPathFigureE = DefiningGeometryArc(myObjDxfAll[i].MajorA, myObjDxfAll[i].MinorA, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA,true,
                            pS1, pC1, ka1);
                        myPathGeometry.Figures.Add(myPathFigureE);

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    case "C":
                        if (myPathGeometry.Figures.Count == 0)
                        {
                          myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        }
                        PathFigure myPathFigureC = new PathFigure();
                        Point pS2 = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                        Point pC2 = new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY);
                        int ka2 = (int)(myObjDxfAll[i].EndA - myObjDxfAll[i].StartA);
                        myPathFigureC = DefiningGeometryArc(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA-0.00001,true,
                            pS2, pC2, ka2);
                        myPathGeometry.Figures.Add(myPathFigureC);

                        X = myObjDxfAll[i].X2;
                        Y = myObjDxfAll[i].Y2;
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                    default:
                        myObjDxfAll[i].Juz_dodany = true;
                        break;
                }

              myPathGeometry.Figures.Add(myPathFigure);

                if (MessageBox.Show("Wyjdź z pętli?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //goto EndWhile;
                }
                else
                {
                    goto EndWhile;
                }



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
                    }
                    else
                    {
                        i++;
                    }
                }

                if (myObjDxfAll.Count - 1 < i)
                {
                    goto EndWhile;
                }

            }

        EndWhile:


            //Path testPath = GetPath("blue");
            //testPath.Data = Geometry.Parse("M100,100 l0,100 M100,100 a25,25 0 0 1 25,-25 M125,75 l125,0 M250,75 a25,25 0 0 1 25,25 M275,100 l0,100");//Geometry.Parse("M100,180L220,180 M220,180L220,152 M220,152L217,150 M217,150L182,150 M180,147L180,132 M182,130L217,130 M217,130L220,127 M220,127L220,80 M220,80L100,80 M100,80L100,180 M182,150L180,147 M180,132L182,130 ");
            //var area = testPath.Data.GetArea();

            //canv.Children.Add(testPath);

            //myPathGeometry.Figures.Add(myPathFigure);
            var path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                //Data = g,
               //Data = myPathGeometry,
               Data = Geometry.Parse("M100,100 l0,100 M100,100 a25,25 0 0 1 25,-25 M125,75 l125,0 M250,75 a25,25 0 0 1 25,25 M275,100 l0,100"),
            Stretch = Stretch.Fill
            };

            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = path;
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


        private void PathGeometryFromDXF(string filename)
        {
            //później wywal
            Style s = this.FindResource("DesignerItemStyle") as Style;

            var contentControl2 = new ContentControl();

            LOAD_SAVE_DXF load_save_dxf = new LOAD_SAVE_DXF();

            // List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();

            myObjDxfAll = load_save_dxf.LOAD(filename);

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

            // myObjDxfAll =  load_save_dxf.LOAD("test.dxf");
            //myObjDxfAll = load_save_dxf.LOAD(filename);

            PathGeometry myPathGeometry = new PathGeometry();

            //  StreamGeometryContext myStreamGeometryContext;

            PathFigure myPathFigure = new PathFigure();

            for (int i = 0; i < myObjDxfAll.Count; i++)
            {
                switch (myObjDxfAll[i].Typ)
                {
                    case "L":
                        myPathFigure.StartPoint = new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1);
                            //myPathFigure.Segments.Add(
                            //new LineSegment(
                            //    new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1),
                            //    true /* IsStroked */ ));
                        myPathFigure.Segments.Add(
                            new LineSegment(
                                new Point(myObjDxfAll[i].X2, myObjDxfAll[i].Y2),
                                false /* IsStroked */ ));
                        break;
                    case "A":
                        //var cas = DefiningGeometryArc(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius, myObjDxfAll[i].StartA, myObjDxfAll[i].EndA);
                       myPathFigure.Segments.Add(
                            new ArcSegment(
                                new Point(myObjDxfAll[i].X1, myObjDxfAll[i].Y1),
                                new Size(myObjDxfAll[i].Radius, myObjDxfAll[i].Radius),
                                myObjDxfAll[i].EndA - myObjDxfAll[i].StartA,
                                true, /* IsLargeArc */
                                SweepDirection.Clockwise,
                                false /* IsStroked */ ));
                        break;
                    case "E":
                        myPathFigure.Segments.Add(
                            new ArcSegment(
                                new Point(myObjDxfAll[i].CenterX, myObjDxfAll[i].CenterY),
                                new Size(myObjDxfAll[i].MajorA, myObjDxfAll[i].MinorA),
                                myObjDxfAll[i].EndA,
                                true, /* IsLargeArc */
                                SweepDirection.Clockwise,
                                false /* IsStroked */ ));
                        break;
                }

                myPathGeometry.Figures.Add(myPathFigure);
            }





            //    PathFigure myPathFigure = new PathFigure();
            //myPathFigure.StartPoint = new Point(10, 50);
            //myPathFigure.Segments.Add(
            //    new BezierSegment(
            //        new Point(100, 0),
            //        new Point(200, 200),
            //        new Point(300, 100),
            //        true /* IsStroked */  ));
            //myPathFigure.Segments.Add(
            //    new LineSegment(
            //        new Point(400, 100),
            //        true /* IsStroked */ ));
            //myPathFigure.Segments.Add(
            //    new ArcSegment(
            //        new Point(200, 100),
            //        new Size(50, 50),
            //        45,
            //        true, /* IsLargeArc */
            //        SweepDirection.Clockwise,
            //        true /* IsStroked */ ));




            //PathGeometry myPathGeometry = new PathGeometry();
            //myPathGeometry.Figures.Add(myPathFigure);





            var path = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                //Data = g,
                Data = myPathGeometry,
                Stretch = Stretch.Fill
            };

            var border = new Border { Background = Brushes.Transparent, VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
            border.IsHitTestVisible = false;
            border.Child = path;
            contentControl.Content = border;
            DesignerCanvas.Children.Add(contentControl);

        }

        public PathFigure DefiningGeometryArc(double StrokeThicknessW, double StrokeThicknessH, double StartAngle, 
            double EndAngle, bool IsStroked, Point startPoint, Point centerPoint, double arcAngle)
        {

            double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThicknessW);
            double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThicknessH);

            PathFigure myPathFigure = new PathFigure();

            //Console.WriteLine(string.Format("* maxWidth={0}, maxHeight={1}", maxWidth, maxHeight));

            double xStart = maxWidth / 2.0 * Math.Cos(StartAngle * Math.PI / 180.0);
            double yStart = maxHeight / 2.0 * Math.Sin(StartAngle * Math.PI / 180.0);

            double xEnd = maxWidth / 2.0 * Math.Cos(EndAngle * Math.PI / 180.0);
            double yEnd = maxHeight / 2.0 * Math.Sin(EndAngle * Math.PI / 180.0);

            myPathFigure.StartPoint = new Point((RenderSize.Width / 2.0) + xStart, (RenderSize.Height / 2.0) - yStart);
            myPathFigure.Segments.Add(
                        new ArcSegment(
                            GetFinalPoint(startPoint, centerPoint, arcAngle),
                            new Size(maxWidth / 2.0, maxHeight / 2),
                            EndAngle,
                            true, /* IsLargeArc */
                            SweepDirection.Clockwise,
                            IsStroked /* IsStroked */ ));

            return myPathFigure;

            //double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThicknessW);
            //double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThicknessH);

            //PathFigure myPathFigure = new PathFigure();

            ////Console.WriteLine(string.Format("* maxWidth={0}, maxHeight={1}", maxWidth, maxHeight));

            //double xStart = maxWidth / 2.0 * Math.Cos(StartAngle * Math.PI / 180.0);
            //double yStart = maxHeight / 2.0 * Math.Sin(StartAngle * Math.PI / 180.0);

            //double xEnd = maxWidth / 2.0 * Math.Cos(EndAngle * Math.PI / 180.0);
            //double yEnd = maxHeight / 2.0 * Math.Sin(EndAngle * Math.PI / 180.0);

            //myPathFigure.StartPoint = new Point((RenderSize.Width / 2.0) + xStart, (RenderSize.Height / 2.0) - yStart);
            //myPathFigure.Segments.Add(
            //            new ArcSegment(
            //                new Point((RenderSize.Width / 2.0) + xEnd, (RenderSize.Height / 2.0) - yEnd),
            //                new Size(maxWidth / 2.0, maxHeight / 2),
            //                0.0,
            //                true, /* IsLargeArc */
            //                SweepDirection.Clockwise,
            //                IsStroked /* IsStroked */ ));

            //return myPathFigure;

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

        public StreamGeometryContext DefiningGeometryArcOLD(double StrokeThicknessW, double StrokeThicknessH, double StartAngle, double EndAngle)
        {

            double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThicknessW);
            double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThicknessH);
            //Console.WriteLine(string.Format("* maxWidth={0}, maxHeight={1}", maxWidth, maxHeight));

            double xStart = maxWidth / 2.0 * Math.Cos(StartAngle * Math.PI / 180.0);
            double yStart = maxHeight / 2.0 * Math.Sin(StartAngle * Math.PI / 180.0);

            double xEnd = maxWidth / 2.0 * Math.Cos(EndAngle * Math.PI / 180.0);
            double yEnd = maxHeight / 2.0 * Math.Sin(EndAngle * Math.PI / 180.0);

            StreamGeometry geom = new StreamGeometry();
            StreamGeometryContext ctx = geom.Open();
            // using (StreamGeometryContext ctx = geom.Open())
            // {
            ctx.BeginFigure(
                new Point((RenderSize.Width / 2.0) + xStart,
                          (RenderSize.Height / 2.0) - yStart),
                false,
                false);
            ctx.ArcTo(
                new Point((RenderSize.Width / 2.0) + xEnd,
                          (RenderSize.Height / 2.0) - yEnd),
                new Size(maxWidth / 2.0, maxHeight / 2),
                0.0,     // rotationAngle
                (EndAngle - StartAngle) > 180,   // greater than 180 deg?
                SweepDirection.Counterclockwise,
                true,    // isStroked
                true);
            // }

            return ctx;
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

        private void BClickEditData(object sender, RoutedEventArgs e)
        {

            W_EDIT_DATA_DXF FRM = new W_EDIT_DATA_DXF();
            FRM.DG_DXF_OBJ.ItemsSource = myObjDxfAll;
            FRM.ShowDialog();

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
                PathGeometryFDXF(OpenFileDialog_DXF.FileName);
                
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
                                    if (stmp == "A" || stmp == "L" || stmp == "M" || generuj_linie_konic_liniDXF == true)
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

