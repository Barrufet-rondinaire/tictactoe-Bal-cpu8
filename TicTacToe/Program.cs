using System;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TicTacToeME
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://localhost:8080/";
            Uri uri = new(url);
            using HttpClient client = new() { BaseAddress = uri };
            
            var text = await client.GetStringAsync("jugadors");
            Console.WriteLine("Jugadors disponibles:");
            var regex = new Regex(@"participant ([\p{L}']+ [\p{L}']+) .*? representa a? ([\p{L}\-]+)");
            var matches = regex.Matches(text);

            Dictionary<string, string> jugadors = new();
            Dictionary<string, int> puntuaciones = new();
            string desqualificat = null;

            // Jugadors i pais
            foreach (Match match in matches)
            {
                if (match.Groups.Count >= 3)
                {
                    string jugador = match.Groups[1].Value;
                    string pais = match.Groups[2].Value;
                    jugadors[jugador] = pais;
                    puntuaciones[jugador] = 0;
                    Console.WriteLine($"{jugador} ({pais})");
                }
            }
            
            var regexDesqualificat = new Regex(@"La participant ([\p{L}]+\s[\p{L}]+).*?ha estat desqualificada");
            var matchDesqualificat = regexDesqualificat.Match(text);
            if (matchDesqualificat.Success)
            {
                desqualificat = matchDesqualificat.Groups[1].Value;
            }

            Console.WriteLine("Començant la revisió de les partides..");
            int totalPartides = 10000;
            
            for (int i = 1; i <= totalPartides; i++)
            {
                var partida = await client.GetFromJsonAsync<Partida>($"partida/{i}");
                if (partida != null)
                {
                    // Si el jugador desqualificat està en la partida no conta
                    if (partida.Jugador1 == desqualificat || partida.Jugador2 == desqualificat)
                    {
                        Console.WriteLine($"Partida {i} - Partida invalida per jugador/a eliminat/da.");
                        continue; 
                    }
                    
                    string guanyador = DeterminarGuanyador(partida.Tauler, partida.Jugador1, partida.Jugador2);
                    AfegirVictoria(guanyador, puntuaciones);
                    Console.WriteLine($"Partida {i} - Guanyador: {guanyador ?? "Empat"}");
                }
            }
            
            if (puntuaciones.Count > 0)
            {
                var millorJugador = puntuaciones.OrderByDescending(x => x.Value).FirstOrDefault();
                Console.WriteLine($"\nEl jugador amb més victòries és {millorJugador.Key} amb {millorJugador.Value} victòries.");
            }
            
            if (!string.IsNullOrEmpty(desqualificat))
            {
                Console.WriteLine($"\nEl jugador/a desqualificat/da és {desqualificat}.");
            }
        }

        // Qui guanya la partida?
        static string DeterminarGuanyador(string[] tauler, string jugador1, string jugador2)
        {
            // Comprovar les files
            for (int i = 0; i < 3; i++)
            {
                if (tauler[i][0] == tauler[i][1] && tauler[i][1] == tauler[i][2] && tauler[i][0] != ' ')
                    return tauler[i][0] == 'X' ? jugador1 : jugador2;

                // Comprovar les columnes
                if (tauler[0][i] == tauler[1][i] && tauler[1][i] == tauler[2][i] && tauler[0][i] != ' ')
                    return tauler[0][i] == 'X' ? jugador1 : jugador2;
            }

            // Comprovar les diagonals
            if (tauler[0][0] == tauler[1][1] && tauler[1][1] == tauler[2][2] && tauler[0][0] != ' ')
                return tauler[0][0] == 'X' ? jugador1 : jugador2;

            if (tauler[0][2] == tauler[1][1] && tauler[1][1] == tauler[2][0] && tauler[0][2] != ' ')
                return tauler[0][2] == 'X' ? jugador1 : jugador2;

            return null; 
        }
        
        static void AfegirVictoria(string guanyador, Dictionary<string, int> puntuaciones)
        {
            if (!string.IsNullOrEmpty(guanyador))
            {
                if (!puntuaciones.ContainsKey(guanyador))
                {
                    puntuaciones[guanyador] = 0;
                }
                puntuaciones[guanyador]++; 
            }
        }
    }
}
