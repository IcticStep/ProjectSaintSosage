using System;
using Code.Runtime.StaticData.GlobalGoals;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Code.Runtime.Logic.GlobalGoals
{
    [RequireComponent(typeof(PlayableDirector))]
    public sealed class GlobalGoalDirector : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _director;
        
        [field: SerializeField]
        public GlobalGoal GlobalGoal { get; private set; }

        private IGoalFinisher _goalFinisher;

        private UniTaskCompletionSource _directorStopCompletionSource = new();

        private void OnValidate() =>
            _director ??= GetComponent<PlayableDirector>();

        private void Awake() =>
            _goalFinisher = GetComponent<IGoalFinisher>();

        private void Start() =>
            _director.stopped += OnDirectorStopped;

        private void OnDestroy() =>
            _director.stopped -= OnDirectorStopped;

        public async UniTask PlayFinishCutscene()
        {
            _director.Play();
            await _directorStopCompletionSource.Task;
            await _goalFinisher.FinishAsync();
        }

        private void OnDirectorStopped(PlayableDirector obj)
        {
            _directorStopCompletionSource.TrySetResult();
            _directorStopCompletionSource = new UniTaskCompletionSource();
        }
    }
}