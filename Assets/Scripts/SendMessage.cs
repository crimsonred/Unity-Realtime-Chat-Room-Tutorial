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
    public Button SubmitButton;
    public InputField UsernameInput;
    public InputField TextInput;
    private Text text;
    // Use this for initialization
    void Start()
    {
        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-700d6386-17c7-4439-8d18-3472814914de";
        pnConfiguration.SubscribeKey = "sub-c-523850fa-5865-11e8-9796-063929a21258";

        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = Random.Range(0f, 999999f).ToString();

        pubnub = new PubNub(pnConfiguration);
        //Debug.Log(pnConfiguration.UUID);
    }

	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
    {
        MyClass myFireObject = new MyClass();
        myFireObject.username = UsernameInput.text;
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
