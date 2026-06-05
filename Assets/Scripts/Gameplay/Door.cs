using Fusion;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Door : NetworkBehaviour, IInteractuable
{
    [Networked]
    public bool isOpen { get; set;  }

    private MeshRenderer _renderer;
    private Collider _collider;

    public override void Spawned()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void Highlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 1);
    }

    public void Interact(PlayerController playerController)
    {
        if (HasStateAuthority)
        {
            RequestOpen();
        }
        else
        {
            RPC_RequestOpen();
        }
    }
    public override void Render()
    {
        _renderer.enabled = !isOpen;
        //_collider.enabled = !isOpen;
    }
    public void UnHighlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 0);
    }

    void RequestOpen()
    {
        isOpen = !isOpen;
    }
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_RequestOpen()
    {
        RequestOpen();
    }
}
