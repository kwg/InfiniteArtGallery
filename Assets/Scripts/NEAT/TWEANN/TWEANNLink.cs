using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANNLink  {

    TWEANNNode target;
    double weight;
    long innovationID;
    bool frozen;
    bool recurrent;


    public TWEANNLink(TWEANNNode target, double weight, long innovationID, bool frozen)
    {
        this.target = target;
        this.weight = weight;
        this.innovationID = innovationID;
        this.frozen = frozen;
    }


    public void transmit(double signal)
    {
        //TODO Sanity checks
        target.SetSum(target.GetSum() + (signal * weight)); 

    }

}
