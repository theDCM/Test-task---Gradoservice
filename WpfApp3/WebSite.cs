using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp3
{
    class WebSite : INotifyPropertyChanged
    {
        int id;
        string url;
        bool? availability;
        HttpClient client = new HttpClient();
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }
        public string IsAvailable
        {
            get
            {
                if (availability is null) return "Ожидает проверки";
                if (availability is true) return "Доступен";
                return "Недоступен";
            }
        }
        //[NotMapped]
        public async void CheckAvailability()
        {
            try
            {
                if ((await client.GetAsync(url)).StatusCode == HttpStatusCode.OK) availability = true;
                else availability = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(new string('=', 20));
                Console.WriteLine(e.Message + Environment.NewLine + e.Data);
                Console.WriteLine(new string('=', 20));
                availability = false;
            }
            OnPropertyChanged("IsAvailable");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
