using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Single node
/// </summary>
public class TWEANNNode {

	NTYPE nType;
    FTYPE fType;
    long innovationID;
    bool frozen;
    double bias;

    List<TWEANNLink> outputs;
    double sum;
    double activation;

    // TODO any other vars needed to create a graphical output goes here

    
    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID) : this(fType, nType, innovationID, 0.0d) { }

    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID, double bias) : this(fType, nType, innovationID, false, bias) { }

    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID, bool frozen, double bias) { }


    public bool IsLinkRecurrent(long targetInnovationID)
    {
        foreach(TWEANNLink link in outputs)
        {
           
        }


        return false;
    }

    public void SetSum(double newSum)
    {
        //TODO Sanity checks
        this.sum = newSum;
    }

    public double GetSum()
    {
        return sum;
    }


}
