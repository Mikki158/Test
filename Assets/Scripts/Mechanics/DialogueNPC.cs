using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueNPC : MonoBehaviour
{
    public GameObject interactHint;   // надпись "E" над NPC (World Space)
    public GameObject dialoguePanel;  // окно диалога (UI)

    public DialogueManager2D dialogueManager;
    public DialogueData dialogueData;

    public bool endLevel;
    public string nextLevel;

    private bool playerInRange = false;
    public PlayerController player;

    //private void Awake()
    //{
    //    player = GetComponent<PlayerController>();
    //}

    void Start()
    {
        interactHint.SetActive(false);
        dialoguePanel.SetActive(false);
        
    }

    void Update()
    {
        if (playerInRange &&
        dialogueManager != null &&
        !dialogueManager.IsDialogueActive &&
        !dialogueManager.JustClosed &&
        !dialogueManager.StopTyping &&
        Keyboard.current != null &&
        Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenDialogue();
        }
    }

    void OpenDialogue()
    {
        player.canMove = false;
        dialogueManager.StartDialogue(dialogueData, this);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        player.canMove = true;
        Debug.Log("Конец");
        if (endLevel)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactHint.SetActive(false);
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
