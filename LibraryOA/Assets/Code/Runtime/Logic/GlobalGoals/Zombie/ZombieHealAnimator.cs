using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.Zombie
{
    internal sealed class ZombieHealAnimator : MonoBehaviour
    {
        private static readonly int Healing = Animator.StringToHash("Healing");
        
        [SerializeField]
        private Animator _animator;

        public void PlayHealAnimation() =>
            _animator.SetTrigger(Healing);
    }
}