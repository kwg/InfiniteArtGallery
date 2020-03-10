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
        for(int i = 0; i < 10; i++)
        {
            s.GetGeneticArt().Mutate();
        }
        //art.SetParentUnityObject(p);
        //s.UpdateGeneratedArt();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

