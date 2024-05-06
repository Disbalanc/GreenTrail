using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Windows.Interop;
using System.Windows.Shell;
using System.Windows.Input;
using System.Windows.Media;

namespace GreenTrail.Source.Funs
{
    internal class Funs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static BitmapSource ChangeImageColor(BitmapSource bitmapSource, Color color)
        {
            FormatConvertedBitmap newBitmap = new FormatConvertedBitmap();
            newBitmap.BeginInit();
            newBitmap.Source = bitmapSource;
            newBitmap.DestinationFormat = PixelFormats.Bgra32;
            newBitmap.EndInit();

            // Создаем DrawingVisual и устанавливаем новый цвет
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(newBitmap, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                drawingContext.DrawRectangle(new SolidColorBrush(color), null, new Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight));
            }

            // Рендерим новое изображение
            RenderTargetBitmap newImage = new RenderTargetBitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            newImage.Render(drawingVisual);

            return newImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        public static string ComputeSHA256Hash(string String)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразование пароля в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(String);

                // Вычисление хэша
                byte[] hash = sha256.ComputeHash(bytes);

                // Преобразование хэша в строку
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public static void MinimizeToTaskbar(Window window)
        {
            IntPtr handle = (new WindowInteropHelper(window)).Handle;
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public static void ToggleMinimizeMaximize(Window window)
        {
            if (window != null)
            {
                if (window.WindowState == WindowState.Normal)
                {
                    // Сохраняем текущую позицию и размер окна
                    window.Tag = new Rect(window.Left, window.Top, window.Width, window.Height);
                    // Меняем состояние окна на Maximized (развернутый на весь экран)
                    window.WindowState = WindowState.Maximized;
                }
                else
                {
                    // Возвращаем окно к исходным размерам
                    if (window.Tag is Rect rect)
                    {
                        window.Left = rect.Left;
                        window.Top = rect.Top;
                        window.Width = rect.Width;
                        window.Height = rect.Height;
                    }
                    // Меняем состояние окна на Normal (обычное состояние)
                    window.WindowState = WindowState.Normal;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="window"></param>
        public static void WindowDragMove(object sender, MouseButtonEventArgs e, Window window)
        {
            if (window.WindowState != WindowState.Normal)
            {
                // Возвращаем окно к исходным размерам
                if (window.Tag is Rect rect)
                {
                    window.Left = rect.Left;
                    window.Top = rect.Top;
                    window.Width = rect.Width;
                    window.Height = rect.Height;
                }
                // Меняем состояние окна на Normal (обычное состояние)
                window.WindowState = WindowState.Normal;
            }
            if (e.ChangedButton == MouseButton.Left)
                window.DragMove();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            if (!email.Contains('@'))
            {
                return false;
            }

            // Дополнительные проверки электронной почты здесь...

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateOTP()
        {
            StringBuilder otp = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int randomIndex = new Random().Next(0, "0123456789".Length);
                otp.Append("0123456789"[randomIndex]);
            }

            return ComputeSHA256Hash(otp.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ExportData()
        {

        }
    }
}
