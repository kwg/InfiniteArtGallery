using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TesterIDTextBox : MonoBehaviour {

    public GameObject HUD;

    private TextMeshProUGUI mText;
    private string testerID;


	// Use this for initialization
	void Start () {
        mText = gameObject.GetComponent<TextMeshProUGUI>();
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
