using System;
using System.Collections.Generic;
using BreakingCat_Project.Assets.Scripts.Domain.Model;
using UnityEngine;

public class Mapa
{
    // Matriz principal del mapa con coordenadas de Unity
    public Posicion[,] matrizMapa;
    private GatoComprador[] Pedidos;
    
    // Configuración de las manzanas
    private struct ConfiguracionManzana
    {
        public float anchoManzana;      // 13.5f (distancia horizontal entre esquinas)
        public float altoManzana;       // 8.57f (distancia vertical entre esquinas)
        public Vector2 origenMapa;      // Punto de referencia superior izquierdo
    }
    
    // Límites del mapa
    private struct LimitesMapa
    {
        public Vector2 superiorIzquierdo;    // (-13, 16.95)
        public Vector2 superiorDerecho;      // (27.91, 16.95)
        public Vector2 inferiorIzquierdo;    // (-13, -8.98)
        public Vector2 inferiorDerecho;      // (28.01, -9.02)
    }
    
    private ConfiguracionManzana configManzana;
    private LimitesMapa limites;

    public Mapa(int cantidadManzanasX, int cantidadManzanasY)
    {
        // Configuración basada en las coordenadas
        configManzana = new ConfiguracionManzana
        {
            anchoManzana = 13.5f,  // Diferencia entre -12.73 y 0.77
            altoManzana = 8.57f,   // Diferencia entre 16.75 y 8.18
            origenMapa = new Vector2(-12.73f, 16.75f)
        };
        
        // Configuración de límites del mapa
        limites = new LimitesMapa
        {
            superiorIzquierdo = new Vector2(-13f, 16.95f),
            superiorDerecho = new Vector2(27.91f, 16.95f),
            inferiorIzquierdo = new Vector2(-13f, -8.98f),
            inferiorDerecho = new Vector2(28.01f, -9.02f)
        };
        
        matrizMapa = new Posicion[cantidadManzanasX, cantidadManzanasY];
        
        // Generar posiciones de manzanas con coordenadas
        for (int x = 0; x < cantidadManzanasX; x++)
        {
            for (int y = 0; y < cantidadManzanasY; y++)
            {
                float posX = configManzana.origenMapa.x + (x * configManzana.anchoManzana);
                float posY = configManzana.origenMapa.y - (y * configManzana.altoManzana);
                
                matrizMapa[x, y] = new Posicion { X = posX, Y = posY };
            }
        }
    }
    /// Genera 3 esquinas de patrullaje desde una manzana aleatoria
    /// Validando que estén dentro de los límites del mapa
    public Vector2[] GenerarPatrullajeAleatorioCardinal()
    {
        System.Random rnd = new System.Random();
        
        // Intentar generar una manzana válida dentro de los límites
        int maxIntentos = 50; // Evitar bucle infinito
        int intentos = 0;
        
        while (intentos < maxIntentos)
        {
            // Seleccionar una manzana aleatoria
            int manzanaX = rnd.Next(0, matrizMapa.GetLength(0));
            int manzanaY = rnd.Next(0, matrizMapa.GetLength(1));
            
            // Obtener la esquina superior izquierda como punto inicial
            Posicion esquinaSuperiorIzq = matrizMapa[manzanaX, manzanaY];
            
            // Calcular las 3 esquinas del patrullaje
            Vector2 puntoInicial = new Vector2(esquinaSuperiorIzq.X, esquinaSuperiorIzq.Y);
            Vector2 esquinaInferiorIzq = new Vector2(
                esquinaSuperiorIzq.X, 
                esquinaSuperiorIzq.Y - configManzana.altoManzana
            );
            Vector2 esquinaInferiorDer = new Vector2(
                esquinaSuperiorIzq.X + configManzana.anchoManzana, 
                esquinaSuperiorIzq.Y - configManzana.altoManzana
            );
            
            // Validar que todas las esquinas estén dentro de los límites
            if (EsquinasDentroDelimites(puntoInicial, esquinaInferiorIzq, esquinaInferiorDer))
            {
                List<Vector2> esquinasPatrullaje = new List<Vector2>();
                esquinasPatrullaje.Add(puntoInicial);        // Superior izquierda
                esquinasPatrullaje.Add(esquinaInferiorIzq);  // Inferior izquierda  
                esquinasPatrullaje.Add(esquinaInferiorDer);  // Inferior derecha
                
                // Debug.Log($"Patrullaje generado en manzana ({manzanaX}, {manzanaY}) - VÁLIDO");
                // Debug.Log($"Esquinas: {esquinasPatrullaje[0]} -> {esquinasPatrullaje[1]} -> {esquinasPatrullaje[2]}");
                
                return esquinasPatrullaje.ToArray();
            }
            else
            {
                // Debug.Log($"Manzana ({manzanaX}, {manzanaY}) fuera de límites, reintentando...");
                intentos++;
            }
        }
        
        // Si no se pudo generar una manzana válida, usar una posición segura
        Debug.LogWarning("No se pudo generar patrullaje dentro de límites, usando posición segura");
        return GenerarPatrullajeSeguro();
    }
    
