using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Timers;
using Topshelf.Logging;

namespace Quartz.NET.Topshelf.ConsoleApp
{
    //客户模式
    public class BootstrapCustomer
    {
        private readonly double InternalInMinutes = 1;      

        private Timer _timer;
        public string Url { get; set; }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public BootstrapCustomer()
        {
            _timer = new Timer(InternalInMinutes * 60 * 1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                Console.WriteLine("It is {0} and all is well", DateTime.Now);
                logger.Info(string.Format("It is {0} and all is well", DateTime.Now));
                new WebClient().DownloadData(Url);
            };
        }

        public void Start()
        {
            try
            {
               _timer.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Topshelf starting occured errors." + ex);
            }
        }

        public void Stop()
        {
            try
            {
                _timer.Stop();
            }
            catch (Exception ex)
            {
                logger.Error("Topshelf stopping occured errors." + ex);
            }
        }
    }
}
