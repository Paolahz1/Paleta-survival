
public class Droga : Item
{
    public int SegundosCrafteo { get; set; }
    public TipoDroga Tipo { get; set; }

    public Droga(int unlockXp, string nombre, int cantidad, int segundosCrafteo, TipoDroga tipo)
        : base(unlockXp, nombre, cantidad)
    {
        this.SegundosCrafteo = segundosCrafteo;
        this.Tipo = tipo;
    }

    public Droga(int unlockXp, string nombre, int cantidad, int segundosCrafteo)
        : base(unlockXp, nombre, cantidad)
    {
        this.SegundosCrafteo = segundosCrafteo;
    }

    public Droga(string nombre, int cantidad)
        : base(0, nombre, cantidad)
    {
        this.SegundosCrafteo = 0;
        this.Tipo = TipoDroga.DEFAULT;
    }
}
