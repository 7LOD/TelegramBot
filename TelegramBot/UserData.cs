using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class UserData // Класс который отвечает за данные пользователя
    {
		private int telegramId; // Унікальний ID для кожного юзера
		public int TelegramId
		{
			get { return telegramId; }
			set { telegramId = value; }
		}

		private string email; // Пошта потрібна при реєстрації черги
		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		private string phoneNumber; // Номер потрібен при реєстрації черги
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set { phoneNumber = value; }
		}

		private string passwordQueue; // Пароль від сайта Echerga
		public string PasswordQueue
		{
			get { return passwordQueue; }
			set { passwordQueue = value; }
		}

		private string typeKey; // Для Shlyah і Cabinet.gov.ua
		public string TypeKey
		{
			get { return typeKey; }
			set { typeKey = value; }
		}

		private string pathKey; // Для Shlyah і Cabinet.gov.ua
        public string PathKey
		{
			get { return pathKey; }
			set { pathKey = value; }
		}

		private string passwordKey; // Для файлового ключа
		public string PasswordKey
		{
			get { return passwordKey; }
			set { passwordKey = value; }
		}
	}
	public class UserTruck // Класс который получает список вантажних транспортів пользователя, і потім він використовується для записі їх в базу данних
	{
		public int TelegramId { get; set; } // Унікальний ID для кожного юзера
        public List<Trucks> Trucks { get; set; } = new(); // Список вантажних транспортів
	}
	public class Trucks 
	{
		public string TypeTruck { get; set; } = ""; // Тип вантажіви, программа візьме його із сайта Shlyah
		public string NumberTruck { get; set; } = ""; // Номер вантажівки, программа візьме його із сайта Shlyah
        public string ModelTruck { get; set; } = ""; // Модель вантажівки, программа візьме його із сайта Shlyah
    } // Класс який потрібен для передачі по одній вантажівці в UserTruck як список
	public class UserDriver
	{
        public int TelegramId { get; set; } // Унікальний ID для кожного юзера

        public string Country = "Україна"; // Стандартна страна для кожного водія, але її краще зробити динамічною щоб юзер міг змінювати її самостійно. А також після запису в базу данних юзер також буде взмозі змінити її

		public List<Drivers> Drivers { get; set; } = new(); // Список з якого буде записуватить водії в базу даних. Список отримується з класса Drivers
    }
	public class Drivers // Класс В якому міститься данні одного водія і потім він передається в список UserDriver. Всі даннні юзера отримуються з сайта Shlyah
	{
        public string FirstName { get; set; } // Ім'я водія 
        public string LastName { get; set; } // Фамилія водія
        public string Passport { get; set; } // Паспорт водія
        //public string BirthDay { get; set; } // День народження, але він вже більше не потрібен згідно з змінами в сайті Echerga
        //public string Email { get; set; }
        //public string Number { get; set; }
    }
	public class UserTrailers
	{
		public int TelegramId { get; set; } // Унікальний ID для кожного юзера

        public string Country = "Україна"; // Стандартна страна для кожного прицепа, але її краще зробити динамічною щоб юзер міг змінювати її самостійно. А також після запису в базу данних юзер також буде взмозі змінити її
        public List<Trailers> Trailers { get; set; } = new(); // Список прицепів юзера які записуться в базу данних. Вони отримуються з сайта Echerga.
    }
	public class Trailers // Класс який містить данні про один прицеп
	{
        public string NumberTrailer { get; set; } // Номер прицепа
    }
}

