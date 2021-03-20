using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class MatchmakingSync : RealtimeComponent<MatchmakingManagementModel>
{
    public int currentTropicalQue = 0;
    public delegate void QueUpdateEvent(int currentNumber);
    public static event QueUpdateEvent OnTropicalQueUpdate;


    protected override void OnRealtimeModelReplaced(MatchmakingManagementModel previousModel, MatchmakingManagementModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.tropicalQueDidChange -= TropicalQueDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.tropicalQue = currentTropicalQue;
            }

            // Update the mesh render to match the new model
            UpdateTropicalQue();

            // Register for events so we'll know if the color changes later
            currentModel.tropicalQueDidChange += TropicalQueDidChange;
        }
    }




    //Turn methods

    private void TropicalQueDidChange(MatchmakingManagementModel model, int value)
    {
        UpdateTropicalQue();
    }

    private void UpdateTropicalQue()
    {
        currentTropicalQue = model.tropicalQue;
        if (OnTropicalQueUpdate != null)
        {
            OnTropicalQueUpdate(currentTropicalQue);
        }
    }

    public void UpdateTropicalQue(int newTropicalQue)
    {
        model.tropicalQue = newTropicalQue;
    }
}
