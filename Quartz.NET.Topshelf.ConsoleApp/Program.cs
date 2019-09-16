using NLog;
using NLog.Config;
using System;
using Topshelf;
using Topshelf.Logging;

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
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();


        static void Main(string[] args)
        {
            try
            {
                var rc = HostFactory.Run(x =>
                {
                    //1、nlog.config必须把复制到输出目录为始终复制
                    //2、log问价在debug中
                    x.UseNLog();

                    //Simple Service
                    // x.Service<Bootstrap>(() => new Bootstrap { Url = HealthAddresss });

                    //Custom Service
                    //http://docs.topshelf-project.com/en/latest/configuration/config_api.html
                    x.Service<BootstrapCustomer>(s =>
                    {
                        // 通过 new BootstrapCustomer() 构建一个服务实例 
                        s.ConstructUsing(name => new BootstrapCustomer { Url = HealthAddresss });
                        // 当服务启动后执行什么
                        s.WhenStarted(tc => tc.Start());
                        // 当服务停止后执行什么
                        s.WhenStopped(tc => tc.Stop());

                    });

                    x.RunAsLocalSystem();

                    x.SetServiceName("Quartz");
                    x.SetDisplayName("Quartz");
                    x.SetDescription("Quartz系统");

                    x.OnException(ex => logger.Error("Topshelf Occur Exception." + ex));

                });

                var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
                Environment.ExitCode = exitCode;

                logger.Info("Topshelf start Exception.");
                Console.WriteLine("Topshelf start ...");
                Console.ReadKey();
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
