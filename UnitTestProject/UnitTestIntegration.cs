using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winium.Cruciatus;
using Winium.Cruciatus.Core;
using Winium.Cruciatus.Elements;
using System.Windows.Automation;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestIntegration
    {
        Application app = null;
        CruciatusElement winMain = null;

        [TestInitialize]
        public void Init()
        {
            app = new Application("..\\..\\..\\ArcBall\\bin\\Debug\\ArcBall.exe");
            app.Start();
            var winMainFinder = By.Uid("MenuForm").AndType(ControlType.Window);
            winMain = CruciatusFactory.Root.FindElement(winMainFinder);
        }


        [TestMethod]
        public void TestMethodHelp()
        {
            winMain.SetFocus();
            winMain.FindElementByUid("button3help").Click();

            var winHelpFinder = By.Uid("HelpForm").AndType(ControlType.Window);
            var winHelp = CruciatusFactory.Root.FindElement(winHelpFinder);

            var textField = winHelp.FindElementByUid("textBox1");

            Assert.IsNotNull(textField);

            winHelp.FindElementByUid("Close").Click();


        }


        [TestMethod]
        public void TestMethodGame()
        {
            winMain.SetFocus();
            winMain.FindElementByUid("button1start").Click();


        }

        [TestCleanup]
        public void EndTests()
        {
            app.Kill();

        }
    }
}
