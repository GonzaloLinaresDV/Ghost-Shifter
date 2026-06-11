using UnityEngine;



[CreateAssetMenu(menuName = "Loot/Loot Definition")]

public class LootDefinition : ScriptableObject
{
    public string itemName;
    public int weight;
    public int baseValue;
    public LootCondition[] Conditions;
}
[System.Serializable]
public class LootCondition
{
    public string conditionName;
    public float valueMultiplier;
}