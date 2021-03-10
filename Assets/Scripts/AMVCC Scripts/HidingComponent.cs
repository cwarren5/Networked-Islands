using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingComponent : IslandsElement
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkSyncManager.OnNetworkTurnStateUpdate += HideChildren;
    }

    private void HideChildren(int turnStateNumber, GameRefModel.TurnState turnState)
    {
        if (turnState == GameRefModel.TurnState.Planning && app.gameRefModel.localTeam == app.networkSyncManager.currentSyncedTurnColor)
        {
            foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        }
        else
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }

    }

    private void OnDestroy()
    {
        NetworkSyncManager.OnNetworkTurnStateUpdate -= HideChildren;
    }
}
