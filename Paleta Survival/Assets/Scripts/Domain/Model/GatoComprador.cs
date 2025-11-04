using BreakingCat_Project.Assets.Scripts.Domain.Model;

public class GatoComprador : Gato
{
    public Droga[] Pedido { get; set; }
    public int Coins { get; set; }
    public int Xp { get; set; } 

    public GatoComprador(string nombre, Droga[] pedido, int coins, int xp) : base()
    {
        this.Nombre = nombre;
        this.Pedido = pedido;
        this.Coins = coins;
        this.Xp = xp;
    }
}