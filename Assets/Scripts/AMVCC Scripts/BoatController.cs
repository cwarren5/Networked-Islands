using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;

public class BoatController : IslandsElement
{
    //[SerializeField] private GameObject explodingParticlesPrefab = default;
    private GameObject bombBlast = default;
    //[SerializeField] private GameObject mine = default;
    [SerializeField] private GameObject planPrefab;
    //[SerializeField] private GameObject turnHighlighter = default;
    //[SerializeField] private GameObject anchorIcon = default;
    //[SerializeField] public GameObject forceField = default;
    private TeamModel myTeam;
    private BoatView myBoatView;
    private GameObject bombIcon = default;
    private RealtimeView realtimeView;

    //private GameObject mineIcon = default;
    //private GameObject[] minerIcons = default;
    private GameObject myBoatPlan = default;
    [SerializeField] private GameRefModel.BoatColors myTeamColor = default;

    private bool dead = false;
    private int runPosition = 0;
    private bool beached = false;
    private int timeOut = 0;
    private bool turnMonitor = true;

    PlanController plan;

    void Start()
    {

        //Attaches local components and events
        myBoatView = GetComponent<BoatView>();
        realtimeView = GetComponent<RealtimeView>();
        NetworkSyncManager.OnNetworkTurnUpdate += BoatActiviation;
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }
        myBoatView.turnHighlighter.SetActive(false);

        //Assigns boat to the correct team and updates the team model
        if (myTeamColor == GameRefModel.BoatColors.Yellow)
        {
            myTeam = app.gameRefModel.yellowTeamModel;
        }
        if (myTeamColor == GameRefModel.BoatColors.Red)
        {
            myTeam = app.gameRefModel.redTeamModel;
        }
        myTeam.boatList.Add(gameObject);
        myTeam.UpdateBoatCount();

        //Done just for this boat
        if (realtimeView.isOwnedLocallyInHierarchy)
        {         
            //Instantiates and stores the PlanController
            myBoatPlan = Instantiate(planPrefab, transform.position, Quaternion.identity);
            myBoatPlan.SetActive(false);
            plan = myBoatPlan.GetComponent<PlanController>();
            myBoatView.turnHighlighter.SetActive(false);
            BoatActiviation(app.networkSyncManager.currentTurnStateNumber, app.networkSyncManager.currentSyncedTurnColor);
            plan.myTeamModel = myTeam;
        }

        //bombIcon = GameObject.FindGameObjectWithTag("bombIcon");
        //mineIcon = GameObject.FindGameObjectWithTag("mineIcon");
        //anchorIcon.SetActive(false);
        /*if (localReferee.startWithForcefields)
        {
            forceField.SetActive(true);
        }*/

    }

    void Update()
    {
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            if (plan.running) //this probably needs to change
            {
                beached = false;
                PlayBoatAlongPath();
                LayBomb();//could this be in PlayBoatAlongPath?
                LayMine();//could this be in PlayBoatAlongPath?
            }
        }
    }

    private void BoatActiviation(int turnNumber, GameRefModel.BoatColors turnColor)
    {
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            if (myTeamColor == turnColor)
            {
                myBoatPlan.SetActive(true);
                myBoatView.turnHighlighter.SetActive(true);
            }
            else
            {
                myBoatPlan.SetActive(false);
                myBoatView.turnHighlighter.SetActive(false);
            }
        }
    }

    private void PlayBoatAlongPath()
    {
        if (runPosition + 1 <= plan.points.Count)
        {
            Vector3 target = plan.points[runPosition];
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = app.gameRefModel.boatSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);
            if (Vector3.Distance(target, transform.position) <= 1)
            {
                runPosition++;
            }
            else
            {
                transform.position = transform.position + moveDirection * app.gameRefModel.boatSpeed * Time.deltaTime;
                transform.position = transform.position + moveDirection * app.gameRefModel.boatSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
        else
        {
            plan.running = false;
            InitiateNextPlayerTurn();
            runPosition = 0;
            myBoatPlan.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other) //this trigger should be handled by the view
    {
        
    }

    public void BoatOnTriggerEnter(GameObject hitObject, String hitTag)
    {
        if (realtimeView.isOwnedLocallyInHierarchy)
        {
            if (hitTag == "sand")
            {
                if (!beached)
                {
                    //anchorIcon.SetActive(true);
                    beached = true;
                    //timeOut = 0;
                    plan.running = false;
                    InitiateNextPlayerTurn();
                    runPosition = 0;
                    myBoatPlan.transform.position = transform.position;
                }
            }
            if (hitTag == "glacier")
            {
                InitiateNextPlayerTurn();
                InitiateSelfDestruct();
            }


            if (hitTag == "bomb" && !plan.running)
            {
                InitiateSelfDestruct();
            }
        }
    }

    /* No sharks for now
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "sharkWaters" && !pathScript.running)
        {
            Debug.Log("In Shark Waters");
            InitiateSelfDestruct();
        }
    }
    */

    private void InitiateSelfDestruct()
    {
        myTeam.boatList.Remove(gameObject);
        myTeam.UpdateBoatCount();
        //localReferee.CheckForWinner();//Should be done at the team level
        Destroy(myBoatPlan);
        //GameObject boatExplosion = Realtime.Instantiate(prefabName: "Boat Explosion", ownedByClient: true, preventOwnershipTakeover: true, useInstance: app.realtime);
        //boatExplosion.transform.position = new Vector3(transform.position.x, boatExplosion.transform.position.y, transform.position.z);
        Realtime.Destroy(gameObject);
    }
    private void InitiateNextPlayerTurn()
    {
        //bombIcon.SetActive(true);//probably should happen somewhere else
        app.gameRefController.NextTurn(myTeamColor);
    }


    private void LayBomb()
    {
        //Debug.Log("PotentialToLayBomb");
        if (plan.bombPosition == runPosition && !myTeam.hasBomb)
        {
            bombBlast = Realtime.Instantiate(prefabName: "Bomb Blast", ownedByClient: true, preventOwnershipTakeover: true, useInstance: app.realtime);
            bombBlast.transform.position = transform.position;
            myTeam.hasBomb = true;
        }
        
    }

    private void LayMine()
    {
        /* Revisit all this
        if (pathScript.minePosition == runPosition && !localReferee.usedMine[(int)boatColor] && pathScript.droppedMine)
        {
            GameObject myBomb = Instantiate(mine, transform.position, Quaternion.identity);
            myBomb.transform.SetParent(gameObject.transform.parent);
            localReferee.usedMine[(int)boatColor] = true;
            localReferee.teamMines[(int)boatColor]--;
            localReferee.UpdateMineDisplay();
            pathScript.droppedMine = false;
        }
        */
    }
}
