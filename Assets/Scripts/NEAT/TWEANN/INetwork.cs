using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetwork {

    int NumInputs();
    int NumOutputs();

    void Flush();

}
