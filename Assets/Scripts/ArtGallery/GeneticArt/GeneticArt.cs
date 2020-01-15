using System.Threading;
using UnityEngine;

public abstract class GeneticArt
{
    private TWEANNGenotype _geno;
    protected IGenoProcess _genoProcesser;
    protected IColorChange _colorChanger;
    protected float[][] _cppnOutput;
    protected Color32[] _adjustedCPPNOutput;
    protected int[] _spatialInputLimits;
    protected int _cppnSize;
    private Thread _cppnProcess;
    public bool NeedsRedraw { get; protected set; }


    private int MUTATION_CYCLES = 2; // TODO move maximum mutations per evolution to config file

    protected GeneticArt(TWEANNGenotype _geno1, int[] _spatialInputLimits1, IGenoProcess _processor1)
    {
        _geno = _geno1;
        _genoProcesser = _processor1;
        _spatialInputLimits = _spatialInputLimits1;
        foreach (NodeGene node in _geno.Nodes)
        {
            //node.fTYPE = ActivationFunctions.RandomFTYPE();
            node.fTYPE = ActivationFunctions.RandomFTYPE2();
        }

        SetCPPNSize();
        //cppnOutput = new float[cppnSize][];

        _colorChanger = new ColorSpaceStandardRGB();
        ProcessGeno();
    }

    abstract protected void UpdateCPPNArt();

    private void SetCPPNSize()
    {
        int total = _spatialInputLimits[0];
        if (_spatialInputLimits[1] != 0) total *= _spatialInputLimits[1];
        if (_spatialInputLimits[2] != 0) total *= _spatialInputLimits[2];
        _cppnSize = total;
    }

    public void Mutate(GeneticArt _champion)
    {
        Debug.Log("Starting geno mutation...");

        _geno = _champion.GetGeno().Copy();

        // TODO detect if this is the champion and change how it mutates

        for(int m = 0; m < MUTATION_CYCLES; m++)
        {
            _geno.Mutate();
        }

        Debug.Log("Mutation complete ...");

        //cppnProcess = new Thread(() => ProcessGeno());
        //cppnProcess.Start();
        ProcessGeno();

    }

    private void ProcessGeno()
    {
        Debug.Log("Starting geno processing...");
        _cppnOutput = _genoProcesser.Process(_geno, _spatialInputLimits);
        Debug.Log("Geno processing complete. Starting color adjustment...");
        _adjustedCPPNOutput = _colorChanger.AdjustColor(_cppnOutput);
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
        return _adjustedCPPNOutput;
    }

    public TWEANNGenotype GetGeno()
    {
        return _geno;
    }

    public void ChangeColorSpace(IColorChange _colorChanger1)
    {
        _colorChanger = _colorChanger1;
        _adjustedCPPNOutput = _colorChanger.AdjustColor(_cppnOutput);

    }

    public void SetGenotype(TWEANNGenotype geno)
    {

        this._geno = geno;
    }

    public TWEANNGenotype GetGenotype()
    {
        return _geno;
    }
}