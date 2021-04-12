using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace SeleniumManager
{
    public class AutomationStart
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait WaitTime { get; set; }
        public int WaitSpeed { get; set; }

        public AutomationStart(bool hideBrowser, int commandWaitSpeedSeconds)
        {
            ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Starting Driver Creation");
            int count = 0;
            while (count < 3)
            {
                try
                {
                    ChromeOptions options = new ChromeOptions();
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                    string defaultDirectoryDownloadFolder = Environment.CurrentDirectory + "\\Downloads";
                    Directory.CreateDirectory(defaultDirectoryDownloadFolder);
                    FolderManagement.DeleteAllDirectories(defaultDirectoryDownloadFolder);
                    FolderManagement.DeleteAllFiles(defaultDirectoryDownloadFolder);
                    options.AddArgument("--start-maximized");
                    options.AddArgument("--lang=pt-BR");
                    options.AddArgument("--disable-infobars");
                    options.AddArgument("--window-size=1920,1080");                    
                    options.AddExcludedArgument("enable-automation");
                    options.AddAdditionalCapability("useAutomationExtension", false);
                    options.AddUserProfilePreference("download.default_directory", defaultDirectoryDownloadFolder);
                    if (hideBrowser)
                    {
                        options.AddArgument("--log-level=3");
                        options.AddArgument("--headless");
                        options.AddArgument("--no-sandbox");
                        options.AddArgument("--disable-gpu");
                    }

                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Arguments Added");
                    service.HideCommandPromptWindow = true;
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Service Added");
                    this.WaitSpeed = commandWaitSpeedSeconds;
                    this.Driver = new ChromeDriver(service, options);
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Driver Started");
                    this.Driver.Manage().Window.Maximize();                    
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Time Control Added");
                    break;
                }
                catch
                {
                    count++;
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Red, "--Driver Inicialization Failed - Count: " + count);
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Blue, "~~Updating ChromeDriver");
                    SeleniumUpdater.DownloadChromeDriver();
                    ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++ChromeDriver Updated");

                }
            }
        }

        private void ThreadSleep()
        {
            if (this.WaitSpeed != 0)
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Waiting " + this.WaitSpeed + " Seconds");
            Thread.Sleep(this.WaitSpeed * 1000);
        }

        public IWebElement FindElement(string xPath)
        {            
            ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Finding Element: " + xPath);
            var elem = this.WaitTime.Until(ExpectedConditions.ElementExists(By.XPath(xPath)));
            ScrollElementIntoView(elem);
            return elem;
        }

        public List<IWebElement> FindElements(string xPath, int waitTimeSeconds)
        {
            this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
            ThreadSleep();            
            FindElement(xPath);
            ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Yellow, "++Finding Elements: " + xPath);
            var elems = this.Driver.FindElements(By.XPath(xPath)).ToList();
            return elems;
        }        

        public string GetText(string xPath, int waitTimeSeconds)
        {
            try
            {
                this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Getting Text: " + xPath);
                return FindElement(xPath).Text;
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                return null;
            }
        }

        public void Click(string xPath, int waitTimeSeconds)
        {
            try
            {
                this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Clicking Element: " + xPath);
                FindElement(xPath).Click();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }

        }

        public void Write(string xPath, string text, int waitTimeSeconds)
        {
            try
            {
                this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Writing on Element: " + xPath + " " + text);
                FindElement(xPath).SendKeys(text);

            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }

        }

        public void Clear(string xPath, int waitTimeSeconds)
        {
            try
            {
                this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Clearing on Element: " + xPath);
                FindElement(xPath).SendKeys(Keys.LeftShift + Keys.Home + Keys.Delete);
                FindElement(xPath).Clear();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }
        }

        public void Find(string xPath, int waitTimeSeconds)
        {
            try
            {
                this.WaitTime = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitTimeSeconds));
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Finding Element: " + xPath);
                FindElement(xPath);
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }
        }

        public void PressKey(string key)
        {
            try
            {                
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Pressing Key on WebBrowser: " + key);
                new Actions(this.Driver).SendKeys(key).Perform();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }

        }

        public void Navigate(string url)
        {
            try
            {
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Navigating to Url: " + url);
                this.Driver.Navigate().GoToUrl(url);
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }
        }

        public void NavigateBack()
        {
            try
            {
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Pressing Back");
                this.Driver.Navigate().Back();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }
        }

        public void NavigateForward()
        {
            try
            {
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Pressing Forward");
                this.Driver.Navigate().Forward();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }
        }

        public void Refresh()
        {
            try
            {
                ThreadSleep();
                ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Green, "++Refreshing The Page");
                this.Driver.Navigate().Refresh();
            }
            catch (Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                Quit();
            }

        }

        public void Quit()
        {
            ThreadSleep();
            ConsoleWriteLineWithColor(ConsoleColor.Red, ConsoleColor.White, "++Quitting Navigation");
            this.Driver.Quit();
        }

        private void ScrollElementIntoView(IWebElement element)
        {
            ((IJavaScriptExecutor)this.Driver).ExecuteScript("window.scroll(" + element.Location.X + "," + (element.Location.Y - 200) + ");");
        }

        public void SwitchTo(int tabnum)
        {
            while (true)
            {
                try
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[tabnum]);
                    break;
                }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("window.open();");
                }
            }
        }

        public static void ConsoleWriteLineWithColor(ConsoleColor background, ConsoleColor foreground, string text)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.WriteLine(text);
        }

    }
}
