using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInformation
{
    public enum UserRole
    {
    Администратор,
    Диспетчер,
    Кассир
    }

    public enum LogType
    {
        Information,
        Warning,
        Error,
        EmployeeAction
    }

    public class LogEntry
    {
        private const string LogFilePath = "train_logs.log";

        // Константы для типов операций
        public const byte LOGIN_OPERATION = 0x11;
        public const byte STATUS_CHANGE_OPERATION = 0x12;
        public const byte SCHEDULE_MODIFICATION_OPERATION = 0x13;

        // Статический метод логирования
        public static void Log(PersonProfile user, LogType type, string message, byte operationType = 0)
        {
            try
            {
                DateTime timestamp = DateTime.Now;

                // Логирование в текстовый файл
                LogTextEntry(user, type, message, timestamp);

                // Если указан тип операции - логируем служебную информацию
                if (operationType != 0)
                {
                    LogEmployeeAction(user, timestamp, operationType);
                }
            }
            catch (Exception ex)
            {
                // Fallback logging to console
                Console.WriteLine($"Error log: {ex.Message}");
            }
        }

        // Метод текстового логирования
        private static void LogTextEntry(PersonProfile user, LogType type, string message, DateTime timestamp)
        {
            string logMessage = $"[{timestamp:yyyy-MM-dd HH:mm:ss}] [{type}] {message}";

            if (user != null)
            {
                logMessage += $" (User: {user.Username}, Role: {user.Role})";
            }

            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }

        // Метод служебного логирования действий сотрудника
        private static void LogEmployeeAction(PersonProfile user, DateTime timestamp, byte operationType)
        {
            if (user == null)
            {
                throw new InvalidOperationException("The user cannot be null to log employee actions");
            }
            using (FileStream fs = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
            {
                // 8 байт - дата/время в формате FILETIME
                byte[] timestampBytes = BitConverter.GetBytes(timestamp.ToFileTime());
                fs.Write(timestampBytes, 0, 8);

                // 1 байт - тип операции
                fs.WriteByte(operationType);

                // Преобразование ID сотрудника в байты
                string employeeId = user.ID.ToString("D8");
                byte idLength = (byte)employeeId.Length;

                // 1 байт - длина ID
                fs.WriteByte(idLength);

                // M байт - ID сотрудника
                byte[] idBytes = Encoding.UTF8.GetBytes(employeeId);
                fs.Write(idBytes, 0, idBytes.Length);

                // 1 байт - роль сотрудника
                byte roleCode = ConvertRoleToCode(user.Role);
                fs.WriteByte(roleCode);
            }
        }

        // Преобразование роли в код
        private static byte ConvertRoleToCode(string role)
        {
            switch (role)
            {
                case "Администратор": return 0x01;
                case "Диспетчер": return 0x02;
                case "Кассир": return 0x03;
                default: throw new ArgumentException("Неизвестная роль");
            }
        }
    }
}
