using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator {

    public static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);


        float result = v1 * s;
        return result ;
    }

    public static bool NextBool()
    {
        return Random.Range(-1.0f, 1.0f) > 0;
    }
}
