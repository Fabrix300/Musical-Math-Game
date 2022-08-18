using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    private Queue<string> sentences;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue[] dialogue)
    {
        // iteramos entre los Dialogue que son por cada personaje
        // Se debe ocultar el ui de movimiento del jugador mientras esté en conversación
    }
}
