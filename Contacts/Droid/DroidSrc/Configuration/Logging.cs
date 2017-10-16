using System;
using Contacts.CommonSrc.Configuration;
using Xamarin.Forms;

[assembly: Dependency(typeof(Contacts.Droid.DroidSrc.Configuration.Logging))]
namespace Contacts.Droid.DroidSrc.Configuration
{
    public class Logging : ILogging
    {
        public Logging()
        {

        }

        public void LogError(string msg)
        {
            Console.WriteLine("MyContacts:Error: " + msg);
        }

        public void LogInfo(string msg)
        {
            Console.WriteLine("MyContacts:Info: " + msg);
        }

        public void LogWarning(string msg)
        {
            Console.WriteLine("MyContacts:Warn: " + msg);
        }
    }
}