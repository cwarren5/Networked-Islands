using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TurnTimer : MonoBehaviour
{
    [SerializeField] private float turnDuration = 90.0f;
    private float currentDuration = 0;
    [SerializeField] private GameRef.BoatColors boatColor = GameRef.BoatColors.Yellow;
    private NetworkSyncManager globalTurnSync;
    private Renderer timerRenderer;
    [SerializeField] Shader testShader = default;
    [SerializeField] int propertyIDTest = 5;
    private float clipNumber;

    // Start is called before the first frame update
    void Start()
    {
        globalTurnSync = FindObjectOfType<NetworkSyncManager>();
        timerRenderer = GetComponent<Renderer>();
        currentDuration = turnDuration;
    }

    // Update is called once per frame
    void Update()
    {
        currentDuration -= Time.deltaTime;
        clipNumber = (currentDuration / turnDuration);
        int testShaderID = testShader.FindPropertyIndex("_AlphaClip");
        Debug.Log("Shader ID : " + testShaderID);
        timerRenderer.material.SetFloat(propertyIDTest, clipNumber);

        if (currentDuration < 0)
        {       
            //globalTurnSync.NextTurn((int)boatColor);
            Realtime.Destroy(gameObject);
        }
    }
}
