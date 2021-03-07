using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TeamController : IslandsElement
{
    private TeamModel teamData;
    private Realtime realtime;
    private RealtimeView realtimeView;
    private NetworkSyncManager globalTurnSync;
    private GameObject personalTimer = default;
    private GameObject teamPlacard;
    // Start is called before the first frame update
    void Awake()
    {
        teamData = GetComponent<TeamModel>();
        if (teamData.teamColor == GameRefModel.BoatColors.Yellow)
        {
            app.gameRefModel.yellowTeamModel = teamData;
            teamData.yourBombButton = app.uiView.yellowButtonBomb;
            teamData.yourMineButton = app.uiView.yellowButtonMine;
        }
        if (teamData.teamColor == GameRefModel.BoatColors.Red)
        {
            app.gameRefModel.redTeamModel = teamData;
            teamData.yourBombButton = app.uiView.redButtonBomb;
            teamData.yourMineButton = app.uiView.redButtonMine;
        }
        app.gameRefController.UpdateGameStartStatus();
    }
    void Start()
    {
        NetworkSyncManager.OnNetworkTurnUpdate += PrepairTeamForTurn;
        realtime = FindObjectOfType<Realtime>();
        realtimeView = GetComponent<RealtimeView>();
        globalTurnSync = FindObjectOfType<NetworkSyncManager>();   
        InstantiateTimer();
        LoadMine();
        LoadBomb();
        DisplayPlacard();
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            app.gameRefModel.localTeamModel = teamData;

            //mine setup
            teamData.totalMines = app.gameRefModel.startingMines;
            if(teamData.teamColor == GameRefModel.BoatColors.Yellow)
            {
                app.uiView.yellowMineText.text = "" + teamData.totalMines;
                app.uiView.yellowMineInventory.SetActive(true);
                
            }
            if (teamData.teamColor == GameRefModel.BoatColors.Red)
            {
                app.uiView.redMineText.text = "" + teamData.totalMines;
                app.uiView.redMineInventory.SetActive(true);
                
            }
        }
    }

    private void OnDestroy()
    {
        NetworkSyncManager.OnNetworkTurnUpdate -= PrepairTeamForTurn;
        if (personalTimer != null)
        {
            Realtime.Destroy(personalTimer);
        }
        teamPlacard.SetActive(false);
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            app.gameRefController.UpdateGameStartStatus();
        }
    }

    void PrepairTeamForTurn(int turnNumber, GameRefModel.BoatColors turnColor)
    {
        Debug.Log("this is what was passed through the event " + turnNumber + turnColor);
        InstantiateTimer();
        LoadBomb();
        LoadMine();
    }

    private void InstantiateTimer()
    {
        if (app.networkSyncManager.currentSyncedTurn == 0 && (int)teamData.teamColor == 0)
        {
            personalTimer = Realtime.Instantiate(prefabName: "YellowTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        }
        else if (app.networkSyncManager.currentSyncedTurn == 1 && (int)teamData.teamColor == 1)
        {
            personalTimer = Realtime.Instantiate(prefabName: "RedTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
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

    private void LoadMine()
    {
        if (app.networkSyncManager.currentSyncedTurn == (int)teamData.teamColor && app.networkSyncManager.currentSyncedTurnColor == app.gameRefModel.localTeam)
        {  
            if (teamData.totalMines > 0)
            {
                teamData.yourMineButton.SetActive(true);
                teamData.hasMine = true;
            }
            else teamData.hasMine = false;
        }
        else
        {
            teamData.yourMineButton.SetActive(false);
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
