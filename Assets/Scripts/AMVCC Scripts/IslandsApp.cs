using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class IslandsElement : MonoBehaviour
{
    // Gives access to the application and all instances.
    public IslandsApp app { get { return GameObject.FindObjectOfType<IslandsApp>(); } }
}
public class IslandsApp : MonoBehaviour
{
    public NetworkSyncManager networkSyncManager;
    public GameRefModel gameRefModel;
    public GameRefController gameRefController;
    public GameUIView uiView;
    public TerrainController terrainController;
    public Realtime realtime;
    // Start is called before the first frame update
    void Awake()
    {
        gameRefModel = FindObjectOfType<GameRefModel>();
        gameRefController = FindObjectOfType<GameRefController>();
        networkSyncManager = FindObjectOfType<NetworkSyncManager>();
        terrainController = FindObjectOfType<TerrainController>();
        realtime = FindObjectOfType<Realtime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
