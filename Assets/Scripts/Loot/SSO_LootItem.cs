using UnityEngine;

namespace Loot
{
    [CreateAssetMenu(fileName = "SSO_LootItem", menuName = "SSO/Loot/SSO_LootItem")]
    public class SSO_LootItem : ScriptableObject
    {
        public string DisplayName;
        public int Value;
        public bool IsLootBox;
        public GameObject Prefab;
        [Range(0f, 1f)] public float SpawnWeight;
    }
}