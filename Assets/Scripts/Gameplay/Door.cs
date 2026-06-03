using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject outline;
    
    public void Highlight()
    {
        outline.SetActive(true);
    }

    public void Interact(PlayerController playerController)
    {
        Debug.Log("Toque puerta");
    }

    public void UnHighlight()
    {
        outline.SetActive(false);
    }
}
