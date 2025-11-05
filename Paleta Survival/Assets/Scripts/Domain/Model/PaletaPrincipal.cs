using UnityEngine;
using BreakingCat_Project.Assets.Scripts.Domain.Model;
using System.Linq;

public class PaletaPrincipal : Gato
{
    public Inventario Inventario { get; set; }
    public static Inventario Cofre { get; set; }
    public static Mesa mesaUsada { get; set; }
    public int Xp { get; set; }
    public int Coins { get; set; }
    public int Alquiler { get; set; }
    public int vida = 3;

    public PaletaPrincipal(string nombre, int espacioInventario) : base()
    {
        this.Nombre = nombre;
        Inventario = new Inventario(espacioInventario);
        Xp = 0;
        Coins = 0;
        Alquiler = 0;
    }

    public static bool AsociarCofre(Inventario inventarioCofre)
    {
        if (Cofre == null && inventarioCofre != null)
        {
            Cofre = inventarioCofre;
            return true;
        }
        return false;
    }

    public static bool DesasociarCofre()
    {
        if (Cofre != null)
        {
            Cofre = null;
            return true;
        }
        return false;
    }

    public static bool AsociarMesa(Mesa mesa)
    {
        if (mesaUsada == null && mesa != null)
        {
            mesaUsada = mesa;
            return true;
        }
        return false;
    }

    public static bool DesasociarMesa()
    {
        if (mesaUsada != null)
        {
            mesaUsada = null;
            return true;
        }
        return false;
    }

    public static void ImprimirNombresItemsCofre()
    {
        if (Cofre != null && Cofre.Items != null)
        {
            foreach (var item in Cofre.Items)
            {
                if (item != null && !string.IsNullOrEmpty(item.Nombre))
                {
                    Debug.Log(item.Nombre);
                }
            }
        }
        else
        {
            Debug.Log("El cofre está vacío o no está asociado.");
        }
    }

    public bool TransferirItemACofre(string nombreItem, int cantidad)
    {
        if (Cofre == null || Inventario == null || string.IsNullOrEmpty(nombreItem) || cantidad <= 0)
            return false;

        var itemEnInventario = Inventario.Items.FirstOrDefault(i => i != null && i.Nombre == nombreItem);
        if (itemEnInventario == null || itemEnInventario.Cantidad < cantidad)
            return false;

        // Remover del inventario principal
        itemEnInventario.Cantidad -= cantidad;
        if (itemEnInventario.Cantidad == 0)
            Inventario.Items.Remove(itemEnInventario);

        // Agregar al cofre
        var itemEnCofre = Cofre.Items.FirstOrDefault(i => i != null && i.Nombre == nombreItem);
        if (itemEnCofre != null)
        {
            itemEnCofre.Cantidad += cantidad;
        }
        else
        {
            Cofre.Items.Add(new Item(0, nombreItem, cantidad));
        }

        return true;
    }

    public bool TransferirItemDesdeCofre(string nombreItem, int cantidad)
    {
        if (Cofre == null || Inventario == null || string.IsNullOrEmpty(nombreItem) || cantidad <= 0)
            return false;

        var itemEnCofre = Cofre.Items.FirstOrDefault(i => i != null && i.Nombre == nombreItem);
        if (itemEnCofre == null || itemEnCofre.Cantidad < cantidad)
            return false;

        // Remover del cofre
        itemEnCofre.Cantidad -= cantidad;
        if (itemEnCofre.Cantidad == 0)
            Cofre.Items.Remove(itemEnCofre);

        // Agregar al inventario principal
        var itemEnInventario = Inventario.Items.FirstOrDefault(i => i != null && i.Nombre == nombreItem);
        if (itemEnInventario != null)
        {
            itemEnInventario.Cantidad += cantidad;
        }
        else
        {
            Inventario.Items.Add(new Item(0, nombreItem, cantidad));
        }

        return true;
    }

    public void TakeCoins(int amount)
    {
        if (amount > 0 && Coins >= amount)
        {
            Coins -= amount;
        }
    }

    public void AddCoins(int amount)
    {
        if (amount > 0)
        {
            Coins += amount;
        }
    }

    public void PerderVida()
    {
        if (vida >= 1)
        {
            vida -= 1;
        }
    }

    public void GanarVida()
    {
        if (vida < 3)
        {
            vida += 1;
        }
    }

}
