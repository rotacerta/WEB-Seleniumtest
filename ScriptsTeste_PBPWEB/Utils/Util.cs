using OpenQA.Selenium;

namespace ScriptsTeste_PBPWEB.Utils
{
    public class Util
    {
        public string ScreenshotsFolder { get; private set; }

        public Util()
        {
            ScreenshotsFolder = @"C:\Users\Home\source\PBPWEB-Evidences\";
        }

        public void Screenshot(IWebDriver driver, string screenshotFileName)
        {
            ITakesScreenshot camera = driver as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile(ScreenshotsFolder + screenshotFileName, ScreenshotImageFormat.Png);
        }

        public void OpenBrowser(string url, IWebDriver Driver)
        {
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(url);
        }
    }
}
