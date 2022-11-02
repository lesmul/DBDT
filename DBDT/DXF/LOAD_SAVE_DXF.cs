using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Blocks;
using netDxf.Collections;
using netDxf.Entities;
using GTE = netDxf.GTE;
using netDxf.Header;
using netDxf.Objects;
using netDxf.Tables;
using netDxf.Units;
using Attribute = netDxf.Entities.Attribute;
using FontStyle = netDxf.Tables.FontStyle;
using Image = netDxf.Entities.Image;
using Point = netDxf.Entities.Point;
using Trace = netDxf.Entities.Trace;
using Vector2 = netDxf.Vector2;
using Vector3 = netDxf.Vector3;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Runtime.Remoting;
using System.Data.Entity.SqlServer;
using Microsoft.SqlServer.Server;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace DBDT.DXF
{
    internal class LOAD_SAVE_DXF
    {

        public List<CDxfOBJ> LOAD(string FilenName, int id_object_c)
        {
            DxfDocument dxf = new DxfDocument();
            dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;

            Angular2LineDimension d = null;

            DxfDocument doc = new DxfDocument();

            try
            {
                doc = DxfDocument.Load(FilenName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Błąd");
                return null;
            }

            // var limit_Rys = doc.Blocks["*Model_Space"];

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();//default constructor

            int id_object = id_object_c;

            //foreach (Dimension dim in doc.Entities.Dimensions)
            //{
            //    dim.Block = DimensionBlock.Build(dim);
            //    if (dim.DimensionType == DimensionType.Angular)
            //        d = (Angular2LineDimension)dim;
            //}

            foreach (Polyline2D pol2d in doc.Entities.Polylines2D)
            {
                var xppol2d = pol2d;

                for (int j = 1; j - 1 < xppol2d.Vertexes.Count; j++)
                {
                    CDxfOBJ myObjDxf = new CDxfOBJ();

                    myObjDxf.Id_object = id_object;
                    myObjDxf.Typ = "L";
                    myObjDxf.X1 = Math.Round(xppol2d.Vertexes[j - 1].Position.X, 3);
                    myObjDxf.Y1 = Math.Round(xppol2d.Vertexes[j - 1].Position.Y, 3);

                    if (j > xppol2d.Vertexes.Count - 1)
                    {
                        if (xppol2d.IsClosed == true)
                        {
                            myObjDxf.X2 = Math.Round(xppol2d.Vertexes[0].Position.X, 3);
                            myObjDxf.Y2 = Math.Round(xppol2d.Vertexes[0].Position.Y, 3);
                        }
                        else
                        {
                            myObjDxf.X2 = Math.Round(xppol2d.Vertexes[j - 1].Position.X, 3);
                            myObjDxf.Y2 = Math.Round(xppol2d.Vertexes[j - 1].Position.Y, 3);
                        }
                    }
                    else
                    {
                        myObjDxf.X2 = Math.Round(xppol2d.Vertexes[j].Position.X, 3);
                        myObjDxf.Y2 = Math.Round(xppol2d.Vertexes[j].Position.Y, 3);
                    }

                    myObjDxf.CenterX = 0;
                    myObjDxf.CenterY = 0;

                    myObjDxf.Handle = xppol2d.Handle;
                    myObjDxf.IntHandle = int.Parse(myObjDxf.Handle, System.Globalization.NumberStyles.HexNumber);

                    myObjDxf.Id = myObjDxfAll.Count;
                    myObjDxfAll.Add(myObjDxf);
                }
            }

            foreach (Ellipse pEllipses in doc.Entities.Ellipses)
            {
                var xpEllipses = pEllipses;
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "E";
                myObjDxf.CenterX = Math.Round(xpEllipses.Center.X, 3);
                myObjDxf.CenterY = Math.Round(xpEllipses.Center.Y, 3);
                myObjDxf.EndA = Math.Round(xpEllipses.EndAngle, 3);
                myObjDxf.MajorA = Math.Round(xpEllipses.MajorAxis, 3);
                myObjDxf.MinorA = Math.Round(xpEllipses.MinorAxis, 3);
                myObjDxf.X1 = Math.Round(myObjDxf.CenterX - myObjDxf.MajorA / 2, 3);
                myObjDxf.Y1 = Math.Round(myObjDxf.CenterY - myObjDxf.MinorA / 2, 3);
                myObjDxf.X2 = Math.Round(myObjDxf.CenterX + myObjDxf.MajorA / 2, 3);
                myObjDxf.Y2 = Math.Round(myObjDxf.CenterY + myObjDxf.MinorA / 2, 3);
                myObjDxf.StartA = xpEllipses.StartAngle;
                myObjDxf.StartA = xpEllipses.EndAngle;

                double radian = xpEllipses.StartAngle * Math.PI / 180;
                double sinus = Math.Sin(radian);
                double cosinus = Math.Cos(radian);

                myObjDxf.MaxX = Math.Round((myObjDxf.MajorA / 2) * Math.Cos(Math.PI * xpEllipses.StartAngle / 180.0), 3) * 2;
                myObjDxf.MaxY = Math.Round((myObjDxf.MinorA / 2) * Math.Cos(Math.PI * xpEllipses.StartAngle / 180.0), 3) * 2;

                myObjDxf.Handle = xpEllipses.Handle;
                myObjDxf.IntHandle = int.Parse(myObjDxf.Handle, System.Globalization.NumberStyles.HexNumber);

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            foreach (Line pLines in doc.Entities.Lines)
            {
                var xpLines = pLines;
                //linieSVG.Add("L;" + pLines.StartPoint + ";" + pLines.EndPoint);
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "L";
                myObjDxf.X1 = Math.Round(pLines.StartPoint.X, 3);
                myObjDxf.Y1 = Math.Round(pLines.StartPoint.Y, 3);
                myObjDxf.X2 = Math.Round(pLines.EndPoint.X, 3);
                myObjDxf.Y2 = Math.Round(pLines.EndPoint.Y, 3);

                myObjDxf.CenterX = 0;
                myObjDxf.CenterY = 0;

                myObjDxf.Handle = pLines.Handle;
                myObjDxf.IntHandle = int.Parse(myObjDxf.Handle, System.Globalization.NumberStyles.HexNumber);

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            foreach (Circle pCircle in doc.Entities.Circles)
            {
                //linieSVG.Add("C;" + pCircle.Center + ";" + pCircle.Radius);
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "C";
                myObjDxf.CenterX = Math.Round(pCircle.Center.X, 3);
                myObjDxf.CenterY = Math.Round(pCircle.Center.Y, 3);
                myObjDxf.Radius = Math.Round(pCircle.Radius, 3);

                myObjDxf.X1 = myObjDxf.CenterX + Math.Round(myObjDxf.Radius * Math.Cos(Math.PI * 180 / 180.0), 3);
                myObjDxf.Y1 = myObjDxf.CenterY + Math.Round(myObjDxf.Radius * Math.Sin(Math.PI * 90 / 180.0), 3);

                myObjDxf.X2 = myObjDxf.CenterX + Math.Round(myObjDxf.Radius * Math.Cos(Math.PI * 0 / 180.0), 3);
                myObjDxf.Y2 = myObjDxf.CenterY + Math.Round(myObjDxf.Radius * Math.Sin(Math.PI * 270 / 180.0), 3);

                myObjDxf.MaxX = Math.Round(myObjDxf.Radius * Math.Cos(Math.PI * 0 / 180.0), 3) * 2;
                myObjDxf.MaxY = Math.Round(myObjDxf.Radius * Math.Sin(Math.PI * 90 / 180.0), 3) * 2;

                myObjDxf.Handle = pCircle.Handle;
                myObjDxf.IntHandle = int.Parse(myObjDxf.Handle, System.Globalization.NumberStyles.HexNumber);

                myObjDxf.KierZegara = true;

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            foreach (Arc pArc in doc.Entities.Arcs)
            {
                //linieSVG.Add("A;" + pArc.Center + ";" + pArc.StartAngle + ";" + pArc.EndAngle + ";" + pArc.Radius);
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "A";
                myObjDxf.CenterX = Math.Round(pArc.Center.X, 3);
                myObjDxf.CenterY = Math.Round(pArc.Center.Y, 3);
                //myObjDxf.StartA = Math.Round(pArc.EndAngle, 3); 
                //myObjDxf.EndA = Math.Round(pArc.StartAngle, 3);
                myObjDxf.StartA = Math.Round(pArc.StartAngle, 3);
                myObjDxf.EndA = Math.Round(pArc.EndAngle, 3);
                myObjDxf.Radius = Math.Round(pArc.Radius, 3);

                myObjDxf.X1 = myObjDxf.CenterX + Math.Round(pArc.Radius * Math.Cos(Math.PI * pArc.StartAngle / 180.0), 3);
                myObjDxf.Y1 = myObjDxf.CenterY + Math.Round(pArc.Radius * Math.Sin(Math.PI * pArc.StartAngle / 180.0), 3);

                myObjDxf.X2 = myObjDxf.CenterX + Math.Round(pArc.Radius * Math.Cos(Math.PI * pArc.EndAngle / 180.0), 3);
                myObjDxf.Y2 = myObjDxf.CenterY + Math.Round(pArc.Radius * Math.Sin(Math.PI * pArc.EndAngle / 180.0), 3);

                //myObjDxf.MaxX = myObjDxf.CenterX + myObjDxf.CenterX - myObjDxf.X1;
                //myObjDxf.MaxY = myObjDxf.CenterX + myObjDxf.CenterY - myObjDxf.Y1;
                //myObjDxf.MaxX = Math.Round(myObjDxf.CenterX + (myObjDxf.Radius * 2 * Math.Pow(2, 2) / 2), 3);
                //myObjDxf.MaxY = Math.Round(myObjDxf.CenterY + (myObjDxf.Radius * 2 * Math.Pow(2, 2) / 2), 3);
                //myObjDxf.MaxX = myObjDxf.CenterX + Math.Round((myObjDxf.Radius * 2 * Math.Pow(2, 2) / 2), 3);
                //myObjDxf.MaxY = myObjDxf.CenterY + Math.Round((myObjDxf.Radius * 2 * Math.Pow(2, 2) / 2), 3);

                myObjDxf.Handle = pArc.Handle;
                myObjDxf.IntHandle = int.Parse(myObjDxf.Handle, System.Globalization.NumberStyles.HexNumber);

                myObjDxf.KierZegara = true;

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            if (myObjDxfAll.Count > 0)
            {

                ArrayList myALMinX = new ArrayList();
                ArrayList myALMinY = new ArrayList();
                myALMinX.Add(myObjDxfAll.Min(row => row.X1));
                myALMinX.Add(myObjDxfAll.Min(row => row.X2));
                myALMinX.Add(myObjDxfAll.Min(row => row.CenterX));

                myALMinY.Add(myObjDxfAll.Min(row => row.Y1));
                myALMinY.Add(myObjDxfAll.Min(row => row.Y2));
                myALMinY.Add(myObjDxfAll.Min(row => row.CenterY));

                myALMinX.Sort();
                myALMinY.Sort();

                if (Convert.ToDouble(myALMinX[0]) < 0.0)
                {
                    double moveX = Math.Round(Math.Abs(Convert.ToDouble(myALMinX[0])), 3);
                    for (int i = 0; i < myObjDxfAll.Count; i++)
                    {
                        myObjDxfAll[i].X1 = Math.Round(myObjDxfAll[i].X1 + moveX, 3);
                        myObjDxfAll[i].X2 = Math.Round(myObjDxfAll[i].X2 + moveX, 3);
                        myObjDxfAll[i].CenterX = Math.Round(myObjDxfAll[i].CenterX + moveX, 3);
                        myObjDxfAll[i].MaxX = Math.Round(myObjDxfAll[i].MaxX + moveX, 3);
                    }
                }

                if (Convert.ToDouble(myALMinY[0]) < 0.0)
                {
                    double moveY = Math.Round(Math.Abs(Convert.ToDouble(myALMinY[0])), 3);
                    for (int i = 0; i < myObjDxfAll.Count; i++)
                    {
                        myObjDxfAll[i].Y1 = Math.Round(myObjDxfAll[i].Y1 + moveY, 3);
                        myObjDxfAll[i].Y2 = Math.Round(myObjDxfAll[i].Y2 + moveY, 3);
                        myObjDxfAll[i].CenterY = Math.Round(myObjDxfAll[i].CenterY + moveY, 3);
                        myObjDxfAll[i].MaxY = Math.Round(myObjDxfAll[i].MaxY + moveY, 3);

                    }
                }

                for (int i = 0; i < myObjDxfAll.Count; i++)
                {
                    if (myObjDxfAll[i].MaxX == 0)
                    {
                        myObjDxfAll[i].MaxX = Math.Round(Convert.ToDouble(myObjDxfAll.Max(row => row.X1)) - Convert.ToDouble(myObjDxfAll.Min(row => row.X2)), 3);
                        myObjDxfAll[i].MaxY = Math.Round(Convert.ToDouble(myObjDxfAll.Max(row => row.Y1)) - Convert.ToDouble(myObjDxfAll.Min(row => row.Y2)), 3);
                        break;
                    }

                }

                //for (int i = 0; i < myObjDxfAll.Count; i++)
                //{
                //    if (myObjDxfAll[i].X1 < myObjDxfAll[i].X2)
                //    {
                //        myObjDxfAll[i].MaxX = Math.Round(myObjDxfAll[i].X2 - myObjDxfAll[i].X1, 3);
                //    }
                //    else
                //    {
                //        myObjDxfAll[i].MaxX = Math.Round(myObjDxfAll[i].X1 - myObjDxfAll[i].X2, 3);
                //    }

                //    if (myObjDxfAll[i].Y1 < myObjDxfAll[i].Y2)
                //    {
                //        myObjDxfAll[i].MaxY = Math.Round(myObjDxfAll[i].Y2 - myObjDxfAll[i].Y1, 3);
                //    }
                //    else
                //    {
                //        myObjDxfAll[i].MaxY = Math.Round(myObjDxfAll[i].Y1 - myObjDxfAll[i].Y2, 3);
                //    }
                //}

            }

            return myObjDxfAll;
        }
        public void SAVE(string FilenName, ArrayList myObjDxfAll, int id, int katOBR)
        {
            if (myObjDxfAll == null) return;
            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            DxfDocument dxf = new DxfDocument();
            dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;
            // Angular2LineDimension angDim = (Angular2LineDimension)d.Clone();
            //   Angular2LineDimension angDim2 = new Angular2LineDimension(angDim.StartFirstLine, angDim.EndFirstLine, angDim.StartSecondLine, angDim.EndSecondLine, 20);
            //    angDim.SetDimensionLinePosition(new Vector2(360, -126));
            //    angDim2.SetDimensionLinePosition(new Vector2(360, -126));
            //dxf.BuildDimensionBlocks = true;
            //  dxf.Entities.Add(angDim);
            //   dxf.Entities.Add(angDim2);

            Block baseBlk = new Block("BaseBlock");
            baseBlk.Record.Units = DrawingUnits.Millimeters;

            dxf.Blocks.Add(baseBlk);

            Layer layer = new Layer("DBDT") { Color = AciColor.Blue };
            dxf.Layers.Add(layer);

            List<CDxfOBJ> myObjDxf = new List<CDxfOBJ>();

            myObjDxf = (List<CDxfOBJ>)myObjDxfAll[id];

            string znak;

            znak = "L";

            Vector2 cpointobr = new Vector2(175, 175);

            var queryL = (from CDxfOBJ objDXF in myObjDxf
                          where objDXF.Typ == znak
                          select objDXF);

            foreach (CDxfOBJ s in queryL)
            {
                Vector2 s1 = new Vector2(s.X1, s.Y1);
                Vector2 s2 = new Vector2(s.X2, s.Y2);

                s1 = RotatePoint(s1, cpointobr, katOBR);
                s2 = RotatePoint(s2, cpointobr, katOBR);

                Line line = new Line(s1, s2) { Layer = layer };
                dxf.Entities.Add(line);
            }

            znak = "C";
            var queryC = from CDxfOBJ objDXF in myObjDxf
                         where objDXF.Typ == znak
                         select objDXF;

            foreach (CDxfOBJ s in queryC)
            {
                Vector2 c1 = new Vector2(s.CenterX, s.CenterY);
                // Vector2 s2 = new Vector2(s.X2, s.Y2);
                c1 = RotatePoint(c1, cpointobr, katOBR);
                Circle circle = new Circle(c1, s.Radius) { Layer = layer };
                dxf.Entities.Add(circle);
            }

            znak = "E";
            var queryE = from CDxfOBJ objDXF in myObjDxf
                         where objDXF.Typ == znak
                         select objDXF;
            foreach (CDxfOBJ s in queryC)
            {
                Vector2 c1 = new Vector2(s.CenterX, s.CenterY);
                c1 = RotatePoint(c1, cpointobr, katOBR);
                Ellipse ellipse = new Ellipse(c1, s.MajorA, s.MinorA) { Layer = layer };

                Viewport viewport2 = new Viewport
                {
                    ClippingBoundary = ellipse,
                    SnapAngle = katOBR,
                };
                dxf.Entities.Add(viewport2);
            }

            znak = "A";

            var queryA = from CDxfOBJ objDXF in myObjDxf
                         where objDXF.Typ == znak
                         select objDXF;
            foreach (CDxfOBJ s in queryA)
            {
                Vector2 c1 = new Vector2(s.CenterX, s.CenterY);
                c1 = RotatePoint(c1, cpointobr, katOBR);
                Arc arcS = new Arc(c1, s.Radius, s.StartA + katOBR, s.EndA + katOBR) { Layer = layer };
                dxf.Entities.Add(arcS);
            }

            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2010;
            //dxf.Save("AutoCad2010.dxf");
            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2007;
            //dxf.Save("AutoCad2007.dxf");
            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2004;
            //dxf.Save("AutoCad2004.dxf");
            dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2000;

            dxf.Save(FilenName);

            //DxfDocument load = DxfDocument.Load(FilenName);
            //load.Save(FilenName);
            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            return;
        }

        public void SAVE_ALL(string FilenName, ArrayList myObjDxfAll, Canvas DesignerCanvas)
        {
            if (myObjDxfAll == null) return;

            int katOBR = 0;

            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            DxfDocument dxf = new DxfDocument();
            dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;

            Block baseBlk = new Block("BaseBlock");
            baseBlk.Record.Units = DrawingUnits.Millimeters;

            dxf.Blocks.Add(baseBlk);

            Layer layer = new Layer("DBDT") { Color = AciColor.Blue };
            dxf.Layers.Add(layer);

            List<CDxfOBJ> myObjDxf = new List<CDxfOBJ>();

            int id = -1;

            foreach (UIElement element in DesignerCanvas.Children)
            {
                var contentControl = new ContentControl();

                contentControl = ((System.Windows.Controls.ContentControl)element);
                var content = contentControl.Content;

                var aktAngle = contentControl.RenderTransform;

                var parent = contentControl.Parent as UIElement;
                var location = contentControl.TranslatePoint(new System.Windows.Point(0, 0), parent);

                double lokX = location.X;
                double lokY = location.Y;

                // if (Convert.ToString(((System.Windows.FrameworkElement)content).Tag) == Convert.ToString(is_select))
                // {

                if (aktAngle.IsSealed == false)
                {
                    katOBR = (int)((RotateTransform)aktAngle).Angle;
                }
                else
                {
                    katOBR = 0;
                }
                //}

                id++;

                myObjDxf = (List<CDxfOBJ>)myObjDxfAll[id];

                string znak;

                znak = "L";

                Vector2 cpointobr = new Vector2(175, 175);

                var queryL = (from CDxfOBJ objDXF in myObjDxf
                              where objDXF.Typ == znak
                              select objDXF);

                foreach (CDxfOBJ s in queryL)
                {
                    Vector2 s1 = new Vector2(s.X1 + lokX, s.Y1 + lokY);
                    Vector2 s2 = new Vector2(s.X2 + lokX, s.Y2 + lokY);

                    s1 = RotatePoint(s1, cpointobr, katOBR);
                    s2 = RotatePoint(s2, cpointobr, katOBR);

                    Line line = new Line(s1, s2) { Layer = layer };
                    dxf.Entities.Add(line);
                }

                znak = "C";
                var queryC = from CDxfOBJ objDXF in myObjDxf
                             where objDXF.Typ == znak
                             select objDXF;

                foreach (CDxfOBJ s in queryC)
                {
                    Vector2 c1 = new Vector2(s.CenterX + lokX, s.CenterY + lokY);
                    // Vector2 s2 = new Vector2(s.X2, s.Y2);
                    c1 = RotatePoint(c1, cpointobr, katOBR);
                    Circle circle = new Circle(c1, s.Radius) { Layer = layer };
                    dxf.Entities.Add(circle);
                }

                znak = "E";
                var queryE = from CDxfOBJ objDXF in myObjDxf
                             where objDXF.Typ == znak
                             select objDXF;
                foreach (CDxfOBJ s in queryC)
                {
                    Vector2 c1 = new Vector2(s.CenterX + lokX, s.CenterY + lokY);
                    c1 = RotatePoint(c1, cpointobr, katOBR);
                    Ellipse ellipse = new Ellipse(c1, s.MajorA, s.MinorA) { Layer = layer };

                    Viewport viewport2 = new Viewport
                    {
                        ClippingBoundary = ellipse,
                        SnapAngle = katOBR,
                    };
                    dxf.Entities.Add(viewport2);
                }

                znak = "A";

                var queryA = from CDxfOBJ objDXF in myObjDxf
                             where objDXF.Typ == znak
                             select objDXF;
                foreach (CDxfOBJ s in queryA)
                {
                    Vector2 c1 = new Vector2(s.CenterX + lokX, s.CenterY + lokY);
                    c1 = RotatePoint(c1, cpointobr, katOBR);
                    Arc arcS = new Arc(c1, s.Radius, s.StartA + katOBR, s.EndA + katOBR) { Layer = layer };
                    dxf.Entities.Add(arcS);
                }

            }

            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2010;
            //dxf.Save("AutoCad2010.dxf");
            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2007;
            //dxf.Save("AutoCad2007.dxf");
            //dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2004;
            //dxf.Save("AutoCad2004.dxf");
            dxf.DrawingVariables.AcadVer = DxfVersion.AutoCad2000;

            dxf.Save(FilenName);

            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            return;
        }

        public Vector2 RotatePoint(Vector2 _Points, Vector2 _CenterPoint, double _Degree)
        {
            Vector2 Output = _Points;

            double Angle = (_Degree / 180.0) * Math.PI;

            double Dx = (_Points.X - _CenterPoint.X);
            double Dy = (_Points.Y - _CenterPoint.Y);

            return new Vector2
            {
                X = Math.Round((Math.Cos(Angle) * Dx) - (Math.Sin(Angle) * Dy) + _CenterPoint.X, 3),
                Y = Math.Round((Math.Sin(Angle) * Dx) + (Math.Cos(Angle) * Dy) + _CenterPoint.Y, 3)
            };
        }


        /// <summary>
        /// Rotates 'p1' about 'p2' by 'angle' degrees clockwise.
        /// </summary>
        /// <param name="p1">Point to be rotated</param>
        /// <param name="p2">Point to rotate around</param>
        /// <param name="angle">Angle in degrees to rotate clockwise</param>
        /// <returns>The rotated point</returns>
        //public Vector2 RotatePoint(Vector2 p1, Vector2 p2, double angle)
        //{

        //    double radians = ConvertToRadians(angle);
        //    double sin = Math.Sin(radians);
        //    double cos = Math.Cos(radians);

        //    // Translate point back to origin
        //    p1.X -= p2.X;
        //    p1.Y -= p2.Y;

        //    // Rotate point
        //    double xnew = p1.X * cos - p1.Y * sin;
        //    double ynew = p1.X * sin + p1.Y * cos;

        //    // Translate point back
        //    Vector2 newPoint = new Vector2((int)xnew + p2.X, (int)ynew + p2.Y);
        //    return newPoint;
        //}

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
