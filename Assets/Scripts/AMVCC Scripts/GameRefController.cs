using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GameRefController : IslandsElement
{
    private Realtime _realtime;
    private GameRef localRef;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
        NetworkSyncManager.OnNetworkGameStateUpdate += OnGameStateUpdate;
        NetworkSyncManager.OnNetworkTurnUpdate += OnTurnUpdate;
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        Debug.Log("the client ID of the current player is - " + _realtime.clientID);
        // Instantiate the Player and Boat for this client once we've successfully connected to the room
        if (_realtime.clientID == 0)
        {
            GameObject playerAvatar = Realtime.Instantiate(prefabName: "Yellow Team Av",  // Prefab name
                                                                          ownedByClient: true,      // Make sure the RealtimeView on this prefab is owned by this client
                                                               preventOwnershipTakeover: true,      // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                                                                            useInstance: realtime); // Use the instance of Realtime that fired the didConnectToRoom event.

            for (int i = 0; i < app.gameRefModel.yellowStartPositions.Length; i++)
            {
                GameObject playerBoat = Realtime.Instantiate(prefabName: "Yellow Ship",
                                                                            ownedByClient: true,
                                                                 preventOwnershipTakeover: true,
                                                                              useInstance: realtime);
                PlaceBoats(playerBoat, i, GameRefModel.BoatColors.Yellow);
            }


            
            app.gameRefModel.localTeam = GameRefModel.BoatColors.Yellow;
        }

        if (_realtime.clientID == 1)
        {
            GameObject playerAvatar = Realtime.Instantiate(prefabName: "Red Team Av",  // Prefab name
                                                                          ownedByClient: true,      // Make sure the RealtimeView on this prefab is owned by this client
                                                               preventOwnershipTakeover: true,      // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                                                                            useInstance: realtime); // Use the instance of Realtime that fired the didConnectToRoom event.

            for (int i = 0; i < app.gameRefModel.yellowStartPositions.Length; i++)
            {
                GameObject playerBoat = Realtime.Instantiate(prefabName: "Red Ship",
                                                                            ownedByClient: true,
                                                                 preventOwnershipTakeover: true,
                                                                              useInstance: realtime);
                PlaceBoats(playerBoat, i, GameRefModel.BoatColors.Red);
            }

           
            app.gameRefModel.localTeam = GameRefModel.BoatColors.Red;
        }
    }

    public void PlaceBoats(GameObject instantiatedBoat, int positionNumber, GameRefModel.BoatColors instantiateColor)
    {
        if (instantiateColor == GameRefModel.BoatColors.Yellow)
        {
            Vector3 spawnPosition = app.gameRefModel.yellowStartPositions[positionNumber].position;
            instantiatedBoat.transform.position = spawnPosition;
        }
        if (instantiateColor == GameRefModel.BoatColors.Red)
        {
            Vector3 spawnPosition = app.gameRefModel.redStartPositions[positionNumber].position;
            instantiatedBoat.transform.position = spawnPosition;
        }
    }

    public void NextTurn(GameRefModel.BoatColors colorThatEndedTurn)
    {
        int newTurn = (int)colorThatEndedTurn;
        newTurn += 1;
        if (newTurn > 1)
        {
            newTurn = 0;
        }
        app.networkSyncManager.UpdateNetworkedTurn(newTurn);
        app.networkSyncManager.UpdateNetworkedTurnState(GameRefModel.TurnState.Thinking);
    }

    public void UpdateGameStartStatus()
    {
        if (app.gameRefModel.yellowTeamModel != null && app.gameRefModel.redTeamModel != null)
        {
            Debug.Log("this is ACTUALLY WORKING");
            app.networkSyncManager.UpdateNetworkedGameState(GameRefModel.GameState.GamePlaying);
        }
        else
        {
            app.networkSyncManager.UpdateNetworkedGameState(GameRefModel.GameState.GameMatching);
        }
    }

    private void OnGameStateUpdate(int gameStateNumber, GameRefModel.GameState gameState)
    {
        if(gameState == GameRefModel.GameState.GameMatching)
        {
            app.uiView.gameStatusText.text = "WAITING FOR PLAYERS";
        }
        if (gameState == GameRefModel.GameState.GamePlaying)
        {
            UpdateTurnText();
        }
        if (gameState == GameRefModel.GameState.GameOver)
        {
            if(app.gameRefModel.localTeamModel.boatCount == 0)
            {
                app.uiView.gameStatusText.text = "YOU LOSE";
            }
            else
            {
                app.uiView.gameStatusText.text = "YOU WIN!";
            }
            app.uiView.playAgain.SetActive(true);
        }
    }

    private void OnTurnUpdate(int turnNumber, GameRefModel.BoatColors turnColor)
    {
        if (app.networkSyncManager.currentGameState == GameRefModel.GameState.GamePlaying)
        {
            UpdateTurnText();
        }
    }
    
    private void UpdateTurnText()
    {
        if (app.networkSyncManager.currentSyncedTurnColor == GameRefModel.BoatColors.Yellow)
        {
            app.uiView.gameStatusText.text = "YELLOW'S TURN";
        }
        if (app.networkSyncManager.currentSyncedTurnColor == GameRefModel.BoatColors.Red)
        {
            app.uiView.gameStatusText.text = "RED'S TURN";
        }
    }

    public void SomeoneLostTheGame(GameRefModel.BoatColors losingColor)
    {
        app.networkSyncManager.UpdateNetworkedGameState(GameRefModel.GameState.GameOver);
    }
}
 