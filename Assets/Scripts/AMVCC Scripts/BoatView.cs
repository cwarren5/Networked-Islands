using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatView : IslandsElement
{
    public GameObject turnHighlighter;
    public GameObject boatModel;
    private BoatController myBoatController;

    // Start is called before the first frame update
    void Start()
    {
        myBoatController = GetComponent<BoatController>();
        NetworkSyncManager.OnNetworkTurnStateUpdate += HideBoatsDuringPlan;
    }

    private void OnDestroy()
    {
        NetworkSyncManager.OnNetworkTurnStateUpdate -= HideBoatsDuringPlan;
    }

    private void OnTriggerEnter(Collider other) //this trigger should be handled by the view
    {
        myBoatController.BoatOnTriggerEnter(other.gameObject, other.tag);
    }

    private void HideBoatsDuringPlan(int turnStateNumber , GameRefModel.TurnState turnState)
    {
        if(turnState == GameRefModel.TurnState.Planning  && app.gameRefModel.localTeam == app.networkSyncManager.currentSyncedTurnColor)
        {
            boatModel.SetActive(false);
            turnHighlighter.SetActive(false);
        }
        else
        {
            if (!boatModel.activeSelf)
            {
                boatModel.SetActive(true);
            }
        }
               
    }
}
