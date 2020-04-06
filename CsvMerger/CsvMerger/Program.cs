﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using CsvMerger.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CsvMerger.Services.Interfaces;
using CsvMerger.Services.ServiceLayers;
using CsvMerger.Services.ServiceLayers.TuiRoutines;
using CsvMerger.Services.ServiceLayers.TuiRoutines.InitialDataInput;
using CsvMerger.Services.ServiceLayers.TuiInterfaces;

namespace CsvMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            // calls the Run method in App, which is replacing Main
            serviceProvider.GetService<App>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();
            services.AddSingleton(config);

            // required to run the application
            services.AddTransient<App>();
            services.AddSingleton<IPercentageCounter, PercentageCounter>();
            services.AddScoped<IMapSetService, MapSetService>();
            services.AddScoped<IServiceOrchestrator, ServiceoOchestrator>();
            services.AddScoped<IRowProcessor, RowProcessor>();
            services.AddScoped<IFileLineReader, FileLineReader>();
            services.AddScoped<IFileStreamProvider, FileStreamProvider>();
            services.AddScoped<IFileLineWriter, FileLineWriter>();
            services.AddScoped<IMakeFile, MakeFile>();
            services.AddScoped<IUserInputValidator, UserInputValidator>();
            services.AddScoped<IInitialDataInput, InitialDataInput>();
            services.AddScoped<IUXTextOutput, UXTextOutput>();
            

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
