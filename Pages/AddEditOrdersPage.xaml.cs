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
    /// Логика взаимодействия для AddEditOrdersPage.xaml
    /// </summary>
    public partial class AddEditOrdersPage : Page
    {
        private readonly Trenia2Context _db = new Trenia2Context();
        private readonly Zakaz? _orders;
        private readonly Action _onSave;
        private string NameStr => _orders == null ? "Добавление товара" : "Редактирование товара"; // устанавливаем соответствующий заголовок
        public AddEditOrdersPage(Zakaz? orders, Trenia2Context db, Action onSaved)
        {
            InitializeComponent();
            _orders = orders;
            _db = db;
            _onSave = onSaved;
            DataContext = this;
            ComboboxsesLoad();
            if(_orders==null)
            {
                DataZak.SelectedDate = DateTime.Now;
                DataDost.SelectedDate = DateTime.Now.AddDays(3);
            }
            else
            {
                LoadOrders();
            }

        }

        //загружаем данные
        private void LoadOrders()
        {
            KodBox.Text = _orders!.Kod.ToString();
            StatusBox.SelectedValue = _orders.IdStatus;
            AdresBox.SelectedValue = _orders.IdAdres;
            DataZak.SelectedDate = _orders.DataZak.ToDateTime(TimeOnly.MinValue);
            DataDost.SelectedDate = _orders.DataDost.ToDateTime(TimeOnly.MinValue);
        }

        //загружаем комбобоксы
        private void ComboboxsesLoad()
        {
            StatusBox.ItemsSource = _db.Statuses.ToList();
            AdresBox.ItemsSource = _db.PunckVidachis.ToList();
        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {
            if (StatusBox.SelectedItem is not Status status)
            {
                MessageBox.Show("Выберете статус");
                return;
            }
            if (AdresBox.SelectedItem is not PunckVidachi adres)
            {
                MessageBox.Show("Выберете адрес");
                return;
            }
            if (KodBox.Text == "")
            {
                MessageBox.Show(" Код не может быть пустым");
                return;
            }
            if (!int.TryParse(KodBox.Text, out int kod))
            {
                MessageBox.Show("Введите корректное значение Код");
                return;
            }
            if(DataDost.SelectedDate==null)
            {
                MessageBox.Show(" Дата доставки не может быть пустым");
                return;
            }
            if (DataZak.SelectedDate == null)
            {
                MessageBox.Show("Дата заказа не может быть пустым");
                return;
            }
            try
            {
                Zakaz orders = _orders ?? new Zakaz
                {
                    IdZakaz = _db.Zakazs.Any() ? _db.Zakazs.Max(x => x.IdZakaz) + 1 : 1,
                    IdUser=_db.Users.Any()? _db.Users.First().IdUser:1
                };
                orders.Kod = kod;
                orders.IdStatus = status.IdStatus;
                orders.IdAdres = adres.IdPuncta;
                orders.DataZak = DateOnly.FromDateTime(DataZak.SelectedDate.Value);
                orders.DataDost = DateOnly.FromDateTime(DataDost.SelectedDate.Value);

                if (_orders == null)
                    _db.Zakazs.Add(orders);
                _db.SaveChanges();
                MessageBox.Show("Успешно сохранено");
                _onSave.Invoke();
                NavigationService.GoBack();

            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void NazadClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
