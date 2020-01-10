using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2d : MonoBehaviour
{
    public GameObject portalObject;


    GeneticArt art;


    // Start is called before the first frame update
    void Start()
    {
        art = new Artwork();
        Portal p = portalObject.GetComponent<Portal>();
        p.InitializePortal();
        art.SetParentUnityObject(p);
        art.ReprocessArtwork();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
