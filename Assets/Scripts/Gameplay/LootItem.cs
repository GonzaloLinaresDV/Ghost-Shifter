using Fusion;
using Mono.Cecil;
using UnityEngine;

public class LootItem : NetworkBehaviour, ILooteable, IInteractuable
{
    [SerializeField]private LootDefinition _definition;

    private MeshRenderer _meshRenderer;

    [Networked]
    public byte ConditionIndex { get; set; }
    public string ItemName => _definition.itemName;
    public float Weight => _definition.weight;
    public int Value => _definition.baseValue;

    public LootCondition condition => _definition.Conditions[ConditionIndex];
    public string displayName=> $"{ItemName} ({condition.conditionName})";

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    override public void Spawned()
    {
        if (!Object.HasStateAuthority) return;

        ConditionIndex = (byte)Random.Range(0, _definition.Conditions.Length);
    }



 
    public int CalculateSellValue()
    {
        return Mathf.RoundToInt(_definition.baseValue * condition.valueMultiplier);
    }

    public void Interact(PlayerController playerController)
    {
        throw new System.NotImplementedException();
    }

    public void Highlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 1);
    }

    public void UnHighlight()
    {
        GetComponent<MeshRenderer>().materials[1].SetFloat("_playerNear", 0);
    }
}
