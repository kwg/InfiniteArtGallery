using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public int portalID;
    public int destinationID;
    private Vector3 destination;

    public void Start()
    {

    }

    public void SetDestinationID(int newDestinationID)
    {
        destinationID = newDestinationID;
    }

    public void ActivatePortal(Player player)
    {
        ChangeColors();
        Teleport(player);
    }

    private void ChangeColors()
    {
        ColorChanger colorChanger = gameObject.GetComponent<ColorChanger>();
        colorChanger.EvolveColor(this);
    }

    private void Teleport(Player player)
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal.portalID == destinationID)
            {
                destination = portal.gameObject.transform.position; // set destination to exit portal position
            }
        }

        // Bump player to just outside of the portal collision box based on the location of the portal relative to the center
        if (destination.x < 0)
        {
            destination.x += 0.25f;
        }
        else
        {
            destination.x -= 0.25f;
        }

        if (destination.z < 0)
        {
            destination.z += 0.25f;
        }
        else
        {
            destination.z -= 0.25f;
        }

        destination.y -= 1.6f; // Fix exit height for player (player is 1.8 tall, portal is 5, center of portal is 2.5, center of player is 0.9. 2.5 - 0.9 = 1.6)

        player.transform.position = destination;
    }
}
