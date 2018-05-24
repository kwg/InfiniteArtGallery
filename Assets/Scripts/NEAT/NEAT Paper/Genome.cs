using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour {

    ArrayList Connections;
    ArrayList Nodes;
    private static int InnovationNumber;

    /// <summary>
    /// Initialize the genome
    /// </summary>
	void Start () {
        Connections = new ArrayList();
        Nodes = new ArrayList();
        InnovationNumber = 0;

    }

    /// <summary>
    /// Utility method to get the next innovation number
    /// </summary>
    /// <returns>int Innovation number</returns>
    public int AssignInnovationnumber()
    {
        return ++InnovationNumber;
    }

    /// <summary>
    /// Add a node gene to the genome
    /// </summary>
    /// <param name="nodeGene">NodeGene to add to the genome</param>
    public void AddNodeGene(NodeGene nodeGene)
    {
        Nodes.Insert(nodeGene.GetID(), nodeGene);
    }

    /// <summary>
    /// Add a connection gene to the genome
    /// </summary>
    /// <param name="connectionGene">ConnectionGene to add to the genome</param>
    public void AddConnectionGene(ConnectionGene connectionGene)
    {
        Connections.Add(connectionGene);
    }

    /// <summary>
    /// Get list of all ConnectionGenes in the genome
    /// </summary>
    /// <returns>ArrayList of all ConnectionGenes in the genome</returns>
    public ArrayList GetConnections()
    {
        return Connections;
    }

    /// <summary>
    /// Get a list of all NodeGenes in the genome
    /// </summary>
    /// <returns>ArrayList of all NodeGenes in the genome</returns>
    public ArrayList GetNodes()
    {
        return Nodes;
    }

    /// <summary>
    /// Connection mutation method.
    /// <para>TODO: Clerify how this should be working and ensure proper implementation</para>
    /// </summary>
    public void AddConnectionMutation()
    {
        /* Select two nodes at random */
        NodeGene Node1 = GetRandomNodeGene();
        NodeGene Node2 = GetRandomNodeGene();
        /* Set a random weight */
        float Weight = Random.value;

        bool Reversed = false;
        bool ConnectionExists = false;
        bool ConnectionLegal = true;

        /* Keep direction proper */
        /* make sure Node1 comes before Node2 in the flow, if not then reverse the order of the nodes */
        /* case 1: Node1 is a HIDDEN node, but Node2 is an INPUT node */
        if(Node1.GetNodeType() == NodeGene.TYPE.HIDDEN && Node2.GetNodeType() == NodeGene.TYPE.INPUT)
        {
            Reversed = true;
        }
        /* case 2: Node1 is an OUTPUT node, but Node2 is a HIDDEN node or INPUT node*/
        else if(Node1.GetNodeType() == NodeGene.TYPE.OUTPUT && (Node2.GetNodeType() == NodeGene.TYPE.HIDDEN || Node2.GetNodeType() == NodeGene.TYPE.INPUT))
        {
            Reversed = true;
        }

        /* loop through all connections and make sure the 2 randomly selected nodes are not already connected */
        /* TODO although, would it really matter? if the point is a mutation, couldn't an exisiting connection be broken and remade? */
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

        /* Do not make connections for nodes of the same type */
        if(Node1.GetType() == Node2.GetType())
        {
            ConnectionLegal = false;
        }

        /* As long as all requirments are satisfied, make new ConnectionGene and add it */
        if(ConnectionLegal && !ConnectionExists)
        {
            ConnectionGene connectionGene = new ConnectionGene();

            /* Deal with reversed flow */
            if (Reversed)
            {
                connectionGene.InitializeConnectionGene(Node2.GetID(),Node1.GetID(), Weight, true, AssignInnovationnumber());
            }
            else
            {
                connectionGene.InitializeConnectionGene(Node1.GetID(), Node2.GetID(), Weight, true, AssignInnovationnumber());
            }

            /* Add new gene */
            Connections.Add(connectionGene);
        }

    }

    public void AddNodeMutation()
    {
        /* Select a random connection gene */
        ConnectionGene connectionGene = GetRandomConnectionGene();

        /* Make a new node */
        NodeGene newNode = new NodeGene();

        /* Make new connection genes to be used with new node */
        ConnectionGene inbound = new ConnectionGene();
        ConnectionGene outbound = new ConnectionGene();

        /* Get refs to the nodes feeding into and out of the selected gene */
        NodeGene inNode = (NodeGene) Nodes[connectionGene.GetInNode()];
        NodeGene outNode = (NodeGene) Nodes[connectionGene.GetOutNode()];

        /* Disable the exisiting connection */
        connectionGene.Disable();
 
        /* Add a new HIDDEN node */
        newNode.InitializeNodeGene(NodeGene.TYPE.HIDDEN, Nodes.Count);

        /* Build connections to the new node */
        inbound.InitializeConnectionGene(inNode.GetID(), newNode.GetID(), 1f, true, AssignInnovationnumber());
        outbound.InitializeConnectionGene(newNode.GetID(), newNode.GetID(), connectionGene.GetWeight(), true, AssignInnovationnumber());

        /* Add the new node to the genome */
        Nodes.Add(newNode);

        /* Add the new connections to the genome */
        Connections.Add(inbound);
        Connections.Add(outbound);

    }

    /// <summary>
    /// Mutates weights
    /// </summary>
    public void Mutate()
    {

    }

    public static Genome Crossover(Genome moreFitParent, Genome lessFitParent)
    {
        /* Make a new Genome */
        Genome child = new Genome();

        /* Nodes */
        /* for every node in the moreFitParent that exisits in the lessFitParent, use the moreFitParent's node */
        foreach (NodeGene moreFitParentNode in moreFitParent.GetNodes())
        {
            if (lessFitParent.GetNodes().Contains(moreFitParentNode))
            {
                child.AddNodeGene(moreFitParentNode.CopyNodeGene());
            }
        }

        /* Connections */
        foreach (ConnectionGene moreFitParentConnection in moreFitParent.GetConnections())
        {
            /* If both parents have matching genes, randomly select which parent */
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
                    ArrayList lessFitConnection = lessFitParent.GetConnections();
                    ConnectionGene temp = (ConnectionGene) lessFitConnection[moreFitParentConnection.GetInnovationNumber()];
                    childConnectionGene = temp;
                }

                child.AddConnectionGene(childConnectionGene);
            }
            /* Disjoint and excess genes */
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
