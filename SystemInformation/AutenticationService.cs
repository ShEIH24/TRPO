using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SystemInformation
{
    internal class AuthenticationService
    {
        private const string UserDataFilePath = "user_data.txt";
        private int nextUserID = 1;
        bool resultAuten = false;

        public bool Login(string username, string password)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                // Если файл не существует, создаем его
                CreateUserDataFile();
            }

            // Проверяем, существует ли пользователь с указанными логином и паролем
            string[] lines = File.ReadAllLines(UserDataFilePath);
            
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[1] == username && parts[2] == password)
                {
                    return resultAuten = true;
                }
            }
            return resultAuten;
        }

        public bool Register(UserCredentials credentials)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                // Если файл не существует, создаем его
                CreateUserDataFile();
            }

            // Проверяем, существует ли пользователь с таким же именем
            string[] lines = File.ReadAllLines(UserDataFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[1] == credentials.Username)
                {
                    return false; // Пользователь с таким именем уже существует
                }
            }

            // Создаем новый профиль пользователя
            PersonProfile newUser = new PersonProfile
            {
                ID = nextUserID++,
                Username = credentials.Username,
                Password = credentials.Password
            };

            // Записываем данные нового пользователя в файл
            using (StreamWriter writer = new StreamWriter(UserDataFilePath, true))
            {
                writer.WriteLine($"{newUser.ID},{newUser.Username},{newUser.Password}");
            }

            return true;
        }

        public bool ResetPassword(string username)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                // Если файл не существует, создаем его
                CreateUserDataFile();
            }

            // Проверяем, существует ли пользователь с указанным логином
            string[] lines = File.ReadAllLines(UserDataFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts[1] == username)
                {
                    // Обновляем пароль пользователя
                    lines[i] = $"{parts[0]},{parts[1]},{GetNewPassword()}";
                    File.WriteAllLines(UserDataFilePath, lines);
                    return true;
                }
            }
            return false;
        }

        public PersonProfile GetUserProfile(string username)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                // Если файл не существует, создаем его
                CreateUserDataFile();
            }

            // Ищем профиль пользователя по имени
            string[] lines = File.ReadAllLines(UserDataFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[1] == username)
                {
                    return new PersonProfile
                    {
                        ID = int.Parse(parts[0]),
                        Username = parts[1],
                        Password = parts[2]
                    };
                }
            }

            return null;
        }

        public bool IsRemembered(bool isChecked)
        {
            return isChecked;
        }

        private string GetNewPassword()
        {
            // Реализуйте логику для получения нового пароля от пользователя
            // Например, с помощью окна ввода данных
            return "newpassword";
        }

        private void CreateUserDataFile()
        {
            // Создаем новый файл с данными пользователей
            File.Create(UserDataFilePath).Close();
        }
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class PersonProfile
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

