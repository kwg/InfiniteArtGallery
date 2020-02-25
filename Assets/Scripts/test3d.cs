using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test3d : MonoBehaviour
{
    public GameObject sculptureObject;


    // Start is called before the first frame update
    void Start()
    {
        ActivationFunctions.ActivateFunction(FTYPE.TANH);
        SculpturePlatform s = sculptureObject.GetComponent<SculpturePlatform>();
        s.InitArtDisplay(new GeneticArt());
        //art.SetParentUnityObject(p);
        //s.UpdateGeneratedArt();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

