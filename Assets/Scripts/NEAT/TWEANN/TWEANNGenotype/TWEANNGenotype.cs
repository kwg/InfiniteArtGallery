using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TWEANNGenotype : INetworkGenotype<TWEANN>
{
    [SerializeField] protected List<NodeGene> nodes;
    [SerializeField] protected List<LinkGene> links;

    [SerializeField] private int numInputs, numOutputs;
    [SerializeField] private long ID; // FIXME need genetic history ID assignment
    [SerializeField] private int archetypeIndex;

    // FIXME This should not be declared here. This should be a parameter so that we can adjust it at run time
    private float mutationChance = 1.0f; // percent chance for a mutation to occur

    public TWEANNGenotype() : this(0, 0, 0) { }

    public void LoadGenotype(List<NodeGene> nodes, List<LinkGene> links)
    {
        this.nodes = nodes;
        this.links = links;

        numInputs = 0;
        numOutputs = 0;

        foreach (NodeGene n in nodes)
        {
            if (n.nTYPE == NTYPE.INPUT) { numInputs++; }
            else if (n.nTYPE == NTYPE.OUTPUT) { numOutputs++; }
        }

        archetypeIndex = 0;
    }

    public TWEANNGenotype(TWEANNGenotype copy) : this(copy.nodes, copy.links) { }

    public TWEANNGenotype(List<NodeGene> nodes, List<LinkGene> links)
    {
        this.nodes = nodes;
        this.links = links;

        numInputs = 0;
        numOutputs = 0;

        foreach(NodeGene n in nodes)
        {
            if(n.nTYPE == NTYPE.INPUT) { numInputs++; }
            else if(n.nTYPE == NTYPE.OUTPUT) { numOutputs++; }
        }

        //HACK CROSSOVER: need archetype for matching genes

        archetypeIndex = 0;

    }

    public TWEANNGenotype(int numInputs, int numOutputs, int archetypeIndex): 
        this(new TWEANN (numInputs, numOutputs, false, FTYPE.ID, archetypeIndex))
    {
    }

    public TWEANNGenotype(int numInputs, int numOutputs, bool featureSelective, FTYPE fType, int archetypeIndex) :
    this(new TWEANN(numInputs, numOutputs, featureSelective, fType, archetypeIndex))
    {
    }

    public TWEANNGenotype(TWEANN tweann)
    {
        numInputs = tweann.NumInputs();
        numOutputs = tweann.NumOutputs();
        links = new List<LinkGene>();
        nodes = new List<NodeGene>(tweann.GetNodes().Count);

        List<TWEANNNode> tweannNodeList = tweann.GetNodes();

        for (int i = 0; i < tweann.GetNodes().Count; i++)
        {

            NodeGene ng = new NodeGene(tweannNodeList[i].GetNType(), tweannNodeList[i].GetFType(), tweannNodeList[i].GetInnovation());
            nodes.Add(ng);
            List<LinkGene> tempLinks = new List<LinkGene>();
            foreach(TWEANNLink l in tweannNodeList[i].GetOutputs())
            {
                LinkGene lg = new LinkGene(tweannNodeList[i].GetInnovation(), l.GetTarget().GetInnovation(), l.GetWeight(), l.GetInnovation());
                tempLinks.Add(lg);
            }
            for(int j = 0; j < tempLinks.Count; j++)
            {
                links.Add(tempLinks[j]);
            }
        }
        
    }


    public int[] GetModuleUsage()
    {
        throw new System.NotImplementedException();
    }

    public int NumberOfModules()
    {
        throw new System.NotImplementedException();
    }

    public void SetModuleUsage(int[] usage)
    {
        throw new System.NotImplementedException();
    }

    public int OutputStartIndex()
    {
        return nodes.Count - numOutputs;
    }

    public int IndexOfNodeInnovation(long innovation)
    {
        return IndexOfGeneInnovation(innovation, nodes);
    }

    public int IndexOfLinkInnovation(long innovation)
    {
        return IndexOfGeneInnovation(innovation, links);
    }

    public int IndexOfGeneInnovation<T>(long innovation, List<T> genes) where T : Gene
    {
        int result = -1;
        bool found = false;
        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i].GetInnovation() == innovation)
            {
                result = i;
                found = true;
                break;
            }

        }

        if (!found)
        {
            throw new System.ArgumentException("Innovation " + innovation + " not found in net (TODO: identity of net)"); // TODO add ident to error msg
        }

        return result;
    }

    public string ToString<T>(List<T> genes) where T : Gene  // FIXME ToString<T>(List<T> gene) this needs to be tied into the override ToString() later
    {
        string result = "";
        foreach(T gene in genes) {
            result += "node innovation: " + gene.GetInnovation() + " ";
        }
        return result;
    }

    public int GetArchetypeIndex()
    {
        return archetypeIndex;
    }

    public List<NodeGene> GetNodes()
    {
        return nodes;
    }

    public List<LinkGene> GetLinks()
    {
        return links;
    }
    
    // Mutate
    // TODO - mutate()

    public void Mutate()
    {
        float mutationRoll = Random.Range(0.0f, 1.0f);
        // TODO Mutate() - maybe multiple mutations can be activated with a single roll? (RPG chart style)
        float linkMutationRoll = Random.Range(0.0f, 1.0f);
        float spliceMutationRoll = Random.Range(0.0f, 1.0f);
        float perturbLinkMutationRoll = Random.Range(0.0f, 1.0f);



        if(mutationRoll < mutationChance)
        {
            int mutationType = Random.Range(0, 4);
            Debug.Log("Mutating... ");
            switch (mutationType)
            {
                case 0:
                    LinkMutation();
                    break;
                case 1:
                    SpliceMutation();
                    break;
                case 2:
                    PerturbLink(Random.Range(0, GetLinks().Count), RandomGenerator.NextGaussian());
                    break;
                case 3:
                    ActivationFunctionMutation();
                    break;
                default:
                    break;
            }


        }

    }



    public void LinkMutation()
    {
        long sourceNodeInnovation = GetLinkByInnovationID(GetRandomLinkInnovation()).GetSourceInnovation();
        //float weight = Random.Range(-1.0f, 1.0f) * 0.001f;
        float weight = RandomGenerator.NextGaussian();
        LinkMutation(sourceNodeInnovation, weight);
    }

    private void LinkMutation(long sourceNodeInnovation, float weight) //HACK LinkMutation(long sourceNodeInnovation, float weight) - recurrent links are possible. We may disable this later.
    {
        string debugMsg = "LinkMutation on link with innovation " + sourceNodeInnovation + " using a weight of " + weight;

        long targetInnovation = GetRandomNodeInnovation(sourceNodeInnovation, false);
        long link = EvolutionaryHistory.NextInnovationID();

        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

        AddLink(sourceNodeInnovation, targetInnovation, weight, link);
    }

    public void SpliceMutation()
    {
        //HACK just doing random ftypes for now
        SpliceMutation(ActivationFunctions.RandomFTYPE());
    }

    private void SpliceMutation(FTYPE fType) // TODO add a factor to change the weights impact (maybe a single factor or one for each weight)
    {
        LinkGene lg = GetLinkByInnovationID(GetRandomLinkInnovation());
        long sourceInnovation = lg.GetSourceInnovation();
        long targetInnovation = lg.GetTargetInnovation();
        long newNode = EvolutionaryHistory.NextInnovationID();
        float weight1 = RandomGenerator.NextGaussian();
        float weight2 = RandomGenerator.NextGaussian();
        long toLink = EvolutionaryHistory.NextInnovationID();
        long fromLink = EvolutionaryHistory.NextInnovationID();

        string debugMsg = "SpliceMutation between " + sourceInnovation + " and " + targetInnovation + ". Adding a node with an innovation of " + newNode + " and an activation function of " + fType;
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);
        SpliceNode(fType, newNode, sourceInnovation, targetInnovation, weight1, weight2, toLink, fromLink);
    }

    public void PerturbLink(int linkIndex, float delta)
    {
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("Perturbing link " + linkIndex + " by " + delta);
        LinkGene lg = links[linkIndex];
        PerturbLink(lg, delta);
    }

    public void PerturbLink(LinkGene lg, float delta)
    {
        lg.SetWeight(lg.GetWeight() + delta);
    }

    private void ActivationFunctionMutation()
    {
        int nodeRoll = Random.Range(0, nodes.Count);
        NodeGene ng = nodes[nodeRoll];
        if (ng == null) throw new System.Exception("Node not found! " + nodeRoll);
        FTYPE fTYPERoll = ActivationFunctions.RandomFTYPE();
        string debugMsg = "ActivationFunctionMutation - changing node " + nodeRoll + " from " + ng.GetFTYPE() + " to " + fTYPERoll;
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);
        ng.SetFTYPE(fTYPERoll);
    }

    private long GetRandomLinkInnovation()
    {
        return links[Random.Range(0, links.Count - 1)].GetInnovation();
    }

    private long GetRandomNodeInnovation(long sourceInnovation, bool includeInputs) // HACK this was made to be simple and is not fully featured
    {
        long result = -1;
        int startingInnovation;
        int endingInnovation = nodes.Count -1;

        if(includeInputs)
        {
            startingInnovation = 0;
        }
        else
        {
            startingInnovation = numInputs - 1;
        }

        result = nodes[Random.Range(startingInnovation, endingInnovation)].GetInnovation();

        return result;
    }

    public LinkGene GetLinkBetween(long sourceInnovation, long targetInnovation)
    {
        LinkGene result = null;
        if (links == null)
        {
            throw new System.Exception("links is null");
        }
        foreach(LinkGene lg in links)
        {
            if(lg.GetSourceInnovation() == sourceInnovation && lg.GetTargetInnovation() == targetInnovation)
            {
                result = lg;
            }
        }

        return result;
    }

    public void AddLink(long sourceInnovation, long targetInnovation, float weight, long innovation)
    {
        if(GetLinkBetween(sourceInnovation, targetInnovation) == null)
        {
            // TODO indexOfNodeinnovation(long innovation), but this is for recurrence
            //int target = IndexOfNodeInnovation(targetInnovation);
            //int source = IndexOfNodeInnovation(sourceInnovation);
            LinkGene lg = new LinkGene(sourceInnovation, targetInnovation, weight, innovation/*, target <= source*/);
            links.Add(lg);
        }
    }

    public void SpliceNode(FTYPE fType, long newNodeInnovation, long sourceInnovation, long targetInnovation,
        float weight1, float weight2, long toLinkInnovation, long fromLinkInnovation)
    {
        NodeGene ng = new NodeGene(NTYPE.HIDDEN, fType, newNodeInnovation);
        LinkGene lg = GetLinkBetween(sourceInnovation, targetInnovation);
        //lg.SetActive(false); // TODO active bool is not in use
        // HACK Links should not be removed when splicing in a new node, but active is not in use yet
        //links.Remove(lg);

        // HACK if this fails then it will be because the index is either < 0 or > count - add fixes or write a container w/ helper methods
        nodes.Insert(System.Math.Min(OutputStartIndex(), System.Math.Max(numInputs, IndexOfNodeInnovation(sourceInnovation) + 1)), ng);
        //int index = EvolutionaryHistory.IndexOfArchetypeInnovation(archetypeIndex, sourceInnovation);
        //int pos = System.Math.Min(EvolutionaryHistory.FirstArchetypeOutputIndex(archetypeIndex), System.Math.Max(numInputs, index + 1));
        //EvolutionaryHistory.AddArchetype(archetypeIndex, pos, ng.Clone(), "origin");
        LinkGene toNew = new LinkGene(sourceInnovation, newNodeInnovation, weight1, toLinkInnovation);
        LinkGene fromNew = new LinkGene(newNodeInnovation, targetInnovation, weight2, fromLinkInnovation);
        links.Add(toNew);
        links.Add(fromNew);
    }

    /// <summary>
    /// For debugging only
    /// </summary>
    public void RemoveLinkBetween(int sourceInnovation, int targetInnovation)
    {
        links.Remove(GetLinkBetween(sourceInnovation, targetInnovation));
    }

    public NodeGene GetNodeByInnovationID(long innovation)
    {
        NodeGene result = null;
        bool found = false;
        foreach(NodeGene ng in nodes)
        {
            if(ng.GetInnovation() == innovation)
            {
                result = ng;
            }
            else
            {
            }
        }
        if (!found)
        {
            throw new System.ArgumentException("Node innovation not found: " + innovation);
        }

        return result;
    }  
    
    public LinkGene GetLinkByInnovationID(long innovation)
    {
        LinkGene result = null;
        bool found = false;
        foreach(LinkGene lg in links)
        {
            if(lg.GetInnovation() == innovation)
            {
                result = lg;
                found = true;
                break;
            }

        }
        if (!found)
        {
            throw new System.ArgumentException("Link innovation not found: " + innovation);
        }

        return result;
    }

    public TWEANN GetPhenotype()
    {
        TWEANN result = new TWEANN(this);
        return result;
    }


    //    copy()
    public TWEANNGenotype Copy()
    {
        return new TWEANNGenotype(this);
    }
    //    newInstance()

    public override string ToString()
    {
        string result = "" + ID;
        //result += " (modules:" + numModules + ")";
        result += "\n" + nodes;
        result += "\n" + links;

        return result;
       
    }
}
