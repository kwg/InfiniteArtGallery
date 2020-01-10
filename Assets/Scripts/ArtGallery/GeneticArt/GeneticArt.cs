using System.Threading;
using UnityEngine;

public abstract class GeneticArt
{
    private TWEANNGenotype geno;
    protected IGenoProcess genoProcesser;
    protected IColorChange colorChanger;
    protected float[][] cppnOutput;
    protected Color32[] adjustedCPPNOutput;
    protected int[] spatialInputLimits;
    protected int cppnSize;
    Thread cppnProcess;
    public bool NeedsRedraw { get; protected set; }


    private int MUTATION_CYCLES = 2; // TODO move maximum mutations per evolution to config file

    protected GeneticArt(TWEANNGenotype _geno, int[] _spatialInputLimits, IGenoProcess _processor)
    {
        geno = _geno;
        genoProcesser = _processor;
        spatialInputLimits = _spatialInputLimits;
        foreach (NodeGene node in geno.Nodes)
        {
            //node.fTYPE = ActivationFunctions.RandomFTYPE();
            node.fTYPE = ActivationFunctions.RandomFTYPE2();
        }

        SetCPPNSize();
        //cppnOutput = new float[cppnSize][];

        colorChanger = new ColorSpaceStandardRGB();
        ProcessGeno();
    }

    abstract protected void UpdateCPPNArt();

    private void SetCPPNSize()
    {
        int total = spatialInputLimits[0];
        if (spatialInputLimits[1] != 0) total *= spatialInputLimits[1];
        if (spatialInputLimits[2] != 0) total *= spatialInputLimits[2];
        cppnSize = total;
    }

    public void Mutate(GeneticArt _champion)
    {
        Debug.Log("Starting geno mutation...");

        geno = _champion.GetGeno().Copy();

        // TODO detect if this is the champion and change how it mutates

        for(int m = 0; m < MUTATION_CYCLES; m++)
        {
            geno.Mutate();
        }

        Debug.Log("Mutation complete ...");

        //cppnProcess = new Thread(() => ProcessGeno());
        //cppnProcess.Start();
        ProcessGeno();

    }

    private void ProcessGeno()
    {
        Debug.Log("Starting geno processing...");
        cppnOutput = genoProcesser.Process(geno, spatialInputLimits);
        Debug.Log("Geno processing complete. Starting color adjustment...");
        adjustedCPPNOutput = colorChanger.AdjustColor(cppnOutput);
        UpdateCPPNArt();
    }

    public void ReprocessArtwork()
    {
        //cppnProcess = new Thread(() => ProcessGeno());
        //cppnProcess.Start();
        ProcessGeno();
        
    }

    public Color32[] GetProcessedOutput()
    {
        return adjustedCPPNOutput;
    }

    public TWEANNGenotype GetGeno()
    {
        return geno;
    }

    public void ChangeColorSpace(IColorChange _colorChanger)
    {
        colorChanger = _colorChanger;
        adjustedCPPNOutput = colorChanger.AdjustColor(cppnOutput);

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