using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine.SceneManagement;
using TwitchLib.Client.Events;
using UnityEngine.UI;

public class InfoHandler : MonoBehaviour
{
    #region Validation
    private GameManager gm;
    public InputField validationWord;
    private bool Validated;
    public GameObject Validating;
    public GameObject Giveaway;
    public GameObject Timer;


    #endregion
    #region Gather Names
    public int count;
    public TwitchClient client;
    public List<string> participants;
    public List<Text> Parts;
    #endregion
    #region Customizable Text
    private string command;
    private string alertmessage;
    private bool message;
    public InputField MessageTime;
    public InputField DrawingTime;
    public InputField JoinCommand;
    public InputField AlertMessage;
    public Toggle subsT;
    public Toggle modsT;
    public Toggle submodT;
    public Toggle SubOnly;
    public Toggle ModOnly;
    public Toggle send;
    public Text timeText;
    public Text QuickPickName;
    #endregion
    #region Drawing
    public float drawtime;
    public float msgTime;
    private bool countdown;
    public bool quickpick;
    #endregion
    public GameObject warning;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Validated = gm.Valid;
        if (Validated)
        {
            Validating.SetActive(false);
            Giveaway.SetActive(true);
            Timer.SetActive(false);
        }
        else
        {
            Validating.SetActive(true);
            Giveaway.SetActive(false);
            Timer.SetActive(false);
        }
    }
    private void Start()
    {
        message = false;
        command = null;
        alertmessage = null;
        for (int i = 0; i < Parts.Count; i++)
            Parts[i].text = "";
        for (int i = 0; i < participants.Count; i++)
            participants[i] = "";
    }
    void FixedUpdate()
    {

        if (countdown)
        {

            drawtime -= Time.deltaTime;
            msgTime -= Time.deltaTime;

            if (msgTime <= 0 && message)
            {
                client.sendMess(alertmessage);
                msgTime = int.Parse(MessageTime.text) * 60;
            }

            if (drawtime > 0)
            {
                DisplayTime(drawtime);
            }

            else if (drawtime <= 0)
            {
                timeText.text = "0:00";
                countdown = false;
                client.sendMess("The entry period is over! Thank you for joining the giveaway.");
            }
        }        
    }
    #region Gather Names
    public void NewMessage(object sender, OnMessageReceivedArgs e)
    {
        if (!Validated)
        {
            quickpick = false;
            if (validationWord.text.Length >= 8)
                if (e.ChatMessage.Username.ToLower() == gm.channelChoice.ToLower())
                {
                    if (e.ChatMessage.Message.ToLower() == validationWord.text.ToLower())
                    {
                        Validated = true;
                        client.sendMess("Connected to Winner Winner Bot!");
                        Validating.SetActive(false);
                        Giveaway.SetActive(true);
                        gm.Valid = true;
                    }
                    else
                    {
                        client.sendMess("That is not the correct Validation word, please try again");
                        return;
                    }
                }
                else
                    return;
            else
                return;
        }
        int value = 0;
        if (countdown)
        {
            Debug.Log(e.ChatMessage.Message);
            quickpick = false;
            if (e.ChatMessage.Message == command)
            {
                Debug.Log("Matched command");
                string name = e.ChatMessage.Username;
               if (e.ChatMessage.UserType == TwitchLib.Client.Enums.UserType.Viewer)
                        value = 0;
                    else if (e.ChatMessage.UserType != TwitchLib.Client.Enums.UserType.Viewer)
                        value = 2;
                    else if (e.ChatMessage.IsSubscriber)
                        value = 1;
                gm.AddList(name, value);
                
                if (e.ChatMessage.Message == "!entries")
                {
                    for (int i = 0; i < Parts.Count; i++)
                    {
                        client.sendMess(participants[i]);
                    }
                }
            }
            if (quickpick)
            {
                if (e.ChatMessage.Username == gm.winnerName)
                {
                    client.sendMess("Congratulations " + e.ChatMessage.Username + " you have won! Not only that, but you proved you're a real person too!");
                    quickpick = false;
                }
            }
        }
    }
    public void DisplayList(string name)
        {
            int a = 0;
            if (count > 20)
                a = 1;
            else if (count > 40)
                a = 2;
            else if (count > 60)
                a = 3;

            if (count > 0)
            {
                for (int i = 0; i < gm.names.Count; i++)
                {
                    if (gm.names[i] == name)
                    {
                        return;
                    }
                }
                count = +1;
                participants[a] += name + "\n";
                Parts[a].text = participants[a];
                client.sendMess("Congratulations " + name + " you have joined the giveaway!");
            }
            else
            {
                count += 1;
                participants[0] += name + "\n";
                Parts[0].text = participants[0];
                client.sendMess("Congratulations " + name + " you have joined the giveaway!");
            }

        }
    #endregion
    public void SetCustoms()
    {
        if (send.isOn)
        {
            message = true;
            alertmessage = AlertMessage.text.ToString();            
            msgTime = 10;
        }
    }
    public void startdrawing()
    {
        count = 0;
        SetCustoms();
        drawtime = int.Parse(DrawingTime.text) * 60;
        command = JoinCommand.text;
        Giveaway.SetActive(false);
        Timer.SetActive(true);
        countdown = true;
    }
    public void endDrawing()
    {
        countdown = false;
        gm.Pickname();
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
    public void Battle (int scene)
    {
        if (participants.Count <= 1)
        {
            warning.SetActive(true);
            return;
        }
        else
        {
            gm.Pickname();
            SceneManager.LoadScene(scene);
        }
    }
}