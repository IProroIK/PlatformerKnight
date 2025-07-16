using UnityEngine;

namespace Core.Service
{
    public class CameraService : ICameraService
    {
        private Camera _unityCamera;

        public CameraService()
        {
            _unityCamera = Camera.main;
        }

        public Camera UnityCamera => _unityCamera;

    }
}