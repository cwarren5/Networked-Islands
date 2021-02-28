using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIView : IslandsElement
{
    public GameObject planBomb = default;
    //public GameObject inventoryBomb = default;
    public GameObject redButtonBomb = default;
    public GameObject yellowButtonBomb = default;
    /*public GameObject planMine = default;
    public GameObject inventoryMine = default;
    public GameObject buttonMine = default;*/
    public GameObject[] YourTeamPlacards = new GameObject[2];
    public GameObject[] OpponentPlacards = new GameObject[2];
}
