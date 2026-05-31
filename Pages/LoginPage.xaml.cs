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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly Trenia2Context _db = new Trenia2Context();
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Vxod_Polzovatel(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (login=="" || password=="")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            var user = _db.Users.FirstOrDefault(x => x.Login.Trim() == login && x.Password.Trim() == password); // проверяем соответвуют ли введенные логин и пароль тем , что в бд

            if(user==null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            NavigationService.Navigate(new MainPage(user));
        }

        private void Vxod_Gost(object sender, RoutedEventArgs e)
        {
            var quest = new User
            {
                IdRole = 4,
                Fio = "Гость"
            };
            NavigationService.Navigate(new MainPage(quest));
        }
    }
}
