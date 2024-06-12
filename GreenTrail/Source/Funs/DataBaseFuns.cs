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
using Microsoft.Win32;
using GreenTrail.Properties;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace GreenTrail.Source.Funs
{
    internal class DataBaseFuns
    {

        internal static BitmapFrame LoadImage()
        {
            using (GreanTrailEntities dbContext = new GreanTrailEntities())
            {
                var user = DataBaseFuns.GetCurrentUser();

                if (user.image != null)
                {
                    byte[] imageData = Convert.FromBase64String(user.image);
                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        stream.Position = 0;
                        BitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        BitmapFrame frame = decoder.Frames[0];

                        return frame;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static void AuthenticateUser(string username, string password,Window window)
        {
            try
            {
                Users user = GreanTrailEntities.GetContext().Users.FirstOrDefault(u => u.login == username && u.password == password);
                if (user != null)
                {
                    // запомнить ID пользователя
                    Funs.RememberUserId(user.id_user);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Упс, ваш аккаунт не найден!", "Упс!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                if (GreanTrailEntities.GetContext().Users.FirstOrDefault(r => r.email == email) != null)
                {
                    MessageBox.Show("Почта уже существует!", "Упс!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (GreanTrailEntities.GetContext().Users.FirstOrDefault(r => r.login == login) != null)
                {
                    MessageBox.Show("Логин не оригинальный!)", "Упс!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Создание нового объекта пользователя
                Users user = new Users
                {
                    full_name = fullName,
                    email = email,
                    login = login,
                    dateOfBirth = dateOfBirth,
                    phoneNumber = phoneNumber,
                    address = address,
                    password = password
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
                    From = new MailAddress(smtpUsername),
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
                if (email == string.Empty)
                {
                    using (GreanTrailEntities db = new GreanTrailEntities())
                    {
                        // Поиск пользователя по электронной почте
                        var user = GreanTrailEntities.GetContext().Users.FirstOrDefault(u => u.id_user == Settings.Default.UserId);

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
                        return;
                    }
                }
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

        public static string GetCurrentRole()
        {
            var user = GreanTrailEntities.GetContext().Users.FirstOrDefault(u => u.id_user == Settings.Default.UserId);
            var userRole = GreanTrailEntities.GetContext().Roles.FirstOrDefault(r => r.id_roles == user.id_roles);
            if(userRole == null) return "0";
            return userRole.name;
        }
        public static Users GetCurrentUser()
        {
            return GreanTrailEntities.GetContext().Users.FirstOrDefault(u => u.id_user == Settings.Default.UserId);
        }
    }
}
