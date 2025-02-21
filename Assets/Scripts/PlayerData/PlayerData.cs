using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData/Data", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Standard")]
    public Material body;
    public Material head;
    public Material cables;
    public Material ribs;

    [Header("Green")]
    public Material bodyGreen;
    public Material headGreen;
    public Material cablesGreen;
    public Material ribsGreen;

    [Header("Red")]
    public Material bodyRed;
    public Material headRed;
    public Material cablesRed;
    public Material ribsRed;

    [Header("Yellow")]
    public Material bodyYellow;
    public Material headYellow;
    public Material cablesYellow;
    public Material ribsYellow;
}
