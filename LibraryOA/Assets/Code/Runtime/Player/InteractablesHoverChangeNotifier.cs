using System;
using Code.Runtime.Logic.Interactions;
using Code.Runtime.Services.Interactions;
using UnityEngine;

namespace Code.Runtime.Player
{
    internal sealed class InteractablesHoverChangeNotifier : MonoBehaviour
    {
        [SerializeField]
        private InteractablesScanner _interactablesScanner;

        private void Start()
        {
            _interactablesScanner.FocusedInteractable += OnInteractableFocused;
            _interactablesScanner.UnfocusedInteractable += OnInteractableUnfocused;
        }

        private void OnDestroy()
        {
            _interactablesScanner.FocusedInteractable -= OnInteractableFocused;
            _interactablesScanner.UnfocusedInteractable -= OnInteractableUnfocused;
        }

        private void OnInteractableFocused(Interactable interactable)
        {
            if(interactable is IHoverStartListener hoverStartListener)
                hoverStartListener.OnHoverStart();
        }

        private void OnInteractableUnfocused(Interactable interactable)
        {
            if(interactable is IHoverEndListener hoverEndListener)
                hoverEndListener.OnHoverEnd();
        }
    }
}