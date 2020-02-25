using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2d : MonoBehaviour
{
    public GameObject portalObject;


    

    // Start is called before the first frame update
    void Start()
    {
        ActivationFunctions.ActivateFunction(FTYPE.TANH);
        Portal p = portalObject.GetComponent<Portal>();
        p.InitArtDisplay(new GeneticArt());
        //art.SetParentUnityObject(p);
        p.UpdateGeneratedArt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
