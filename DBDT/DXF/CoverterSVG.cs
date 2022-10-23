using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DBDT.DXF
{
    //    //M = przenieś do
    //    //L = linia do
    //    //H = linia pozioma do
    //    //V = linia pionowa do
    //    //C = krzywa do
    //    //S = gładka krzywa do
    //    //Q = kwadratowa krzywa Béziera
    //    //T = gładka kwadratowa krzywa Bézierato
    //    //A = łuk eliptyczny
    //    //Z = ścieżka zamnknij
    public class CoverterSVG
    {
        private void _M()
        {

        }
        private void _L()
        {

        }
        private void _H()
        {

        }
        private void _A()
        {
            // kolor lini.
            //Pen blackPen = new Pen(Color.Black, 3);

            // Prostokąt ze specyfikacjami x1,
            // y1, x2, y2 odpowiednio
            //Rectangle rect = new Rectangle(0, 0, 100, 200);

            // Utwórz kąty początkowe i kąty przeciągnięcia na elipsie.
            //float startAngle = 45.0F;
            //float sweepAngle = 270.0F;

            //Narysuj łuk na ekranie.
            //e.Graphics.DrawArc(blackPen, rect, startAngle, sweepAngle);

        }
        private void _Z()
        {
        }

        public ArrayList ConvertSVGDXF(ArrayList data)
        {
            for (int i = 1; i < data.Count; i++)
            {
               

            }
            //"A101;102;0;0;1;99;100"
            //gc.ArcTo(
            //point: new Point(99, 100),
            //size: new Size(101, 102),

            return data;

            //return null;
        }

    }
}
