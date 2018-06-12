using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANNGenotype : INetworkGenotype<TWEANN>
{
    protected List<NodeGene> nodes;
    protected List<LinkGene> links;

    private int numInputs, numOutputs;
    private long ID; // FIXME need genetic history ID assignment
    private int archetypeIndex;


    public TWEANNGenotype(TWEANNGenotype copy) : this(copy.nodes, copy.links) { }

    public TWEANNGenotype(List<NodeGene> nodes, List<LinkGene> links)
    {
        this.nodes = nodes;
        this.links = links;

        numInputs = 0;
        numOutputs = 0;

        foreach(NodeGene n in nodes)
        {
            if(n.nType == NTYPE.INPUT) { numInputs++; }
            else if(n.nType == NTYPE.OUTPUT) { numOutputs++; }
        }

        //HACK this needs to come from EvolutionaryHistory
        archetypeIndex = 0;

    }

    public TWEANNGenotype(int numInputs, int numOutputs, int archetypeIndex): 
        this(new TWEANN (numInputs, numOutputs, false, FTYPE.ID,archetypeIndex))
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

            NodeGene ng = new NodeGene(tweannNodeList[i].GetNType(), tweannNodeList[i].GetFType(), tweannNodeList[i].GetInnovationID());
            nodes.Add(ng);
            List<LinkGene> tempLinks = new List<LinkGene>();
            foreach(TWEANNLink l in tweannNodeList[i].GetOutputs())
            {
                LinkGene lg = new LinkGene(tweannNodeList[i].GetInnovationID(), l.GetTarget().GetInnovationID(), l.GetWeight(), l.GetInnovationID());
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
        for (int i = 0; i < genes.Count; i++)
        {
            if(genes[i].GetInnovation() == innovation)
            {
                result = i;
            }
            else
            {
               // throw new System.ArgumentException("Innovation " + innovation + " not found in net (TODO: identity of net)"); // TODO add ident to error msg
            }
        }

        return result;
    }

    public string toString<T>(List<T> genes) where T : Gene
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
    // TODO - mutate
    public void LinkMutation()
    {
        LinkMutation(GetRandomLinkInnovation(), Random.Range(-1.0f, 1.0f));
    }

    public void LinkMutation(long source, float weight)
    {

    }

    private long GetRandomLinkInnovation()
    {
        return nodes[Random.Range(0, nodes.Count - 1)].GetInnovation();
    }

    // nodes/links
    //    perturbLink(int linkIndex, double delta)
    public void PerturbLink(int linkIndex, double delta)
    {
        LinkGene lg = links[linkIndex];
        PerturbLink(lg, delta);
    }

    //    perturbLink(LinkGene lg, double delta)
    public void PerturbLink(LinkGene lg, double delta)
    {
        lg.SetWeight(lg.GetWeight() + delta);
    }

    //    setWeight()
    /* Done in LinkGene */

    //    getLinkBetween()
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

    //    addLink()
    public void AddLink(long sourceInnovation, long targetInnovation, double weight, long innovation)
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

    //    spliceNode()
    public void SpliceNode(FTYPE fType, long newNodeInnovation, long sourceInnovation, long targetInnovation,
        double weight1, double weight2, long toLinkInnovation, long fromLinkInnovation)
    {
        NodeGene ng = new NodeGene(NTYPE.HIDDEN, fType, newNodeInnovation);
        LinkGene lg = GetLinkBetween(sourceInnovation, targetInnovation);
        //lg.SetActive(false); // TODO active bool is not in use
        // HACK Links should not be removed when splicing in a new node, but active is not in use yet
        links.Remove(lg);

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

    // ?  getNodeWithInnovationID()
    public NodeGene GetNodeByInnovationID(long innovation)
    {
        NodeGene result = null;
        foreach(NodeGene ng in nodes)
        {
            if(ng.GetInnovation() == innovation)
            {
                result = ng;
            }
            else
            {
                //throw new System.ArgumentException("Node innovation not found: " + innovation);
            }
        }

        return result;
    }   


    // Crossovers
    // TODO - crossover


    // Additional:
    //    getPhenotype()

    public TWEANN GetPhenotype()
    {
        TWEANN result = new TWEANN(this);
        return result;
    }
    //    copy()
    //    newInstance()

}
