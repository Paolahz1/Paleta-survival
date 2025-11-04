using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialogPanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private Coroutine typingCoroutine;

    public Canvas tradeCanvas;

    public Trade trade;

    public PlayerInteraction playerInteraction;

    public bool CanInteract() => !isDialogueActive;

    public void Interact()
    {
        if (dialogueData == null || (PauseController.IsGamePaused && isDialogueActive))
            return;

        if (isDialogueActive)
            NextLine();
        else
            StartDialogue();
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        if (nameText != null) nameText.text = dialogueData.npcName;
        if (portraitImage != null) portraitImage.sprite = dialogueData.npcPortrait;

        if (dialogPanel != null) dialogPanel.SetActive(true);

        // Pausar el juego (Time.timeScale = 0) - usamos WaitForSecondsRealtime en la coroutine
        PauseController.setPause(true);

        // Iniciar la coroutine usando la referencia
        typingCoroutine = StartCoroutine(Typeline());
    }

    void NextLine()
    {
        if (isTyping)
        {
            // detener solo la corutina de tipeo actual (no todas)
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            // mostrar la línea completa inmediatamente
            dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(Typeline());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator Typeline()
    {
        isTyping = true;
        dialogueText.text = "";

        string line = dialogueData.dialogueLines[dialogueIndex];
        for (int i = 0; i < line.Length; i++)
        {
            dialogueText.text += line[i];

            // usa tiempo real, para que siga aun cuando Time.timeScale == 0
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines != null &&
            dialogueData.autoProgressLines.Length > dialogueIndex &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isDialogueActive = false;
        dialogueText.text = "";
        if (dialogPanel != null) dialogPanel.SetActive(false);
        OpenTrade();
    }

    public void OpenTrade()
    {
        tradeCanvas.enabled = true;
        trade.traderInventory.RemoverTodosLosItem(new Item("Coin"));
        trade.traderInventory.AgregarItem(trade.CrearItem("Coin", playerInteraction.gatoPrincipal.Coins), "Coin");
    }

    public void CloseTrade()
    {
        tradeCanvas.enabled = false;
        PauseController.setPause(false);
    }
}
