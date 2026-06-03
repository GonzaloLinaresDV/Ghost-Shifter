using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractuable
{
    
    public void Highlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 1);
    }

    public void Interact(PlayerController playerController)
    {
        Debug.Log("Toque puerta");
    }

    public void UnHighlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 0);
    }
}
