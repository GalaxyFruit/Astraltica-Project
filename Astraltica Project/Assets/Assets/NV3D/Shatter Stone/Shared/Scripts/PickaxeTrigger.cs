using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShatterStone

{

public class PickaxeTrigger : MonoBehaviour
{

bool hasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OreNode") && !hasEntered) //Checks Tag of collided object.
        {
            other.GetComponent<OreNode>()?.oreHit(); //Triggers Ore Hit on node.
            hasEntered = true;
        }
    }

    private void OnTriggerExit(Collider other) //Resets collision event after it has left.
    {
        hasEntered = false;
    }
}
}
