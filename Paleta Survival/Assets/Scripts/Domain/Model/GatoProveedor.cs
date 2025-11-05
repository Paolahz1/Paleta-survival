
public class GatoProveedor : Gato
{
    public ItemComprable[] items;

    public GatoProveedor(string nombre, ItemComprable[] items) : base()
    {
        this.Nombre = nombre;
        this.items = items;
    }
}