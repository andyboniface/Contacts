using Contacts.CommonSrc.Database;
using Contacts.CommonSrc.Pages;
using Xamarin.Forms;

namespace Contacts
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            DBDataSource ds = DBDataSource.GetInstance();
            if (ds.Running == false)
            {
                ds.StartDataSource();
            }
            MainPage = new NavigationPage(new ContactListPage());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            DBDataSource ds = DBDataSource.GetInstance();
            ds.StopDataSource();
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            DBDataSource ds = DBDataSource.GetInstance();
            ds.StartDataSource();
        }
    }
}
