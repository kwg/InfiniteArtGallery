using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculpturePlatform : MonoBehaviour, IUnityGeneticArtwork {

    public int PortalID { get; set; }
    public int DestinationID { get; set; }

    private Renderer _rend;
    private Sculpture _sculpture;
    private bool initialized;

    // Use this for initialization
    void Start () {

        _rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (initialized)
        {
	        if(_sculpture.NeedsRedraw)
            {
                gameObject.GetComponent<MeshFilter>().mesh = _sculpture.GetMesh();
            }
            if (_sculpture.Art.Mutated)
            {
                _sculpture.Art.Mutated = false;
                _sculpture.UpdateCPPNArt();
            }
        }
	}

    public void InitArtDisplay(GeneticArt art)
    {
        _sculpture = new Sculpture(art);
        _rend = gameObject.GetComponent<MeshRenderer>();
        initialized = true;
    }

    public void SetColor(Color newColor)
    {
        _rend.material.color = newColor;
    }

    /* IUnityGeneticArtwork */
    public void UpdateGeneratedArt()
    {
        _sculpture.UpdateCPPNArt();
        gameObject.GetComponent<MeshFilter>().mesh = _sculpture.GetMesh();
    }

    public GeneticArt GetGeneticArt()
    {
        return _sculpture.Art;
    }

    public bool SetGeneticArt(GeneticArt newGeneticArt)
    {
        _sculpture.Art = newGeneticArt;
        return true;
    }
}
