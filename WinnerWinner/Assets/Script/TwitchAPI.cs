using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using System;

public class TwitchAPI : MonoBehaviour
{
    static public TwitchAPI instance;
    public Api api;
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
        Application.runInBackground = true;
        api = new Api();
        api.Settings.AccessToken = Secrets.bot_access_token;
        api.Settings.ClientId = Secrets.client_id;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Client client = gameObject.GetComponent<TwitchClient>().client;
            api.Invoke(api.Undocumented.GetChattersAsync(client.JoinedChannels[0].Channel), GetChatterListCallback);
        }
    }
    private void GetChatterListCallback(List<ChatterFormatted> listofChatters)
    {
        Debug.Log("List of " + listofChatters.Count + "Viewers: ");
            foreach (var chatterobject in listofChatters)
        {
            Debug.Log(chatterobject.Username);
        }
    }
}
