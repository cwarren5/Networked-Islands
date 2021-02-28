using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : IslandsElement
{
    public delegate void TurnUpdateAction();
    public static event TurnUpdateAction OnTurnUpdate;
    private int turnChecker = -1;
    private NetworkSyncManager _globalTurnSync;

    void Start()
    {
        _globalTurnSync = FindObjectOfType<NetworkSyncManager>();
    }

    void Update()
    {
            if (turnChecker != _globalTurnSync.currentSyncedTurn)
            {
                turnChecker = _globalTurnSync.currentSyncedTurn;
                if (OnTurnUpdate != null)
                {
                    OnTurnUpdate();
                }
            }
    }
}
