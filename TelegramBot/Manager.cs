using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger(); // Старт Логгера
Log.Information("Прогмамма запустилась");
Log.Information("Создание основних папок хранения");
Storage.EnsureStorage(); // Создание папок хранение деклараций каждого пользователя

BaseData.CreateBaseData(); // Создание базы данных
UserData user = new UserData { TelegramId = 0456460, Email = "qwerty@gmail.com", PasswordKey = "78965412", PasswordQueue = "qweasdzxc", PathKey = "c/fd/cd/g", PhoneNumber = "+38045654322", TypeKey = "ukraine forever" };
BaseData.NewUsers(user);
//Echerga.GetDataUser();

Echerga.GetDataUser();
//Shlyah.GetDataUser();