using UnityEngine;

public class WinLvl2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            GameObject victoryObject = GameObject.Find("Victory");
            if (victoryObject != null)
            {
                Canvas victoryCanvas = victoryObject.GetComponent<Canvas>();
                if (victoryCanvas != null)
                {
                    victoryCanvas.enabled = true;
                }
            }
        }
    }
}
