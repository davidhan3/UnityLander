using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private List<GameLevel> gameLevelList;

        private static int currentLevelNumber = 1;

        private int score;
        private float gameTime;
        private bool gameActive;

        private void Awake()
        {
            Instance = this;
            gameActive = false;
        }

        private void Start()
        {
            Lander.Instance.OnCoinPickup += Player_OnCoinPickup;
            Lander.Instance.OnLanding += Player_OnLanding;
            Lander.Instance.OnStateChanged += Player_OnStateChange;

            LoadCurrentLevel();
        }

        private void LoadCurrentLevel()
        {
            foreach (var gameLevel in gameLevelList)
            {
                if (gameLevel.GetLevelNumber() == currentLevelNumber)
                {
                    var spawnedLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                    Lander.Instance.transform.position = spawnedLevel.GetLanderStartPosition();
                }
            }
        }

        private void Player_OnStateChange(object sender, Lander.OnStateChangedArgs args)
        {
            switch (args.State)
            {
                case Lander.GameState.Playing:
                    gameActive = true;
                    break;
            }
        }

        private void Update()
        {
            if (gameActive)
            {
                gameTime += Time.deltaTime;
            }
        }

        private void Player_OnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            score += args.Score;
        }

        private void Player_OnCoinPickup(int coinWorth)
        {
            score += coinWorth;
        }

        public void GoToNextLevel()
        {
            currentLevelNumber++;
            SceneManager.LoadScene(0);
        }

        public void RetryLevel()
        {
            SceneManager.LoadScene(0);
        }

        public int GetScore()
        {
            return score;
        }

        public float GetTime()
        {
            return gameTime;
        }

        public int GetLevelNumber()
        {
            return currentLevelNumber;
        }
    }
}