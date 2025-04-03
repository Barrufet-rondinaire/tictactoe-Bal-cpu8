namespace TicTacToeME;
class GuanyadorTorneig
{
    private readonly Dictionary<string, Jugador> _jugadors = new();

    public void AfegirJugador(Jugador jugador)
    {
        if (!_jugadors.ContainsKey(jugador.Nom))
        {
            _jugadors[jugador.Nom] = jugador;
        }
    }

    public void AfegirVictoria(string nomJugador)
    {
        if (_jugadors.TryGetValue(nomJugador, out var jugador))
        {
            jugador.AfegirVictoria();
        }
        else
        {
            Console.WriteLine($"Error: el jugador {nomJugador} no està registrat.");
        }
    }

    public List<Jugador> ObtenirResultats()
    {
        return new List<Jugador>(_jugadors.Values);
    }
}
