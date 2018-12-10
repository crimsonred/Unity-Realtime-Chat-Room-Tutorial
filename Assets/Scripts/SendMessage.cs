using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;

public class SendMessage : MonoBehaviour {

    public string username;
    public string chat;
    public Button SubmitButton;
    public InputField UsernameInput;
    public InputField TextInput;
    // Use this for initialization
    void Start()
    {
        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(SubmitText);
    }

    void SubmitText(){
        var usernametext = UsernameInput.text;
        var chattext = TextInput.text;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
