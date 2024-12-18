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
            // Проверяем согласие с политикой конфиденциальности
            if (!checkBox2.Checked)
            {
                MessageBox.Show(
                    "Ознакомьтесь с политикой конфиденциальности",
                    "Ошибка авторизации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

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
            checkBox1.Checked = false;
            // Если галочка не установлена
            if (!checkBox2.Checked)
            {
                // Показываем сообщение об ошибке
                MessageBox.Show(
                    "Ознакомьтесь с политикой конфиденциальности",
                    "Ошибка авторизации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Блокируем кнопку входа
                button3.Enabled = false;
            }
            else
            {
                // Разблокируем кнопку входа, если другие условия позволяют
                button3.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Запрос логина
            string username = Interaction.InputBox("Введите логин для восстановления пароля:", "Восстановление пароля");

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Логин не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Проверяем существование пользователя
                PersonProfile user = authService.GetUserProfile(username);

                if (user != null)
                {
                    // Запрос нового пароля
                    string newPassword = Interaction.InputBox("Введите новый пароль:", "Восстановление пароля");

                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Попытка сброса пароля
                    if (authService.ResetPassword(username, newPassword))
                    {
                        // Логируем смену пароля
                        LogEntry.Log(
                            user,
                            LogType.EmployeeAction,
                            "Password Reset",
                            LogEntry.STATUS_CHANGE_OPERATION
                        );

                        MessageBox.Show("Пароль успешно изменен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось изменить пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Логируем неудачную попытку сброса пароля
                    LogEntry.Log(
                        null,
                        LogType.Error,
                        $"An unsuccessful attempt to recover the user's password {username}"
                    );

                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Логируем системную ошибку
                LogEntry.Log(
                    null,
                    LogType.Error,
                    $"System error during password recovery: {ex.Message}"
                );

                MessageBox.Show($"Произошла системная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Переходим на вторую вкладку TabControl
            tabControl1.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Проверяем наличие сохраненных credentials
            var (savedUsername, savedPassword) = authService.GetRememberedCredentials();

            if (!string.IsNullOrWhiteSpace(savedUsername) &&
                !string.IsNullOrWhiteSpace(savedPassword))
            {
                textBox1.Text = savedUsername;
                textBox2.Text = savedPassword;
                checkBox1.Checked = true;
            }
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
            if (checkBox1.Checked)
            {
                // Сохраняем текущие credentials, если они не пустые
                if (!string.IsNullOrWhiteSpace(textBox1.Text) &&
                    !string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    // Проверяем корректность логина
                    if (authService.Login(textBox1.Text, textBox2.Text))
                    {
                        authService.SaveRememberedCredentials(textBox1.Text, textBox2.Text);
                        // Логируем действие
                        PersonProfile currentUser = authService.GetUserProfile(textBox1.Text);
                        LogEntry.Log(currentUser, LogType.Information, "The 'Remember Me' option is enabled'");
                    }
                    else
                    {
                        MessageBox.Show("Неверные учетные данные. Авторизация не выполнена.");
                        checkBox1.Checked = false;
                    }
                }
            }
            else
            {
                // Очищаем сохраненные credentials
                authService.ClearRememberedCredentials();
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
