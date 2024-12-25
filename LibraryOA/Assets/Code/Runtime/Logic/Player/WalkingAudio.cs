using Code.Runtime.Data;
using Code.Runtime.Logic.Audio;
using UnityEngine;
using Zenject;

namespace Code.Runtime.Logic.Player
{
    internal sealed class WalkingAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] _stepsSounds;
        
        private AudioPlayer _audioPlayer;

        [Inject]
        private void Construct(AudioPlayer audioPlayer) =>
            _audioPlayer = audioPlayer;

        private void OnStep() =>
            _audioPlayer?.PlayUnimportantSfx(_stepsSounds.RandomElement());
    }
}