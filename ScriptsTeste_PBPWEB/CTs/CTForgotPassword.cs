using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScriptsTeste_PBPWEB.Utils;
using System;
using System.Threading;

namespace ScriptsTeste_PBPWEB.CTs
{
    public class CTForgotPassword
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

        private void GoToForgotPasswordView()
        {
            Thread.Sleep(2000);
            IWebElement forgotPassLink = Driver.FindElement(By.CssSelector("#loginForm form a[href=/pbp/Account/ForgotPassword?class=side-link]"));
            forgotPassLink.Click();
            Thread.Sleep(2000);
        }

        private void SendRequest(string mail)
        {
            IWebElement mailInput = Driver.FindElement(By.Name("Email"));
            mailInput.SendKeys(mail);
            Thread.Sleep(2000);

            IWebElement sendbtn = Driver.FindElement(By.CssSelector("body > div.container.body-content > div > div > form > div:nth-child(5) > div > input"));
            sendbtn.Click();
            Thread.Sleep(2000);
        }

        [Order(1)]
        [Test(Description = "Fail test - Empty fields")]
        public void EmptyTest()
        {
            GoToForgotPasswordView();
            SendRequest("");
            
            IWebElement validationSummaryItem = Driver.FindElement(By.CssSelector("body > div.container.body-content > div > div > form > div.text-danger.validation-summary-errors > ul > li"));
            try
            {
                Assert.AreEqual("O campo E-mail é obrigatório.", validationSummaryItem.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "CTForgotPassword", "EmptyTest"));
            }
            CloseBrowser();
        }

        [Order(2)]
        [Test(Description = "Fail test - Unregistered user")]
        public void UnregisteredUserTest()
        {
            GoToForgotPasswordView();
            SendRequest("mail@mail.com");

            IWebElement validationSummaryItem = Driver.FindElement(By.CssSelector("body > div.container.body-content > div > div > form > div.text-danger.validation-summary-errors > ul > li"));
            try
            {
                Assert.AreEqual("Usuário inexistente.", validationSummaryItem.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "CTForgotPassword", "UnregisteredUserTest"));
            }
            CloseBrowser();
        }

        [Order(3)]
        [Test(Description = "Succes test")]
        public void SuccesTest()
        {
            GoToForgotPasswordView();
            SendRequest("teste@teste.com");

            try
            {
                Assert.AreEqual("Redefinição de senha - PickByPath", Driver.Title);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "CTForgotPassword", "SuccesTest"));
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
