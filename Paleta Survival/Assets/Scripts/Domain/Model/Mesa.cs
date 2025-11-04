using System;
using UnityEngine;

public class Mesa : ItemComprable
{
    public Receta[] recetas;

    public Transform mesaCanvasContent;

    public Mesa(int unlockXp, string nombre, int cantidad, int precioCompra, Receta[] recetas)
        : base(unlockXp, nombre, cantidad, precioCompra)
    {
        this.recetas = recetas;

        switch (nombre)
        {
            case "Mesa1":
                this.recetas = new Receta[]
                {
                    new Receta(
                        new Ingrediente[]
                        {
                            new Ingrediente("Catnip Plant", 1)
                        },
                        new Droga(0, "Catnip Ziplock", 1, 0)
                    )
                    // ,
                    // new Receta(
                    //     new Ingrediente[]
                    //     {
                    //         new Ingrediente("Componente A", 1),
                    //         new Ingrediente("Componente B", 1)
                    //     },
                    //     new Droga(0, "Metcat", 1, 0)
                    // )
                };
                // Debug.Log("Recetas asignadas a Mesa1: " + recetas.Length);
                break;
            case "Mesa2":
                this.recetas = new Receta[]
                {
                    new Receta(
                        new Ingrediente[]
                        {
                            new Ingrediente("Componente A", 1),
                            new Ingrediente("Componente B", 1)
                        },
                        new Droga(0, "Componente C", 1, 0)
                    )
                    ,
                    new Receta(
                        new Ingrediente[]
                        {
                            new Ingrediente("Componente C", 1),
                            new Ingrediente("Componente X", 1)
                        },
                        new Droga(0, "Cocat", 1, 0)
                    )
                };
                break;
            case "Mesa3":
                this.recetas = new Receta[]
                {
                    new Receta(
                        new Ingrediente[]
                        {
                            new Ingrediente("Componente B", 1),
                            new Ingrediente("Componente C", 1)
                        },
                        new Droga(0, "Componente Y", 1, 0)
                    )
                    ,
                    new Receta(
                        new Ingrediente[]
                        {
                            new Ingrediente("Componente Y", 1),
                            new Ingrediente("Componente Z", 1)
                        },
                        new Droga(0, "Metcat", 1, 0)
                    )
                };
                break;
        }
    }

    public void AddReceta(Receta receta)
    {
        if (receta == null) return;

        Array.Resize(ref recetas, recetas.Length + 1);
        recetas[recetas.Length - 1] = receta;
    }

    public void RemoveReceta(Receta receta)
    {
        if (receta == null) return;

        int index = Array.IndexOf(recetas, receta);
        if (index < 0) return;

        for (int i = index; i < recetas.Length - 1; i++)
        {
            recetas[i] = recetas[i + 1];
        }

        Array.Resize(ref recetas, recetas.Length - 1);
    }

    public void SetRecetas(Receta[] nuevasRecetas)
    {
        if (nuevasRecetas == null) return;
        recetas = nuevasRecetas;
    }

    public Receta[] GetRecetas()
    {
        return recetas;
    }

    public Item[] GetIngredientesByResultadoName(string resultadoName)
    {
        if (string.IsNullOrEmpty(resultadoName)) return new Item[0];

        foreach (Receta receta in recetas)
        {
            if (receta != null && receta.Resultado != null && receta.Resultado.Nombre == resultadoName)
            {
                return receta.Ingredientes;
            }
        }

        return new Item[0];
    }

    public bool AgregarItem(Item item, string tipoItem)
    {
        item.GameObject.SetActive(true);

        if (tipoItem == "Crafteo1")
        {
            DrawItemForCrafteo1(item);
        }
        else if (tipoItem == "Crafteo2")
        {
            DrawItemForCrafteo2(item);
        }
        else if (tipoItem == "Resultado")
        {
            DrawItemForResultado(item);
        }
        else
        {
            Debug.LogWarning($"Unknown item type: {tipoItem}. Item will not be positioned.");
        }

        var textComponent = item.GameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        // Debug.Log($"Found text component: {textComponent}");
        if (textComponent != null)
        {
            // Debug.Log($"Setting text for item: {item.Nombre} to {item.Cantidad}");
            textComponent.text = item.Cantidad.ToString();
            // Debug.Log($"Updated text position for item: {item.Nombre}");
        }
        return true;
    }

    private int crafteo1Count = 0;
    private void DrawItemForCrafteo1(Item item)
    {
        if (crafteo1Count < 1)
        {
            item.GameObject.transform.localPosition = new Vector2(-202, 242);
            crafteo1Count++;
        }
        else
        {
            item.GameObject.transform.localPosition = new Vector2(-679, 242);
        }
    }

    private int crafteo2Count = 0;
    private void DrawItemForCrafteo2(Item item)
    {
        if (crafteo2Count < 1)
        {
            item.GameObject.transform.localPosition = new Vector2(-202, -274);
            crafteo2Count++;
        }
        else
        {
            item.GameObject.transform.localPosition = new Vector2(-679, -274);
        }
    }

    private int resultadoCount = 0;
    private void DrawItemForResultado(Item item)
    {
        if (resultadoCount < 1)
        {
            item.GameObject.transform.localPosition = new Vector2(725, 242);
            resultadoCount++;
        }
        else
        {
            item.GameObject.transform.localPosition = new Vector2(725, -274);
        }
    }

}
