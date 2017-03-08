using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArcBall
{
    //класс игрового блока
    public class Block : IBlock
    {
        protected int sizeX, sizeY, health;
        protected double x, y;
        protected Graphics g;


        //конструктор
        public Block(Graphics g, double x, double y, int sizeX, int sizeY, int health)
        {
            this.g = g;
            //координаты
            this.x = x;
            this.y = y;
            //размер
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            //прочность
            this.health = health;
        }

        //функция отрисовки блока
        public void Draw()
        {
            Brush curBrush;
            //цвет зависит от прочности
            switch (health)
            {
                case 1: { curBrush = Brushes.Red; break; }
                case 2: { curBrush = Brushes.Orange; break; }
                case 3: { curBrush = Brushes.Yellow; break; }
                case 4: { curBrush = Brushes.Green; break; }
                case 5: { curBrush = Brushes.Blue; break; }
                case 6: { curBrush = Brushes.Purple; break; }
                default:
                    curBrush = Brushes.Black;
                    break;
            }

            g.FillRectangle(curBrush, (int)x, (int)y, sizeX, sizeY);
            g.DrawRectangle(Pens.Black, (int)x, (int)y, sizeX, sizeY);
        }


        //функция повреждения блока
        public void Damage(int power)
        {
            if (health > 0)
            {
                health -= power;
            }
        }

        //поля для доступа к свойствам из вне
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
            set { sizeX = value; }
        }

        public int Health
        {
            get { return health; }
        }

    }

}
