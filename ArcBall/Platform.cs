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

    //класс управляемой игроком платформы
    public class Platform : IPlatform
    {

        int sizeX, sizeY, speed;
        double x, y;
        Graphics g;

        int fieldSizeX;

        //функция считывает нажатую клавишу
        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);

        //конструктор
        public Platform(Graphics g, double x, double y, int sizeX, int sizeY, int fieldSizeX)
        {
            this.g = g;
            //координаты
            this.x = x;
            this.y = y;
            //размер
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            //скорость
            speed = 2;

            this.fieldSizeX = fieldSizeX;
        }

        //функция отрисовки
        public void Draw()
        {
            g.FillRectangle(Brushes.Black, (int)x, (int)y, sizeX, sizeY);
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

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //функция передвижения платформы в зависимости от нажатой клавиши
        public void Move()
        {
            //определение клавиши
            Keys key = Keys.None;
            if (GetAsyncKeyState(0x25) != 0) key = Keys.Left;
            else if (GetAsyncKeyState(0x27) != 0) key = Keys.Right;

            //задание скорости движения
            if (key == Keys.Left && x > 0)
            {
                x -= speed;
            }
            else if (key == Keys.Right && (x + sizeX) < fieldSizeX)
            {
                x += speed;
            }
        }

    }



}
