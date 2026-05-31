using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        private readonly Trenia2Context _db = new Trenia2Context();
        private readonly Tovar? _product;
        private readonly Action _onSave;

        private string _photo = "picture.png";

        private string NameStr => _product == null ? "Добавление товара" : "Редактирование товара";  // устанавливаем соответствующий заголовок
        public AddEditProductPage(Tovar? product, Trenia2Context db, Action onSaved)
        {
            InitializeComponent();
            _product = product;
            _db = db;
            _onSave = onSaved;
            DataContext = this;
            ComboboxesLoad();

            if(_product==null)
            {
                IdBox.Text = "Автоматически";
                ShowPhoto(_photo);
            }
            else
            {
                LoadProduct();
            }
        }

       //загрузка данных
        private void LoadProduct()
        {
            IdBox.Text = _product!.IdTovar.ToString();
            NameBox.Text = _product.NameTovar;
            Opisanie.Text = _product.Opisanie;
            SaleBox.Text = _product.Sale.ToString();
            KolSkladBox.Text = _product.KolSklad.ToString();
            Price.Text = _product.Price.ToString();
            CategoryBox.SelectedValue = _product.IdCategory;
            EdIzmBox.SelectedValue = _product.IdEdIzm;
            ProizBox.SelectedValue = _product.IdProiz;
            PostBox.SelectedValue = _product.IdPost;
            _photo = string.IsNullOrEmpty(_product.Photo) ? "picture.png" : _product.Photo;
            ShowPhoto(_photo);
        }

        private void ShowPhoto(string photo)
        {
            ImagesBox.Source = new BitmapImage(new Uri(photo, UriKind.RelativeOrAbsolute));
        }



        private void ComboboxesLoad()
        {
            CategoryBox.ItemsSource = _db.Categories.ToList();
            EdIzmBox.ItemsSource = _db.EdIzms.ToList();
            PostBox.ItemsSource = _db.Postavshiks.ToList();
            ProizBox.ItemsSource = _db.Proizvoditels.ToList();
        }

        private void SaveBtn(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text=="")
            {
                MessageBox.Show("Наименование не может быть пустым");
                return;
            }
            if (Opisanie.Text == "")
            {
                MessageBox.Show("Описание не может быть пустым");
                return;
            }
            if (SaleBox.Text == "")
            {
                MessageBox.Show("Скидка не может быть пустым");
                return;
            }
            if(!int.TryParse(SaleBox.Text, out int sale))
            {
                MessageBox.Show("Введите корректное значение скидки");
                return;
            }
            if(sale<0)
            {
                MessageBox.Show("Скидка не может быть отрицательным");
                return;
            }
            if (Price.Text == "")
            {
                MessageBox.Show("Цена не может быть пустым");
                return;
            }
            if (!decimal.TryParse(Price.Text, out decimal price))
            {
                MessageBox.Show("Введите корректное значение цены");
                return;
            }
            if (price < 0)
            {
                MessageBox.Show("Цена не может быть отрицательным");
                return;
            }
            if (KolSkladBox.Text == "")
            {
                MessageBox.Show("Количество не может быть пустым");
                return;
            }
            if (!int.TryParse(KolSkladBox.Text, out int kol))
            {
                MessageBox.Show("Введите корректное значение количество");
                return;
            }
            if (kol < 0)
            {
                MessageBox.Show("Количество не может быть отрицательным");
                return;
            }

            if (CategoryBox.SelectedItem is not Category category)
            {
                MessageBox.Show("Выберете категорию");
                return;
            }
            if (PostBox.SelectedItem is not Postavshik post)
            {
                MessageBox.Show("Выберете поставщика");
                return;
            }
            if (ProizBox.SelectedItem is not Proizvoditel proiz)
            {
                MessageBox.Show("Выберете производителя");
                return;
            }
            if (EdIzmBox.SelectedItem is not EdIzm edIzm)
            {
                MessageBox.Show("Выберете единицу измерения");
                return;
            }
            try
            {
                Tovar product = _product ?? new Tovar
                {
                    IdTovar= _db.Tovars.Any()? _db.Tovars.Max(x=>x.IdTovar)+1:1
                };
                product.Articul = "ART" + product.IdTovar;
                product.NameTovar = NameBox.Text.Trim();
                product.Opisanie = Opisanie.Text.Trim();
                product.Sale = sale;
                product.Price = price;
                product.KolSklad = kol;
                product.IdCategory = category.IdCategory;
                product.IdPost = post.IdPost;
                product.IdProiz = proiz.IdProizv;
                product.IdEdIzm = edIzm.IdEdIzm;
                product.Photo = _photo;
                if (_product == null)
                    _db.Tovars.Add(product);
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


        //открытие окна выбора изображений
        private void SavePhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpeg;*.png;*.jpg";
            if(dialog.ShowDialog()==true)
            {
                _photo = dialog.FileName;
                ShowPhoto(_photo);
            }
        }
    }
}
