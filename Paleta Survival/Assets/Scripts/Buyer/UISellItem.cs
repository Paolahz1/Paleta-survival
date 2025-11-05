using UnityEngine;

public class UISellItem : MonoBehaviour
{
    public GameObject effectPrefab;

    public string itemName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        // Debug.Log("Clicked on UISellItem");
        PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();

        if (playerInteraction != null)
        {
            if (itemName == playerInteraction.playerInventory.askedItem.Nombre)
            {
                playerInteraction.playerInventory.RemoverUnItem(new Item(0, itemName, 1));

                switch (itemName)
                {
                    case "Catnip Ziplock":
                        playerInteraction.gatoPrincipal.AddCoins(75);
                        break;
                    case "Cocat":
                        playerInteraction.gatoPrincipal.AddCoins(1300);
                        break;
                    case "Metcat":
                        playerInteraction.gatoPrincipal.AddCoins(2600);
                        break;
                    default:
                        break;
                }

                playerInteraction.completeBuyerOrder();

                playerInteraction.HideInventory();

                AnadirGatoAtendido();

                // Debug.Log("Item sold: " + itemName);
                GameObject playerCat = GameObject.Find("Player cat");
                // Debug.Log("Player Cat found: " + (playerCat != null));
                if (playerCat != null)
                {
                    GameObject effect = Instantiate(effectPrefab, new Vector3(playerCat.transform.position.x, playerCat.transform.position.y, -10), Quaternion.identity);
                    // Debug.Log("Effect instantiated at position: " + playerCat.transform.position);
                }
            }
        }
    }
    
    public void AnadirGatoAtendido()
    {
        SceneInfo.gatosAtendidos += 1;
        Debug.Log("Gatos atendidos: " + SceneInfo.gatosAtendidos);
        if (SceneInfo.gatosAtendidos >= 5)
        {
            SceneInfo.gatosAtendidos = 0;

            SceneInfo.cantidadManzanasY = 6;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene_Level2");

            Debug.Log("Level up! Moving to Level 2.");
        }
    }
}
