using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class BoatExplosionComponent : MonoBehaviour
{
    private RealtimeView realtimeView;
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        Debug.Log("particle played");
        realtimeView = GetComponent<RealtimeView>();
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }  
        Invoke("SelfDestruct", 4.8f);
    }

    // Update is called once per frame
    private void SelfDestruct()
    {
        Realtime.Destroy(gameObject);
    }
}
