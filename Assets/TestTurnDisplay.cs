using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurnDisplay : MonoBehaviour
{

    private NetworkSyncManager localTurnSync;
    [SerializeField] private Material yellowBoatMaterial = default;
    [SerializeField] private Material redBoatMaterial = default;
    private Renderer localRenderer;
    
    void Start()
    {
        localTurnSync = GetComponent<NetworkSyncManager>();
        localRenderer = GetComponent<Renderer>();
        //EventManager.OnTurnUpdate += UpdateTurnDisplay;
    }

    void UpdateTurnDisplay()
    {
        Debug.Log("the current turn is : " + localTurnSync.currentSyncedTurn);
        if (localTurnSync.currentSyncedTurn == 0)
        {
            localRenderer.material = yellowBoatMaterial;
        }
        if (localTurnSync.currentSyncedTurn == 1)
        {
            localRenderer.material = redBoatMaterial;
        }
    }
}
