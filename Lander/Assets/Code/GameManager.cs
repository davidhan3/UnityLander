using System;
using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private int Score;
        private float GameTime;
        private bool GameActive;

        private void Awake()
        {
            Instance = this;
            GameActive = false;
        }

        private void Start()
        {
            Lander.Instance.OnCoinPickup += Player_OnCoinPickup;
            Lander.Instance.OnLanding += Player_OnLanding;
            Lander.Instance.OnStateChanged += Player_OnStateChange;
        }

        private void Player_OnStateChange(object sender, Lander.OnStateChangedArgs args)
        {
            switch (args.State)
            {
                case Lander.GameState.Playing:
                    GameActive = true;
                    break;
            }
        }

        private void Update()
        {
            if (GameActive)
            {
                GameTime += Time.deltaTime;
            }
        }

        private void Player_OnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            Score += args.Score;
        }

        private void Player_OnCoinPickup(int coinWorth)
        {
            Score += coinWorth;
        }

        public int GetScore()
        {
            return Score;
        }

        public float GetTime()
        {
            return GameTime;
        }
    }
}