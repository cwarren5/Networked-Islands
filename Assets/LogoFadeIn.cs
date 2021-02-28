using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoFadeIn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer islandsLogo;
    [SerializeField] private float fadeLength = 4.0f;
    private float timer = 0.0f;
    private bool logoStart = false;
    // Start is called before the first frame update
    void Start()
    {
        islandsLogo = GetComponent<SpriteRenderer>();
        Invoke("StartLogoIntro", 0.5f);
        islandsLogo.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 1.0f && logoStart)
        {
            timer += Time.deltaTime / fadeLength;
            float inOut = Mathf.Sin(timer * Mathf.PI);
            float clampInOut = Mathf.Clamp(inOut * 1, 0, 1.0f);
            islandsLogo.color = new Color(1f, 1f, 1f, clampInOut);
        }
        else if(timer >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void StartLogoIntro()
    {
        logoStart = true;
    }
}
