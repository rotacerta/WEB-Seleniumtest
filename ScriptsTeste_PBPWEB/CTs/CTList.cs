using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ScriptsTeste_PBPWEB.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScriptsTeste_PBPWEB.CTs
{
    /**
     * Limitação para este caso de teste: Não é possível selecionar nenhum produto por conta da forma como o Bootstrap p trata
     */
    [TestFixture]
    public class CTList
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

        private void ExecuteLogin(string mail, string pass)
        {
            utils.OpenBrowser(BaseURL, Driver);
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

        #region CreateList
        private void GoToCreateView()
        {
            ExecuteLogin("teste@teste.com", "Foguete");
            Driver.Navigate().GoToUrl(BaseURL + "/Lists/ChooseProducts");
            Thread.Sleep(2000);
        }

        [Order(1)]
        [Test(Description = "Fail test - No products selected")]
        public void NoProductsSelectedTest()
        {
            GoToCreateView();

            IWebElement continuebtn = Driver.FindElement(By.CssSelector("form div.form-group input[type=submit]"));
            continuebtn.Click();
            Thread.Sleep(2000);

            IWebElement validationSummaryItem = Driver.FindElement(By.CssSelector("body > div.container.body-content > div > form > div.validation-summary-errors.text-danger > ul > li"));
            try
            {
                Assert.AreEqual("Selecione ao menos um produto.", validationSummaryItem.Text);
            }
            catch (Exception e)
            {
                utils.Screenshot(Driver, String.Format(ScreenshotsBaseName, "NoProductsSelectedTest", "NoProductsSelected"));
            }
            CloseBrowser();
        }

        [Order(2)]
        [Test(Description = "Fail test - Untitled List")]
        public void UntitledListTest()
        {
            GoToCreateView();

            List<IWebElement> checkboxes = Driver.FindElements(By.CssSelector("body > div.container.body-content > div > form > table > tbody > tr:nth-child(1) > td:nth-child(4) > div > input[type=checkbox]:nth-child(2n + 1)")).ToList();

            foreach (IWebElement cb in checkboxes)
            {
                cb.Click();
            }

            IWebElement continuebtn = Driver.FindElement(By.CssSelector("form div.form-group input[type=submit]"));
            continuebtn.Click();
            Thread.Sleep(2000);
            CloseBrowser();
        }

        [Order(3)]
        [Test(Description = "Fail test - Without List Requester")]
        public void WithoutListRequesterTest() { }

        [Order(4)]
        [Test(Description = "Fail test - With Negative Numbers In Required Quantity")]
        public void WithNegativeNumbersInRequiredQuantityTest() { }

        [Order(5)]
        [Test(Description = "Fail test - With Numbers Greater Than Allowed In Required Quantity")]
        public void WithNumbersGreaterThanAllowedInRequiredQuantityTest() { }

        [Order(6)]
        [Test(Description = "Success test")]
        public void SuccessCreateListTest() { }
        #endregion CreateList

        #region ConsultList
        private void GoToConsultView()
        {
            ExecuteLogin("teste@teste.com", "Foguete");
            Driver.Navigate().GoToUrl(BaseURL + "/Lists/Index");
            Thread.Sleep(2000);
        }

        [Order(7)]
        [Test(Description = "Success test")]
        public void ConsultWithData()
        {
            "body > div.container.body-content > div > table > tbody > tr:nth-child(1)"
        }
        #endregion ConsultList

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
