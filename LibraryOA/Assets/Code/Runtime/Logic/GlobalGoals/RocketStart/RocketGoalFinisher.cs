using Code.Runtime.Infrastructure.Services.Camera;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Logic.GlobalGoals.RocketStart
{
    internal sealed class RocketGoalFinisher : MonoBehaviour, IGoalFinisher
    {
        [SerializeField]
        private Rocket _rocket;
        private ICameraProvider _cameraProvider;

        [Inject]
        private void Construct(ICameraProvider cameraProvider) =>
            _cameraProvider = cameraProvider;

        public void Finish() =>
            FinishAsync()
                .Forget();

        public async UniTask FinishAsync()
        {
            _cameraProvider.StartLookingAfter(_rocket.CameraTargetOnFly);
            await _rocket.LaunchAsync();
        }
    }
}