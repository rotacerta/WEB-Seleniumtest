using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScriptsTeste_PBPWEB.Utils;
using System;
using System.Threading;

namespace ScriptsTeste_PBPWEB.CTs
{
    [TestFixture]
    public class CTLogin
    {
        private Util utils;
        public string ScreenshotsBaseName { get; set; }
        public IWebDriver Driver { get; set; }
        public string BaseURL { get; set; }

        [SetUp]
        public void SetUp()
        {
            Driver = new ChromeDriver(@"C:\Users\Home\Downloads\chromeDriver2");
            BaseURL = "http://localhost/pbp";
            ScreenshotsBaseName = "Login-{0}-{1}.png";
            utils = new Util();
        }

        private void OpenBrowser(string url)
        {
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(url);
        }

        private void ExecuteLogin(string mail, string pass)
        {
            OpenBrowser(BaseURL);
            IWebElement mailInput = Driver.FindElement(By.Name("Email"));
            IWebElement passInput = Driver.FindElement(By.Name("Password"));
            IWebElement btnLogin = Driver.FindElement(By.CssSelector("section#loginForm form input[type=submit]"));
            Thread.Sleep(2000);

            mailInput.SendKeys(mail);
            passInput.SendKeys(pass);
            Thread.Sleep(2000);

            btnLogin.Click();
            Thread.Sleep(2000);
        }

        [Order(1)]
        [Test(Description = "Fail test - Empty fields")]
        public void EmptyTest()
        {
            ExecuteLogin("", "");

            IWebElement mailError = Driver.FindElement(By.Id("Email-error"));
            try
            {
                Assert.AreEqual("O campo E-mail é obrigatório.", mailError.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "EmptyTest", "Mail"));
            }

            IWebElement passError = Driver.FindElement(By.Id("Password-error"));
            try
            {
                Assert.AreEqual("O campo Senha é obrigatório.", passError.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "EmptyTest", "Pass"));
            }
            CloseBrowser();
        }

        [Order(2)]
        [Test(Description = "Fail test - Unregistered user")]
        public void UnregisteredUserTest()
        {
            ExecuteLogin("mail@mail.com", "123456");

            IWebElement validationSummaryItem = Driver.FindElement(By.CssSelector("#loginForm > form > div.validation-summary-errors.text-danger > ul > li"));
            try
            {
                Assert.AreEqual("Usuário inexistente.", validationSummaryItem.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "UnregisteredUserTest", "UnregisteredError"));
            }
            CloseBrowser();
        }

        [Order(3)]
        [Test(Description = "Fail test - Wrong data")]
        public void WrongDataTest()
        {
            ExecuteLogin("teste@teste.com", "123456");

            IWebElement validationSummaryItem = Driver.FindElement(By.CssSelector("#loginForm > form > div.validation-summary-errors.text-danger > ul > li"));
            try
            {
                Assert.AreEqual("Tentativa de login inválida.", validationSummaryItem.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "WrongDataTest", "WrongDataError"));
            }
            CloseBrowser();
        }

        [Order(4)]
        [Test(Description = "Succes test")]
        public void SuccesTest()
        {
            ExecuteLogin("teste@teste.com", "Foguete");

            try
            {
                Assert.AreEqual("Home Page - PickByPath", Driver.Title);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "SuccesTest", "Error"));
            }
            CloseBrowser();
        }

        private void CloseBrowser()
        {
            Driver.Close();
        }

        [TearDown]
        public void TeardownTest()
        {
        }
    }
}
