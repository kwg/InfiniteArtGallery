using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputText : MonoBehaviour {

    public TextMesh onScreenText;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Text(string text)
    {
        onScreenText.text = text;
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(1);
        onScreenText.text = "";
    }
}
