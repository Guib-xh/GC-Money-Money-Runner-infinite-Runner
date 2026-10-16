using UnityEngine;

namespace Loot
{
    [CreateAssetMenu(fileName = "SSO_LootItem", menuName = "SSO/SSO_LootItem")]
    public class SSO_LootItem : ScriptableObject
    {
        public string DisplayName;
        public int Value;
        public GameObject Prefab;
        [Range(0f, 1f)] public float SpawnWeight;
    }
}