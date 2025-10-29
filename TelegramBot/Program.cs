using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V133.PWA;
using Serilog;
using TelegramBot;


namespace TelegramBot
{
    public static class Driver
    {
        public static UserData user = new UserData();
        static ChromeOptions option = GetChromeOptions();

        public static IWebDriver webDriver = new ChromeDriver(option);
        static ChromeOptions GetChromeOptions()
        {

            var options = new ChromeOptions();
            string downloadPath = "";
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36");
            options.PageLoadStrategy = PageLoadStrategy.Eager;

            Log.Information("путь установки загруки файлов пользователя");
            options.AddUserProfilePreference("download.default_directory", Path.Combine(Storage.BasePath, "users", Storage.CreateUserStorage(user)));
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("safebrowsing.enabled", true);
            return options;
        }
    }
}

static class UIFunction
{
    public static void CheckElementsVisible(By by)
    {
        var isLoad = Driver.webDriver.FindElements(by);
        if (isLoad.Count > 0)
        {
            Log.Information("Елемент успішно загрузився");
            return;
        }
        else
        {
            Log.Warning("Елемент ще не загрузився");
            Thread.Sleep(1000);
            CheckElementsVisible(by);
        }
    }
    public static void SingWithFile()
    {
        CheckElementsVisible(By.XPath("//*[@id=\"root\"]/div/div[1]/div[2]/div/div[2]/table/tbody/tr[1]/td[2]/a")); // Проверка отображение элемента
        Log.Information("Переходимо у вибор через що будемо заходити");
        Driver.webDriver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[1]/div[2]/div/div[2]/table/tbody/tr[1]/td[2]/a")).Click(); // Вход через нужный ключ

        CheckElementsVisible(By.XPath("//*[@id=\"CAsServersSelect\"]/option[23]")); // Проверка отображение элемента
        Log.Information("Вводимо данные");
        Driver.webDriver.FindElement(By.XPath("//*[@id=\"CAsServersSelect\"]/option[22]")).Click(); // Вибір нужной компанії ключа 

        Driver.webDriver.FindElement(By.XPath("//*[@id=\"PKeyFileInput\"]")).SendKeys("C:\\Users\\ukhal\\OneDrive\\Desktop\\File"); // Выбор нужного ключа
        Driver.webDriver.FindElement(By.XPath("//*[@id=\"PKeyPassword\"]")).SendKeys("Password"); // ввод пароля
        Log.Information("Переходимо далее");
        Driver.webDriver.FindElement(By.XPath("//*[@id=\"id-app-login-sign-form-file-key-sign-button\"]")).Click(); // нажатие кнопки продовжити

        Log.Information("Подтверджение особы");
        CheckElementsVisible(By.XPath("//*[@id=\"btnAcceptUserDataAgreement\"]")); // Проверка отображение элемента
        Driver.webDriver.FindElement(By.XPath("//*[@id=\"btnAcceptUserDataAgreement\"]")).Click(); // Нажатие на кнопку продовжити
    }
}

    