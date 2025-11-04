using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    [Header("Escena a la que se teletransportar�")]
    public string sceneToLoad;

    [Header("Nombre del punto de aparici�n en la nueva escena (opcional)")]
    public string spawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            SceneInfo.GatoPrincipal = collision.GetComponent<PlayerInteraction>().gatoPrincipal;

            PlayerPrefs.SetString("SpawnPoint", spawnPointName);
            SceneManager.LoadScene(sceneToLoad);

            GameObject playerCat = GameObject.FindGameObjectWithTag("Player");
            if (playerCat != null)
            {
                if(sceneToLoad == "MainScene_cat_house")
                {
                    playerCat.transform.position = new Vector3(0.22f, -2.74f, playerCat.transform.position.z);
                }
                else
                {
                    playerCat.transform.position = new Vector3(16.61f, -7.3f, playerCat.transform.position.z);
                }
            }
        }
    }
}
