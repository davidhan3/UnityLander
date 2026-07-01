using UnityEngine;

namespace Code
{
    public class GameLevel : MonoBehaviour
    {
        [SerializeField] private int levelNumber;
        [SerializeField] private Transform landerStartPosition;
        [SerializeField] private Transform cameraStartTarget;
        [SerializeField] private float zoomedOutOrthographicSize;

        public int GetLevelNumber()
        {
            return levelNumber;
        }

        public Transform GetCameraStartTarget()
        {
            return cameraStartTarget;
        }

        public Vector3 GetLanderStartPosition()
        {
            return landerStartPosition.position;
        }

        public float GetZoomedOutOrthographicSize()
        {
            return zoomedOutOrthographicSize;
        }
    }
}