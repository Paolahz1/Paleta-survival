using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PoliceSpawner : MonoBehaviour
{
    [Header("Configuración de Spawns")]
    public GameObject policePrefab;              // Prefab del policía
    public Transform[] spawnPoints;              // Puntos de aparición
    public int cantidadPolicias = 3;             // Cuántos policías generar

    private List<GameObject> policiasActuales = new List<GameObject>();

    void Start()
    {
        GenerarPolicias();
    }

    void GenerarPolicias()
    {
        LimpiarPolicias(); // Por si ya había algunos

        List<int> indicesUsados = new List<int>();

        for (int i = 0; i < cantidadPolicias; i++)
        {
            // Elegir un punto de spawn aleatorio que no esté ocupado
            int indice;
            do
            {
                indice = Random.Range(0, spawnPoints.Length);
            } while (indicesUsados.Contains(indice));

            indicesUsados.Add(indice);

            // Instanciar policía en ese punto
            GameObject nuevoPolicia = Instantiate(policePrefab, spawnPoints[indice].position, Quaternion.identity);
            policiasActuales.Add(nuevoPolicia);
        }
    }

    void LimpiarPolicias()
    {
        foreach (GameObject p in policiasActuales)
        {
            if (p != null)
                Destroy(p);
        }
        policiasActuales.Clear();
    }

    // 👇 Método para regenerar policías (cuando el gato es atrapado)
    public void RegenerarPolicias()
    {
        LimpiarPolicias();
        GenerarPolicias();
    }

    // 👇 También puedes regenerar automáticamente al reiniciar la escena
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GenerarPolicias();
    }
}
