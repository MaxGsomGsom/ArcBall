using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcBall
{
    class Platform
    {

        int sizeX, sizeY, speed;
        double x, y;
        Graphics g;

        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);

        public Platform(Graphics g, double x, double y, int sizeX, int sizeY)
        {
            this.g = g;
            this.x = x;
            this.y = y;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            speed = 2;
        }

        public void Draw()
        {
            g.FillRectangle(Brushes.Black, (int)x, (int)y, sizeX, sizeY);
        }


        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public int Size_X
        {
            get { return sizeX; }
            set { sizeX = value; }
        }

        public int Size_Y
        {
            get { return sizeY; }
        }


        internal void Move(int fieldSizeX)
        {
            Keys key=Keys.None;
            if (GetAsyncKeyState(0x25)!=0) key=Keys.Left;
            else if (GetAsyncKeyState(0x27) != 0) key = Keys.Right;


            if (key == Keys.Left && x>0)
            {
                x -= speed;
            }
            else if (key == Keys.Right && (x + sizeX) < fieldSizeX)
            {
                x += speed;
            }
        }

        public int Speed { 
            get { return speed; }
            set { speed = value; }
        }
    }


    
}
