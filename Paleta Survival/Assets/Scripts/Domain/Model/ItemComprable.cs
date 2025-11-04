public class ItemComprable : Item
{
    protected int precioCompra;

    public int PrecioCompra
    {
        get { return precioCompra; }
        set { precioCompra = value; }
    }

    public ItemComprable(int unlockXp, string nombre, int cantidad, int precioCompra)
        : base(unlockXp, nombre, cantidad)
    {
        this.precioCompra = precioCompra;
    }
}