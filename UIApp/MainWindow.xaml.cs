using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIApp.Models;

namespace UIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();

        private ObservableCollection<Contact> allContacts;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<Contact> AllContacts
        {
            get { return allContacts; }
            set { allContacts = value; OnPropertyChanged(); }
        }

        private Contact contact;

        public Contact Contact
        {
            get { return contact; }
            set { contact = value; OnPropertyChanged(); }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Contact = new Contact();
        }

        private void GetAll_Click(object sender, RoutedEventArgs e)
        {
            GetAllContacts();
        }

        private async void GetAllContacts()
        {
            response = await httpClient.GetAsync($"https://localhost:22950/c");
            var str = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Contact>>(str);
            AllContacts = new ObservableCollection<Contact>(items);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Contact != null && Contact.Id != 0)
            {
                response = await httpClient.DeleteAsync($"https://localhost:22950/c/{Contact.Id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    GetAllContacts();
                    MessageBox.Show("Contact deleted successfully");
                }
            }
            else
            {
                MessageBox.Show("Please select any contact");
            }
        }

        private async void addContact_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Contact.Firstname) && !string.IsNullOrEmpty(Contact.Lastname))
            {
                var myContent = JsonConvert.SerializeObject(Contact);
                var buffer = Encoding.UTF8.GetBytes(myContent);
                var byteContent =new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                response = await httpClient.PostAsync("https://localhost:22950/c", byteContent);
                var str=await response.Content.ReadAsStringAsync();
                var item=JsonConvert.DeserializeObject<Contact>(str);
                if (item.Id != 0)
                {
                    GetAllContacts();
                    MessageBox.Show("Added successfully");
                }
                else
                {
                    MessageBox.Show("Error in add contact");
                }
            }
        }

        private void updateContact_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}