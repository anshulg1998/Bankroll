using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject config for DiceManager settings.
/// </summary>
[CreateAssetMenu(fileName = "DiceManagerConfig", menuName = "Game/DiceManagerConfig", order = 1)]
public class DiceManagerConfig : ScriptableObject
{
    public int diceCount = 1;
    public GameObject dicePrefab;
}
