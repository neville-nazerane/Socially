using System;
using System.Text.Json;

namespace Socially.Website.Utils
{
    public static class ConsoleUtils
    {

        public static void Log<T>(string text, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            Console.WriteLine($"{text}:\n {json}");
        }

    }
}
