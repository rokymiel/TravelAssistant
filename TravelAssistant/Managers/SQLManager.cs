using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using TravelAssistant.Models;

namespace TravelAssistant.Managers
{
    public class SQLManager<T> where T : Item, new()
    {
        public string StatusMessage { get; set; }
        SQLiteConnection connection;

        public SQLManager(string dbPath)
        {
            connection = new SQLiteConnection(dbPath);

            connection.CreateTable<T>();
        }
        public List<Note> GetNotes()
        {
            try
            {
                return connection.Table<Note>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Note>();
        }
        public List<Reminder> GetReminder()
        {
            try
            {
                return connection.Table<Reminder>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Reminder>();
        }
        /// <summary>
        /// Дает все элементы  из таблицы.
        /// </summary>
        /// <returns>Список элементов.</returns>
        public List<T> GetItems()
        {
            try
            {
                return connection.Table<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<T>();
        }
        /// <summary>
        /// Дает поездку по Id.
        /// </summary>
        /// <param name="tripId">Id поездки.</param>
        /// <returns>Поездку.</returns>
        public Trip GetTripById(string tripId)
        {
            return connection.Get<Trip>(tripId);
        }
        /// <summary>
        /// Получает список элементов типа R, которые содержат одинаковый TripId.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public List<R> GetTripItems<R>(string tripId) where R:TripData, new()
        {
            try
            {
                //return connection.Query<T>($"select * from notes where TripId='{tripId}'");
                return connection.Table<R>().Where(x=>x.TripId==tripId).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<R>();
        }
        public List<MoneyOperation> GetMoneyOperations()
        {
            try
            {
                return connection.Table<MoneyOperation>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<MoneyOperation>();
        }
        public Money GetMoney()
        {
            try
            {
                return connection.Table<Money>().ToList()[0];
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
            return null;
        }
        public List<Document> GetDocuments()
        {
            try
            {
                return connection.Table<Document>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Document>();
        }
        public List<Place> GetPlaces()
        {
            try
            {
                return connection.Table<Place>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Place>();
        }
        /// <summary>
        /// Обновить элемент в таблице.
        /// </summary>
        /// <param name="item">Элемент для обновления.</param>
        public void Update(Item item)
        {
            connection.Update(item);
        }
        /// <summary>
        /// Добавить новый элемент в таблицу.
        /// </summary>
        /// <param name="item">Новый элемент.</param>
        public void AddItem(T item)
        {
            int result = 0;
            result = connection.Insert(item);
        }
        /// <summary>
        /// Удалить переданный элемент.
        /// </summary>
        /// <param name="item">Элемент для удаления.</param>
        public void DeleteItem(Item item)
        {
            connection.Delete(item);
        }
        /// <summary>
        /// Удалить все содержимое таблицы.
        /// </summary>
        public void DeleteAll()
        {
            connection.DeleteAll<T>();
        }
        public bool ContainsPlace(Place item)
        {
            return connection.Find<Place>(item.Id) != null;
        }

    }
}
