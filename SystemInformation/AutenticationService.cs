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
            // Проверяем входные параметры на null или пустые значения
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                // Если файл не существует, создаем его
                CreateUserDataFile();
            }

            try
            {
                // Проверяем, существует ли пользователь с указанными логином и паролем
                string[] lines = File.ReadAllLines(UserDataFilePath);

                foreach (string line in lines)
                {
                    // Защита от возможных пустых или некорректных строк
                    string[] parts = line.Split(',');

                    // Проверяем, что массив имеет достаточную длину
                    if (parts.Length >= 3 &&
                        parts[1].Trim() == username &&
                        parts[2].Trim() == password)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки (рекомендуется использовать систему логирования)
                Console.WriteLine($"Ошибка при входе: {ex.Message}");
            }

            return false;
        }

        public bool Register(UserCredentials credentials)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                CreateUserDataFile();
            }

            string[] lines = File.ReadAllLines(UserDataFilePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();

                if (parts.Length >= 2 && parts[1] == credentials.Username)
                {
                    return false; // Пользователь с таким именем уже существует
                }
            }

            PersonProfile newUser = new PersonProfile
            {
                ID = nextUserID++,
                Username = credentials.Username,
                Password = credentials.Password,
                Role = credentials.Role
            };

            using (StreamWriter writer = new StreamWriter(UserDataFilePath, true))
            {
                // Добавляем роль в запись файла
                writer.WriteLine($"{newUser.ID:D8},{newUser.Username},{newUser.Password},{newUser.Role}");
            }

            return true;
        }

        public bool ResetPassword(string username, string newPassword)
        {
            try
            {
                // Проверяем, существует ли файл с данными пользователей
                if (!File.Exists(UserDataFilePath))
                {
                    CreateUserDataFile();
                    return false;
                }

                // Читаем все строки
                string[] lines = File.ReadAllLines(UserDataFilePath);

                // Флаг для отслеживания успешности изменения
                bool passwordChanged = false;

                // Обновляем пароль
                for (int i = 0; i < lines.Length; i++)
                {
                    // Пропускаем пустые строки
                    if (string.IsNullOrWhiteSpace(lines[i]))
                        continue;

                    // Разделяем строку, убирая лишние пробелы
                    string[] parts = lines[i].Split(',').Select(p => p.Trim()).ToArray();

                    // Проверяем, что в строке достаточно элементов
                    if (parts.Length >= 4 && parts[1] == username)
                    {
                        // Обновляем пароль и сохраняем роль
                        lines[i] = $"{parts[0]},{parts[1]},{newPassword},{parts[3]}";
                        passwordChanged = true;
                        break;
                    }
                }

                // Если пользователь найден, перезаписываем файл
                if (passwordChanged)
                {
                    File.WriteAllLines(UserDataFilePath, lines);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                // В случае любой ошибки возвращаем false
                return false;
            }
        }

        public PersonProfile GetUserProfile(string username)
        {
            // Проверяем, существует ли файл с данными пользователей
            if (!File.Exists(UserDataFilePath))
            {
                return null;
            }

            // Читаем все строки из файла
            string[] lines = File.ReadAllLines(UserDataFilePath);

            foreach (string line in lines)
            {
                // Пропускаем пустые строки
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Разделяем строку, убирая лишние пробелы
                string[] parts = line.Split(',').Select(p => p.Trim()).ToArray();

                // Проверяем, что в строке достаточно элементов
                if (parts.Length >= 3 && parts[1] == username)
                {
                    return new PersonProfile
                    {
                        ID = int.Parse(parts[0]),
                        Username = parts[1],
                        Password = parts[2],
                        Role = parts[3]
                    };
                }
            }

            return null;
        }

        public bool IsRemembered(bool isChecked)
        {
            return isChecked;
        }

        private void CreateUserDataFile()
        {
            // Создаем новый файл с данными пользователей
            File.Create(UserDataFilePath).Close();
        }

        // Добавьте в класс AuthenticationService новый метод для работы с "Запомнить меня"
        public void SaveRememberedCredentials(string username, string password)
        {
            try
            {
                // Создаем файл для хранения последних введенных учетных данных
                File.WriteAllText("remembered_credentials.txt", $"{username},{password}");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка при сохранении учетных данных: {ex.Message}");
            }
        }

        public (string Username, string Password) GetRememberedCredentials()
        {
            try
            {
                if (File.Exists("remembered_credentials.txt"))
                {
                    string[] credentials = File.ReadAllText("remembered_credentials.txt").Split(',');
                    if (credentials.Length == 2)
                    {
                        return (credentials[0], credentials[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении сохраненных учетных данных: {ex.Message}");
            }
            return (string.Empty, string.Empty);
        }

        public void ClearRememberedCredentials()
        {
            try
            {
                if (File.Exists("remembered_credentials.txt"))
                {
                    File.Delete("remembered_credentials.txt");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении сохраненных учетных данных: {ex.Message}");
            }
        }
    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class PersonProfile
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}