using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    /// <summary> 
    /// Класс який відповідає за взаємодію з сайтом Echerga 
    /// </summary>
    public static class Echerga
    {
        /// <summary> 
        /// Метод який відкриває сайт і входить у профіль юзера
        /// </summary>
        public static void GoToSite()
        {
            try // Спроба ввійти на сайт у профіль
            {
                Log.Information("Пробуемо зайти на сайт Echerha"); // запис у логи 
                Driver.webDriver.Navigate().GoToUrl("https://echerha.gov.ua/login"); // перехід по силці на сайт
                Log.Information("Ввод логина и пароля"); // запис у логи 
                Thread.Sleep(3000); // пауза для того щоб загрузвся сайт
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[1]/div[1]/label")).SendKeys("email"); // пошук потрібного поля для ввода пошти
                Thread.Sleep(2000); // Пауза щоб імітувати людину
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[2]/div[1]/label")).SendKeys("password"+ Keys.Enter); // пошук потрібного поля для ввода пароля
                Log.Information("Натискаемо на кнопку"); // запис у логи
                Thread.Sleep(1500); // Пауза щоб імітувати людину 
                //Driver.webDriver.FindElement(By.XPath("//*[@id=\"loginForm\"]/button")).Click(); // Клікаємо щоб пітвердити ввод данних
            }
            catch (Exception ex) // при будь якій помилці
            {
                Log.Error(ex, "Виникла помилка при вході на сайт echerha");  // запис помилки у логи
            }
            Log.Information("Вхід на сайт виполнився");
        }
        /// <summary>
        /// Метод для отримання прицепів юзера, і запис їх в базу данних
        /// </summary>
        public static void GetDataUser()
        {
            Log.Information("метод отриманян данних прицепів почав роботу");
            Trailers trailers = new Trailers(); // переменна прицеппа
            UserTrailers userTrailers = new UserTrailers { TelegramId = 0456460 }; // переменна списка прицепів
            GoToSite(); // вход на сайт Echerga            

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"__nuxt\"]/div/div/header/div/div[1]/button")); // провірка загрузки елемента
            Log.Information("Клік по кнопці\"Стати в чергу\" ");
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/header/div/div[1]/button")).Click(); // клік на потрібний елемент
            Log.Information("Клік пройшов успішно");

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div[1]/div[2]/form/button"));
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div[1]/div[2]/form/button")).Click();

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[1]/div/div/div[1]/label"));
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[1]/div/div/div[1]/label")).Click();
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[1]/div/div/div[1]/label")).SendKeys("Україна");
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[1]/div/ul/li")).Click();

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[2]/div[1]/div[1]/label")).SendKeys("Name");

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[2]/div[2]/div[1]/label")).SendKeys("LastName");

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[3]/div[1]/label")).SendKeys("Passport");

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[4]/div[1]/label")).SendKeys("mail");

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[5]/div[1]/div/div/div[1]/label")).Click();
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[5]/div[1]/div/div/div[1]/label")).SendKeys("Україна +380");
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[5]/div[1]/div/ul/li")).Click();

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[5]/div[2]/div[1]/label")).SendKeys("phoneNumber");

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[2]/button[1]")).Click();

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[6]/label/input"));
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[1]/div[6]/label/input")).Click();

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"__nuxt\"]/div/div/main/div/div/form/div[2]/button[1]")).Click();

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"truckForm\"]/div[5]/div/div/label[1]"));
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"truckForm\"]/div[5]/div/div/label[1]")).Click();
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"truckForm\"]/div[5]/div[4]/div/div/div[1]/label")).Click();

            var userData = Driver.webDriver.FindElement(By.XPath("//*[@id=\"truckForm\"]/div[5]/div[4]/div/ul"));

            var userTrailer = userData.FindElements(By.XPath("./li")); 

            foreach(var numberTrailer in userTrailer)
            {
                trailers.NumberTrailer = numberTrailer.Text;
                userTrailers.Trailers.Add(trailers);
            }
            BaseData.AddUserTrailers(userTrailers);
        }
    }
}
