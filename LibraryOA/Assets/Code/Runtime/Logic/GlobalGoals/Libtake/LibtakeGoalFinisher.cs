using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Runtime.Logic.GlobalGoals.Libtake
{
    internal sealed class LibtakeGoalFinisher : MonoBehaviour, IGoalFinisher
    {
        [SerializeField]
        private GameObject _finalScreen;

        [SerializeField]
        private List<GameObject> _otherScreens;
        
        [SerializeField]
        private float _delayFinishTime;

        public async UniTask FinishAsync()
        {
            await UniTask.DelayFrame(1);
            _otherScreens.ForEach(screen => screen.SetActive(false));
            _finalScreen.SetActive(true);
            await UniTask.WaitForSeconds(_delayFinishTime);
        }
    }
}