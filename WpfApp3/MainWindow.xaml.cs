using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread updaterThread = new Thread(UpdateAvailability);
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            db.WebSites.Load();
            db.CounterStores.Load();
            DataContext = db.WebSites.Local.ToBindingList();
            intervalTextBox.DataContext = db.CounterStores.Local.ToBindingList().First();
            updaterThread.Start(db);
        }

        private void SetInterval_Click(object sender, RoutedEventArgs e)
        {
            db.CounterStores.First().Counter = Convert.ToInt32(intervalTextBox.Text);
            db.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            db.WebSites.Remove(webSitesList.SelectedItem as WebSite);
            db.SaveChanges();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            db.WebSites.Add(new WebSite() { Url = urlTextBox.Text });
            db.SaveChangesAsync();
        }

        private static void UpdateAvailability(object param)
        {
            ApplicationContext db = param as ApplicationContext;
            while (true)
            {
                for (int i = 0; i < db.WebSites.Count(); i++)
                    db.WebSites.ToList<WebSite>()[i].CheckAvailability();
                Thread.Sleep(new TimeSpan(0, 0, db.CounterStores.First().Counter));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updaterThread.Abort();
            db.SaveChanges();
            db.Dispose();
        }
    }
}