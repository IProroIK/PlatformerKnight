using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    public class Collision2DEvents : MonoBehaviour
    {
        public Action<Collision2D> OnCollisionEnter2DEvent;
        public Action<Collision2D> OnCollisionStay2DEvent;
        public Action<Collision2D> OnCollisionExit2DEvent;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnter2DEvent?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStay2DEvent?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExit2DEvent?.Invoke(collision);
        }
    }
}