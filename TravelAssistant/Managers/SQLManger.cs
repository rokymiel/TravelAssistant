using System;
using System.Collections.Generic;
using SQLite;
using TravelAssistant.Models;

namespace TravelAssistant.Managers
{
    public class SQLManger<T> where T : Item
    {
        public string StatusMessage { get; set; }
        SQLiteConnection connection;

        public SQLManger(string dbPath)
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
        public void Update(Item item)
        {
            connection.Update(item);
        }

        public void AddItem(T item)
        {
            int result = 0;
            result = connection.Insert(item);
        }
        public void DeleteItem(Item item)
        {
            connection.Delete(item);
        }
        public bool ContainsPlace(Place item)
        {
            return connection.Find<Place>(item.Id)!=null;
        }

    }
}
