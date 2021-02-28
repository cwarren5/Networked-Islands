using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private GameRef.BoatColors boatColor = GameRef.BoatColors.Yellow;
    private Realtime _realtime;
    private NetworkSyncManager globalTurnSync;
    private GameObject personalTimer = default;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnTurnUpdate += InstantiateTimer;
        _realtime = FindObjectOfType<Realtime>();
        globalTurnSync = FindObjectOfType<NetworkSyncManager>();
        if (globalTurnSync.currentSyncedTurn == 0 && (int)boatColor == 0)
        {
            personalTimer = Realtime.Instantiate(prefabName: "YellowTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
        else if (globalTurnSync.currentSyncedTurn == 1 && (int)boatColor == 1)
        {
            personalTimer = Realtime.Instantiate(prefabName: "RedTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
    }

    private void OnDestroy()
    {
        EventManager.OnTurnUpdate -= InstantiateTimer;
    }

    void InstantiateTimer()
    {
        if (globalTurnSync.currentSyncedTurn == 0 && (int)boatColor == 0)
        {
            personalTimer = Realtime.Instantiate(prefabName: "YellowTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
        else if (globalTurnSync.currentSyncedTurn == 1 && (int)boatColor == 1)
        {
            personalTimer = Realtime.Instantiate(prefabName: "RedTimer", ownedByClient: true, preventOwnershipTakeover: true, useInstance: _realtime);
        }
        else
        {
            if(personalTimer != null)
            {
                Realtime.Destroy(personalTimer);
            }
        }
    }
}
