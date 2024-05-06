using GreenTrail.Forms;
using GreenTrail.Forms.Welcome;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Net;
using System.Configuration;

namespace GreenTrail.Source.Funs
{
    internal class DataBaseFuns
    {

        public static void AuthenticateUser(string username, string password,Window window)
        {
            try
            {
                Users user = GreanTrailEntities.GetContext().Users.FirstOrDefault(u => u.login == username && u.password == password);
                if (user == null)
                {
                    MessageBox.Show("Упс, ваш аккаунт не найден!","Упс!",MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Что-то пошло не так(",MessageBoxButton.OK,MessageBoxImage.Error );
            }
        }

        public static void Regin(string fullName, string email, string login, string dateOfBirth, string phoneNumber, string address, string password, Window window)
        {
            try
            {
                // Создание нового объекта пользователя
                Users user = new Users
                {

                };

                // Добавление нового пользователя в контекст базы данных
                GreanTrailEntities.GetContext().Users.Add(user);

                // Сохранение изменений в базе данных
                GreanTrailEntities.GetContext().SaveChanges();

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Что-то пошло не так(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool SeartchEmail(string email)
        {
            try
            {
                using (GreanTrailEntities db = new GreanTrailEntities())
                {
                    // Поиск пользователя по электронной почте
                    var user = db.Users.FirstOrDefault(u => u.email == email);

                    // Возврат true, если пользователь найден, иначе false
                    return user != null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Что-то пошло не так(", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }

        public static void SendOTP(string email, string OTP)
        {
            // Настройте учетную запись SMTP и учетные данные
            string smtpHost = "smtp.mail.ru";
            int smtpPort = 587;
            string smtpUsername = ConfigurationManager.AppSettings["mailApp"];
            string smtpPassword = ConfigurationManager.AppSettings["passwordMailApp"];

            // Создание SMTP-клиента
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                // Установка учетных данных SMTP
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                // Включение TLS для безопасной передачи
                client.EnableSsl = true;

                // Создание сообщения электронной почты
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(ConfigurationManager.AppSettings["mailApp"]),
                    To = { new MailAddress(email) },
                    Subject = "OTP код для сброса пароля",
                    Body = $"Ваш OTP код: {OTP}"
                };

                // Отправка сообщения электронной почты
                client.Send(message);
            }
        }

        public static void RessetPass(string email, string pass)
        {
            try
            {
                using (GreanTrailEntities db = new GreanTrailEntities())
                {
                    // Поиск пользователя по электронной почте
                    var user = db.Users.FirstOrDefault(u => u.email == email);

                    // Если пользователь не найден, выходим из метода
                    if (user == null)
                    {
                        return;
                    }

                    // Генерация нового пароля
                    string newPassword = pass;

                    // Установка нового пароля для пользователя
                    user.password = newPassword;

                    // Сохранение изменений в базе данных
                    db.SaveChanges();

                    // Отправка нового пароля пользователю по электронной почте
                    SendOTP(email, newPassword);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Что-то пошло не так(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
