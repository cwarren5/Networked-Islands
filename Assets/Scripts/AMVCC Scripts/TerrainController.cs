using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : IslandsElement
{
    private Terrain topographicMap = default;
    private float lerpProgress = 0;
    public bool planning = false;
    // Start is called before the first frame update
    void Start()
    {
        topographicMap = FindObjectOfType<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (planning && lerpProgress < 1)
        {

            lerpProgress += Time.deltaTime * app.gameRefModel.flattenSpeed;
            lerpProgress = Mathf.Clamp(lerpProgress, 0, 1);
            LerpTerrainScale();

        }
        else if (!planning && lerpProgress > 0)
        { 

            lerpProgress -= Time.deltaTime * app.gameRefModel.flattenSpeed;
            lerpProgress = Mathf.Clamp(lerpProgress, 0, 1);
            LerpTerrainScale();
        }
    }

    private void LerpTerrainScale()
    {
        float currentTerrainHeight = Mathf.Lerp(10, 0f, lerpProgress);
        topographicMap.terrainData.size = new Vector3(topographicMap.terrainData.size.x, currentTerrainHeight, topographicMap.terrainData.size.z);
    }
}
