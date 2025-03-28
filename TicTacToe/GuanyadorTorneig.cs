namespace TicTacToeME;
public class GuanyadorTorneig
{
    private Dictionary<string, Jugador> jugadors;

    public GuanyadorTorneig()
    {
        jugadors = new Dictionary<string, Jugador>();
    }

    public void AfegirJugador(Jugador jugador)
    {
        if (!jugadors.ContainsKey(jugador.Nom))
        {
            jugadors.Add(jugador.Nom, jugador);
        }
    }

    public void AfegirVictòria(string nomJugador)
    {
        if (jugadors.ContainsKey(nomJugador))
        {
            jugadors[nomJugador].AfegirVictòria();
        }
    }

    public string DeterminarGuanyador()
    {
        Jugador guanyador = null;
        int maxVictories = 0;

        foreach (var jugador in jugadors.Values)
        {
            if (jugador.Victories > maxVictories)
            {
                guanyador = jugador;
                maxVictories = jugador.Victories;
            }
        }

        return guanyador?.Nom;
    }
}
