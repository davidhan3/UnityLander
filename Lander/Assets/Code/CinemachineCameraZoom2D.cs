using Unity.Cinemachine;
using UnityEngine;

namespace Code
{
    public class CinemachineCameraZoom2D : MonoBehaviour
    {
        private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

        public static CinemachineCameraZoom2D Instance { get; private set; }

        [SerializeField] private CinemachineCamera cinemachineCamera;

        private float targetOrthographicSize = 10f;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            float zoomSpeed = 2f;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize,
                targetOrthographicSize, Time.deltaTime * zoomSpeed);
        }

        public void SetTargetOrthographicSize(float target)
        {
            targetOrthographicSize = target;
        }

        public void SetNormalOrthographicSize()
        {
            targetOrthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
        }
    }
}