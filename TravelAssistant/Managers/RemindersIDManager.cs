using System;
using System.Collections.Generic;
using TravelAssistant.Models;

namespace TravelAssistant.Managers
{
    // TODO Возможно изменить реализацию выдачи уникальных id для уведомлений!!
    public class RemindersIDManager
    {
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
        private int freeId;
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
        // TODO сделать более эфеткивно!!
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
        public void DeleteId(int id)
        {
            // Доп. проверка.
            if (UsedId.Contains(id))
            {
                freeId = id;
                UsedId.Remove(id);
            }
        }
        private List<int> UsedId = new List<int>();
    }
}
