using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemInformation
{
    public partial class Form1 : Form
    {
        private AuthenticationService authService = new AuthenticationService();
        private PersonProfile currentUser;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (authService.Login(username, password))
            {
                // Получаем профиль пользователя
                currentUser = authService.GetUserProfile(username);

                // Логируем успешный вход
                LogEntry.Log(
                    currentUser,
                    LogType.EmployeeAction,
                    "Successful authentication",
                    LogEntry.LOGIN_OPERATION
                );

                MessageBox.Show("Успешная аутентификация!");
                // Открываем Form2
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            else
            {
                LogEntry.Log(
                    null,
                    LogType.Error,
                    $"An unsuccessful login attempt for the user {username}"
                );

                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string username = Interaction.InputBox("Введите логин для восстановления пароля:", "Восстановление пароля");

            if (authService.ResetPassword(username))
            {
                // Получаем профиль пользователя
                PersonProfile user = authService.GetUserProfile(username);

                // Логируем смену пароля
                LogEntry.Log(
                    user,
                    LogType.EmployeeAction,
                    "Password Reset",
                    LogEntry.STATUS_CHANGE_OPERATION
                );

                MessageBox.Show("Пароль успешно изменен.");
                // Здесь можно добавить логику для перехода к следующей форме
            }
            else
            {
                // Логируем неудачную попытку сброса пароля
                LogEntry.Log(
                    null,
                    LogType.Error,
                    $"An unsuccessful attempt to reset the user's password {username}"
                );

                MessageBox.Show("Пользователь не найден.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Переходим на вторую вкладку TabControl
            tabControl1.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string username = textBox3.Text;
            string password = textBox4.Text;
            string role = comboBox1.Text;

            UserCredentials credentials = new UserCredentials
            {
                Username = username,
                Password = password,
                Role = role
            };

            if (authService.Register(credentials))
            {
                PersonProfile newUser = authService.GetUserProfile(username);

                LogEntry.Log(
                    newUser,
                    LogType.EmployeeAction,
                    "New registration",
                    LogEntry.STATUS_CHANGE_OPERATION
                );

                MessageBox.Show("Регистрация успешна.");
                // Возвращаемся на первую вкладку TabControl
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                // Логируем неудачную регистрацию
                LogEntry.Log(
                    null,
                    LogType.Error,
                    $"Failed registration attempt for the user {username}"
                );

                MessageBox.Show("Не удалось зарегистрировать пользователя.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool isRemembered = authService.IsRemembered(checkBox1.Checked);
            if (isRemembered)
            {
                // Здесь можно добавить логику для автозаполнения полей логина и пароля
                textBox1.Text = "savedUsername";
                textBox2.Text = "savedPassword";
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
