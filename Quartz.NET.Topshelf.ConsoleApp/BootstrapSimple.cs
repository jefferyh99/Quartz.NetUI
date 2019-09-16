using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Timers;
using Topshelf;

namespace Quartz.NET.Topshelf.ConsoleApp
{
    //简单模式
    public class BootstrapSimple : ServiceControl
    {
        private double InternalInMinutes = 1;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Timer _timer;
        public string Url { get; set; }

        public BootstrapSimple()
        {
            _timer = new Timer(InternalInMinutes * 60 * 1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                Console.WriteLine("It is {0} and all is well", DateTime.Now);
                new WebClient().DownloadData(Url);
            };
        }

        public bool Start(HostControl hostControl)
        {
            try
            {
                _timer.Start();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Topshelf starting occured errors." + ex);
                return false;
            }
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                _timer.Stop();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Topshelf stopping occured errors." + ex);
                return false;
            }
        }
    }
}
