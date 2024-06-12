using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace GreenTrail.Source.Funs
{
    public class NotificationService
    {
        //private readonly GreanTrailEntities _context;
        //private readonly int _pollingInterval;
        //private readonly CancellationTokenSource _cts;
        //string[] shownNotifications;
        //private Task _pollingTask;

        //public NotificationService(GreanTrailEntities context, int pollingInterval)
        //{
        //    _context = context;
        //    _pollingInterval = pollingInterval;
        //    _cts = new CancellationTokenSource();
        //}

        //public void Start()
        //{
        //    if (_pollingTask != null)
        //    {
        //        throw new InvalidOperationException("Notification service is already started");
        //    }

        //    _pollingTask = Task.Run(() => PollNotificationsAsync(_cts.Token));
        //}

        //public void Stop()
        //{
        //    if (_pollingTask == null)
        //    {
        //        throw new InvalidOperationException("Notification service is not started");
        //    }

        //    _cts.Cancel();
        //    _pollingTask.Wait();
        //    _pollingTask = null;
        //}

        //private async Task PollNotificationsAsync(CancellationToken cancellationToken)
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var notifications = _context.Notification.ToList();
        //                shownNotifications = System.Windows.Forms.Application.UserAppDataRegistry.GetValue("ShownNotifications") as string[];
        //                foreach (var notification in notifications)
        //                {
        //                    if (!shownNotifications.Contains(notification.id_notification.ToString()))
        //                    {
        //                        SendNotification(notification);
        //                    }
        //                }

        //                _context.SaveChanges();
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                System.Windows.MessageBox.Show($"{ex}");
        //            }
        //        }

        //        await Task.Delay(_pollingInterval, cancellationToken);
        //    }
        //}

        //private void SendNotification(Notification notification)
        //{
        //    // Create a new toast notification
        //    var bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Source/Image/App/iconApp.ico"));
        //    var bitmap = BitmapFromBitmapImage(bitmapImage);
        //    var icon = Icon.FromHandle(((Bitmap)bitmap).GetHbitmap());
        //    var notifyIcon = new NotifyIcon()
        //    {
        //        Icon = icon,
        //        Visible = true,
        //        BalloonTipText = "У вас появилась новая работенка!",
        //        BalloonTipTitle = "GreenTrail"
        //    };

        //    try
        //    {
        //        notifyIcon.ShowBalloonTip(5000);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show("Error showing balloon tip: " + ex.Message);
        //    }

        //    var shownNotifications = System.Windows.Forms.Application.UserAppDataRegistry.GetValue("ShownNotifications") as string[];
        //    if (shownNotifications == null)
        //    {
        //        shownNotifications = new string[0];
        //    }
        //    shownNotifications = shownNotifications.Concat(new[] { notification.id_notification.ToString() }).ToArray();
        //    System.Windows.Forms.Application.UserAppDataRegistry.SetValue("ShownNotifications", shownNotifications);
        //}

        //Bitmap BitmapFromBitmapImage(BitmapImage bitmapImage)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        BitmapEncoder encoder = new BmpBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        //        encoder.Save(memoryStream);
        //        using (var bitmap = new Bitmap(memoryStream))
        //        {
        //            return new Bitmap(bitmap);
        //        }
        //    }
        //}
    }
}
