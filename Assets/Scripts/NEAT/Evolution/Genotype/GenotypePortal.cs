using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenotypePortal : MonoBehaviour, IGenotype<ArrayList> {

    float r, g, b;

    public void RandomizeRGB()
    {
        SetRGB(Random.value, Random.value, Random.value);

    }

    public void SetRGB(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public void SetR(float r)
    {
        this.r = r;
    }

    public void SetG(float g)
    {
        this.g = g;
    }

    public void SetB(float b)
    {
        this.b = b;
    }

    public Color GetRGB()
    {
        return new Color(r, g, b);
    }

    public void AddParentID(long id)
    {
        throw new System.NotImplementedException();
    }

    public IGenotype<ArrayList> Copy()
    {
        throw new System.NotImplementedException();
    }



 /*
    public GenotypePortal Crossover(GenotypePortal g)
    {
        float rLerp, gLerp, bLerp;

        rLerp = Random.value;
        gLerp = Random.value;
        bLerp = Random.value;

        Color ChildColor = Color.Lerp(new Color(r, g, b), g.GetRGB());

        return null;
    }
*/

    public long GetID()
    {
        throw new System.NotImplementedException();
    }

    public List<long> GetParentIDs()
    {
        throw new System.NotImplementedException();
    }

    public ArrayList GetPhenotype()
    {
        throw new System.NotImplementedException();
    }

    public void Mutate()
    {
        throw new System.NotImplementedException();
    }

    public IGenotype<ArrayList> NewInstance()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /* This is an alternate - delete once the original is fixed */
    public IGenotype<ArrayList> Crossover(IGenotype<ArrayList> g)
    {
        throw new System.NotImplementedException();
    }
}
