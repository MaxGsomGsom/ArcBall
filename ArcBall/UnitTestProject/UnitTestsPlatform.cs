using ArcBall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsPlatform
    {
        Bitmap img = null;
        Graphics g = null;
        Platform pl = null;

        //инициализация объекта для тестирования
        [TestInitialize]
        public void Init()
        {
            img = new Bitmap(100, 100);
            g = Graphics.FromImage(img);

            g.Clear(Color.White);

            pl = new Platform(g, 10, 10, 20, 20, 200);
        }


        //проверка конструктора
        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(pl.X == 10);
            Assert.IsTrue(pl.Y == 10);

            Assert.IsTrue(pl.Size_X == 20);
            Assert.IsTrue(pl.Size_Y == 20);

            Assert.IsTrue(pl.Speed > 0);
        }


        //проверка отрисовки
        [TestMethod]
        public void TestDraw()
        {
            pl.Draw();

            Assert.IsTrue(img.GetPixel((int)pl.X + 5, (int)pl.Y + 5).ToArgb() != Color.White.ToArgb());
        }

        //проверка положения платформы при отсутсвии движения
        [TestMethod]
        public void TestMoveZero()
        {
            double x1 = pl.X;
            double y1 = pl.Y;
            pl.Move(Keys.None);
            double x2 = pl.X;
            double y2 = pl.Y;

            Assert.IsTrue(x1 == x2 && y1 == y2);
        }

        //проверка положения платформы при движении вправо
        [TestMethod]
        public void TestMoveRight()
        {
            double x1 = pl.X;
            double y1 = pl.Y;
            pl.Move(Keys.Right);
            double x2 = pl.X;
            double y2 = pl.Y;

            Assert.IsTrue(x1 < x2 && y1 == y2);
        }

        //проверка положения платформы при движении влево
        [TestMethod]
        public void TestMoveLeft()
        {
            double x1 = pl.X;
            double y1 = pl.Y;
            pl.Move(Keys.Left);
            double x2 = pl.X;
            double y2 = pl.Y;

            Assert.IsTrue(x1 > x2 && y1 == y2);
        }


        //проверка столкновения со стеной слева и справа
        [TestMethod]
        public void TestWallRight()
        {
            for (int i = 0; i < 500; i++)
                pl.Move(Keys.Right);

            Assert.IsTrue((pl.X + pl.Size_X) == 200);

            for (int i = 0; i < 500; i++)
                pl.Move(Keys.Left);

            Assert.IsTrue(pl.X == 0);
        }


    }
}
