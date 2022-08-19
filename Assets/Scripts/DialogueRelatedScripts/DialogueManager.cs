using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Animator dialogueBoxAnim;
    public Text dialogueBoxCharacterName;
    public Image dialogueBoxCharacterImage;
    public Text dialogueBoxSentenceBox;
    public bool onConversation = false;

    private Queue<Dialogue> dialoguesQueue;
    private Queue<string> sentencesQueue;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dialoguesQueue = new Queue<Dialogue>();
        sentencesQueue = new Queue<string>();
    }

    public void SetPlayerMovement(PlayerMovement _player)
    {
        playerMovement = _player;
    }

    public void StartDialogue(Dialogue[] dialogueArray)
    {
        // iteramos entre los Dialogue que son por cada personaje
        // Se debe ocultar el ui de movimiento del jugador mientras esté en conversación
        //playerControls.gameObject.SetActive(false);
        onConversation = true;
        playerMovement.SetInCombat(true);
        dialoguesQueue.Clear();
        foreach (Dialogue d in dialogueArray)
        {
            dialoguesQueue.Enqueue(d);
        }
        DisplayNextDialogue();
        dialogueBoxAnim.SetInteger("state", 1);
    }

    public void DisplayNextDialogue()
    {
        if(dialoguesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        Dialogue dialogue = dialoguesQueue.Dequeue();
        dialogueBoxCharacterName.text = dialogue.characterName;
        dialogueBoxCharacterImage.sprite = dialogue.characterImage;
        sentencesQueue.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentencesQueue.Count == 0)
        {
            DisplayNextDialogue();
            return;
        }
        string sentence = sentencesQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueBoxSentenceBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueBoxSentenceBox.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueBoxAnim.SetInteger("state", 2);
        onConversation = false;
        playerMovement.SetInCombat(false);
    }
}
