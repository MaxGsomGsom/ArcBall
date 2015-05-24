using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArcBall
{
    class Block
    {
        int sizeX, sizeY, health;
        double x, y;
        Graphics g;
        Bonus bonus;

        public Block(Graphics g, double x, double y, int sizeX, int sizeY, int health, Bonus bonus)
        {
            this.bonus = bonus;
            this.g = g;
            this.x = x;
            this.y = y;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.health = health;
        }

        public void Draw()
        {
            Brush curBrush;
            switch (health)
            {
                case 1: { curBrush = Brushes.Red; break; }
                case 2: { curBrush = Brushes.Orange; break; }
                case 3: { curBrush = Brushes.Yellow; break; }
                case 4: { curBrush = Brushes.Green; break; }
                case 5: { curBrush = Brushes.Blue; break; }
                case 6: { curBrush = Brushes.Purple; break; }
                default: curBrush = Brushes.Black;
                    break;
            }


            g.FillRectangle(curBrush, (int)x, (int)y, sizeX, sizeY);
            g.DrawRectangle(Pens.Black, (int)x, (int)y, sizeX, sizeY);

            if (bonus!=Bonus.none) g.DrawString("*", new Font(FontFamily.GenericSerif, 16, FontStyle.Bold), Brushes.Black, (int)x, (int)y);

        }



        public void Damage(int power)
        {
            if (health > 0)
            {
                health-=power;
            }
        }


        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public double Size_X
        {
            get { return sizeX; }
        }

        public double Size_Y
        {
            get { return sizeY; }
        }

        public int Health
        {
            get { return health; }
        }

        public Bonus Bonus
        {
            get { return bonus; }
        }
    }


  
}
