using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWEANNGenotype : INetworkGenotype<TWEANN>
{

    protected List<NodeGene> nodes;
    protected List<LinkGene> links;

    private int numInputs, numOutputs;
    private long ID; // FIXME need genetic history ID assignment


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



    }

    public TWEANNGenotype(TWEANN tweann)
    {
        numInputs = tweann.NumInputs();
        numOutputs = tweann.NumOutputs();

        nodes = new List<NodeGene>(tweann.GetNodes().Length);

        for(int i = 0; i < nodes.Count; i++)
        {
            TWEANNNode n = tweann.GetNodes()[i];
            NodeGene ng = new NodeGene(n.GetNType(), n.GetFType(), n.GetInnovationID());
            nodes.Add(ng);
            List<LinkGene> tempLinks = new List<LinkGene>();
            foreach(TWEANNLink l in n.GetOutputs())
            {
                LinkGene lg = new LinkGene(n.GetInnovationID(), l.GetTarget().GetInnovationID(), l.GetWeight(), l.GetInnovationID());
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


    // Mutate
    // TODO - mutate

    // nodes/links
    //    perturbLink(int linkIndex, double delta)
    //    perturbLink(LinkGene lg, double delta)
    //    setWeight()
    //    getLinkBetween()
    //    addLink()
    //    spliceNode()
    // ?  getNodeWithInnovationID()
    //    


    // Crossovers
    // TODO - crossover


    // Additional:
    //    getPhenotype()
    //    copy()
    //    newInstance()

}
