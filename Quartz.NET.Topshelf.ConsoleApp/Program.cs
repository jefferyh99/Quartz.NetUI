using NLog;
using System;
using Topshelf;

namespace Quartz.NET.Topshelf.ConsoleApp
{
    /// <summary>
    /// 安装：Quartz.NET.Topshelf.ConsoleApp.exe install
    /// 启动：Quartz.NET.Topshelf.ConsoleApp.exe start
    /// 卸载：Quartz.NET.Topshelf.ConsoleApp.exe uninstall
    /// </summary>
    class Program
    {
        //健康检查地址
        private const string HealthAddresss = @"https://localhost:5001/Health/KeepAlive";
        private static Logger logger = LogManager.GetCurrentClassLogger();


        static void Main(string[] args)
        {
            try
            {
                var rc = HostFactory.Run(x =>
                {
                    x.UseNLog();

                    //Simple Service
                    x.Service<Bootstrap>(() => new Bootstrap { Url = HealthAddresss });

                    //Custom Service
                    //http://docs.topshelf-project.com/en/latest/configuration/config_api.html

                    x.RunAsLocalSystem();

                    x.SetServiceName("Quartz");
                    x.SetDisplayName("Quartz");
                    x.SetDescription("Quartz系统");

                    x.OnException(ex => logger.Error("Topshelf Occur Exception." + ex));

                });

                var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
                Environment.ExitCode = exitCode;
            }
            catch (Exception ex)
            {
                logger.Error("Topshelf start Exception." + ex);
                Console.WriteLine("Topshelf start error." + ex);
                Console.ReadKey();
            }
        }
    }
}
