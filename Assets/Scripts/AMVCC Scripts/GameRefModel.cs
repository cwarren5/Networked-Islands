using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRefModel : IslandsElement
{
    public float flattenSpeed = 1.0f;
    public float boatSpeed = 10.0f;
    public int drawnLineLength = 50;
    public int startingMines = 0;
    public float bombBlastRadius = 4.0f;

    //global enums
    public enum BoatColors { Yellow, Red };
    public enum TurnState { Thinking, Planning, Executing };
    public enum GameState { GameMatching, GamePlaying, GameOver };
    
    public BoatColors currentTurn;
    public BoatColors localTeam;
    public GameState currentGameState = GameState.GameMatching;
    public GameObject bombPrefab;
    public GameObject minePrefab;
    public Transform[] yellowStartPositions = new Transform[3];
    public Transform[] redStartPositions = new Transform[3];
    public TeamModel yellowTeamModel;
    public TeamModel redTeamModel;

    void Start()
    {
        //EventManager.OnTurnUpdate += UpdateTurnReference;
    }

    private void UpdateTurnReference()
    {
        currentTurn = (GameRefModel.BoatColors)app.networkSyncManager.currentSyncedTurn;
    }
}
