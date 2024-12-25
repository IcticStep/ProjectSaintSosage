using System.Collections.Generic;
using Code.Runtime.Logic.GlobalGoals.RocketStart;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.Zombie
{
    internal sealed class ZombieGoalFinisher : MonoBehaviour, IGoalFinisher
    {
        [SerializeField]
        private List<Ladder> _cageLadders;
        
        [SerializeField]
        private ZombieHealAnimator _zombieAnimator;
        
        [SerializeField]
        private GameObject _zombie;
        
        [SerializeField]
        private GameObject _human;

        [SerializeField]
        private float _healingAwaitDuration;

        [SerializeField]
        private float _finishDelay;
        
        public async UniTask FinishAsync()
        {
            _cageLadders.ForEach(ladder => ladder.Throw());
            _zombieAnimator.PlayHealAnimation();
            await UniTask.WaitForSeconds(_healingAwaitDuration);
            
            _zombie.SetActive(false);
            _human.SetActive(true);
            await UniTask.WaitForSeconds(_finishDelay);
        }
    }
}