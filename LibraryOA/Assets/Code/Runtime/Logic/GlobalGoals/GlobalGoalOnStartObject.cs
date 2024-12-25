using Code.Runtime.StaticData.GlobalGoals;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals
{
    public sealed class GlobalGoalOnStartObject : MonoBehaviour
    {
        [field: SerializeField]
        public GlobalGoal GlobalGoal { get; private set; }
    }
}