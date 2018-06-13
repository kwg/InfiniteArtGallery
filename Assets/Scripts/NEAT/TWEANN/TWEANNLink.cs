using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Synaptic link between neurons
/// </summary>
public class TWEANNLink  {

    TWEANNNode target;
    float weight;
    long innovation;
    bool frozen;
    bool recurrent;


    /// <summary>
    /// Make a new link to target node
    /// </summary>
    /// <param name="target">TWEANNNode that link leads to</param>
    /// <param name="weight">Synaptic weight</param>
    /// <param name="innovation">Innovation number of the link</param>
    /// <param name="recurrent">Whether the link is recurrent</param>
    /// <param name="frozen">Whenter the link is frozen</param>
    public TWEANNLink(TWEANNNode target, float weight, long innovation, bool recurrent, bool frozen)
    {
        if (target != null) this.target = target;
        else throw new System.ArgumentException("Target can not be null");

        this.weight = weight;
        this.innovation = innovation;
        this.recurrent = recurrent;
        this.frozen = frozen;
    }

    /// <summary>
    /// Propgate signal along the link, adjusting it with the weight
    /// </summary>
    /// <param name="signal">Signal to propagate</param>
    public void Transmit(float signal)
    {
        //TODO Sanity checks
        //Debug.Log("transmit to " + target.GetInnovationID() + " : " + target.GetSum() + " += receiving " + signal + "*"+ weight);
        target.SetSum(target.GetSum() + (signal * weight));
        //Debug.Log("new " + target.GetInnovationID() + " sum: " + target.GetSum());
        //Debug.Log("After Transmit along link with ID: " + innovationID + " ->  signal=" + signal + ", weight=" + weight + " , target(" + target.GetInnovationID() + ").sum=" + target.GetSum());

    }

    /// <summary>
    /// The target TWEANNNode of this link
    /// </summary>
    /// <returns>The TWEANNNode this link targets</returns>
    public TWEANNNode GetTarget()
    {
        return target;
    }

    /// <summary>
    /// Whether thislink is recurrent
    /// </summary>
    /// <returns>True if link is recurrent, false otherwise</returns>
    public bool IsRecurrent()
    {
        return recurrent;
    }

    public float GetWeight()
    {
        return weight;
    }

    public long GetInnovation()
    {
        return innovation;
    }

    public override string ToString()
    {
        string result = "";
        result += "(" + innovation + ":" + weight + ":" + target.GetInnovation() + ":"
                + (recurrent ? "recurrent" : "forward") + ")";
        return result;
    }

}
