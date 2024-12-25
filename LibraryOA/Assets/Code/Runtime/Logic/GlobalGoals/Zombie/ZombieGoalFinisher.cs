using System.Collections.Generic;
using Code.Runtime.Logic.GlobalGoals.RocketStart;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.Zombie
{
    internal sealed class ZombieGoalFinisher : MonoBehaviour
    {
        [SerializeField]
        private List<Ladder> _cageLadders;
        
        public void Finish()
        {
            _cageLadders.ForEach(ladder => ladder.Throw());
        }
    }
}