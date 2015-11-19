using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winium.Cruciatus;
using Winium.Cruciatus.Core;
using System.Windows.Automation;
using WindowsInput;
using WindowsInput.Native;
using Winium.Cruciatus.Settings;
using System.Threading;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestIntegration
    {
        Application app = null;
        InputSimulator input = null;


        [TestInitialize]
        public void Init()
        {
            input = new InputSimulator();
            CruciatusFactory.Settings.KeyboardSimulatorType = KeyboardSimulatorType.BasedOnInputSimulatorLib;
            CruciatusFactory.Settings.SearchTimeout = 1000;

            //запуск программы
            app = new Application("..\\..\\..\\ArcBall\\bin\\Debug\\ArcBall.exe");
            app.Start();


        }


        //тестирование окна помощи
        [TestMethod]
        public void TestMethod()
        {
            //нахождение главного окна
            var winMainFinder = By.Uid("MenuForm").AndType(ControlType.Window);
            var winMain = CruciatusFactory.Root.FindElement(winMainFinder);

            //клик по кнопке помощь
            winMain.SetFocus();
            winMain.FindElementByUid("button3help").Click();

            //проверка окна
            var winHelpFinder = By.Uid("HelpForm").AndType(ControlType.Window);
            var winHelp = CruciatusFactory.Root.FindElement(winHelpFinder);

            var textField = winHelp.FindElementByUid("textBox1");

            Assert.IsNotNull(textField);


        }


        //тест игрового окна
        [TestMethod]
        public void TestMethodGame()
        {
            //нахождение главного окна
            var winMainFinder = By.Uid("MenuForm").AndType(ControlType.Window);
            var winMain = CruciatusFactory.Root.FindElement(winMainFinder);

            //нажатие кнопки старт
            winMain.SetFocus();
            winMain.FindElementByUid("button1start").Click();

            var winGameFinder = By.Uid("GameForm").AndType(ControlType.Window);
            var winGame = CruciatusFactory.Root.FindElement(winGameFinder);

            //проверка элементов игрового окна
            var level = winGame.FindElementByUid("Levellabel");
            var score = winGame.FindElementByUid("scoreLabel");
            var lifes = winGame.FindElementByUid("lifesLabel");

            Assert.IsTrue(level.Properties.Name == "Уровень: 1");
            Assert.IsTrue(score.Properties.Name == "Счёт: 0");
            Assert.IsTrue(lifes.Properties.Name == "Жизни: 3");


            winGame.SetFocus();

            //нажатие кнопокпробел, вправо, влево
            //пока не потратятся все жизни
            while (Convert.ToInt32(lifes.Properties.Name.Substring(7)) > 0)
            {
                input.Keyboard.KeyPress(VirtualKeyCode.SPACE);

                input.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
                input.Keyboard.KeyDown(VirtualKeyCode.LEFT);
                Thread.Sleep(1000);
                input.Keyboard.KeyUp(VirtualKeyCode.LEFT);
                input.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                Thread.Sleep(1000);

            }

            //проверка, что игра окончена, проверка окна с результатом
            Assert.IsTrue(Convert.ToInt32(score.Properties.Name.Substring(6)) > 0);
            Assert.IsTrue(lifes.Properties.Name == "Жизни: 0");

            var textEnd = winGame.FindElementByUid("65535");
            Assert.IsTrue(textEnd.Properties.Name.Contains("Ваш счёт"));

            input.Keyboard.KeyPress(VirtualKeyCode.SPACE);

            Thread.Sleep(1000);
            Assert.IsNull(CruciatusFactory.Root.FindElement(winGameFinder));


        }

        [TestCleanup]
        public void Clean()
        {
            try
            {
                app.Kill();
            }
            catch { }
        }


    }
}
