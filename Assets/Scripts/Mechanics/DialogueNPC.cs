using Platformer.Mechanics;
using System.Collections;
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
    public bool isFinal;

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
        if (isFinal)
        {
            StartCoroutine(Final());
        }
    }

    void Update()
    {
        if (playerInRange &&
        dialogueManager != null &&
        !dialogueManager.IsDialogueActive &&
        !dialogueManager.JustClosed &&
        !dialogueManager.StopTyping &&
        Keyboard.current != null &&
        Keyboard.current.eKey.wasPressedThisFrame &&
        !Keyboard.current.aKey.isPressed &&
        !Keyboard.current.dKey.isPressed)
        {
            OpenDialogue();
        }
    }

    IEnumerator Final()
    {
        yield return new WaitForSeconds(3f);
        OpenDialogue();
    }

    void OpenDialogue()
    {
        if (!isFinal)
            player.canMove = false;
        dialogueManager.StartDialogue(dialogueData, this);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);

        if (!isFinal)
        {
            Time.timeScale = 1f;
            player.canMove = true;
            Debug.Log("Конец");
            if (endLevel)
            {
                SceneManager.LoadScene(nextLevel);
            }
        } else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFinal)
        {
            playerInRange = true;
            interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFinal)
        {
            playerInRange = false;
            interactHint.SetActive(false);
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
