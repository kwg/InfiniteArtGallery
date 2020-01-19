using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculpturePlatform : MonoBehaviour, IUnityGeneticArtwork {

    private Renderer _rend;
    private Sculpture _sculpture;


    // Use this for initialization
    void Start () {

        _rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(_sculpture.NeedsRedraw)
        {
            gameObject.GetComponent<MeshFilter>().mesh = _sculpture.GetMesh();
        }	
	}

    public void InitArtDisplay(GeneticArt art)
    {
        _sculpture = new Sculpture(art);
        _rend = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetColor(Color newColor)
    {
        _rend.material.color = newColor;
    }

    /* IUnityGeneticArtwork */
    public void UpdateGeneratedArt()
    {
        throw new System.NotImplementedException();
    }

    public GeneticArt GetGeneticArt()
    {
        throw new System.NotImplementedException();
    }

    public bool SetGeneticArt(GeneticArt newGeneticArt)
    {

        return true;
    }
}
