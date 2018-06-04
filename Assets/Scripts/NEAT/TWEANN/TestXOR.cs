using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestXOR {

    TWEANN xorTest;
    double[] inputs;

    public void Start()
    {
        xorTest = new TWEANN(2, 1, false, FTYPE.SINE, 0);
        inputs = new double[] { 0, 1 };


        Debug.Log("Starting test using inputs");
        foreach (double d in inputs)
        {
            Debug.Log(d);
        }

        double[] results = xorTest.Process(inputs);

        foreach (double sum in results)
        {
            Debug.Log("Ending test: result = " + sum);
        }
    }



}
