using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite; //библиотека Microsoft BaseDate
using Serilog; //библиотека для логов

namespace TelegramBot
{
    public static class BaseData
    {
        public static void CreateBaseData() // створення таблиць
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\ukhal\\AppData\\Local\\TelegramAutoBot\\DateBase.db")) // зробити місце находження в перемінній|конфігові
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Users " +
                    "(" +
                    "Id INTEGER NOT NULL PRIMARY KEY UNIQUE, " +
                    "Email TEXT NOT NULL, " +
                    "DefaultNumber TEXT NOT NULL UNIQUE," +
                    "Password_Queue TEXT NOT NULL," +
                    "TypeKey TEXT NOT NULL, " +
                    "PathKey TEXT NOT NULL, " +
                    "Password_Key TEXT NOT NULL " +
                    ");" +
                    "" +
                    "CREATE TABLE IF NOT EXISTS Trucks " +
                    "(" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "User_Id INTEGER," +
                    "Country TEXT," +
                    "Type TEXT," +
                    "Model TEXT," +
                    "NumberVehicle TEXT," +
                    "FOREIGN KEY (User_Id) REFERENCES Users (Id)" +
                    ");" +
                    "" +
                    "CREATE TABLE IF NOT EXISTS Drivers " +
                    "(" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "User_Id INTEGER," +
                    "Country TEXT, " +
                    "FirstName TEXT, " +
                    "LastName TEXT, " +
                    "Passpord TEXT, " +
                    "BirthDay TEXT, " +
                    "Email TEXT, " +
                    "Number TEXT, " +
                    "FOREIGN KEY (User_Id) REFERENCES Users (Id)" +
                    ");" +
                    "" +
                    "CREATE TABLE IF NOT EXISTS Trailers" +
                    "(" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "User_Id INTEGER," +
                    "Country TEXT," + // Зробити по умолчанию Україна, а також реалізувати можливість зміни страни на потрібну
                    "NumberTrailer TEXT," +
                    "FOREIGN KEY (User_Id) REFERENCES Users (Id)" +
                    ");";
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void NewUsers(UserData user) // Создаемо нового юзера
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\ukhal\\AppData\\Local\\TelegramAutoBot\\DateBase.db"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Users (Id, Email, DefaultNumber, Password_Queue, TypeKey, PathKey, Password_Key)" +
                        "VALUES (@id, @email, @defaultNumber, @password_Queue, @typeKey, @pathKey, @password_Key)";
                    command.Parameters.AddWithValue("@id", user.TelegramId);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@defaultNumber", user.PhoneNumber);
                    command.Parameters.AddWithValue("@password_Queue", user.PasswordQueue);
                    command.Parameters.AddWithValue("@typeKey", user.TypeKey);
                    command.Parameters.AddWithValue("@pathKey", user.PathKey);
                    command.Parameters.AddWithValue(@"password_Key", user.PasswordKey);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex) // Потрібно доробити опис помилки
                    {
                        Log.Information("Пользователь с таким ID уже существует");
                        Console.WriteLine(ex.Message);

                    }

                }
            }
        }
        public static void AddUserTrucks(UserTruck userTruck)
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\ukhal\\AppData\\Local\\TelegramAutoBot\\DateBase.db"))
            {
                connection.Open();
                for (int i = 0; i < userTruck.Trucks.Count; i++)
                {
                    using (var command = connection.CreateCommand()) // Немає обробки вантажівки якщо вона вже була у пользователя
                    {

                        command.CommandText = @"INSERT INTO Trucks (User_Id, Type, Model, NumberVehicle)" +
                                              $"VALUES (@id, @type, @model, @numberVehicle)";
                        command.Parameters.AddWithValue("@id", userTruck.TelegramId);
                        command.Parameters.AddWithValue("@type", userTruck.Trucks[i].TypeTruck);
                        command.Parameters.AddWithValue("@model", userTruck.Trucks[i].ModelTruck);
                        command.Parameters.AddWithValue("@numberVehicle", userTruck.Trucks[i].NumberTruck);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        public static void AddUserDrivers(UserDriver userDriver)
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\ukhal\\AppData\\Local\\TelegramAutoBot\\DateBase.db"))
            {
                connection.Open();
                for (int i = 0; i < userDriver.Drivers.Count; i++)
                {
                    using (var command = connection.CreateCommand()) // Немає обробки якщо водій вже був у пользователя
                    {

                        command.CommandText = @"INSERT INTO Drivers (User_Id, FirstName, LastName, Passpord, BirthDay)" +
                                               $"VALUES (@id, @firstName, @lastName, @passport, @birthDay)";
                        command.Parameters.AddWithValue("@id", userDriver.TelegramId);
                        command.Parameters.AddWithValue("@firstName", userDriver.Drivers[i].FirstName);
                        command.Parameters.AddWithValue("@lastName", userDriver.Drivers[i].LastName);
                        command.Parameters.AddWithValue("@passport", userDriver.Drivers[i].Passport);
                        //command.Parameters.AddWithValue("@birthDay", userDriver.Drivers[i].BirthDay);
                        command.ExecuteNonQuery();
                    }
                }
            }            
        }
        public static void AddUserTrailers(UserTrailers userTrailers)
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\ukhal\\AppData\\Local\\TelegramAutoBot\\DateBase.db"))
            {
                connection.Open();
                for (int i = 0; i < userTrailers.Trailers.Count; i++)
                {
                    using (var command = connection.CreateCommand())
                    {



                        command.CommandText = "INSERT INTO Trailers (User_Id, NumberTrailer)" +
                                              "VALUES (@user_Id, @numberTrailer)";
                        command.Parameters.AddWithValue("@user_Id", userTrailers.TelegramId);
                        command.Parameters.AddWithValue("@numberTrailer", userTrailers.Trailers[i].NumberTrailer); // Добавити  по умолчанию Україна, а також реалізувати можливість зміни страни на потрібну
                        command.ExecuteNonQuery();

                    }
                }
            }
        }
    }
}
