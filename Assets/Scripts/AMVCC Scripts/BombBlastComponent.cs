using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class BombBlastComponent : IslandsElement
{
    private RealtimeView realtimeView;
    void Start()
    {
        transform.localScale = new Vector3(app.gameRefModel.bombBlastRadius, app.gameRefModel.bombBlastRadius, app.gameRefModel.bombBlastRadius);
        realtimeView = GetComponent<RealtimeView>();
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }
        Invoke("SelfDestruct", 1.0f);
    }

    private void SelfDestruct()
    {
        Realtime.Destroy(gameObject);
    }
}
