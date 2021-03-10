using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamModel : IslandsElement
{
    // Start is called before the first frame update
    public GameRefModel.BoatColors teamColor;
    public bool isLocalTeam = false;
    public bool itsYourTurn = false;
    public int boatCount = 0;
    public int totalMines = 0;
    public bool hasBomb;
    public bool hasMine;
    public List<GameObject> boatList = new List<GameObject>();
    public List<Vector3> points = new List<Vector3>();
    public Transform[] boatStartPositions = new Transform[3];
    public GameObject yourBombButton;
    public GameObject yourMineButton;


    void Awake()
    {

    }

    public void UpdateBoatCount()
    {
        boatCount = boatList.Count;
        if(boatCount == 0)
        {
            app.gameRefController.SomeoneLostTheGame(teamColor);
        }
    }

    public void UpdateMineCountDisplay()
    {
        if (teamColor == GameRefModel.BoatColors.Yellow)
        {
            app.uiView.yellowMineText.text = "" + totalMines;

        }
        if (teamColor == GameRefModel.BoatColors.Red)
        {
            app.uiView.redMineText.text = "" + totalMines;
        }
    }
}
