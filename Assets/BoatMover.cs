using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Normal.Realtime;

public class BoatMover : MonoBehaviour
{
    private LineRenderer boatPath;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    private bool running = false;
    private int runPosition = 0;
    private Rigidbody boatRigid;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameRef.BoatColors boatColor = GameRef.BoatColors.Yellow;
    [SerializeField] private GameObject highlighter = default;
    //private GameRef localRef;
    //Multiplayer
    private RealtimeView _realtimeView;
    private Realtime _realtime;
    private NetworkSyncManager _globalTurnSync;

    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        boatRigid = GetComponent<Rigidbody>();
        _realtimeView = GetComponent<RealtimeView>();
        _realtime = FindObjectOfType<Realtime>();
        _globalTurnSync = FindObjectOfType<NetworkSyncManager>();
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
            highlighter.SetActive(true);
        }
        else { highlighter.SetActive(false); }
        //localRef = FindObjectOfType<GameRef>();
        //localRef.AddToBoatList(gameObject, boatColor);
    }

    void Update()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            if (running)
            {
                if (runPosition + 1 <= points.Count)
                {
                    Vector3 target = points[runPosition];
                    Vector3 moveDirection = (target - transform.position).normalized;
                    float singleStep = speed * Time.deltaTime;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);
                    //transform.position = points[runPosition];
                    if (Vector3.Distance(target, transform.position) <= 1)
                    {
                        runPosition++;
                    }
                    else
                    {
                        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                        transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                else
                {
                    running = false;
                    runPosition = 0;
                    points.Clear();
                    //_globalTurnSync.NextTurn((int)boatColor);
                }
            }
        }
    }

    void OnMouseDown()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            points.Clear();
            boatPath.SetPositions(points.ToArray());
            boatPath.enabled = true;
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    void OnMouseDrag()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            //Debug.Log(GetMouseAsWorldPoint() + mOffset);
            Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
            if (DistanceToLastPoint(mousePoint) > .25f)
            {
                points.Add(mousePoint);
                boatPath.positionCount = points.Count;
                boatPath.SetPositions(points.ToArray());
                //transform.position = mousePoint;
            }
        }
    }

    void OnMouseUp()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            Invoke("StartBoatMovement", 0.1f); //Todo - Attach to flatten speed
            boatPath.enabled = false;
        }
    }

    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
        {
            return Mathf.Infinity;
        }
        else
        {
            return Vector3.Distance(points.Last(), point);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "island")
        {
            Realtime.Destroy(gameObject);
            running = false;
            runPosition = 0;
            points.Clear();
            //_globalTurnSync.NextTurn((int)boatColor);
        }
    }

    private void StartBoatMovement()
    {
        running = true;
        runPosition = 0;
    }
}

