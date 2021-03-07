using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIView : IslandsElement
{
    public GameObject planBomb = default;
    public GameObject redButtonBomb = default;
    public GameObject yellowButtonBomb = default;

    public GameObject planMine = default;
    public GameObject redButtonMine = default;
    public GameObject yellowButtonMine = default;
    public GameObject yellowMineInventory = default;
    public GameObject redMineInventory = default;
    public TextMesh yellowMineText = default;
    public TextMesh redMineText = default;

    public GameObject[] YourTeamPlacards = new GameObject[2];
    public GameObject[] OpponentPlacards = new GameObject[2];
    public GameObject gameStatusDisplay;
    public TextMesh gameStatusText;
    public GameObject playAgain = default;

    void Start()
    {
        gameStatusText = gameStatusDisplay.GetComponent<TextMesh>();
    }
}
