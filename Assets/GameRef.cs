using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;

public class GameRef : MonoBehaviour
{
    [HideInInspector] public enum BoatColors { Yellow, Red };
    [HideInInspector] public List<GameObject> yellowBoatList = new List<GameObject>();
    [HideInInspector] public List<GameObject> redBoatList = new List<GameObject>();
    [SerializeField] public Transform[] yellowStartPositions = new Transform[3];
    [SerializeField] public Transform[] redStartPositions = new Transform[3];
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectYellow()
    {
        for (int i = 0; i < yellowBoatList.Count; i++)
        {
            GameObject targetBoat = yellowBoatList[i];
            targetBoat.GetComponent<RealtimeView>().RequestOwnership();
            targetBoat.GetComponent<RealtimeTransform>().RequestOwnership();
        }                         
    }

    public void ConnectRed()
    {
        for (int i = 0; i < redBoatList.Count; i++)
        {
            GameObject targetBoat = redBoatList[i];
            targetBoat.GetComponent<RealtimeView>().RequestOwnership();
            targetBoat.GetComponent<RealtimeTransform>().RequestOwnership();
        }
    }

    public void AddToBoatList(GameObject boatObject, BoatColors boatColor)
    {
        if(boatColor == BoatColors.Yellow)
        {
            yellowBoatList.Add(boatObject);
        }
        if (boatColor == BoatColors.Red)
        {
            redBoatList.Add(boatObject);
        }
    }

    public void PlaceYellowBoats(GameObject instantiatedBoat, int positionNumber)
    {
        Vector3 spawnPosition = yellowStartPositions[positionNumber].position;
        instantiatedBoat.transform.position = spawnPosition;
    }
    public void PlaceRedBoats(GameObject instantiatedBoat, int positionNumber)
    {
        Vector3 spawnPosition = redStartPositions[positionNumber].position;
        instantiatedBoat.transform.position = spawnPosition;
    }
}
