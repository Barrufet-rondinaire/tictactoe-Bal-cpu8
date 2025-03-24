using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TicTacToeME
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:8080/";
            Uri uri = new(url);

            Console.WriteLine(uri.Host);
            Console.WriteLine(uri.Port);

            using HttpClient client = new() { BaseAddress = uri };

            // Obtener la llista de jugadors
            var jugadors = await client.GetFromJsonAsync<List<string>>("jugadors");
            
            Console.WriteLine("\nJugadors disponibles:");
            foreach (var jugador in jugadors)
            {
                Console.WriteLine("- " + jugador);
            }
        }
    }
}