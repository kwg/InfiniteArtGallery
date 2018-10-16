using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTextBox : MonoBehaviour {


    public GameObject HUD;

    private Text mText;
    private string counter;


    // Use this for initialization
    void Start()
    {
        mText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mText.text = counter;
    }

    public void SetCounterText(string counter)
    {
        this.counter = counter;
    }

    public void SetCounterTextColor(Color color)
    {
        mText.color = color;
    }
}
