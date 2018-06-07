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

        /* Quick test for dotProduct */ 
        double[] dotProdTestInputs = new double[] { 3, 5 };
        Debug.Log("Staring test: dotproduct using inputs 3, 5");
        double[] dotProdTestResults = new TWEANN(xorTest).Process(dotProdTestInputs);
        foreach (double sum in dotProdTestResults)
        {
            Debug.Log("Ending test: dotproduct = " + sum);
        }

        xorTest.SpliceNode(FTYPE.TANH, 202, -1, -3, 0, 1.6108399303873728, 100, 101);
        xorTest.SpliceNode(FTYPE.TANH, 991, -2, -3, 0.896084404385038, 0, 102, 103);
        xorTest.AddLink(-1, 991, -0.7074283523090363, 104);
        xorTest.AddLink(-2, 202, 0.8321696661686222, 105);
        xorTest.AddLink(-3, 991, -0.9123817190422137, 106);
        //xorTest.AddLink(991, 202, 1.6257777356280263, 107);

        // Set activation function of output node
        xorTest.GetNodeByInnovationID(-1).fType = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-2).fType = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-3).fType = FTYPE.TANH;
        // Set bias manually

        // Set weights manually
        xorTest.GetLinkBetween(-2, -3).SetWeight(0.0);
        xorTest.GetLinkBetween(-1, -3).SetWeight(1.1881892501298341);


        /* List all nodes to output to verify network */
        foreach (NodeGene ng in xorTest.GetNodes()) {
            Debug.Log(ng.ToString());
        }
        foreach (LinkGene lg in xorTest.GetLinks())
        {
            Debug.Log(lg.ToString());
        }

        /* XOR test */

        List<double[]> inputs = new List<double[]>();
        inputs.Add(new double[] { 0, 0 });
        inputs.Add(new double[] { 0, 1 });
        inputs.Add(new double[] { 1, 0 });
        inputs.Add(new double[] { 1, 1 });
        TWEANN XORNetwork = new TWEANN(xorTest);

        for(int test = 0; test < inputs.Count; test++)
        {
            string debugString = "Starting test using inputs ";
            foreach (double d in inputs[test])
            {
                debugString += d + ", ";
            }
            Debug.Log("." + debugString);

            double[] results = XORNetwork.Process(inputs[test]);
            foreach (double sum in results)
            {
               Debug.Log("Ending test: result = " + sum);
            }
        }
    }
}
