using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TelegramBot
{
    public static class Storage
    {
        
        public static string BasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TelegramAutoBot");

        public static void EnsureStorage()// Створення папки Users в пути Appdata/Local
        {
            Directory.CreateDirectory(BasePath); // создание основной папки
            Directory.CreateDirectory(Path.Combine(BasePath, "Users")); //Создание папки пользователей
        }
        public static string CreateUserStorage(UserData user) //Створення папки для кожного пользователя
        {
            
            string PathStorageUser = Path.Combine(BasePath, "Users", $"{user.TelegramId}");
            Directory.CreateDirectory(PathStorageUser);
            Directory.CreateDirectory(Path.Combine(PathStorageUser, "DownloadDeclaration"));
            Directory.CreateDirectory(Path.Combine(PathStorageUser, "DeclarationInPdf"));
            return Path.Combine(BasePath, "Users", $"{user.TelegramId}", "DownloadDeclaration");
        }

    }
}
