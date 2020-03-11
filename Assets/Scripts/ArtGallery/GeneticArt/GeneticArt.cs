using System.Threading;
using UnityEngine;

public class GeneticArt
{
    private TWEANNGenotype geno;
    private int MUTATION_CYCLES = 2; // TODO move maximum mutations per evolution to config file
    public bool Mutated { get; set; }

    public GeneticArt() : this(new TWEANNGenotype(8, 4, 0)) { }

    public GeneticArt(int archtypeIndex) : this(new TWEANNGenotype(8, 4, archtypeIndex)) { }

    public GeneticArt(TWEANNGenotype geno)
    {
        this.geno = geno;

        foreach (NodeGene node in this.geno.Nodes)
        {
            node.fTYPE = ActivationFunctions.GetWeightedRandom();
            //node.fTYPE = ActivationFunctions.RandomFTYPE2();
        }
    }

    public void Mutate(int cycles = 1)
    {
        if(cycles == 1)
        {

        }
        for(int m = 0; m < MUTATION_CYCLES; m++)
        {
            geno.Mutate();
        }

        Mutated = true;
    }

    public void SetGenotype(TWEANNGenotype geno)
    {

        this.geno = geno;
    }

    public TWEANNGenotype GetGenotype()
    {
        return geno;
    }
}