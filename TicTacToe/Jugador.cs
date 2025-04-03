namespace TicTacToeME;
public class Jugador
{
    public string Nom { get; }
    public string Pais { get; }
    public int Victories { get; private set; } = 0;

    public Jugador(string nom, string pais)
    {
        Nom = nom;
        Pais = pais;
    }

    public void AfegirVictoria()
    {
        Victories++;
    }
}

