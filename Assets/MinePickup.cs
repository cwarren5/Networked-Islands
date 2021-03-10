using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePickup : IslandsElement
{


    private void OnTriggerEnter(Collider other)
    {
        /*if (localReferee.teamMines[(int)localReferee.currentTurn] < localReferee.minerIcons.Length)
        {
            localReferee.teamMines[(int)localReferee.currentTurn]++;
            localReferee.UpdateMineDisplay();
        }
        Destroy(gameObject);*/
    }
}
