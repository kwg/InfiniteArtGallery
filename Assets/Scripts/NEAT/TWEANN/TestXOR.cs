using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestXOR : MonoBehaviour{


    TWEANNGenotype xorTest;
    double[] inputs;

    public void Start()
    {
        xorTest = new TWEANNGenotype(2, 1, 0);
        List<NodeGene> nodes = xorTest.GetNodes();

        TWEANN temp1 = new TWEANN(xorTest);
        double[] results1 = temp1.Process(new double[] { 3, 5 });
        foreach (double sum in results1)
        {
            Debug.Log("Ending test: dotproduct = " + sum);
        }

        xorTest.SpliceNode(FTYPE.TANH, -4, -1, -3, 1, 1, 100, 101);
        xorTest.SpliceNode(FTYPE.TANH, -5, -2, -3, -1, 1, 102, 103);
        xorTest.AddLink(-1, -5, -1, 104);
        xorTest.AddLink(-2, -4, 1, 105);
        foreach (NodeGene g in xorTest.GetNodes()) {
            Debug.Log("node in xor " g);
        }

        xorTest.GetNodeByInnovationID(-3).fType = FTYPE.TANH;

        inputs = new double[] { 1, 1 };
        Debug.Log("Starting test using inputs");
        foreach (double d in inputs)
        {
            Debug.Log(d);
        }

        TWEANN temp = new TWEANN(xorTest);
        double[] results = temp.Process(inputs);
        foreach (double sum in results)
        {
           Debug.Log("Ending test: result = " + sum);
        }
    }
}
