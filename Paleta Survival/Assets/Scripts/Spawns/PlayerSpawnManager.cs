using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        string spawnName = PlayerPrefs.GetString("SpawnPoint", "");
        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawn = GameObject.Find(spawnName);
            if (spawn != null)
            {
                transform.position = spawn.transform.position;
            }
        }
    }
}
