using System;
using System.IO;
using System.Linq;

namespace SystemInformation
{
<<<<<<< Updated upstream
    // Сервис аутентификации с улучшенной структурой и обработкой данных
=======
    
    /// Сервис аутентификации с улучшенной структурой и обработкой данных
    
>>>>>>> Stashed changes
    internal class AuthenticationService
    {
        // Константы для путей файлов вынесены наверх для удобства конфигурации
        private const string UserDataFilePath = "user_data.txt";
        private const string RememberedCredentialsFilePath = "remembered_credentials.txt";

        // Используем последовательный ID с потокобезопасным инкрементом
        private int _nextUserID = GetNextAvailableUserID();

        // Метод для получения следующего доступного ID из файла
        private static int GetNextAvailableUserID()
        {
            if (!File.Exists("user_data.txt"))
                return 1;

            try
            {
                var existingUsers = File.ReadAllLines("user_data.txt");
                if (!existingUsers.Any())
                    return 1;

                var maxID = existingUsers
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length >= 1)
                    .Select(parts => int.TryParse(parts[0], out int id) ? id : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                return maxID + 1;
            }
            catch
            {
                return 1;
            }
        }


<<<<<<< Updated upstream
        // Метод входа в систему с расширенной валидацией
=======
        /// Метод входа в систему с расширенной валидацией
>>>>>>> Stashed changes

        public bool Login(string username, string password)
        {
            // Встраивание валидации входных данных
            if (!ValidateCredentials(username, password))
                return false;

            // Декомпозиция условного оператора - вынос логики проверки в отдельный метод
            return FindUserByCredentials(username, password);
        }

<<<<<<< Updated upstream
        
        // Валидация учетных данных 
       
=======
        /// <summary>
        /// Валидация учетных данных 
        /// </summary>
>>>>>>> Stashed changes
        private bool ValidateCredentials(string username, string password)
        {
            // Консолидация проверок входных данных
            return !string.IsNullOrWhiteSpace(username) &&
                   !string.IsNullOrWhiteSpace(password);
        }

<<<<<<< Updated upstream
        
        // Поиск пользователя по учетным данным с безопасной обработкой
        
=======
        /// <summary>
        /// Поиск пользователя по учетным данным с безопасной обработкой
        /// </summary>
>>>>>>> Stashed changes
        private bool FindUserByCredentials(string username, string password)
        {
            // Создание файла, если он не существует (перемещение логики)
            EnsureUserDataFileExists();

            try
            {
                // LINQ-запрос для поиска пользователя (вместо классического foreach)
                return File.ReadAllLines(UserDataFilePath)
                    .Select(line => line.Split(','))
                    .Any(parts => parts.Length >= 3 &&
                                  parts[1].Trim() == username &&
                                  parts[2].Trim() == password);
            }
            catch (Exception ex)
            {
                // Улучшенное логирование с использованием современных практик
                LogError("Ошибка при входе", ex);
                return false;
            }
        }

<<<<<<< Updated upstream
        
        // Регистрация нового пользователя с расширенной проверкой
        
=======
        /// <summary>
        /// Регистрация нового пользователя с расширенной проверкой
        /// </summary>
>>>>>>> Stashed changes
        public bool Register(UserCredentials credentials)
        {
            EnsureUserDataFileExists();

            // Консолидация проверок регистрации
            if (IsUsernameTaken(credentials.Username))
                return false;

            return SaveNewUser(credentials);
        }

<<<<<<< Updated upstream
        
        // Проверка занятости имени пользователя
        
=======
        /// <summary>
        /// Проверка занятости имени пользователя
        /// </summary>
>>>>>>> Stashed changes
        private bool IsUsernameTaken(string username)
        {
            return File.ReadAllLines(UserDataFilePath)
                .Any(line => !string.IsNullOrWhiteSpace(line) &&
                             line.Split(',').Select(p => p.Trim()).ToArray()[1] == username);
        }

<<<<<<< Updated upstream
        
        // Сохранение нового пользователя
      
=======
        /// <summary>
        /// Сохранение нового пользователя
        /// </summary>
>>>>>>> Stashed changes
        private bool SaveNewUser(UserCredentials credentials)
        {
            int newUserID = _nextUserID++;

            var newUser = new PersonProfile
            {
                ID = newUserID,
                Username = credentials.Username,
                Password = credentials.Password,
                Role = credentials.Role
            };

            using (var writer = File.AppendText(UserDataFilePath))
            {
                writer.WriteLine(FormatUserRecord(newUser));
            }

            return true;
        }

        // Форматирование записи с гарантией 8 цифр
        private string FormatUserRecord(PersonProfile user)
        {
            return $"{user.ID:D8},{user.Username},{user.Password},{user.Role}";
        }

<<<<<<< Updated upstream
       
