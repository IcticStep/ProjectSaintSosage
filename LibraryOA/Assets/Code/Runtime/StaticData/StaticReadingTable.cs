using UnityEngine;

namespace Code.Runtime.StaticData
{
    [CreateAssetMenu(fileName = "ReadingTableData", menuName = "Static data/Reading table", order = 0)]
    public class StaticReadingTable : ScriptableObject
    {
        [field: SerializeField, Range(0.1f, 10000f)]
        public float SecondsToRead { get; private set; } = 1f;
    }
}