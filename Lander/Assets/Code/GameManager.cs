using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private int score;
        private float time = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Lander.Instance.OnCoinPickup += PlayerOnOnCoinPickup;
            Lander.Instance.OnLanding += InstanceOnOnLanding;
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        private void InstanceOnOnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            score += args.Score;
            Debug.Log("Current score: " + score);
        }

        private void PlayerOnOnCoinPickup(int coinWorth)
        {
            score += coinWorth;
            Debug.Log("Current score: " + score);
        }

        public int GetScore()
        {
            return score;
        }

        public float GetTime()
        {
            return time;
        }
    }
}