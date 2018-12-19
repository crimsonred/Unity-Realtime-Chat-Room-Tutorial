using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using SimpleJSON;

public class pubnubscript : MonoBehaviour
{
    public static PubNub pubnub;
    public string username;
    public string chat;
    public InputField UsernameInput;
    public InputField TextInput;
    private Text text;
    public Canvas renderCanvas;

    public Text hello;
    // Use this for initialization
    protected void Start()
    {
        renderCanvas = GameObject.FindObjectOfType<Canvas>();
        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-700d6386-17c7-4439-8d18-3472814914de";
        pnConfiguration.SubscribeKey = "sub-c-523850fa-5865-11e8-9796-063929a21258";

        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        //pnConfiguration.UUID = Random.Range(0f, 999999f).ToString();

        pubnub = new PubNub(pnConfiguration);
        //Debug.Log(pnConfiguration.UUID);

        pubnub.Subscribe()
           .Channels(new List<string>() {
                "chatchannel"
           })
       .WithPresence()
       .Execute();

  
        int ct = 1;
        pubnub.SusbcribeCallback += (sender, e) =>
        {
            SusbcribeEventEventArgs message = e as SusbcribeEventEventArgs;
            if (message.Status != null)
            {
            }
            if (message.MessageResult != null)
            {
                Debug.Log(message.MessageResult.Payload);

            //    Debug.Log(message.MessageResult.Timetoken);
                Dictionary<string, object> playerJson = message.MessageResult.Payload as Dictionary<string, object>;
                if(playerJson.ContainsKey("username")){
                    Debug.Log(playerJson["username"]);
                    string usernameText = playerJson["username"].ToString();
                }
                if(playerJson.ContainsKey("text")){
                    string inputText = playerJson["text"].ToString();


                    Text tempTextBox = Instantiate(hello) as Text;
                    //Parent to the panel
                    tempTextBox.transform.SetParent(renderCanvas.transform, false);
                    //Set the text box's text element font size and style:
                    // tempTextBox.fontSize = 12;
                    //Set the text box's text element to the current textToDisplay:
                    tempTextBox.text = "text";
                }
               //JSONObject playerJson = (JSONObject)JSON.Parse(message.MessageResult.Payload.ToString());
            }
            if (message.PresenceEventResult != null)
            {
                Debug.Log("In Example, SusbcribeCallback in presence" + message.PresenceEventResult.Channel + message.PresenceEventResult.Occupancy + message.PresenceEventResult.Event);
            }
        };
    }

}
