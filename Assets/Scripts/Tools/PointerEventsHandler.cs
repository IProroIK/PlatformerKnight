using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public class PointerEventsHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Action OnDownEvent;
        public Action OnUpEvent;
        public Action OnEnterEvent;
        public Action OnExitEvent;

        public void OnPointerDown(PointerEventData eventData) => OnDownEvent?.Invoke();
        public void OnPointerUp(PointerEventData eventData) => OnUpEvent?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => OnEnterEvent?.Invoke();
        public void OnPointerExit(PointerEventData eventData) => OnExitEvent?.Invoke();
    }
}