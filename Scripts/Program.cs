using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;

namespace SeleniumManager
{
    class Program
    {
        static void Main(string[] args)
        {
            AutomationStart automationStart = new AutomationStart(true, 0);
            automationStart.Navigate("https://www.fundsexplorer.com.br/funds/xpcm11");
            string xpcm11 = automationStart.GetText("/html/body/section/section/div/div/div/div[3]/div/span[1]", 1000);
            Console.WriteLine(xpcm11);
            automationStart.Quit();
        }
    }
}
