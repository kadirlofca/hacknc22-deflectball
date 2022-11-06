using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{
    public static Session current;
    public static Ball ball;
    public float startCountdown = 5;
    public TMPro.TextMeshProUGUI deflectText;

    [HideInInspector] public static Player playerA;
    [HideInInspector] public static Player playerB;

    private bool started = false;
    private int deflects = 0;

    private void Awake()
    {
        current = this;
        ball = FindObjectOfType<Ball>();
        Player[] foundPlayers = FindObjectsOfType<Player>();
        playerA = foundPlayers[0];
        playerB = foundPlayers[1];
    }

    private void FixedUpdate()
    {
        Countdown();
    }

    public void OnPlayerHit()
    {
        SceneManager.LoadScene("scn_DeflectBallArena");
    }

    private void Countdown()
    {
        if (startCountdown > 0)
        {
            startCountdown -= Time.fixedDeltaTime;
        }

        if(!started && startCountdown < 0)
        {
            started = true;

            ball.currentTarget = FindObjectsOfType<Player>()[Random.Range(0,2)];
        }

    }

    public void IncrementDeflectCount()
    {
        deflects++;
        deflectText.text = deflects.ToString() + "\nDEFLECTED";
    }
}