using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArcBall
{
    class Ball
    {

        int radius;
        double x, y, x_prev, y_prev;
        int speed;
        Graphics g;
        Collision curCollision;
        double slideX;
        int power;


        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);




        public Ball(Graphics g, double x, double y, int radius)
        {
            this.g = g;
            this.x = x;
            this.y = y;
            this.radius = radius;
            speed = 4;
            slideX = 0;
            power = 1;

            x_prev = x;
            y_prev = y;

        }


        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public double X_prev
        {
            get { return x_prev; }
        }

        public double Y_prev
        {
            get { return y_prev; }
        }

        public void Draw()
        {
            g.FillEllipse(Brushes.Black, (int)x, (int)y, radius, radius);
        }


        public bool TestIntersection(double blockX, double blockY, double blockSizeX, double blockSizeY, bool isPlatform=false)
        {
            curCollision = Collision.none;

            if ((x+radius) > blockX && x < (blockX + blockSizeX) && (y+radius) > blockY && y < (blockY + blockSizeY))
            {
                double dxS = Math.Abs(blockX - (x+radius));
                double dxL = Math.Abs(blockX + blockSizeX - x);
                double dyS = Math.Abs(blockY - (y+radius));
                double dyL = Math.Abs(blockY + blockSizeY - y);

                if (dxS < dxL && dxS < dyS && dxS < dyL && !isPlatform)
                {
                    curCollision = Collision.left;
                }
                else if (dxL < dxS && dxL < dyS && dxL < dyL && !isPlatform)
                {
                    curCollision = Collision.right;
                }
                else if (dyS < dxS && dyS < dxL && dyS < dyL)
                {
                    curCollision = Collision.top;
                }
                else if (!isPlatform)
                {
                    curCollision = Collision.bottom;
                }

            }

            if (curCollision != Collision.none) return true;
            else return false;

            
        }


        public void Slide()
        {
            Keys key = Keys.None;
            if (GetAsyncKeyState(0x25) != 0) key = Keys.Left;
            else if (GetAsyncKeyState(0x27) != 0) key = Keys.Right;

            if (key == Keys.Left) slideX = -0.5;
            else if (key == Keys.Right) slideX = 0.5;
        }


        public void Move()
        {
           
            double length = Math.Sqrt(Math.Abs(y - y_prev) * Math.Abs(y - y_prev) + Math.Abs(x - x_prev) * Math.Abs(x - x_prev));

            if (length == 0)
            {
                if (GetAsyncKeyState(0x20) != 0)
                {
                    x_prev = x;
                    y_prev = y + 1;
                }
            }
            else
            {
                double x_buf = 0, y_buf = 0;

                double y_sin = (y - y_prev) / length;
                double x_cos = (x - x_prev) / length;

                x_cos += slideX;
                slideX = 0;
                if (y_sin < 0.1 && y_sin > -0.1) y_sin = -0.5;

                if (curCollision == Collision.none)
                {
                    x_buf = x_cos * speed + x;
                    y_buf = y_sin * speed + y;
                }
                else if (curCollision == Collision.bottom || curCollision == Collision.top)
                {
                    x_buf = x_cos * speed + x;
                    y_buf = -y_sin * speed + y;
                }
                else if (curCollision == Collision.left || curCollision == Collision.right)
                {
                    x_buf = -x_cos * speed + x;
                    y_buf = y_sin * speed + y;
                }


                x_prev = x;
                y_prev = y;
                x = x_buf;
                y = y_buf;
            }
        }


        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }


        public int Power 
        {
            get { return power; }
            set { power = value; }
        }
    }
}
