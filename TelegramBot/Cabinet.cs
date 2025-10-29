using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace TelegramBot
{
    public static class Cabinet
    {
        public static void GoToSite() // треба трохи доробити
        {
            //start sign
            try
            {
                Log.Information("Пробуемо зайти на сайт Cabinet");
                Driver.webDriver.Navigate().GoToUrl("https://cabinet.customs.gov.ua/login"); //Url site
                UIFunction.CheckElementsVisible(By.ClassName("rz-variant-filled"));

                Driver.webDriver.FindElement(By.ClassName("rz-variant-filled")).Click(); // Вхід в профіль

                UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"root\"]/div/div[1]/div[2]/div/div[2]/table/tbody/tr[1]/td[2]/a"));
                Log.Information("Переходимо у вибор через що будемо заходити");
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div[2]/div/div[2]/table/tbody/tr[1]/td[2]/a")).Click(); // Choose file medium

                UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"CAsServersSelect\"]/option[23]"));
                Log.Information("Вводимо данные");
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"CAsServersSelect\"]/option[23]")).Click(); // Вибір компанії ключа 

                Driver.webDriver.FindElement(By.XPath("//*[@id=\"PKeyFileInput\"]")).SendKeys("C:\\Users\\ukhal\\OneDrive\\Desktop\\file"); // file
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"PKeyPassword\"]")).SendKeys("password"); // password
                Log.Information("Переходимо далее");
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"id-app-login-sign-form-file-key-sign-button\"]")).Click();

                Log.Information("Подтверджение особы");
                UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"btnAcceptUserDataAgreement\"]")); // підтвердження особистості
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"btnAcceptUserDataAgreement\"]")).Click();
                //finish sigh


                CheckError(By.XPath("/html/body/app/div[1]"));

                UIFunction.CheckElementsVisible(By.XPath("/html/body/app/div[2]/div[2]/div/main/div[2]/div[1]/div/ul/li[1]"));
                Log.Information("Переходимо в 'Мої митні декларації'");

                var elem = Driver.webDriver.FindElement(By.XPath("/html/body/app/div[2]/div[2]/div/main/div[2]/div[1]/div/ul/li[1]")); // Мої митні декларації
                var content = elem.FindElement(By.CssSelector("a"));
                content.Click();

                Log.Information("Получаемо список деклараций за этот месяц");
                CheckDecklaration(DateTime.Today, DateTime.Now.Day - 1);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Виникла помилка при вході на сайт cabinet");
            }


        }
        private static void CheckDecklaration(DateTime date, int day) // дата повинна бути при першому іспользуванню сьогоднішня (потрібно записувати дату загрузки), інакше записаною датою(потім міняти на дату якого дня пользователь скачував декларацію)
        {

            date -= TimeSpan.FromDays(day);
            string formateDate = date.ToString("dd.MM.yyyy");
            Log.Information("Вводимо дату відколи буде формувати список");
            UIFunction.CheckElementsVisible(By.XPath("//input[contains(@class, 'rz-inputtext')]"));
            Driver.webDriver.FindElement(By.XPath("//input[contains(@class, 'rz-inputtext')]")).Clear();
            Driver.webDriver.FindElement(By.XPath("//input[contains(@class, 'rz-inputtext')]")).SendKeys(formateDate); // вибір від коли сортувати декларації від дати календаря от:
            Log.Information("Обираемо потрібну особу або фірму");
            Driver.webDriver.FindElement(By.XPath(("//div[contains(@class, 'rz-state-empty') and contains (@onmousedown, 'Radzen.activeElement = null')]"))).Click();
            Thread.Sleep(600);
            Log.Information("Формуємо список на сайті");
            Driver.webDriver.FindElement(By.XPath("//div[@class='rz-dropdown-items-wrapper']//ul[contains(@class, 'rz-dropdown-item')]//li[contains(@aria-label, 'password')]")).Click();
            Driver.webDriver.FindElement(By.XPath("//button[contains(@class, 'rz-variant-filled') and contains(@tabindex, '0')]")).Click();
            Thread.Sleep(600);

            DownLoadDeclaration();
        }

        private static void DownLoadDeclaration() // потрібно учитувати чи декларації опрацьовані, тому що вони можуть бути лише в роботі. а також вони можуть бути подані ще до 16:00
        {
            Log.Information("Получение количества декларацій");
            var countDoc = Driver.webDriver.FindElements(By.XPath("//tbody[contains(@class, \"rz-datatable-data\")]/tr"));
            Log.Information($"Получено {countDoc.Count} декларацій");
            if (countDoc.Count > 0)
            {

                foreach (var element in countDoc)
                {
                    Log.Information("Завантаження декларації");
                    var button = element.FindElement(By.XPath(".//button"));
                    button.Click();
                    Thread.Sleep(2000);
                }
            }
            ExtractingPdf.GetPdfFile(Driver.user);
        }
        private static void CheckError(By by, int count = 0) // доробити оброботчик помилок, а також додати таймер для повторного запиту на сайт якщо він не процював
        {
            if (count < 10)
            {
                var isload = Driver.webDriver.FindElements(by);
                if (isload.Count == 1)
                {
                    Log.Information("Данні користувача загрузились корректно!");
                    return;
                }
                else
                {
                    Log.Warning("В данний час сайт не може загрузити данні користувача");
                    Thread.Sleep(1000);
                    CheckError(by, count++);
                }
            }
            else
            {
                Log.Error("Перевищено кількість спроб перевірки помилки. Можливо, сайт наразі не працює.");
                throw new Exception("Не вдалось завантажити дані користувача.");
            }
        }


    }
}
