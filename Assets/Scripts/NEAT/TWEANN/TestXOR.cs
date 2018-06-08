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

        xorTest.SpliceNode(FTYPE.TANH, 2158, -1, -3, -0.3964445706032944, -0.4269614531487551, 100, 101);
        xorTest.AddLink(-1, -3, -0.5081867337293002, 106);
        xorTest.SpliceNode(FTYPE.TANH, 150, -2, -3, -3.275181751309399, -0.9183280870360113, 102, 103);
        xorTest.AddLink(-1, 150, -2.202539496376981, 104);
        xorTest.AddLink(-2, 2158, 0.9295958853236486, 105);

        // Set activation function of output node
        xorTest.GetNodeByInnovationID(-1).fType = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-2).fType = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-3).fType = FTYPE.TANH;
        // Set bias manually

        // Set weights manually



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
            Debug.Log("");
            string debugString = "Starting test using inputs ";
            foreach (double d in inputs[test])
            {
                debugString += d + ", ";
            }
            Debug.Log(debugString);

            double[] results = XORNetwork.Process(inputs[test]);
            foreach (double sum in results)
            {
               Debug.Log("Ending test: result = " + sum);
            }
        }
    }
}
