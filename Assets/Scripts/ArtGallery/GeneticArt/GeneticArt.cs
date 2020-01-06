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
    protected IArtworkDisplay parentUnityObject;


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
    }

    public void SetParentUnityObject(IArtworkDisplay _parentUnityObject)
    {
        parentUnityObject = _parentUnityObject;
        ProcessGeno();
        parentUnityObject.UpdateGeneratedArt(adjustedCPPNOutput, spatialInputLimits);

        //cppnProcess = new Thread(() => ProcessGeno());
        //cppnProcess.Start();
    }

    private void SetCPPNSize()
    {
        int total = spatialInputLimits[0];
        if (spatialInputLimits[1] != 0) total *= spatialInputLimits[1];
        if (spatialInputLimits[2] != 0) total *= spatialInputLimits[2];
        cppnSize = total;
    }

    public void Mutate(GeneticArt _champion)
    {
        geno = _champion.GetGeno().Copy();

        // TODO detect if this is the champion and change how it mutates

        for(int m = 0; m < MUTATION_CYCLES; m++)
        {
            geno.Mutate();
        }

        cppnProcess = new Thread(() => ProcessGeno());
        cppnProcess.Start();
        parentUnityObject.UpdateGeneratedArt(adjustedCPPNOutput, spatialInputLimits);

    }

    private void ProcessGeno()
    {

        cppnOutput = genoProcesser.Process(geno, spatialInputLimits);
        adjustedCPPNOutput = colorChanger.AdjustColor(cppnOutput);
    }

    public void ReprocessArtwork()
    {
        cppnProcess = new Thread(() => ProcessGeno());
        cppnProcess.Start();
        parentUnityObject.UpdateGeneratedArt(adjustedCPPNOutput, spatialInputLimits);

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
        parentUnityObject.UpdateGeneratedArt(adjustedCPPNOutput, spatialInputLimits);

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