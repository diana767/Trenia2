using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Trenia2.Models;

namespace Trenia2.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private readonly Trenia2Context _db;
        private readonly User CurrentUser;
        public OrdersPage(User user, Trenia2Context db)
        {
            InitializeComponent();
            CurrentUser = user;
            _db = db;
            DataContext = this;
            if (CurrentUser.IdRole!=1)
            {
                AddBtn.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Collapsed;
            }
            LoadProduct();
        }

        private void LoadProduct()
        {
            OrdersList.ItemsSource = _db.Zakazs
                .Include(x=>x.IdAdresNavigation)
                .Include(x => x.IdStatusNavigation)
                .Include(x => x.IdUserNavigation)
                .ToList();
        }

        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (CurrentUser.IdRole == 1 && OrdersList.SelectedItem is Zakaz orders)
            {
                NavigationService.Navigate(new AddEditOrdersPage(orders, _db, LoadProduct));
            }

        }

        private void VixodClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOrdersPage(null, _db, LoadProduct));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersList.SelectedItem is not Zakaz orders)
            {
                MessageBox.Show("Выберете заказ для удаления");
                return;
            }

           
            var rez = MessageBox.Show("Удалить заказ?", "Подтверждение", MessageBoxButton.YesNo);

            if (rez == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                //удаление товара
                _db.Database.ExecuteSqlInterpolated($"DELETE FROM SostavZakaz WHERE IdZakaz ={orders.IdZakaz}");
                _db.Zakazs.Remove(orders);
                _db.SaveChanges();
                LoadProduct();
                MessageBox.Show("Заказ успешно удален");

            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
