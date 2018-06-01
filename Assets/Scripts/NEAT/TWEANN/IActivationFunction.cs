using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivationFunction {

    double Function(double x);
    string Name();

}
