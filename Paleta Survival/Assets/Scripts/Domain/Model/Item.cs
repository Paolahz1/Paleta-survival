using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public int UnlockXp { get; set; }
    public string Nombre { get; set; }
    public int Cantidad { get; set; }
    public GameObject GameObject { get; set; }
    public Sprite Imagen { get; set; }

    public int SlotIndex { get; set; }

    public Item(int unlockXp, string nombre, int cantidad)
    {
        this.UnlockXp = unlockXp;
        this.Nombre = nombre;
        this.Cantidad = cantidad;
    }

    public Item(string nombre)
    {
        this.Nombre = nombre;
        this.Cantidad = 1;
    }
}
