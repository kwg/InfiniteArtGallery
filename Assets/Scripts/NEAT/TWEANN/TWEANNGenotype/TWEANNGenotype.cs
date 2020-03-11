using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TWEANNGenotype : INetworkGenotype<TWEANN>
{
    public List<NodeGene> Nodes { get; set; }
    public List<LinkGene> Links { get; set; }
    public long ID { get; private set; } // FIXME need genetic history ID assignment

    public int numInputs { get; set; }
    public int numOutputs { get; set; }
    public int archetypeIndex { get; set; }

    // FIXME This should not be declared here. This should be a parameter so that we can adjust it at run time
    private float mutationChance = 1.0f; // percent chance for a mutation to occur

    //public TWEANNGenotype() : this(0, 0, 0) { }

    public void LoadGenotype(List<NodeGene> nodes, List<LinkGene> links)
    {
        Nodes = nodes;
        Links = links;

        numInputs = 0;
        numOutputs = 0;

        foreach (NodeGene n in nodes)
        {
            if (n.nTYPE == NTYPE.INPUT) { numInputs++; }
            else if (n.nTYPE == NTYPE.OUTPUT) { numOutputs++; }
        }

        archetypeIndex = 0;
        ID = EvolutionaryHistory.NextGenotypeID();

    }

    public TWEANNGenotype(TWEANNGenotype copy) : this(copy.Nodes, copy.Links, copy.archetypeIndex) { }

    public TWEANNGenotype(List<NodeGene> nodes, List<LinkGene> links, int archetypeIndex)
    {
        Nodes = nodes;
        Links = links;

        numInputs = 0;
        numOutputs = 0;

        foreach(NodeGene n in nodes)
        {
            if(n.nTYPE == NTYPE.INPUT) { numInputs++; }
            else if(n.nTYPE == NTYPE.OUTPUT) { numOutputs++; }
        }

        this.archetypeIndex = archetypeIndex;
        ID = EvolutionaryHistory.NextGenotypeID();

    }

    public TWEANNGenotype(int numInputs, int numOutputs, int archetypeIndex) : this(new TWEANN (numInputs, numOutputs, false, FTYPE.ID, archetypeIndex)) { }

    public TWEANNGenotype(int numInputs, int numOutputs, bool featureSelective, FTYPE fType, int archetypeIndex) : this(new TWEANN(numInputs, numOutputs, featureSelective, fType, archetypeIndex)) { }

    public TWEANNGenotype(TWEANN tweann)
    {
        numInputs = tweann.NumInputs();
        numOutputs = tweann.NumOutputs();
        archetypeIndex = tweann.ArchetypeIndex;
        ID = EvolutionaryHistory.NextGenotypeID();

        Links = new List<LinkGene>();
        Nodes = new List<NodeGene>(tweann.GetNodes().Count);

        List<TWEANNNode> tweannNodeList = tweann.GetNodes();

        for (int i = 0; i < tweann.GetNodes().Count; i++)
        {

            NodeGene ng = new NodeGene(tweannNodeList[i].GetNType(), tweannNodeList[i].GetFType(), tweannNodeList[i].GetInnovation());
            Nodes.Add(ng);
            List<LinkGene> tempLinks = new List<LinkGene>();
            foreach(TWEANNLink l in tweannNodeList[i].GetOutputs())
            {
                LinkGene lg = new LinkGene(tweannNodeList[i].GetInnovation(), l.GetTarget().GetInnovation(), l.GetWeight(), l.GetInnovation());
                tempLinks.Add(lg);
            }
            for(int j = 0; j < tempLinks.Count; j++)
            {
                Links.Add(tempLinks[j]);
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
        return Nodes.Count - numOutputs;
    }

    public int IndexOfNodeInnovation(long innovation)
    {
        return IndexOfGeneInnovation(innovation, Nodes);
    }

    public int IndexOfLinkInnovation(long innovation)
    {
        return IndexOfGeneInnovation(innovation, Links);
    }

    public int IndexOfGeneInnovation<T>(long innovation, List<T> genes) where T : Gene
    {
        int result = -1;
        bool found = false;
        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i].Innovation == innovation)
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
            result += "node innovation: " + gene.Innovation + " ";
        }
        return result;
    }

    public int GetArchetypeIndex()
    {
        return archetypeIndex;
    }

  
    // Mutate

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
            switch (mutationType)
            {
                case 0:
                    LinkMutation();
                    break;
                case 1:
                    SpliceMutation();
                    break;
                case 2:
                    PerturbLinks(perturbLinkMutationRoll);
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
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);

        long targetNodeInnovation = GetRandomNodeInnovation(sourceNodeInnovation, false);

        NTYPE sourceNTYPE = GetNodeByInnovationID(sourceNodeInnovation).nTYPE;
        NTYPE targetNTYPE = GetNodeByInnovationID(targetNodeInnovation).nTYPE;


        if (
            /* (sourceNTYPE == NTYPE.INPUT && targetNTYPE == NTYPE.INPUT) ||    // both inputs */
            (sourceNTYPE == NTYPE.INPUT && targetNTYPE == NTYPE.HIDDEN) ||  // input -> hidden
            (sourceNTYPE == NTYPE.INPUT && targetNTYPE == NTYPE.OUTPUT) ||  // input -> output
            /* (sourceNTYPE == NTYPE.HIDDEN && targetNTYPE == NTYPE.HIDDEN) || // hidden -> hidden */
            (sourceNTYPE == NTYPE.HIDDEN && targetNTYPE == NTYPE.OUTPUT))   // hidden -> output
        {
            long link = EvolutionaryHistory.NextInnovationID();
            AddLink(sourceNodeInnovation, targetNodeInnovation, weight, link);
        }
    }

    public void SpliceMutation()
    {
        //HACK just doing random ftypes for now (selected from active functions) we may want this to optionally use the parent function

        SpliceMutation(ActivationFunctions.GetRandom());

        //ArtGallery ag = ArtGallery.GetArtGallery();
        //SpliceMutation(ag.GetRandomCollectedFunction());
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

    public void PerturbLinks(float cutoffValue)
    {
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("Perturbing " + Links.Count + " links: cutoffValue: " + cutoffValue);
        float delta;
        foreach(LinkGene lg in Links)
        {
            float roll = Random.Range(0.0f, 1.0f);
            if(roll < cutoffValue)
            {
                delta = RandomGenerator.NextGaussian();
                PerturbLink(lg, delta);
            }
        }
    }

    public void PerturbLink(LinkGene lg, float delta)
    {
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log("Perturbing link: " + lg.Innovation + " by " + delta);
        lg.SetWeight(lg.GetWeight() + delta); // TODO PerturbLink(LinkGene lg, float delta) - Do we want the weight to be bounded in any way?
    }

    private void ActivationFunctionMutation()
    {
        string debugMsg = "ActivationFunctionMutation";
        int nodeRoll = Random.Range(0, Nodes.Count);
        NodeGene ng = Nodes[nodeRoll];
        if (ng == null) throw new System.Exception("Node not found! " + nodeRoll);
        FTYPE fTYPERoll = ActivationFunctions.GetWeightedRandom();
        if(fTYPERoll != ng.fTYPE)
        {
            debugMsg += " - Changing node " + nodeRoll + " from " + ng.GetFTYPE() + " to " + fTYPERoll;
            ng.SetFTYPE(fTYPERoll);
        }
        else
        {
            debugMsg += " - Not changing node. Random node selection was same as previous function type";
        }
        if (ArtGallery.DEBUG_LEVEL > ArtGallery.DEBUG.NONE) Debug.Log(debugMsg);
    }

    private long GetRandomLinkInnovation()
    {
        return Links[Random.Range(0, Links.Count - 1)].Innovation;
    }

    private long GetRandomNodeInnovation(long sourceInnovation, bool includeInputs) // HACK this was made to be simple and is not fully featured
    {
        long result = -1;
        int startingInnovation;
        int endingInnovation = Nodes.Count -1;

        if(includeInputs)
        {
            startingInnovation = 0;
        }
        else
        {
            startingInnovation = numInputs - 1;
        }

        result = Nodes[Random.Range(startingInnovation, endingInnovation)].Innovation;

        return result;
    }

    public LinkGene GetLinkBetween(long sourceInnovation, long targetInnovation)
    {
        LinkGene result = null;
        if (Links == null)
        {
            throw new System.Exception("links is null");
        }
        foreach(LinkGene lg in Links)
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
            Links.Add(lg);
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
        Nodes.Insert(System.Math.Min(OutputStartIndex(), System.Math.Max(numInputs, IndexOfNodeInnovation(sourceInnovation) + 1)), ng);
        //int index = EvolutionaryHistory.IndexOfArchetypeInnovation(archetypeIndex, sourceInnovation);
        //int pos = System.Math.Min(EvolutionaryHistory.FirstArchetypeOutputIndex(archetypeIndex), System.Math.Max(numInputs, index + 1));
        EvolutionaryHistory.AddArchetype(archetypeIndex, Nodes.IndexOf(ng), ng.Clone(), "origin");
        LinkGene toNew = new LinkGene(sourceInnovation, newNodeInnovation, weight1, toLinkInnovation);  
        LinkGene fromNew = new LinkGene(newNodeInnovation, targetInnovation, weight2, fromLinkInnovation);
        Links.Add(toNew);
        Links.Add(fromNew);
    }

    /// <summary>
    /// For debugging only
    /// </summary>
    public void RemoveLinkBetween(int sourceInnovation, int targetInnovation)
    {
        Links.Remove(GetLinkBetween(sourceInnovation, targetInnovation));
    }

    public NodeGene GetNodeByInnovationID(long innovation)
    {
        NodeGene result = null;
        bool found = false;
        foreach(NodeGene ng in Nodes)
        {
            if(ng.Innovation == innovation)
            {
                result = ng;
                found = true;
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
        foreach(LinkGene lg in Links)
        {
            if(lg.Innovation == innovation)
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
        List<NodeGene> copyNodes = new List<NodeGene>();
        List<LinkGene> copyLinks = new List<LinkGene>();

        foreach(LinkGene lg in Links)
        {
            copyLinks.Add(new LinkGene(lg.GetSourceInnovation(), lg.GetTargetInnovation(), lg.GetWeight(), lg.Innovation));
        }

        foreach(NodeGene ng in Nodes)
        {
            copyNodes.Add(new NodeGene(ng.nTYPE, ng.fTYPE, ng.Innovation));
        }

       TWEANNGenotype copy = new TWEANNGenotype(copyNodes, copyLinks, archetypeIndex);
        return copy;
    }
    
    
    //    newInstance()

    public override string ToString()
    {
        string result = "" + ID;
        string nodesOut = "";
        foreach(NodeGene ng in Nodes)
        {
            nodesOut += ng.ToString() + "\n";
        }

        string linksOut = "";
        foreach (LinkGene lg in Links)
        {
            linksOut += lg.ToString() + "\n";
        }

        //result += " (modules:" + numModules + ")";
        result += "\n:NODES:\n" + nodesOut;
        result += "\n:LINKS:\n" + linksOut;

        return result;
       
    }
}
