using DE.Models;
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

namespace DE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            authUser.Text = $"ID:{AppState.CurrentUser!.Id}, ФИО:{AppState.CurrentUser!.LastName} {AppState.CurrentUser!.FirstName} {AppState.CurrentUser!.Surname}";
            loadUsers();
            loadComboBoxes();
        }

        private void loadComboBoxes()
        {
            try
            {
                var db = new shoesshopContext();
                var roles = db.Roles.ToArray();
                var departments = db.Departments.ToArray();

                filterBox1.ItemsSource = roles;
                filterBox1.DisplayMemberPath = "Name";
                filterBox1.SelectedValuePath = "Id";
                filterBox2.ItemsSource = departments;
                filterBox2.DisplayMemberPath = "Name";
                filterBox2.SelectedValuePath = "Id";
            }
            catch(Exception ex)
            {
                    MessageBox.Show(ex.Message);
            }
        }
        private void loadUsers()
        {
            try
            {
                var db = new shoesshopContext();
                userData.ItemsSource = (AppState.CurrentUser!.ManagerId.Equals(null) ? 
                    db.Users.ToList() : db.Users.Where(x => x.ManagerId == AppState.CurrentUser!.Id)
                    .ToList()).Join(db.Departments, u => u.DepartmentId, d => d.Id, (u, d) => new { u, d })
                    .Join(db.Roles, combined => combined.u.Role, r => r.Id, (combined, r) => new 
                    { Id = combined.u.Id, LastName = combined.u.LastName, FirstName = combined.u.FirstName, Surname = combined.u.Surname,
                        Login = combined.u.Login, Password = combined.u.Password, ManagerId = combined.u.ManagerId,
                        DepartmentName = combined.d.Name, RoleName = r.Name, DepartmentId = combined.u.DepartmentId, RoleId = combined.u.Role })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new shoesshopContext();
                var filter = searchBox.Text;
                userData.ItemsSource = (string.IsNullOrWhiteSpace(filter) ? 
                    db.Users.Where(x => x.ManagerId == AppState.CurrentUser!.Id) 
                    : db.Users.Where(x => x.FirstName.Contains(filter) 
                    || x.LastName.Contains(filter) 
                    || x.Surname.Contains(filter))
                    .Where(x => x.ManagerId == AppState.CurrentUser!.Id))
                    .Join(db.Departments, u => u.DepartmentId, d => d.Id, (u, d) => new { u, d })
                    .Join(db.Roles, combined => combined.u.Role, r => r.Id, (combined, r)
                    => new { Id = combined.u.Id, LastName = combined.u.LastName, FirstName = combined.u.FirstName, Surname = combined.u.Surname,
                        Login = combined.u.Login, Password = combined.u.Password, ManagerId = combined.u.ManagerId,
                        DepartmentName = combined.d.Name, RoleName = r.Name, DepartmentId = combined.u.DepartmentId, RoleId = combined.u.Role })
                    .Where(x => filterBox1.SelectedValue == null || x.RoleId == (int)filterBox1.SelectedValue)
                    .Where(x => filterBox2.SelectedValue == null || x.DepartmentId == (int)filterBox2.SelectedValue)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
