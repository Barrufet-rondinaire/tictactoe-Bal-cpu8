namespace TicTacToeME;

public class Jugador
{
    public string Nom { get; }
    public string Pais { get; }
    public int Victories { get; private set; }

    public Jugador(string nom, string pais)
    {
        Nom = nom;
        Pais = pais;
        Victories = 0;
    }

    public void AfegirVictòria()
    {
        Victories++;
    }
}
