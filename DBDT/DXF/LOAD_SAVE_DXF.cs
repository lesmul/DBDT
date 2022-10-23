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

namespace DBDT.DXF
{
    internal class LOAD_SAVE_DXF
    {

        public List<CDxfOBJ> LOAD(string FilenName, int id_object_c)
        {
            DxfDocument dxf = new DxfDocument();
            dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;

            Angular2LineDimension d = null;
            //DxfDocument doc = DxfDocument.Load(@"C:\Users\Leszek Mularski\Desktop\PRZYKLADY\DXF\netDxf-master\TestDxfDocument\sample.dxf");
            //DxfDocument doc = DxfDocument.Load(@"C:\Users\Leszek Mularski\Documents\SOLID_WORKS\WYDRUKI_3D\Część1.DXF");
            DxfDocument doc = DxfDocument.Load(FilenName);

            List<CDxfOBJ> myObjDxfAll = new List<CDxfOBJ>();//default constructor

            int id_object = id_object_c;

            //if(myObjDxfAll.Count > 0)
            //{
            //    var maxid = myObjDxfAll.Max(row => row.Id_object);
            //    id_object = maxid + 1;
            //}

            foreach (Dimension dim in doc.Entities.Dimensions)
            {
                dim.Block = DimensionBlock.Build(dim);
                if (dim.DimensionType == DimensionType.Angular)
                    d = (Angular2LineDimension)dim;
            }

            foreach (Polyline2D pol2d in doc.Entities.Polylines2D)
            {
                var xppol2d = pol2d;

                for (int j = 1; j < xppol2d.Vertexes.Count; j++)
                {
                    CDxfOBJ myObjDxf = new CDxfOBJ();

                    myObjDxf.Id_object = id_object;
                    myObjDxf.Typ = "L";
                    myObjDxf.X1 = Math.Round(xppol2d.Vertexes[j - 1].Position.X, 3);
                    myObjDxf.Y1 = Math.Round(xppol2d.Vertexes[j - 1].Position.Y, 3);
                    myObjDxf.X2 = Math.Round(xppol2d.Vertexes[j].Position.X, 3);
                    myObjDxf.Y2 = Math.Round(xppol2d.Vertexes[j].Position.Y, 3);
                    myObjDxf.MaxX = Math.Round(xppol2d.Vertexes[j].Position.X, 3);
                    myObjDxf.MaxY = Math.Round(xppol2d.Vertexes[j].Position.Y, 3);
                    
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
                myObjDxf.MaxX = Math.Round(xpEllipses.MajorAxis, 3);
                myObjDxf.MaxY = Math.Round(xpEllipses.MinorAxis, 3);

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
                myObjDxf.MaxX = Math.Round(pLines.StartPoint.X, 3);
                myObjDxf.MaxY = Math.Round(pLines.EndPoint.Y, 3);

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            foreach (Circle pCircle in doc.Entities.Circles)
            {
                //linieSVG.Add("C;" + pCircle.Center + ";" + pCircle.Radius);
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "C";
                myObjDxf.CenterX = Math.Round(pCircle.Center.X,3);
                myObjDxf.CenterY = Math.Round(pCircle.Center.Y,3);
                myObjDxf.Radius = Math.Round(pCircle.Radius,3);
                myObjDxf.MaxX = Math.Round(pCircle.Radius,3);
                myObjDxf.MaxY = Math.Round(pCircle.Radius,3);

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            foreach (Arc pArc in doc.Entities.Arcs)
            {
                //linieSVG.Add("A;" + pArc.Center + ";" + pArc.StartAngle + ";" + pArc.EndAngle + ";" + pArc.Radius);
                CDxfOBJ myObjDxf = new CDxfOBJ();
                myObjDxf.Id_object = id_object;
                myObjDxf.Typ = "A";
                myObjDxf.CenterX = Math.Round(pArc.Center.X,3);
                myObjDxf.CenterY = Math.Round(pArc.Center.Y,3);
                myObjDxf.StartA = Math.Round(pArc.EndAngle, 3); 
                myObjDxf.EndA = Math.Round(pArc.StartAngle, 3);
                myObjDxf.Radius = Math.Round(pArc.Radius,3);

                myObjDxf.Y1 = myObjDxf.CenterX + Math.Round(pArc.Radius * Math.Cos(Math.PI * pArc.StartAngle / 180.0),3);
                myObjDxf.X1 = myObjDxf.CenterY + Math.Round(pArc.Radius * Math.Sin(Math.PI * pArc.StartAngle / 180.0), 3);

                myObjDxf.Y2 = myObjDxf.CenterX + Math.Round(pArc.Radius * Math.Cos(Math.PI * pArc.EndAngle / 180.0), 3);
                myObjDxf.X2 = myObjDxf.CenterY + Math.Round(pArc.Radius * Math.Sin(Math.PI * pArc.EndAngle / 180.0), 3);

                myObjDxf.Id = myObjDxfAll.Count;
                myObjDxfAll.Add(myObjDxf);
            }

            if (myObjDxfAll.Count > 0)
            {

                ArrayList myALMinX = new ArrayList();
                ArrayList myALMinY = new ArrayList();
                myALMinX.Add(myObjDxfAll.Min(row => row.X1));
                myALMinX.Add(myObjDxfAll.Min(row => row.X2));
                myALMinY.Add(myObjDxfAll.Min(row => row.Y1));
                myALMinY.Add(myObjDxfAll.Min(row => row.Y2));
                myALMinX.Add(myObjDxfAll.Min(row => row.CenterX));
                myALMinY.Add(myObjDxfAll.Min(row => row.CenterY));

                myALMinX.Sort();
                myALMinY.Sort();

                if (Convert.ToDouble(myALMinX[0]) < 0.0)
                {
                    double moveX = Math.Abs(Convert.ToDouble(myALMinX[0]));
                    for (int i = 0; i < myObjDxfAll.Count; i++)
                    {
                        myObjDxfAll[i].X1 = myObjDxfAll[i].X1 + moveX;
                        myObjDxfAll[i].X2 = myObjDxfAll[i].X2 + moveX;
                        myObjDxfAll[i].CenterX = myObjDxfAll[i].CenterX + moveX;
                        myObjDxfAll[i].MaxX = myObjDxfAll[i].MaxX + moveX;

                    }
                }

                if (Convert.ToDouble(myALMinY[0]) < 0.0)
                {
                    double moveY = Math.Abs(Convert.ToDouble(myALMinY[0]));
                    for (int i = 0; i < myObjDxfAll.Count; i++)
                    {
                        myObjDxfAll[i].Y1 = myObjDxfAll[i].Y1 + moveY;
                        myObjDxfAll[i].Y2 = myObjDxfAll[i].Y2 + moveY;
                        myObjDxfAll[i].CenterY = myObjDxfAll[i].CenterY + moveY;
                        myObjDxfAll[i].MaxY = myObjDxfAll[i].MaxX + moveY;

                    }
                }

            }

            return myObjDxfAll;
        }
        public void SAVE(string FilenName)
        {
            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            DxfDocument dxf = new DxfDocument();
            dxf.DrawingVariables.InsUnits = DrawingUnits.Millimeters;



            //Angular2LineDimension d = null;
            ////DxfDocument doc = DxfDocument.Load(@"C:\Users\Leszek Mularski\Desktop\PRZYKLADY\DXF\netDxf-master\TestDxfDocument\sample.dxf");
            //DxfDocument doc = DxfDocument.Load(@"C:\Users\Leszek Mularski\Documents\SOLID_WORKS\WYDRUKI_3D\Część1.DXF");


            //foreach (Dimension dim in doc.Entities.Dimensions)
            //{
            //    dim.Block = DimensionBlock.Build(dim);
            //    if (dim.DimensionType == DimensionType.Angular)
            //        d = (Angular2LineDimension)dim;
            //}


            //return;

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

            Vector2 s1 = new Vector2(-2, 20);
            Vector2 s2 = new Vector2(20, -2);
            Layer layer = new Layer("Layer1") { Color = AciColor.Blue };
            dxf.Layers.Add(layer);
            Line line = new Line(s1, s2) { Layer = layer };

            dxf.Entities.Add(line);

            Ellipse ellipse = new Ellipse(new Vector2(100), 200, 150);
            Viewport viewport2 = new Viewport
            {
                ClippingBoundary = ellipse,
            };

            

            // Add the viewport to the document. This will also add the ellipse to the document.
            dxf.Entities.Add(viewport2);

            dxf.Save("test.dxf");
            //****************************************************************************************************************************************
            //****************************************************************************************************************************************
            return;
        }
    }
}
