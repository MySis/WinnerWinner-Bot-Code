using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using TwitchLib.Client.Events;
using UnityEngine.UI;

public class WinnerScene : MonoBehaviour
{
    public float timer;
    private bool confirm = false;
    private bool winner;
    private GameManager gm;
    [SerializeField] GameObject rerun;

    // Start is called before the first frame update
    void Start()
    {
        winner = false;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (confirm)
        {
            if (!winner && timer < 0)
            {
                NoWinner();
            }
        }
    }
    public void ConfirmWinner(object sender, OnMessageReceivedArgs e)
    {
        Debug.Log("confrim: " + confirm + " winner: " + winner);
        if (confirm && !winner)
        {
            Debug.Log("name: " + e.ChatMessage.Username);
            if (e.ChatMessage.Username == gm.winnerName)
            {
                Debug.Log("send message");
                GameObject.Find("TwitchIRC").GetComponent<TwitchClient>().sendMess("Congratulations " + e.ChatMessage.Username + " you have won! Not only that, but you proved you're a real person too!");
                winner = true;
                confirm = false;
            }
        }
    }
public void endBattle()
    {
        GameObject.Find("TwitchIRC").GetComponent<TwitchClient>().sendMess("Congratulations " + gm.winnerName + "! You have won! Type in chat to redeem your prize!");
       timer = 45;
        confirm = true;
    }
    public void NoWinner()
    { 
        rerun.SetActive(true);
        confirm = false;
    }
}
