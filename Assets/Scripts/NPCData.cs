using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Target Data", menuName = "SO/Target")]
public class NPCData : ScriptableObject
{
    public AnimatorController animController;
    public string targetName = "";
    public string targetSobriet = "";
    public List<string> clues = new();
    public List<string> fakeClues = new();
    public DialogueContainer dialogueContainer;
    public Vector3 maskPosition;
    public string color;
}
