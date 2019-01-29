using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TesterIDTextBox : MonoBehaviour {

    public GameObject HUD;

    private Text mText;
    private string testerID;


	// Use this for initialization
	void Start () {
        mText = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        mText.text = testerID;
	}

    public void SetTesterID(string testerID)
    {
        this.testerID = testerID;
    }
}
