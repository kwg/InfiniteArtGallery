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

    /// <summary>
    /// New node with no target. Frozen by default
    /// </summary>
    /// <param name="fType">Activation function</param>
    /// <param name="nType">Node type</param>
    /// <param name="innovationID">Uniqu innovation number for node</param>
    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID) : this(fType, nType, innovationID, 0.0d) { }

    /// <summary>
    /// New node with no target. Frozen by default
    /// </summary>
    /// <param name="fType">Activation function</param>
    /// <param name="nType">Node type</param>
    /// <param name="innovationID">Unique innovation number for node</param>
    /// <param name="bias">Bias offset added to neuron sum before activation. Primarily needed by substrate networks from CPPNs</param>
    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID, double bias) : this(fType, nType, innovationID, false, bias) { }

    /// <summary>
    /// New node with no target. Frozen by default
    /// </summary>
    /// <param name="fType">Activation function</param>
    /// <param name="nType">Node type</param>
    /// <param name="innovationID">Uniqu innovation number for node</param>
    /// <param name="frozen">True if new link mutations cannot target this node</param>
    /// <param name="bias">Bias offset added to neuron sum before activation. Primarily needed by substrate networks from CPPNs</param>
    public TWEANNNode(FTYPE fType, NTYPE nType, long innovationID, bool frozen, double bias)
    {
        this.fType = fType;
        this.nType = nType;
        this.innovationID = innovationID;
        this.frozen = frozen;
        this.bias = bias;
        outputs = new List<TWEANNLink>();
        Flush();
    }

    /// <summary>
    /// Check given innovationID against all links for recurrence
    /// </summary>
    /// <param name="targetInnovationID">InnovationID of node to check for recurrence</param>
    /// <returns>True is link is recurrent, false otherwise</returns>
    /// <exception cref="System.ArgumentException">Thrown if target innovationID is not found in outputs</exception>
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

    public NTYPE GetNType()
    {
        return nType;
    }

    public FTYPE GetFType()
    {
        return fType;
    }

    public List<TWEANNLink> GetOutputs()
    {
        return outputs;
    }


    /// <summary>
    /// Set the value of sum
    /// </summary>
    /// <param name="newSum">New value for sum</param>
    public void SetSum(double newSum)
    {
        //TODO Sanity checks
        this.sum = newSum;
    }

    /// <summary>
    /// Current value of the sum
    /// </summary>
    /// <returns>Current value of sum</returns>
    public double GetSum()
    {
        return sum;
    }

    /// <summary>
    /// This node's innovationID
    /// </summary>
    /// <returns>innovationID</returns>
    public long GetInnovationID()
    {
        return innovationID;
    }

    /// <summary>
    /// An input is added to the sum in case it holds recurrent activation.
    /// </summary>
    /// <param name="input">Sensor input</param>
    public void Load(double input)
    {
        // TODO Sanity checks
        sum += input;
    }

    /// <summary>
    /// Get the activation
    /// </summary>
    /// <returns>Activation</returns>
    public double Output()
    {
        return activation;
    }

    /// <summary>
    /// Should only be used for testing purposes
    /// </summary>
    /// <param name="activation">A new activation</param>
    public void ArtificiallySetActivation(double activation)
    {
        this.activation = activation;
    }

    /// <summary>
    /// Used when the network enters a new environment and should no longer remember anything
    /// </summary>
    public void Flush()
    {
        sum = bias;
        activation = 0.0;
    }

    /// <summary>
    /// Internal activation function 
    /// </summary>
    private void Activate()
    {
        activation = ActivationFunctions.Activation(fType, sum);
        Debug.Log("Activation of " + GetInnovationID() + " is " + activation);
    }

    public void ActivateAndTransmit()
    {
        Activate();
        Debug.Log("Sum before reset of " + GetInnovationID() + " is " + sum);
        // Reset sum to original bias after activation
        sum = bias;

        foreach(TWEANNLink link in outputs)
        {
            
            link.Transmit(activation);
        }

    }

    /// <summary>
    /// Creates a new connection from this node to target node via a new link
    /// </summary>
    /// <param name="target">TWEANNNode to link to</param>
    /// <param name="weight">Synaptic weight between the nodes</param>
    /// <param name="innovationID">Innovation number of new link</param>
    /// <param name="recurrent">Whether or not the link is recurrent</param>
    /// <param name="frozen">Whether or not the link can be changed</param>
    public void Connect(TWEANNNode target, double weight, long innovationID, bool recurrent, bool frozen)
    {
        TWEANNLink link = new TWEANNLink(target, weight, innovationID, recurrent, frozen);
        //Debug.Log("Adding link from " + GetInnovationID() + " to " + target.GetInnovationID());
        outputs.Add(link);
    }

    /// <summary>
    /// Returns true if the node is connected to another node with the given innovation number
    /// </summary>
    /// <param name="innovationID">InnovationID of the target node to search for</param>
    /// <returns>True if node is connected, false otherwise</returns>
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

    public TWEANNLink GetLinkToTargetNode(TWEANNNode targetNode)
    {
        TWEANNLink result = null;
        foreach(TWEANNLink link in outputs)
        {
            if(link.GetTarget() == targetNode)
            {
                result = link;
            }
            else
            {
                throw new System.ArgumentException("No link to node found!");
            }
        }
        return result;
    }
}
