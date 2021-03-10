using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class MineController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("TurnOnCollider", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TurnOnCollider()
    {
        GetComponent<Collider>().enabled = true;
    }

}
