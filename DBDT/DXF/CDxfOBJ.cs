using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDT.DXF
{
    class CDxfOBJ
    {
        private double id;

        public double Id
        {
            get { return id; }
            set { id = value; }
        }

        private int id_object;

        public int Id_object
        {
            get { return id_object; }
            set { id_object = value; }
        }

        private int inthandle;
        public int IntHandle
        {
            get { return inthandle; }
            set { inthandle = value; }
        }

        private int intwezel;
        public int IntWezel
        {
            get { return intwezel; }
            set { intwezel = value; }
        }

        private string typ;

        public string Typ
        {
            get { return typ; }
            set { typ = value; }
        }

        private double x1;
        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        private double y1;
        public double Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        private double x2;
        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        private double y2;
        public double Y2
        {
            get { return y2; }
            set { y2 = value; }
        }

        private double centerX;
        public double CenterX
        {
            get { return centerX; }
            set { centerX = value; }
        }

        private double centerY;
        public double CenterY
        {
            get { return centerY; }
            set { centerY = value; }
        }

        private double radius;
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        private double startA;
        public double StartA
        {
            get { return startA; }
            set { startA = value; }
        }

        private double endA;
        public double EndA
        {
            get { return endA; }
            set { endA = value; }
        }

        private double majorA;
        public double MajorA
        {
            get { return majorA; }
            set { majorA = value; }
        }

        private double minorA;
        public double MinorA
        {
            get { return minorA; }
            set { minorA = value; }
        }

        private double maxX;
        public double MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }

        private double maxY;
        public double MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }

        private bool kierZegara = false;
        public bool KierZegara
        {
            get { return kierZegara; }
            set { kierZegara = value; }
        }

        private string handle;
        public string Handle
        {
            get { return handle; }
            set { handle = value; }
        }

        private bool juz_dodany=false;
        public bool Juz_dodany
        {
            get { return juz_dodany; }
            set { juz_dodany = value; }
        }

    }
}
