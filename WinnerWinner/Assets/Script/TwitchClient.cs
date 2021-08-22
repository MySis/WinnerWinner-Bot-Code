using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using TwitchLib.Client.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TwitchClient : MonoBehaviour
{
    static public TwitchClient instance;
    public Client client;
    public InfoHandler info;
    private string channel_name;
    // Start is called before the first frame update

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }

    }

    void Start()
    {
            channel_name = GameObject.Find("GameManager").GetComponent<GameManager>().channelChoice;
            Application.runInBackground = true;
            ConnectionCredentials credentials = new ConnectionCredentials("winnerwinnerbot", Secrets.bot_access_token);
            client = new Client();
            client.Initialize(credentials, channel_name);

            client.Connect();
        
            client.OnMessageReceived += MyMessageReceivedFunction;
    }
    private void Update()
    {
    }
    private void MyMessageReceivedFunction(object sender, OnMessageReceivedArgs e)
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene == 1)
        {
            InfoHandler info = GameObject.Find("InfoHandler").GetComponent<InfoHandler>();
            info.NewMessage(sender, e);
        }
        else
        {
            WinnerScene winner = GameObject.Find("SpawnPoints").GetComponent<WinnerScene>();
            winner.ConfirmWinner(sender, e);
        }

    }
    public void sendMess(string message)
    {
        client.SendMessage(client.JoinedChannels[0], message);
    }
}