    // Verifica si las 3 esquinas del patrullaje están dentro de los límites del mapa
    private bool EsquinasDentroDelimites(Vector2 superiorIzq, Vector2 inferiorIzq, Vector2 inferiorDer)
    {
        return EsPuntoDentroDelimites(superiorIzq) && 
               EsPuntoDentroDelimites(inferiorIzq) && 
               EsPuntoDentroDelimites(inferiorDer);
    }
    
    // Verifica si un punto está dentro de los límites del mapa
    private bool EsPuntoDentroDelimites(Vector2 punto)
    {
        return punto.x >= limites.superiorIzquierdo.x && 
               punto.x <= limites.superiorDerecho.x &&
               punto.y >= limites.inferiorIzquierdo.y && 
               punto.y <= limites.superiorIzquierdo.y;
    }
    
    // Genera un patrullaje seguro en el centro del mapa cuando no se pueden generar esquinas válidas
    private Vector2[] GenerarPatrullajeSeguro()
    {
        // Calcular el centro del mapa
        Vector2 centroMapa = new Vector2(
            (limites.superiorIzquierdo.x + limites.superiorDerecho.x) / 2f,
            (limites.superiorIzquierdo.y + limites.inferiorIzquierdo.y) / 2f
        );
        
        // Crear un patrullaje pequeño alrededor del centro
        float offset = 3f;
        List<Vector2> esquinasSeguras = new List<Vector2>();
        esquinasSeguras.Add(new Vector2(centroMapa.x - offset, centroMapa.y + offset)); // Superior izquierda
        esquinasSeguras.Add(new Vector2(centroMapa.x - offset, centroMapa.y - offset)); // Inferior izquierda
        esquinasSeguras.Add(new Vector2(centroMapa.x + offset, centroMapa.y - offset)); // Inferior derecha
        
        Debug.Log("Patrullaje seguro generado en el centro del mapa");
        return esquinasSeguras.ToArray();
    }
    
    // Obtiene los límites del mapa para visualización o validación externa
    public Vector2[] ObtenerLimitesMapa()
    {
        return new Vector2[]
        {
            limites.superiorIzquierdo,
            limites.superiorDerecho,
            limites.inferiorDerecho,
            limites.inferiorIzquierdo
        };
    }

    // Matriz 2x2 de posiciones aleatorias dentro del mapa
    public Posicion[] GenerarEsquinasPolicia()
    {
        System.Random rnd = new System.Random();
        int ancho = matrizMapa.GetLength(0);
        int alto = matrizMapa.GetLength(1);
        HashSet<string> usadas = new HashSet<string>();
        List<Posicion> esquinas = new List<Posicion>();
        while (esquinas.Count < 4)
        {
            int x = rnd.Next(0, ancho);
            int y = rnd.Next(0, alto);
            string key = $"{x},{y}";
            if (!usadas.Contains(key))
            {
                esquinas.Add(matrizMapa[x, y]);
                usadas.Add(key);
            }
        }
        return esquinas.ToArray();
    }

    // Crea un GatoPolicia con posiciones aleatorias en el mapa
    public GatoPolicia CrearGatoPolicia(string nombre, int radioBusqueda, int radioDeteccion)
    {
        Posicion[] esquinas = GenerarEsquinasPolicia();
        return new GatoPolicia(nombre, radioBusqueda, radioDeteccion, esquinas);
    }
}