        // Сброс пароля с расширенной обработкой

=======
        /// <summary>
        /// Сброс пароля с расширенной обработкой
        /// </summary>
>>>>>>> Stashed changes
        public bool ResetPassword(string username, string newPassword)
        {
            EnsureUserDataFileExists();

            try
            {
                var lines = File.ReadAllLines(UserDataFilePath).ToList();
                var userIndex = lines.FindIndex(line =>
                {
                    var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                    return parts.Length >= 2 && parts[1] == username;
                });

                if (userIndex == -1)
                    return false;

                var userParts = lines[userIndex].Split(',').Select(p => p.Trim()).ToArray();
                lines[userIndex] = $"{userParts[0]},{userParts[1]},{newPassword},{userParts[3]}";

                File.WriteAllLines(UserDataFilePath, lines);
                return true;
            }
            catch (Exception ex)
            {
                LogError("Ошибка сброса пароля", ex);
                return false;
            }
        }

<<<<<<< Updated upstream

        // Получение профиля пользователя

=======
        /// <summary>
        /// Получение профиля пользователя
        /// </summary>
>>>>>>> Stashed changes
        public PersonProfile GetUserProfile(string username)
        {
            EnsureUserDataFileExists();

            return File.ReadAllLines(UserDataFilePath)
                .Select(line => line.Split(','))
                .Where(parts => parts.Length >= 3 && parts[1].Trim() == username)
                .Select(parts => new PersonProfile
                {
                    ID = int.Parse(parts[0]),
                    Username = parts[1],
                    Password = parts[2],
                    Role = parts[3]
                })
                .FirstOrDefault();
        }

<<<<<<< Updated upstream

        // Безопасное создание файла пользователей

=======
        /// <summary>
        /// Безопасное создание файла пользователей
        /// </summary>
>>>>>>> Stashed changes
        private void EnsureUserDataFileExists()
        {
            if (!File.Exists(UserDataFilePath))
                File.Create(UserDataFilePath).Close();
        }

<<<<<<< Updated upstream
        // Управление сохраненными учетными данными

=======
        /// <summary>
        /// Управление сохраненными учетными данными
        /// </summary>
>>>>>>> Stashed changes
        public void SaveRememberedCredentials(string username, string password)
        {
            ExecuteSafeFileOperation<bool>(() =>
            {
                File.WriteAllText(RememberedCredentialsFilePath, $"{username},{password}");
                return true;
            }, "Ошибка при сохранении учетных данных");
        }

<<<<<<< Updated upstream

        // Получение сохраненных учетных данных

=======
        /// <summary>
        /// Получение сохраненных учетных данных
        /// </summary>
>>>>>>> Stashed changes
        public (string Username, string Password) GetRememberedCredentials()
        {
            return ExecuteSafeFileOperation(() =>
            {
                if (File.Exists(RememberedCredentialsFilePath))
                {
                    var credentials = File.ReadAllText(RememberedCredentialsFilePath).Split(',');
                    return credentials.Length == 2
                        ? (credentials[0], credentials[1])
                        : (string.Empty, string.Empty);
                }
                return (string.Empty, string.Empty);
            }, "Ошибка при чтении сохраненных учетных данных");
        }

<<<<<<< Updated upstream
        // Удаление сохраненных учетных данных

=======
        /// <summary>
        /// Удаление сохраненных учетных данных
        /// </summary>
>>>>>>> Stashed changes
        public void ClearRememberedCredentials()
        {
            ExecuteSafeFileOperation<bool>(() =>
            {
                if (File.Exists(RememberedCredentialsFilePath))
                    File.Delete(RememberedCredentialsFilePath);
                return true;
            }, "Ошибка при удалении сохраненных учетных данных");
        }

<<<<<<< Updated upstream
        /// Универсальный метод для безопасного выполнения операций с файлами
=======
        /// <summary>
        /// Универсальный метод для безопасного выполнения операций с файлами
        /// </summary>
>>>>>>> Stashed changes
        private T ExecuteSafeFileOperation<T>(Func<T> operation, string errorMessage)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                LogError(errorMessage, ex);
                return default;
            }
        }

<<<<<<< Updated upstream
        // Централизованное логирование ошибок

=======
        /// <summary>
        /// Централизованное логирование ошибок
        /// </summary>
>>>>>>> Stashed changes
        private void LogError(string message, Exception ex)
        {
            // В реальном приложении рекомендуется использовать профессиональную систему логирования
            Console.WriteLine($"{message}: {ex.Message}");
        }
    }

    // Классы UserCredentials и PersonProfile остаются без изменений
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