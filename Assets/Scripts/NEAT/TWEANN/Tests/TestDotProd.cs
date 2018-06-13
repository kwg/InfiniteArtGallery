using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDotProd : MonoBehaviour {

    TWEANN dotProdTest;
    float[] inputs;

    public void Start()
    {
        dotProdTest = new TWEANN(2, 1, false, FTYPE.ID, 0);
        inputs = new float[] { 3, 5 };


        Debug.Log("Starting test using inputs");
        foreach(float d in inputs)
        {
            Debug.Log(d);    
        }

        float[] results = dotProdTest.Process(inputs);

        foreach(float sum in results)
        {
            Debug.Log("Ending test: result = " + sum);
        }
    }
}
