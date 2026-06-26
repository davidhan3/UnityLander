using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        private int score;

        private void Start()
        {
            Lander.Instance.OnCoinPickup += PlayerOnOnCoinPickup;
            Lander.Instance.OnLanding += InstanceOnOnLanding;
        }

        private void InstanceOnOnLanding(int landingScore)
        {
            score += landingScore;
            Debug.Log("Current score: " + score);
        }

        private void PlayerOnOnCoinPickup(int coinWorth)
        {
            score += coinWorth;
            Debug.Log("Current score: " + score);
        }
    }
}
