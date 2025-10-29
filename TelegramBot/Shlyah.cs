using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot;
using static System.Collections.Specialized.BitVector32;

namespace TelegramBot
{
    public static class Shlyah
    {
        private static void GoToSite()
        {
            Driver.webDriver.Navigate().GoToUrl("https://shlyah.dsbt.gov.ua/models/adminui-vue/views/ub-auth.html?returnUrl=https%3A%2F%2Fshlyah.dsbt.gov.ua%2F"); // Переход по сылке
            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"auth-page\"]/div/div[2]/div[2]/form/div[1]/div/div[2]/div/button"));  // Поиск отображение элемента
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"auth-page\"]/div/div[2]/div[2]/form/div[1]/div/div[2]/div/button")).Click();// Нажатие основной кнопки входа
            var windowHandles = new List<string>(Driver.webDriver.WindowHandles); // Создание списка окон
            Driver.webDriver.SwitchTo().Window(windowHandles[1]);// смена окна
            UIFunction.SingWithFile(); // вход в профиль
            Driver.webDriver.SwitchTo().Window(windowHandles[0]); //смена окна
        }

        private static void GoToCarrier()
        {

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"component-1010\"]/div/div[2]/div[1]")); // Проверка отображение элемента
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/div[2]/div[1]")).Click(); // Нажатие на выдвежный элемент

            Thread.Sleep(200);

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/div[2]/div[2]/div[2]/div[2]")).Click(); // Нажатие на перевозчик
            Thread.Sleep(200);
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/div[1]")).Click(); // Открыть окно выбора
            Thread.Sleep(200);
        }
        public static void GetDataUser(UserData user)
        {
            GoToSite(); // Запуск сайта
            GoToCarrier(); // перехов во вкладку Перевізник
            GetUserTrucks(user);
            GetUserDrivers(user);


        }
        private static void GetUserTrucks(UserData user)
        {
            UserTruck userTruck = new UserTruck { TelegramId = user.TelegramId };

            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/ul/li[1]/div")).Click(); // Нажатине на вкладку перевозчик

            Thread.Sleep(200);
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/ul/li[1]/ul/li[2]")).Click(); //Нажатине на вкладку Мои транспортные средства
            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"ubtableview-1049-body\"]")); // Проверка отображение элемента
            var dataUser = Driver.webDriver.FindElement(By.XPath("//*[@id=\"ubtableview-1049-body\"]")); // Получение всех данных об транспортах
            var typeCar = dataUser.FindElements(By.XPath("//td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1037')]"));
            var carNumber = dataUser.FindElements(By.XPath("//td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1038')]"));// Получение номера тягача
            var carModel = dataUser.FindElements(By.XPath("//td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1042')]"));// Получение модели тягача
            foreach (var number in carNumber) // Чтение номера тягача и добавление обьекта в список
            {

                userTruck.Trucks.Add(new Trucks { NumberTruck = number.Text });
            }
            for (int i = 0; i < carModel.Count; i++) // Чтение модели и типа тягача и обновление данных в списке
            {
                userTruck.Trucks[i].ModelTruck = carModel[i].Text;
                userTruck.Trucks[i].TypeTruck = typeCar[i].Text;
            }


            BaseData.AddUserTrucks(userTruck);
        }
        private static void GetUserDrivers(UserData user)
        {
            Actions action = new Actions(Driver.webDriver);
            Driver.webDriver.FindElement(By.XPath("//*[@id=\"component-1010\"]/div/ul/li[1]/ul/li[5]")).Click(); // переход в вкладку Мої водії
            UserDriver userDriver = new UserDriver { TelegramId = user.TelegramId };

            UIFunction.CheckElementsVisible(By.XPath("//*[@id=\"ubtableview-1111-body\"]")); // Проверка отображение елемента
            var dataUser = Driver.webDriver.FindElement(By.XPath("//tbody[contains(@id, 'ubtableview-1111-body')]")); // получение всех данных об водителях
            var lastNameDrivers = dataUser.FindElements(By.XPath("//td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1106')]")); // получение фамилии
            var firstNameDrivers = dataUser.FindElements(By.XPath("//td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1107')]")); // Получение имени            
            var everyDrivers = dataUser.FindElements(By.XPath("./tr")); // Получение списка для перехода во вкладки каждого водителя
            for (int i = 0; i < lastNameDrivers.Count; i++)
            {
                userDriver.Drivers.Add(new Drivers { LastName = lastNameDrivers[i].Text });
            }
            for (int i = 0; i < firstNameDrivers.Count; i++) // обновление данных в списке (имя)
            {
                userDriver.Drivers[i].FirstName = firstNameDrivers[i].Text;
            }
            for (int i = 0; i < everyDrivers.Count - 1; i++) // Получение паспорта и дня народження
            {
                var driver = everyDrivers[i];
                var driverClick = driver.FindElement(By.XPath("./td[contains(@class, 'x-grid-cell-headerId-gridcolumn-1109')]")); // Поиск нужного элемента для двойного нажатия
                Thread.Sleep(100);
                action.DoubleClick(driverClick).Perform(); // двойное нажатие
                Thread.Sleep(200);
                //userDriver.Drivers[i].BirthDay = Driver.webDriver.FindElement(By.XPath("//input[contains(@name, 'birthDate')]")).Text; // получение данных об рождении
                UIFunction.CheckElementsVisible(By.XPath("//span[contains(text(), 'Додатково')]/ancestor::a")); // проверка отображения элемента
                Thread.Sleep(1000);
                Driver.webDriver.FindElement(By.XPath("//span[contains(text(), 'Додатково')]/ancestor::a")).Click(); // нажатие на элемент

                UIFunction.CheckElementsVisible(By.XPath("//input[contains(@name, 'frgPassportSeries')]")); // проверка отображения элемента
                Thread.Sleep(200);
                string? seriaPassport = Driver.webDriver.FindElement(By.XPath("//input[contains(@name, 'frgPassportSeries')]")).GetAttribute("value"); // получение серии паспорта
                string? numberPassport = Driver.webDriver.FindElement(By.XPath("//input[contains(@name, 'frgPassportNum')]")).GetAttribute("value"); // получение номера паспорта
                userDriver.Drivers[i].Passport = seriaPassport + numberPassport;
                Driver.webDriver.FindElement(By.XPath("//*[@id=\"ubCenterViewport\"]/div[2]/div[2]/div[3]/i")).Click(); // закрытие выбраного водителя
                Thread.Sleep(100);
            }
            BaseData.AddUserDrivers(userDriver);
        }


    }
}

