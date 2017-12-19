using System;
using System.Diagnostics;
using Domotica_API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domotica_API.Seeds
{
    public class DatabaseSeeder
    {
        public static void Initialize()
        {
            StartSeeder<BackgroundSeeder>();
            StartSeeder<UserSeeder>();
            StartSeeder<BookmarkSeeder>();
            StartSeeder<WidgetSeeder>();
        }

        private static async void StartSeeder<T>(string name = "") where T : Seeder, new()
        {
            name = name == "" ? typeof(T).ToString() : name;
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            await new T().Run();
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Console.WriteLine(name + ": " + elapsedTime);
        }
    }
}
