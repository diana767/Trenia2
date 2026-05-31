using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly Trenia2Context _db = new Trenia2Context();
        private ObservableCollection<Tovar> _allTovars = new ObservableCollection<Tovar>();
        public User CurrentUser { get; set; }
        public MainPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
            SortBox.SelectedIndex = 0;
            SuppliersLoad();
            BuIdRole();
            LoadProduct();
        }

        private void LoadProduct()
        {
            var product = _db.Tovars
                .Include(x=>x.IdCategoryNavigation)
                .Include(x => x.IdEdIzmNavigation)
                .Include(x => x.IdPostNavigation)
                .Include(x => x.IdProizNavigation)
                .ToList();
            _allTovars = new ObservableCollection<Tovar>(product);
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var quest = _db.Tovars.AsEnumerable();
            if(!string.IsNullOrEmpty(PoiskBox.Text))


            {
                string text = PoiskBox.Text.ToLower();
                quest = quest.Where(x =>
                (x.Articul != null && x.Articul.ToLower().Contains(text)) ||
                 (x.Opisanie != null && x.Opisanie.ToLower().Contains(text)) ||
                  (x.NameTovar != null && x.NameTovar.ToLower().Contains(text)) ||
                   (x.IdCategoryNavigation.Category1 != null && x.IdCategoryNavigation.Category1.ToLower().Contains(text)) ||
                    (x.IdEdIzmNavigation.EdIzm1 != null && x.IdEdIzmNavigation.EdIzm1.ToLower().Contains(text)) ||
                     (x.IdPostNavigation.Name != null && x.IdPostNavigation.Name.ToLower().Contains(text)) ||
                      (x.IdProizNavigation.Name != null && x.IdProizNavigation.Name.ToLower().Contains(text)));
            }
            if(PostBox.SelectedItem is Postavshik suppliers&&suppliers.IdPost!=0)
            {
                quest = quest.Where(x => x.IdPost == suppliers.IdPost);
            }
            if(SortBox.SelectedIndex==1)//сортировка по возрастанию
            {
                quest = quest.OrderBy(x => x.KolSklad);
            }
            else

             if (SortBox.SelectedIndex == 2)//сортировка по убыванию
            {
                quest = quest.OrderByDescending(x => x.KolSklad);
            }
            else
            {
                quest = quest.OrderBy(x => x.IdTovar);
            }
            ProductList.ItemsSource = new ObservableCollection<Tovar>(quest);
        }

        private void BuIdRole()
        {
          if (CurrentUser.IdRole==1)
            {
                AddBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Visible;
                OrdersBtn.Visibility = Visibility.Visible;
                FilterPanel.Visibility = Visibility.Visible;
            }

          else
          //скрываем для менеджера кнопки добавления и удаления
                if (CurrentUser.IdRole == 2)
            {
                AddBtn.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Collapsed;
                OrdersBtn.Visibility = Visibility.Visible;
                FilterPanel.Visibility = Visibility.Visible;
            }
          else
            {
                //скрываем для авторизированного пользователя все кнопки
                AddBtn.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Collapsed;
                OrdersBtn.Visibility = Visibility.Collapsed;
                FilterPanel.Visibility = Visibility.Collapsed;
            }

        }

        //загружаем поставщиков
        private void SuppliersLoad()
        {
            var suppliers = _db.Postavshiks.ToList();

            suppliers.Insert(0, new Postavshik { IdPost = 0, Name = "Все поставщики" });

            PostBox.ItemsSource = suppliers;
            PostBox.SelectedIndex = 0;
        }

        private void PoiskBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void PostBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null, _db, LoadProduct));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ProductList.SelectedItem is not Tovar product)
            {
                MessageBox.Show("Выберете товар для удаления");
                return;
            }

            var isOnOrders = _db.SostavZakazs.Any(x => x.IdTovar == product.IdTovar);

            if (isOnOrders)
            {
                MessageBox.Show("Нельзя удалить товар, который есть в заказе");
                return;
            }
            var rez = MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo);

            if(rez==MessageBoxResult.No)
            {
                return;
            }
            try
            {
                _db.Tovars.Remove(product);
                _db.SaveChanges();
                LoadProduct();
                MessageBox.Show("Товар успешно удален");

            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage(CurrentUser, _db));
        }

        private void VixodClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void ProductList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(CurrentUser.IdRole==1&&ProductList.SelectedItem is Tovar product)
            {
                NavigationService.Navigate(new AddEditProductPage(product, _db, LoadProduct));
            }
            
        }
    }
}
