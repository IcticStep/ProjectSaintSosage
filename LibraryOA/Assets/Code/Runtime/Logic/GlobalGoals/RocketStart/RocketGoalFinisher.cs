using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.RocketStart
{
    internal sealed class RocketGoalFinisher : MonoBehaviour
    {
        [SerializeField]
        private Rocket _rocket;
        
        public void Finish() =>
            FinishAsync()
                .Forget();

        private async UniTaskVoid FinishAsync() =>
            await _rocket.LaunchAsync();
    }
}