using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanController : IslandsElement
{
    GameManagementModel localReferee;
    GameRefModel gameRefModel;

    [SerializeField] private float lineFidelity = .25f;

    private LineRenderer boatPath;
    public List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    public bool running = false;
    public int bombPosition = default;
    public int minePosition = default;
    public TeamModel myTeamModel = default;

    // Start is called before the first frame update
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        boatPath.enabled = false;
        
        //bombIcon = app.uiView.planBomb; //wait till i get bombs set up and a bomb Icon
    }


    //Have boat turn collider off when not its turn
    void OnMouseDown()
    {
        CreateNewBoatPath();
        //localReferee.nightEnv.SetActive(true);
        //localReferee.dayEnv.SetActive(false);
        boatPath.enabled = true;
        app.terrainController.planning = true;
        app.networkSyncManager.UpdateNetworkedTurnState(GameRefModel.TurnState.Planning);
        if(myTeamModel.totalMines > 0)
        {
            myTeamModel.hasMine = true;
        }
    }

    void OnMouseDrag()
    {
        DrawBoatPath();
        WeaponsCheck();
    }

    void OnMouseUp()
    {
        running = true;
        //runPosition = 0;
        //localReferee.nightEnv.SetActive(false);
        //localReferee.dayEnv.SetActive(true);
        boatPath.enabled = false;
        app.terrainController.planning = false;
        app.networkSyncManager.UpdateNetworkedTurnState(GameRefModel.TurnState.Executing);
        app.uiView.planBomb.SetActive(false);
        app.uiView.planMine.SetActive(false);
        //Destroy(activeBombX);
        //Destroy(activeMineM);
    }



    private void DrawBoatPath()
    {
        Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
        if (DistanceToLastPoint(mousePoint) > lineFidelity)
        {
            points.Add(mousePoint);
            if (points.Count <= app.gameRefModel.drawnLineLength)
            {
                boatPath.positionCount = points.Count;
                boatPath.SetPositions(points.ToArray());
            }
            else
            {
                Vector3[] newPositions = new Vector3[app.gameRefModel.drawnLineLength];
                for (int i = 0; i < app.gameRefModel.drawnLineLength; i++)
                {
                    newPositions[i] = points[points.Count - app.gameRefModel.drawnLineLength + i];
                }
                boatPath.SetPositions(newPositions);
            }
        }
    }

    private void CreateNewBoatPath()
    {
        points.Clear();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any()) { return Mathf.Infinity; }
        else { return Vector3.Distance(points.Last(), point); }
    }

    private void WeaponsCheck()
    {
        if (Input.GetKeyDown("b") && myTeamModel.hasBomb)
        {
            //activeBombX = Instantiate(bombX, points[points.Count - 1], Quaternion.identity);          
            app.uiView.planBomb.SetActive(true);
            app.uiView.planBomb.transform.position = points[points.Count - 1];
            bombPosition = points.Count - 1;
            myTeamModel.yourBombButton.SetActive(false);
            myTeamModel.hasBomb = false;
            // bombIcon.SetActive(false);
        }
        if (Input.GetKeyDown("m") && myTeamModel.hasMine)
        {
            app.uiView.planMine.SetActive(true);
            app.uiView.planMine.transform.position = points[points.Count - 1];
            minePosition = points.Count - 1;
            myTeamModel.yourMineButton.SetActive(true);
            myTeamModel.hasMine = false;
        }
    }
}
