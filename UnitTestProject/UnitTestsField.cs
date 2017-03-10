using ArcBall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsField
    {
        Bitmap img = null;
        Graphics g = null;
        Field f = null;

        //инициализация объекта для тестирования
        [TestInitialize]
        public void Init()
        {
            img = new Bitmap(100, 100);
            g = Graphics.FromImage(img);

            g.Clear(Color.White);

            f = new Field(g, 1, 3, 150, 200, 200);
        }


        //проверка конструктора
        [TestMethod]
        public void TestConstructor()
        {

            Assert.IsTrue(f.Lifes == 3);
            Assert.IsTrue(f.Score == 150);
        }

        //проверка загрузки уровня
        [TestMethod]
        public void TestLoadLevel()
        {
            Assert.IsTrue(f.Ball != null);
            Assert.IsTrue(f.Blocks.Count > 0);
            Assert.IsTrue(f.Platform != null);

            try
            {
                Field err = new Field(g, 999, 1, 0, 300, 300);
                Assert.Fail();

            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("loading error"));
            }
        }


        //проверка события нового уровня
        [TestMethod]
        public void TestNextLevelEvent()
        {

            bool flag = false;
            f.NextLevel += (object sender, EventArgs e) =>
            {
                flag = true;
            };

            f.Blocks.Clear();
            f.GameStep(this, new EventArgs());

            Assert.IsTrue(flag);
        }


        //проверка события окончания игры
        [TestMethod]
        public void TestGameOverEvent()
        {
            bool flag = false;
            f.GameOver += (object sender, EventArgs e) =>
            {
                flag = true;
            };

            f.Lifes = 1;
            f.LoseBall();
            f.GameStep(this, new EventArgs());

            Assert.IsTrue(flag);
        }

    }
}
