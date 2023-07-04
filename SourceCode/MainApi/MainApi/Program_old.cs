//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Serilog;
//using System;
//using System.Runtime.InteropServices;
//using Microsoft.AspNetCore.Builder;

//namespace MainApi
//{
//    public class Program
//    {
//        private const int MF_BYCOMMAND = 0x00000000;
//        public const int SC_CLOSE = 0xF060;

//        [DllImport("user32.dll")]
//        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

//        [DllImport("user32.dll")]
//        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

//        [DllImport("kernel32.dll", ExactSpelling = true)]
//        private static extern IntPtr GetConsoleWindow();

//        public static void Main(string[] args)
//        {
//            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

//            var builder = CreateHostBuilder(args);

//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            builder.Build();
//        }

//        public static WebApplicationBuilder CreateHostBuilder(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            builder.Host.UseSerilog((context, services, configuration) => configuration
//                        .ReadFrom.Configuration(context.Configuration)
//                        .ReadFrom.Services(services)
//                        .Enrich.FromLogContext()
//                        .WriteTo.Console()
//                )
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                    //webBuilder.UseUrls("http://localhost");
//                    //webBuilder.UseKestrel();
//                })
//                .ConfigureServices(services =>
//                {
//                    //services.AddHostedService<WorkerAmazonInventory>();
//                    //services.AddHostedService<WorkerAmazonOrder>();
//                    //services.AddHostedService<WorkerAmazonOrderItem>();
//                    //services.AddHostedService<WorkerAmazonMonthlySales>();
//                });

//            return builder;
//        }
//    }
//}
