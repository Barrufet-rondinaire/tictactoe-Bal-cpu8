using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToeME
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:8080/";
            Uri uri = new(url);
            
            using HttpClient client = new() { BaseAddress = uri };

            // Obtenir la llista de jugadors
            var text = await client.GetStringAsync("jugadors");

            Console.WriteLine("\nJugadors disponibles:");

           //Trobar noms i paisos
            var regex = new Regex(@"participant ([\p{L}']+) .*? representa a? ([\p{L}\-]+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(text);
            
            foreach (Match match in matches)
            {
                if (match.Groups.Count == 3)
                {
                    string jugador = match.Groups[1].Value;
                    string pais = match.Groups[2].Value;
                    Console.WriteLine($"- {jugador} ({pais})");
                }
            }
        }
    }
}