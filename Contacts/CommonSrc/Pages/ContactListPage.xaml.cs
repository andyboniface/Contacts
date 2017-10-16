using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Contacts.CommonSrc.Database;
using Xamarin.Forms;

namespace Contacts.CommonSrc.Pages
{
    public partial class ContactListPage : ContentPage
    {
        public ContactListPage()
        {
            InitializeComponent();

            this.Title = "Contacts";
            NavigationPage.SetHasNavigationBar(this, false);

            DBDataSource ds = DBDataSource.GetInstance();
            Items = ds.ActiveContacts;
            BindingContext = this;

        }

        public async void OnSelected(object sender, EventArgs e)
        {
            if (ContactListView.SelectedItem != null)
            {
                Contact contact = ContactListView.SelectedItem as Contact;
                //DisplayAlert("SelectedItem", MemberListView.SelectedItem.ToString(), "OK");
                await Navigation.PushAsync(ProfilePage.GetInstance(contact));
                ContactListView.SelectedItem = null;
            }
        }

        public ObservableCollection<Contact> Items { get; private set; }

    }
}
