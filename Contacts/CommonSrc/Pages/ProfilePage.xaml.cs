using System;
using System.Collections.Generic;
using Contacts.CommonSrc.Configuration;
using Contacts.CommonSrc.Database;
using Xamarin.Forms;

namespace Contacts.CommonSrc.Pages
{
    public partial class ProfilePage : ContentPage
    {
        private static ProfilePage _instance;

        public static ProfilePage GetInstance(Contact contact)
        {
            if (_instance == null)
            {
                _instance = new ProfilePage();
            }
            _instance.Initialise(contact);
            return _instance;
        }

        public ProfilePage()
        {
            InitializeComponent();

            if (DesignTimeHelper.DesignModeOn)
            {
                Contact fakeContact = new Contact()
                {
                    Id = 1,
                    EmailAddress = "fake@faked.com",
                    KIKAccountName = "fakekik",
                    Address = "Tunbridge Wells",
                    Notes = "My Notes",
                    RealName = "Fake Name",
                    PostCode = "TN21 8ST",
                    TelephoneNumber = "01234 5678901",
                    WhatsAppNumber = "01234 5677890",
                };
                Initialise(fakeContact);
            }
        }

        protected void Initialise(Contact contact)
        {
            this.Title = contact.RealName;// + " (" + member.Age + ")";

            //DBDataSource ds = DBDataSource.GetInstance();

            //ds.GetMemberProfile(contact);

            BindingContext = contact;
        }

        protected override void OnDisappearing()
        {
            DBDataSource ds = DBDataSource.GetInstance();

            Contact contact = BindingContext as Contact;
            if (contact != null)
            {
                ds.SaveContact(contact);
            }
        }

    }
}
