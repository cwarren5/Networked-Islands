using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TeamController : IslandsElement
{
    private TeamModel teamData;
    private Realtime _realtime;
    private NetworkSyncManager globalTurnSync;
    private GameObject personalTimer = default;
    private GameObject teamPlacard;
    // Start is called before the first frame update
    void Start()
    {
        NetworkSyncManager.OnNetworkTurnUpdate += PrepairTeamForTurn;
        _realtime = FindObjectOfType<Realtime>();
        globalTurnSync = FindObjectOfType<NetworkSyncManager>();
        teamData = GetComponent<TeamModel>();
        InstantiateTimer();
        LoadBomb();
        DisplayPlacard();
    }

    private void OnDestroy()
    {
        NetworkSyncManager.OnNetworkTurnUpdate -= PrepairTeamForTurn;
        if (personalTimer != null)
        {
            Realtime.Destroy(personalTimer);
        }
        teamPlacard.SetActive(false);
    }

    void PrepairTeamForTurn(int turnNumber, GameRefModel.BoatColors turnColor)
    {
        Debug.Log("this is what was passed through the event " + turnNumber + turnColor);
        InstantiateTimer();
        LoadBomb();
    }

    private void InstantiateTimer()
    {
        if (app.networkSyncManager.currentSyncedTurn == 0 && (int)teamData.teamColor == 0)
        {
            personalTimer = Realtime.Instantiate(prefabName: "YellowTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
        else if (app.networkSyncManager.currentSyncedTurn == 1 && (int)teamData.teamColor == 1)
        {
            personalTimer = Realtime.Instantiate(prefabName: "RedTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
        else
        {
            if (personalTimer != null)
            {
                Realtime.Destroy(personalTimer);
            }
        }
    }

    private void LoadBomb()
    {
        if (app.networkSyncManager.currentSyncedTurn == (int)teamData.teamColor && app.networkSyncManager.currentSyncedTurnColor == app.gameRefModel.localTeam)
        {
            teamData.yourBombButton.SetActive(true);
            teamData.hasBomb = true;
        }
        else
        {
            teamData.yourBombButton.SetActive(false);
        }
    }

    private void DisplayPlacard()
    {
        if (teamData.teamColor == app.gameRefModel.localTeam)
        {
            teamPlacard = app.uiView.YourTeamPlacards[(int)teamData.teamColor];
            teamPlacard.SetActive(true);
        }
        else
        {
            teamPlacard = app.uiView.OpponentPlacards[(int)teamData.teamColor];
            teamPlacard.SetActive(true);
        }
    }
}
