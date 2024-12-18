/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInformation
{
    // Класс TrainModel
    public class TrainModel
    {
        // Уникальный номер поезда
        public int TrainNumber { get; set; }

        // Маршрут следования поезда
        public string TrainRoute { get; set; }

        // Время отправления поезда
        public DateTime DepartureTime { get; set; }

        // Время прибытия поезда
        public DateTime ArrivalTime { get; set; }

        // Текущий статус поезда (например, "По расписанию", "Задержка", "Отменен")
        public TrainStatus Status { get; set; }

        // Список типов вагонов, входящих в состав поезда
        public List<TrainCarType> CarTypes { get; set; }
    }

    // Класс TrainType
    public class TrainType
    {
        // Название типа поезда ("Скоростной", "Пригородный", "Экспресс")
        public string Name { get; set; }

        // Описание типа поезда
        public string Description { get; set; }
    }

    // Класс TrainDirection
    public class TrainDirection
    {
        // Название направления ("Прибытие", "Отправление")
        public string Name { get; set; }

        // Флаг, указывающий, является ли данное направление прибытием
        public bool IsArrival { get; set; }
    }

    // Класс TrainSchedule
    public class TrainSchedule
    {
        private List<TrainModel> _trains;

        // Список поездов, входящих в расписание
        public List<TrainModel> Trains
        {
            get { return _trains; }
        }

        // Метод для фильтрации списка поездов по дате и направлению
        public List<TrainModel> FilterTrains(DateTime date, TrainDirection direction)
        {
            return _trains.Where(t => t.DepartureTime.Date == date.Date && t.Direction == direction).ToList();
        }

        // Метод для сортировки списка поездов по различным критериям
        public List<TrainModel> SortTrains(SortCriteria criteria)
        {
            switch (criteria)
            {
                case SortCriteria.DepartureTime:
                    return _trains.OrderBy(t => t.DepartureTime).ToList();
                case SortCriteria.ArrivalTime:
                    return _trains.OrderBy(t => t.ArrivalTime).ToList();
                case SortCriteria.TrainNumber:
                    return _trains.OrderBy(t => t.TrainNumber).ToList();
                default:
                    return _trains;
            }
        }
    }

    // Класс TrainService
    public class TrainService
    {
        // Метод для получения подробной информации о поезде по его номеру
        public TrainModel GetTrainDetails(int trainNumber)
        {
            // Реализация логики получения информации о поезде
            // Возвращает объект TrainModel
        }

        // Метод для обновления статуса поезда
        public bool UpdateTrainStatus(int trainNumber, TrainStatus newStatus)
        {
            // Реализация логики обновления статуса поезда
            // Возвращает true в случае успешного обновления, false - в противном случае
        }

        

        // Класс TrainLogger для логирования информации о поездах и действиях пользователей
        public class TrainLogger
        {
            // Список всех логов
            private List<LogEntry> _logEntries;

            // Метод для добавления записи в лог
            public void LogEvent(LogType type, string message, PersonProfile user = null)
            {
                LogEntry entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Type = type,
                    Message = message,
                    User = user
                };

                _logEntries.Add(entry);
                SaveLogToFile(entry);
            }

            // Метод для получения логов за определенный период
            public List<LogEntry> GetLogs(DateTime startDate, DateTime endDate)
            {
                return _logEntries
                    .Where(log => log.Timestamp >= startDate && log.Timestamp <= endDate)
                    .ToList();
            }

            // Приватный метод для сохранения лога в файл
            private void SaveLogToFile(LogEntry entry)
            {
                // Реализация логики сохранения лога в файл
                // Можно использовать различные форматы: txt, csv, json
            }

            // Метод для очистки старых логов
            public void ClearOldLogs(int daysToKeep)
            {
                _logEntries.RemoveAll(log => log.Timestamp < DateTime.Now.AddDays(-daysToKeep));
            }
        }
    }
}
*/