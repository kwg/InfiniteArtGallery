using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour {

    ArrayList Connections;
    ArrayList Nodes;
    static int InnovationNumber;

	void Start () {
        Connections = new ArrayList();
        Nodes = new ArrayList();
        InnovationNumber = 0;

    }

    public void AddNodeGene(NodeGene nodeGene)
    {
        Nodes.Insert(nodeGene.GetID(), nodeGene);
    }

    public void AddConnectionGene(ConnectionGene connectionGene)
    {
        Connections.Add(connectionGene);
    }

    public ArrayList GetConnections()
    {
        return Connections;
    }

    public ArrayList GetNodes()
    {
        return Nodes;
    }

    public void AddConnectionMutation()
    {
        NodeGene Node1 = GetRandomNodeGene();
        NodeGene Node2 = GetRandomNodeGene();
        float Weight = Random.value;
        bool Reversed = false;
        bool ConnectionExists = false;

        if(Node1.GetNodeType() == NodeGene.TYPE.HIDDEN && Node2.GetNodeType() == NodeGene.TYPE.INPUT)
        {
            Reversed = true;
        }
        else if(Node1.GetNodeType() == NodeGene.TYPE.OUTPUT && Node2.GetNodeType() == NodeGene.TYPE.HIDDEN)
        {
            Reversed = true;
        }
        else if(Node1.GetNodeType() == NodeGene.TYPE.OUTPUT && Node2.GetNodeType() == NodeGene.TYPE.INPUT)
        {
            Reversed = true;
        }

        foreach (ConnectionGene cGene in Connections)
        {
            if(cGene.GetInNode() == Node1.GetID() && cGene.GetOutNode() == Node2.GetID())
            {
                ConnectionExists = true;
                break;
            }
            else if(cGene.GetInNode() == Node1.GetID() && cGene.GetOutNode() == Node2.GetID())
            {
                ConnectionExists = true;
                break;
            }
        }

        if (ConnectionExists)
        {
            return;
        }

        ConnectionGene connectionGene = new ConnectionGene();
        if (Reversed)
        {
            connectionGene.InitializeConnectionGene(Node2.GetID(),Node1.GetID(), Weight, true, InnovationNumber++);
        }
        else
        {
            connectionGene.InitializeConnectionGene(Node1.GetID(), Node2.GetID(), Weight, true, InnovationNumber++);
        }
        Connections.Add(connectionGene);

    }

    public void AddNodeMutation()
    {
        ConnectionGene connectionGene = GetRandomConnectionGene();
        NodeGene newNode = new NodeGene();
        ConnectionGene inbound = new ConnectionGene();
        ConnectionGene outbound = new ConnectionGene();
        NodeGene inNode = (NodeGene) Nodes[connectionGene.GetInNode()];
        NodeGene outNode = (NodeGene) Nodes[connectionGene.GetOutNode()];

        connectionGene.Disable();
 
        newNode.InitializeNodeGene(NodeGene.TYPE.HIDDEN, Nodes.Count);
        inbound.InitializeConnectionGene(inNode.GetID(), newNode.GetID(), 1f, true, inNode.GetInnovationNumber());
        outbound.InitializeConnectionGene(newNode.GetID(), newNode.GetID(), connectionGene.GetWeight(), true, outNode.GetInnovationNumber());

        Nodes.Add(newNode);
        Connections.Add(inbound);
        Connections.Add(outbound);

    }

    public static Genome Crossover(Genome moreFitParent, Genome lessFitParent)
    {
        Genome child = new Genome();

        foreach (NodeGene moreFitParentNode in moreFitParent.GetNodes())
        {
            if (lessFitParent.GetNodes().Contains(moreFitParentNode))
            {
                child.AddNodeGene(moreFitParentNode.CopyNodeGene());
            }
        }

        foreach (ConnectionGene moreFitParentConnection in moreFitParent.GetConnections())
        {
            if (lessFitParent.GetConnections().Contains(moreFitParentConnection.GetInnovationNumber()))
            {
                ConnectionGene childConnectionGene = new ConnectionGene();
                /* psudo random choice */
                if(Time.time % 2 == 0)
                {
                    childConnectionGene = moreFitParentConnection.CopyConnectionGene();
                }
                else
                {
                    ArrayList lessFitConnections = lessFitParent.GetConnections();
                    ConnectionGene temp = (ConnectionGene) lessFitConnections[moreFitParentConnection.GetInnovationNumber()];
                    childConnectionGene = temp.CopyConnectionGene();
                }
                child.AddConnectionGene(childConnectionGene);
            }
            else
            {
                ConnectionGene childConnectionGene = moreFitParentConnection.CopyConnectionGene();
                child.AddConnectionGene(childConnectionGene);
            }
        }

        return child;
    }

    private NodeGene GetRandomNodeGene()
    {
        return (NodeGene) Nodes[Random.Range(0, Nodes.Count - 1)];
    }

    private ConnectionGene GetRandomConnectionGene()
    {
        return (ConnectionGene)Connections[Random.Range(0, Connections.Count - 1)];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
