using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class NetworkSyncManager : RealtimeComponent<GameManagementModel>
{
    public int currentSyncedTurn = 0;
    public GameRefModel.BoatColors currentSyncedTurnColor = (GameRefModel.BoatColors)0;
    public delegate void TurnUpdateEvent(int turnNumber, GameRefModel.BoatColors turnColor);
    public static event TurnUpdateEvent OnNetworkTurnUpdate;

    public int currentTurnStateNumber = 0;
    public GameRefModel.TurnState currentTurnState = (GameRefModel.TurnState)0;
    public delegate void TurnStateUpdateEvent(int turnStateNumber, GameRefModel.TurnState turnState);
    public static event TurnStateUpdateEvent OnNetworkTurnStateUpdate;

    public int currentGameStateNumber = 0;
    public GameRefModel.GameState currentGameState = (GameRefModel.GameState)0;
    public delegate void GameStateUpdateEvent(int gameStateNumber, GameRefModel.GameState gameState);
    public static event GameStateUpdateEvent OnNetworkGameStateUpdate;



    protected override void OnRealtimeModelReplaced(GameManagementModel previousModel, GameManagementModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.playerTurnDidChange -= PlayerTurnDidChange;
            previousModel.turnStateDidChange -= TurnStateDidChange;
            previousModel.gameStateDidChange -= GameStateDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.playerTurn = currentSyncedTurn;
                currentModel.turnState = currentTurnStateNumber;
                currentModel.gameState = currentGameStateNumber;
            }

            // Update the mesh render to match the new model
            UpdateTurn();
            UpdateTurnState();
            UpdateGameState();

            // Register for events so we'll know if the color changes later
            currentModel.playerTurnDidChange += PlayerTurnDidChange;
            currentModel.turnStateDidChange += TurnStateDidChange;
            currentModel.gameStateDidChange += GameStateDidChange;

        }
    }




    //Turn methods

    private void PlayerTurnDidChange(GameManagementModel model, int value)
    {
        UpdateTurn();
    }

    private void UpdateTurn()
    {
        currentSyncedTurn = model.playerTurn;
        currentSyncedTurnColor = (GameRefModel.BoatColors)currentSyncedTurn;
        if (OnNetworkTurnUpdate != null)
        {
            OnNetworkTurnUpdate(currentSyncedTurn, currentSyncedTurnColor);
        }
    }

    public void UpdateNetworkedTurn(int newTurn)
    {
        model.playerTurn = newTurn;
    }




    //Turn State Methods

    private void TurnStateDidChange(GameManagementModel model, int value)
    {
        UpdateTurnState();
    }

    private void UpdateTurnState()
    {
        currentTurnStateNumber = model.turnState;
        currentTurnState = (GameRefModel.TurnState)currentTurnStateNumber;
        if (OnNetworkTurnStateUpdate != null)
        {
            OnNetworkTurnStateUpdate(currentTurnStateNumber, currentTurnState);
        }
        Debug.Log("the following turn state has been changed and propogated - " + currentTurnState);
    }

    public void UpdateNetworkedTurnState(GameRefModel.TurnState newTurnState)
    {
        Debug.Log("this new turn state was passed locally - " + newTurnState);
        model.turnState = (int)newTurnState;
        Debug.Log("the network model turnState is now - " + model.turnState);
    }




    //Game State Methods
    private void GameStateDidChange(GameManagementModel model, int value)
    {
        UpdateGameState();
    }

    private void UpdateGameState()
    {
        currentGameStateNumber = model.gameState;
        currentGameState = (GameRefModel.GameState)currentGameStateNumber;
        if (OnNetworkGameStateUpdate != null)
        {
            OnNetworkGameStateUpdate(currentGameStateNumber, currentGameState);
        }
    }

    public void UpdateNetworkedGameState(GameRefModel.GameState newGameState)
    {
        model.gameState = (int)newGameState;
    }
}
