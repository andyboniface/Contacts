using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Contacts.CommonSrc.Configuration;
using Contacts.CommonSrc.Utils;
using Xamarin.Forms;

namespace Contacts.CommonSrc.Database
{
    static class Extensions
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
            {
                collection.Move(collection.IndexOf(sorted[i]), i);
            }
        }
    }

    public class DBDataSource
    {
        private readonly DBService _externalDb;
        private readonly ILogging _logger;
        private const string DS_TASK_NAME = "DBDataSource";
        private static DBDataSource _instance;
        private const int INITIAL_DELAY = 5;

        public static DBDataSource GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DBDataSource();
            }
            return _instance;
        }

        private DBDataSource()
        {
            _logger = DependencyService.Get<ILogging>();

            ActiveContacts = new ObservableCollection<Contact>();
            _externalDb = new DBService();
        }

        public ObservableCollection<Contact> ActiveContacts { get; private set; }
        public bool Running { get; private set; }

        private async Task<TimeSpan> LoadFromExternalDatabase()
        {
            ActiveContacts.Clear();

            var table = await _externalDb.FetchAllContacts();

            _logger.LogInfo("Found " + table.Count + " contacts in databse");
            foreach (var contact in table)
            {
                ActiveContacts.Add(contact);
            }

            ActiveContacts.Sort();

            //TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME, TimeSpan.FromSeconds(INITIAL_DELAY), () => OnTimedEvent());
            return TimeScheduler.STOP_TIMER;                // This stops us being re-scheduled
        }

        public async void SaveContact(Contact contact)
        {

            _logger.LogInfo("Saving contact " + contact.Id + " profile=" + contact.RealName);

            if (contact.Id == 0)
            {
                Contact newContact = await _externalDb.AddNewContact(contact);
                if (newContact != null)
                {

                    contact.Id = newContact.Id;
                    ActiveContacts.Add(contact);
                }
            }
            else
            {
                Contact newContact = await _externalDb.ModifyContact(contact);
            }
        }

        public void StartDataSource()
        {
            //
            // Start our main scheduler....
            //
            TimeScheduler.GetTimeScheduler().AddTask(DS_TASK_NAME, TimeSpan.FromSeconds(INITIAL_DELAY), () => LoadFromExternalDatabase());
            Running = true;
        }


        public void StopDataSource()
        {
            TimeScheduler.GetTimeScheduler().RemoveTask(DS_TASK_NAME);
            Running = false;
        }

    }
}
