using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Contacts.CommonSrc.Database
{
    public class Contact : IComparable, INotifyPropertyChanged
    {
        public Contact()
        {
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("realName")]
        public string RealName { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("telephoneNumber")]
        public string TelephoneNumber { get; set; }
        [JsonProperty("kikAccountName")]
        public string KIKAccountName { get; set; }
        [JsonProperty("whatsAppNumber")]
        public string WhatsAppNumber { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("postcode")]
        public string PostCode { get; set; }
        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }
        [JsonProperty("firstMeet")]
        public DateTime FirstMeet { get; set; }

        public string UsernameAndAge
        {
            get
            {
                string Age = "?";
                return RealName + " (" + Age + ")";
            }
        }
        public string ThumbnailUrl
        {
            get
            {
                return DBService.FETCH_THUMBNAIL_URL + Id;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object o)
        {
            Contact a = this;
            Contact b = (Contact)o;
            return string.Compare(a.RealName, b.RealName);
        }



        public void FirePropertyChangeEvent()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));            // Empty string means everything has changed
            }
        }
    }
}
