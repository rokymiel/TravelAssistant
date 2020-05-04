using System;
using System.Collections.Generic;
using TravelAssistant.Models;

namespace TravelAssistant.Managers
{
    public class RemindersIDManager
    {
        /// <summary>
        /// Свободный Id.
        /// </summary>
        private int freeId;
        /// <summary>
        /// Список использованных Id.
        /// </summary>
        private List<int> UsedId = new List<int>();
        
        public RemindersIDManager(List<Reminder> reminders)
        {
            if (reminders != null && reminders.Count > 0)
            {
                foreach (var item in reminders)
                {
                    if (item.NotNotified)
                    {
                        UsedId.Add(item.NotificationId);
                    }
                    else
                    {
                        item.HasNotification = false;
                        item.NotificationId = 0;
                    }
                }
            }

            freeId = GetNewFreeId();
        }
        /// <summary>
        /// Свободный Id.
        /// </summary>
        public int FreeId
        {
            get
            {
                var oldFree = freeId;
                UsedId.Add(oldFree);
                freeId = GetNewFreeId();
                return oldFree;
            }
        }
        /// <summary>
        /// Обновление свободного Id.
        /// </summary>
        /// <returns></returns>
        private int GetNewFreeId()
        {
            for (int i = int.MinValue; i <= int.MaxValue; i++)
            {
                if (!UsedId.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// Удаление используемого Id.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteId(int id)
        {
            // Доп. проверка.
            if (UsedId.Contains(id))
            {
                freeId = id;
                UsedId.Remove(id);
            }
        }
        
    }
}
