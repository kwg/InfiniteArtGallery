using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGene : MonoBehaviour {
    
    public enum TYPE
    {
        INPUT, HIDDEN, OUTPUT
    }

    private TYPE Type;
    private int ID;

    public void InitializeNodeGene(TYPE type, int id)
    {
        Type = type;
        ID = id;
    }

    public TYPE GetNodeType()
    {
        return Type;
    }

    public int GetID()
    {
        return ID;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public NodeGene CopyNodeGene()
    {
        NodeGene copy = new NodeGene();
        copy.InitializeNodeGene(Type, ID);

        return copy;
    }
}
