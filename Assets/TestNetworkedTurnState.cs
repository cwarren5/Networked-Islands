using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetworkedTurnState : IslandsElement
{
    // Start is called before the first frame update
    public GameObject displayCube;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(app.networkSyncManager.currentTurnStateNumber == 1)
        {
            displayCube.SetActive(true);
        }
        else
        {
            displayCube.SetActive(false);
        }
    }
}
