using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

            // Trobar noms i paisos
            var regex = new Regex(@"participant ([\p{L}']+) .*? representa a? ([\p{L}\-]+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(text);
            
            List<string> jugadors = new List<string>();
            Dictionary<string, string> jugadorPais = new Dictionary<string, string>();

            foreach (Match match in matches)
            {
                if (match.Groups.Count == 3)
                {
                    string jugador = match.Groups[1].Value;
                    string pais = match.Groups[2].Value;
                    jugadors.Add($"{jugador} ({pais})");
                    jugadorPais.Add(jugador, pais);
                }
            }

            // Mostrar la llista de jugadors
            foreach (var jugador in jugadors)
            {
                Console.WriteLine(jugador);
            }

            Console.WriteLine("\nComençant la revisió de les partides...\n");
            
            int totalPartides = 10000;
            for (int i = 1; i <= totalPartides; i++)
            {
                var partida = await client.GetFromJsonAsync<Partida>($"partida/{i}");

                if (partida != null)
                {
                    string guanyador = ObtenirGuanyador(partida.tauler, partida.jugador1, partida.jugador2);
                    if (!string.IsNullOrEmpty(guanyador))
                    {
                        Console.WriteLine($"Partida {partida.numero} - Guanyador: {guanyador}");
                    }
                }
            }
        }

        static string ObtenirGuanyador(string[] tauler, string jugador1, string jugador2)
        {
            for (int i = 0; i < 3; i++)
            {
                // Comprovar files
                if (tauler[i][0] == tauler[i][1] && tauler[i][1] == tauler[i][2] && tauler[i][0] != 'O')
                    return tauler[i][0] == 'X' ? jugador2 : jugador1;
                
                // Comprovar columnes
                if (tauler[0][i] == tauler[1][i] && tauler[1][i] == tauler[2][i] && tauler[0][i] != 'O')
                    return tauler[0][i] == 'X' ? jugador2 : jugador1;
            }
            
            // Comprovar diagonals
            if (tauler[0][0] == tauler[1][1] && tauler[1][1] == tauler[2][2] && tauler[0][0] != 'O')
                return tauler[0][0] == 'X' ? jugador2 : jugador1;
            
            if (tauler[0][2] == tauler[1][1] && tauler[1][1] == tauler[2][0] && tauler[0][2] != 'O')
                return tauler[0][2] == 'X' ? jugador2 : jugador1;

            return null;
        }
    }
}
