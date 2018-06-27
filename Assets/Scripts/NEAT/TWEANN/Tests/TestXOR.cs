using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestXOR : MonoBehaviour{


    TWEANNGenotype xorTest;
    float[] inputs;

    public void Start()
    {
        xorTest = new TWEANNGenotype(2, 1, 0);
        List<NodeGene> nodes = xorTest.Nodes;

        /* Quick test for dotProduct */
        float[] dotProdTestInputs = new float[] { 3, 5 };
        Debug.Log("Staring test: dotproduct using inputs 3, 5");
        float[] dotProdTestResults = new TWEANN(xorTest).Process(dotProdTestInputs);
        foreach (float sum in dotProdTestResults)
        {
            Debug.Log("Ending test: dotproduct = " + sum);
        }

        xorTest.SpliceNode(FTYPE.TANH, 2158, -1, -3, -0.3964445706032944f, -0.4269614531487551f, 100, 101);
        xorTest.AddLink(-1, -3, -0.5081867337293002f, 106);
        xorTest.SpliceNode(FTYPE.TANH, 150, -2, -3, -3.275181751309399f, -0.9183280870360113f, 102, 103);
        xorTest.AddLink(-1, 150, -2.202539496376981f, 104);
        xorTest.AddLink(-2, 2158, 0.9295958853236486f, 105);

        // Set activation function of output node
        xorTest.GetNodeByInnovationID(-1).fTYPE = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-2).fTYPE = FTYPE.TANH;
        xorTest.GetNodeByInnovationID(-3).fTYPE = FTYPE.TANH;
        // Set bias manually

        // Set weights manually



        /* List all nodes to output to verify network */
        foreach (NodeGene ng in xorTest.Nodes) {
            Debug.Log(ng.ToString());
        }
        foreach (LinkGene lg in xorTest.Links)
        {
            Debug.Log(lg.ToString());
        }

        /* XOR test */

        List<float[]> inputs = new List<float[]>();
        inputs.Add(new float[] { 0, 0 });
        inputs.Add(new float[] { 0, 1 });
        inputs.Add(new float[] { 1, 0 });
        inputs.Add(new float[] { 1, 1 });
        TWEANN XORNetwork = new TWEANN(xorTest);

        for(int test = 0; test < inputs.Count; test++)
        {
            Debug.Log("");
            string debugString = "Starting test using inputs ";
            foreach (float f in inputs[test])
            {
                debugString += f + ", ";
            }
            Debug.Log(debugString);

            float[] results = XORNetwork.Process(inputs[test]);
            foreach (float sum in results)
            {
               Debug.Log("Ending test: result = " + sum);
            }
        }
    }
}
