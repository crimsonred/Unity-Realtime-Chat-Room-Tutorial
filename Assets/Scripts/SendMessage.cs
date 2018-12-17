using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using SimpleJSON;

public class MyClass
{
    public string username;
    public string text;
}

public class SendMessage : MonoBehaviour {
    public static PubNub pubnub;
    public string username;
    public string chat;
    public Font customFont;
    public Button SubmitButton;
    public Canvas canvasObject;
    public InputField UsernameInput;
    public InputField TextInput;
    private Text text;
    // Use this for initialization
    void Start()
    {
        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        Canvas canvas = canvasObject.GetComponent<Canvas>();

        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-9b7b20ed-a8a6-4c07-96b3-e406135df207";
        pnConfiguration.SubscribeKey = "sub-c-9e6ff722-023b-11e9-a39c-e60c31199fb2";

        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = Random.Range(0f, 999999f).ToString();

        pubnub = new PubNub(pnConfiguration);

        pubnub.FetchMessages()
            .Channels(new List<string> { "chatchannel" })
            .Async((result, status) => {
                if (status.Error)
                {
                    Debug.Log(string.Format(" FetchMessages Error: {0} {1} {2}", status.StatusCode, status.ErrorData, status.Category));
                }
                else
                {
                    Debug.Log(string.Format("In FetchMessages, result: "));//,result.EndTimetoken, result.Messages[0].ToString()));
                    foreach (KeyValuePair<string, List<PNMessageResult>> kvp in result.Channels)
                    {
                        Debug.Log("kvp channelname" + kvp.Key);

                        int counter = 0;
                        foreach (PNMessageResult pnMessageResult in kvp.Value)
                        {
                            JSONObject textJson = (JSONObject)JSON.Parse(pnMessageResult.Payload.ToString());
                            Debug.Log(textJson["username"]);

                            GameObject chatMessage = new GameObject("chatMessage");
                            chatMessage.transform.SetParent(canvas.transform);
                            chatMessage.AddComponent<Text>().text = string.Concat(textJson["username"], textJson["text"]);

                            var chatText = chatMessage.GetComponent<Text>();
                            chatText.font = customFont;
                            chatText.color = UnityEngine.Color.black;
                            chatText.fontSize = 35;
                            
                            RectTransform rectTransform;
                            rectTransform = chatText.GetComponent<RectTransform>();
                            rectTransform.localPosition = new Vector3(-10, 430 - counter, 0);
                            rectTransform.sizeDelta = new Vector2(650, 50);
                            
                            counter = counter + 50;
                            
                            Debug.Log("Channel: " + pnMessageResult.Channel);
                            Debug.Log("payload: " + pnMessageResult.Payload.ToString());
                            Debug.Log("timetoken: " + pnMessageResult.Timetoken.ToString());
                        }
                    }
                }
            });
        //Debug.Log(pnConfiguration.UUID);
    }

	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
    {
        MyClass myFireObject = new MyClass();
        myFireObject.username = string.Concat(UsernameInput.text, ": ");
        myFireObject.text = TextInput.text;
        string fireobject = JsonUtility.ToJson(myFireObject);
        Debug.Log("click");
        pubnub.Publish()
            .Channel("chatchannel")
            .Message(fireobject)
            .Async((result, status) =>
            {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
                else
                {
                    Debug.Log(string.Format("Fire Timetoken: {0}", result.Timetoken));
                }
            });
    }
}
