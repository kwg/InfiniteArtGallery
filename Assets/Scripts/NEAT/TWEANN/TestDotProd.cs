using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDotProd : MonoBehaviour {

    TWEANN dotProdTest;
    double[] inputs;

    public void Start()
    {
        dotProdTest = new TWEANN(2, 1, false, FTYPE.ID, 0);
        inputs = new double[] { 3, 5 };


        Debug.Log("Starting test using inputs");
        foreach(double d in inputs)
        {
            Debug.Log(d);    
        }

        double[] results = dotProdTest.Process(inputs);

        foreach(double sum in results)
        {
            Debug.Log("Ending test: result = " + sum);
        }
    }
}
