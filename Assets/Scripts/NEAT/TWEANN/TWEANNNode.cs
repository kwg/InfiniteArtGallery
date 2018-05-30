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
        bool result = false;
        foreach(TWEANNLink link in outputs)
        {
           if(link.GetTarget().GetInnovationID() == targetInnovationID)
            {
                result = link.IsRecurrent();
            }
            else
            {
                throw new System.ArgumentException("The target innovationID " + targetInnovationID + " was not found in " + outputs);
            }
        }

        return result;
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

    public long GetInnovationID()
    {
        return innovationID;
    }


    public void Load(double input)
    {
        // TODO Sanity checks
        sum += input;
    }

    public double Output()
    {
        return activation;
    }

    public void ArtificiallySetActivation(double activation)
    {
        this.activation = activation;
    }

    public void Flush()
    {
        sum = bias;
        activation = 0.0;
    }

    public void Activate()
    {
        // TODO How do we want to handle the FTYPE -> finding the activation function? 
        //TODO add whatever method to find the activation function and use it

    }

    public void ActivateAndTransmit()
    {
        Activate();

        // Reset sum to original bias after activation
        sum = bias;

        foreach(TWEANNLink link in outputs)
        {
            link.Transmit(activation);
        }

    }

    public void Connect(TWEANNNode target, double weight, long innovationID, bool recurrent, bool frozen)
    {
        TWEANNLink link = new TWEANNLink(target, weight, innovationID, recurrent, frozen);
        outputs.Add(link);
    }

    public bool IsConnectedTo(long innovationID)
    {
        bool result = false;
        foreach(TWEANNLink link in outputs)
        {
            if(link.GetTarget().GetInnovationID() == innovationID)
            {
                result = true;
            }
        }

        return result;
    }




}
