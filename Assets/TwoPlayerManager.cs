using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TwoPlayerManager : MonoBehaviour
{
    private Realtime _realtime;
    private GameRef localRef;
    [HideInInspector] public enum PlayerStatus { Empty, OnePlayer, TwoPlayer};
    [HideInInspector] public PlayerStatus currentGameStatus = PlayerStatus.Empty;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();
        localRef = GetComponent<GameRef>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
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

            for (int i = 0; i < localRef.yellowStartPositions.Length; i++)
            {
                GameObject playerBoat = Realtime.Instantiate(prefabName: "Yellow Ship",
                                                                            ownedByClient: true,
                                                                 preventOwnershipTakeover: true,
                                                                              useInstance: realtime);
                localRef.PlaceYellowBoats(playerBoat, i);
            }


            currentGameStatus = PlayerStatus.OnePlayer;
            //localRef.ConnectYellow();
        }

        if (_realtime.clientID == 1)
        {
            GameObject playerAvatar = Realtime.Instantiate(prefabName: "Red Team Av",  // Prefab name
                                                                          ownedByClient: true,      // Make sure the RealtimeView on this prefab is owned by this client
                                                               preventOwnershipTakeover: true,      // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                                                                            useInstance: realtime); // Use the instance of Realtime that fired the didConnectToRoom event.

            for (int i = 0; i < localRef.yellowStartPositions.Length; i++)
            {
                GameObject playerBoat = Realtime.Instantiate(prefabName: "Red Ship",
                                                                            ownedByClient: true,
                                                                 preventOwnershipTakeover: true,
                                                                              useInstance: realtime);
                localRef.PlaceRedBoats(playerBoat, i);
            }

            currentGameStatus = PlayerStatus.TwoPlayer;
            //localRef.ConnectRed();
        }



        // Get a reference to the player
        //BoatMover player = playerGameObject.GetComponent<BoatMover>();
    }
}
