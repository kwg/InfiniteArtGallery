using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour {

    public Image display;
    public Sprite tutorial_00;
    public Sprite tutorial_01;
    public Sprite tutorial_02;
    public Sprite tutorial_03;
    public Sprite tutorial_04;
    int currentSlide = 0;
    Sprite[] sprites;
    float lockoutTime = 0.5f;
    float lastTime = 0.0f;

    // Use this for initialization
    void Start () {
        sprites = new Sprite[]
        {
            tutorial_00,
            tutorial_01,
            tutorial_02,
            tutorial_03,
            tutorial_04
        };
	}
	
	// Update is called once per frame
	void Update () {
		

        if((Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if(Time.time - lockoutTime > lastTime)
            {
                currentSlide++;
                if(currentSlide <= 4)
                {
                    display.sprite = sprites[currentSlide];
                }
                else
                {
                    SceneManager.LoadScene("RoomTest_02");
                }

                lastTime = Time.time;
            }
        }
	}
}
