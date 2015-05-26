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
    class Block
    {
        int sizeX, sizeY, health;
        double x, y;
        Graphics g;
        Bonus bonus;

        //конструктор
        public Block(Graphics g, double x, double y, int sizeX, int sizeY, int health, Bonus bonus)
        {
            //тип бонуса
            this.bonus = bonus;
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
                default: curBrush = Brushes.Black;
                    break;
            }

            g.FillRectangle(curBrush, (int)x, (int)y, sizeX, sizeY);
            g.DrawRectangle(Pens.Black, (int)x, (int)y, sizeX, sizeY);

            //отрисовка звездочки, если есть бонус
            if (bonus!=Bonus.none) g.DrawString("*", new Font(FontFamily.GenericSerif, 16, FontStyle.Bold), Brushes.Black, (int)x, (int)y);

        }


        //функция повреждения блока
        public void Damage(int power)
        {
            if (health > 0)
            {
                health-=power;
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
