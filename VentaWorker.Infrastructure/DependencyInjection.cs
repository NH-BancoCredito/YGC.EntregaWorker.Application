﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using Stocks.Infrastructure.Repositories;
using MongoDB.Driver;
using Confluent.Kafka;
using EntregaWorker.Domain.Service.Events;
using EntregaWorker.Infrastructure.Services.Events;
using System.Net;
using Polly.Extensions.Http;
using Polly;
using EntregaWorker.Domain.Repositories;
using EntregaWorker.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using EntregaWorker.CrossCutting.Configs;


namespace EntregaWorker.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, IConfiguration configInfo
            )
        {
            var appConfiguration = new AppConfiguration(configInfo);

            services.AddDataBaseFactories(appConfiguration.ConexionDBEntregas);
            //services.AddDataBaseFactories(connectionString);
            services.AddProducer(appConfiguration.KafkaDbCollection);
            services.AddEventServices();
            services.AddConsumer(appConfiguration.KafkaDbCollection);
            services.AddRepositories(Assembly.GetExecutingAssembly());
        }

        private static void AddDataBaseFactories(this IServiceCollection services, string connectionString)
        {            
            services.AddSingleton(mongoDatabase =>
            {
                var mongoClient = new MongoClient(connectionString);
                return mongoClient.GetDatabase("db-entregas");
            });

        }

        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var respositoryTypes = assembly
                .GetExportedTypes().Where(item => item.GetInterface(nameof(IRepository)) != null).ToList();


            foreach (var repositoryType in respositoryTypes)
            {
                var repositoryInterfaceType = repositoryType.GetInterfaces()
                    .Where(item => item.GetInterface(nameof(IRepository)) != null)
                    .First();

                services.AddScoped(repositoryInterfaceType, repositoryType);
            }
        }

        private static IServiceCollection AddProducer(this IServiceCollection services, string KafkaDbCollection)
        {
            var config = new ProducerConfig
            {
                Acks = Acks.Leader,
                BootstrapServers = KafkaDbCollection,//"127.0.0.1:9092",
                ClientId = Dns.GetHostName(),
            };

            services.AddSingleton<IPublisherFactory>(sp => new PublisherFactory(config));
            return services;
        }

        private static IServiceCollection AddConsumer(this IServiceCollection services,string KafkaDbCollection)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = KafkaDbCollection, //"127.0.0.1:9092",
                GroupId = "venta-actualizar-stocks",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            services.AddSingleton<IConsumerFactory>(sp => new ConsumerFactory(config));
            return services;
        }

        private static void AddEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, EventSender>();
        }
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2,
                            retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                Console.WriteLine(result);
            }
            ;
            Action onReset = null;
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30),
                onBreak, onReset
                );


        }
       
    }
}
