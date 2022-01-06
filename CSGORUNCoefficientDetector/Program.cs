using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium;
using System.IO;
using System;

namespace CSGORUNCoefficientDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Write Path to File, Example - \"" + @"C:\\myFolder\games.txt" + "\"");
            string path = Console.ReadLine();
            if (!File.Exists(path)) File.Create(path).Dispose();
            Console.WriteLine("Write Coefficient, Examples - 2.00, 1.05, 166.20");
            string cfDetectStr = Console.ReadLine();
            int cfDetect = 100;
            if (cfDetectStr.Split('.')[1].Length != 2)
            {
                Console.WriteLine("This Coefficient have Wrong Format. Use Only x.xx Format.");
                Console.ReadKey(); return;
            }
            else cfDetect = int.Parse(cfDetectStr.Replace(".", ""));
            IWebDriver driver = new ChromeDriver(@"C:\");
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://csgorun.gg/");
            while (true)
            {
                if (driver.FindElements(By.XPath(".//*[@id='root']/div[1]/div[2]/div[1]/div[1]/div/div/div/div/div[1]")).Count > 0) break;
                else { Thread.Sleep(1000); Console.Write("."); };
            }
            IWebElement cfLabel = driver.FindElement(By.XPath(".//*[@id='root']/div[1]/div[2]/div[1]/div[1]/div/div/div/div/div[1]"));
            bool start = false;
            int nowCF = 0, cf = 100, cfCombo = 0, cfMiss = 0;
            string backs = "0", players = "0", skins = "0";
            while (true)
            {
                string text = cfLabel.Text;
                if (!text.Contains("s"))
                {
                    start = true;
                    cf = int.Parse(text.Replace("x", "").Replace(".", "").Trim());
                    backs = driver.FindElement(By.XPath(".//*[@id='root']/div[1]/div[2]/div[2]/div[2]/div/ul/li[1]/b/span")).Text;
                    players = driver.FindElement(By.XPath(".//*[@id='root']/div[1]/div[2]/div[2]/div[2]/div/ul/li[2]/b/span")).Text;
                    skins = driver.FindElement(By.XPath("//*[@id='root']/div[1]/div[2]/div[2]/div[2]/div/ul/li[3]/b/span")).Text;
                }
                else
                {
                    if (start)
                    {
                        if (cf >= cfDetect) { File.AppendAllText(path, $"[{DateTime.Now.ToString("yy.MM.dd.HH:mm:ss")}] {cf / 100}.{cf % 100}cf {backs}$ {players}p {skins}s{(cfMiss > 0 ? $" || Miss - {cfMiss}" : (cfCombo > 0 ? $" || Combo - {cfCombo}" : ""))}" + "\n"); cfMiss = 0; cfCombo++; }
                        else { cfCombo = 0; cfMiss++; };
                    }
                    start = false;
                }
                Thread.Sleep(10);
            }
        }
    }
}
