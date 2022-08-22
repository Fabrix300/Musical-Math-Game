using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Character", menuName = "Dialogue Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
}
