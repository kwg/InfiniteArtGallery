using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionGene : MonoBehaviour {

    private int InNode;
    private int OutNode;
    private float Weight;
    private bool Expressed;
    private int InnovationNumber;

    public void InitializeConnectionGene(int inNode, int outNode, float weight, bool expressed, int innovationNumber)
    {
        InNode = inNode;
        OutNode = outNode;
        Weight = weight;
        Expressed = expressed;
        InnovationNumber = innovationNumber;
    }

    public int GetInNode()
    {
        return InNode;
    }

    public int GetOutNode()
    {
        return OutNode;
    }

    public float GetWeight()
    {
        return Weight;
    }

    public bool IsExpressed()
    {
        return Expressed;
    }

    public int GetInnovationNumber()
    {
        return InnovationNumber;
    }

    public void Disable()
    {
        Expressed = false;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ConnectionGene CopyConnectionGene()
    {
        ConnectionGene copy = new ConnectionGene();
        copy.InitializeConnectionGene(InNode, OutNode, Weight, Expressed, InnovationNumber);

        return copy;
    }
}
