
public class Ingrediente : ItemComprable
{
    public TipoIngrediente Tipo { get; set; }

    public Ingrediente(int unlockXp, string nombre, int cantidad, int precioCompra, TipoIngrediente tipo)
        : base(unlockXp, nombre, cantidad, precioCompra)
    {
        this.Tipo = tipo;
    }

    public Ingrediente(string nombre, int cantidad)
        : base(0, nombre, cantidad, 0)
    {
        this.Tipo = TipoIngrediente.DEFAULT;
    }
}