using System;
using common;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using StructureMap;

namespace worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            
            container.AddHangfireFrameworkServices();
            
            //var redis = ConnectionMultiplexer.Connect("redis");

            //GlobalConfiguration.Configuration.UseRedisStorage(redis);
         GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=C:\\USERS\\TCORV\\DOCUMENTS\\SYNUIT.PLATFORM.DEV.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
         GlobalConfiguration.Configuration.UseActivator(new WorkerActivator(container));

            using (var server = new BackgroundJobServer())
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    internal class WorkerActivator : JobActivator
    {
        private IContainer Container { get; }
        
        public WorkerActivator(IContainer container)
        {
            Container = container;
        }

        public override object ActivateJob(Type jobType) => Container.GetInstance(jobType);
    }
}
