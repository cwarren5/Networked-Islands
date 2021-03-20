using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchmakerController : MonoBehaviour
{
    public int totalBoats = 0;
    public int targetScene = 1;
    public TextMesh matchmakingText = default;
    // Start is called before the first frame update
    void Start()
    {
        matchmakingText.text = "0/2";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "sand")
        {
            totalBoats++;
            matchmakingText.text = totalBoats + "/2";
            if(totalBoats > 1)
            {
                Invoke("ShowGo", 0.5f);
                Invoke("SendToLevel", 1.0f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "sand")
        {
            totalBoats--;
            matchmakingText.text = totalBoats + "/2";
        }
    }

    private void ShowGo()
    {
        matchmakingText.text = "GO!";
    }

    private void SendToLevel()
    {
        SceneManager.LoadScene(targetScene);
    }
}
