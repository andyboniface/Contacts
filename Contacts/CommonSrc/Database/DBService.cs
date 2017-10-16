using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contacts.CommonSrc.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;

namespace Contacts.CommonSrc.Database
{
    public class DBService
    {
        private const string FETCH_ALL_URL = "http://www.dinokits.co.uk/MyContacts/getContacts.php";
        private const string ADD_NEW_CONTACT_URL = "http://www.dinokits.co.uk/MyContacts/addContact.php";
        private const string MODIFY_CONTACT_URL = "http://www.dinokits.co.uk/MyContacts/modifyContact.php";
        public const string FETCH_THUMBNAIL_URL = "http://www.dinokits.co.uk/MyContacts/getThumbnail.php?id=";

        private readonly ILogging _logger;

        public DBService()
        {
            _logger = DependencyService.Get<ILogging>();
        }

        public async Task<Contact> AddNewContact(Contact contact)
        {
            DBCommand cmd = new DBCommand();
            cmd.contact = contact;

            var dateTimeConverter = new IsoDateTimeConverter();
            string json = JsonConvert.SerializeObject(cmd, dateTimeConverter);

            var httpClient = new HttpClient();
            HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await httpClient.PostAsync(ADD_NEW_CONTACT_URL, contents);
            if ((resp != null) && (resp.IsSuccessStatusCode))
            {
                //
                // We need to parse the results - somehow convert back into an array of IEMembers....
                //
                string jsonReply = await resp.Content.ReadAsStringAsync();

                Contact replyMember = JsonConvert.DeserializeObject<Contact>(jsonReply, dateTimeConverter);
                return replyMember;
            }
            return null;
        }

        public async Task<Contact> ModifyContact(Contact contact)
        {
            DBCommand cmd = new DBCommand();
            cmd.contact = contact;

            var dateTimeConverter = new IsoDateTimeConverter();
            string json = JsonConvert.SerializeObject(cmd, dateTimeConverter);

            var httpClient = new HttpClient();
            HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await httpClient.PostAsync(MODIFY_CONTACT_URL, contents);
            if ((resp != null) && (resp.IsSuccessStatusCode))
            {
                //
                // We need to parse the results - somehow convert back into an array of IEMembers....
                //
                string jsonReply = await resp.Content.ReadAsStringAsync();

                _logger.LogInfo("ModifyContact reply= " + jsonReply);

                try
                {
                    Contact replyMember = JsonConvert.DeserializeObject<Contact>(jsonReply, dateTimeConverter);
                    return replyMember;
                }
                catch (Exception e)
                {
                    _logger.LogError("ModifyContact error=" + e.Message);
                    _logger.LogError("Stack=" + e.StackTrace);
                }
            }
            return null;
        }

        public async Task<List<Contact>> FetchAllContacts()
        {
            DBCommand cmd = new DBCommand();
            //cmd.requestingUsername = myUsername;

            string json = JsonConvert.SerializeObject(cmd);

            var httpClient = new HttpClient();
            HttpContent contents = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await httpClient.PostAsync(FETCH_ALL_URL, contents);
            if ((resp != null) && (resp.IsSuccessStatusCode))
            {
                //
                // We need to parse the results - somehow convert back into an array of IEMembers....
                //
                var dateTimeConverter = new IsoDateTimeConverter();

                string jsonReply = await resp.Content.ReadAsStringAsync();
                DBReply reply = JsonConvert.DeserializeObject<DBReply>(jsonReply, dateTimeConverter);

                return reply.contacts.ToList<Contact>();
            }
            return new List<Contact>();
        }

    }
}
