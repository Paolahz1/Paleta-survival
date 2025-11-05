using System;

public class Receta
{
    public Ingrediente[] Ingredientes { get; set; }
    public Droga Resultado { get; set; }

    public Receta(Ingrediente[] ingredientes, Droga resultado)
    {
        this.Ingredientes = ingredientes;
        this.Resultado = resultado;
    }
}
