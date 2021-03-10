using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SpawnPointController : IslandsElement
{

    private Transform[] spawnPoints;
    private Vector3[] spawnPositions;
    [SerializeField] private GameObject pickUp = default;
    [SerializeField] private int turnsBetweenPickups = 5;
    [SerializeField] private int pickupDuration = 4;
    private GameObject placedPickup = default;
    private int currentTurn = 0;
    private int nextPickupTurn = 2;
    private int pickupExpiration = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        spawnPositions = new Vector3[spawnPoints.Length - 1];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPositions[i] = spawnPoints[i + 1].position;
            Debug.Log("All the spawn positions are: " + spawnPositions[i]);
        }
        NetworkSyncManager.OnNetworkTotalTurnUpdate += OnTurnCountUpdate;
    }


    private void OnTurnCountUpdate(int turnCount)
    {       
        currentTurn = turnCount;
        CheckToPlacePickup();
    }

    private void CheckToPlacePickup()
    {
        Debug.Log("the current turn count is" + currentTurn);
        if (app.realtime.clientID == 0)
        {
            if (currentTurn == nextPickupTurn)
            {
                int randomSpot = Random.Range(0, spawnPositions.Length);
                placedPickup = Realtime.Instantiate(prefabName: "Mine Pickup", ownedByClient: true, preventOwnershipTakeover: true, useInstance: app.realtime);
                placedPickup.GetComponent<RealtimeTransform>().RequestOwnership();
                placedPickup.transform.position = spawnPositions[randomSpot];
                nextPickupTurn += turnsBetweenPickups;
                pickupExpiration = currentTurn + pickupDuration;
            }
            if (currentTurn == pickupExpiration)
            {
                Realtime.Destroy(placedPickup);
            }
        }
    }
}
