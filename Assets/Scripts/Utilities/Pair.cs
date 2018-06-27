using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<X, Y> {

    public X t1;
    public Y t2;

    public Pair(X t1, Y t2)
    {
        this.t1 = t1;
        this.t2 = t2;
    }


    public override string ToString()
    {
        return "( " + t1 + ", " + t2 + ")";
    }


}
