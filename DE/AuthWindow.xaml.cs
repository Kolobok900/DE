using DE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DE
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text.Trim();
            string password = passwordBox.Text.Trim();

            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string hash = Convert.ToHexString(bytes).ToLower();

            var db = new shoesshopContext();
            var user = db.Users.FirstOrDefault(x => x.Login == login);

            if (user == null)
            {
                MessageBox.Show("Неверный Логин!");
                return;
            }
            if (hash != user.Password)
            {
                MessageBox.Show("Неверный Пароль!");
                return;
            }

            AppState.CurrentUser = user;

            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
