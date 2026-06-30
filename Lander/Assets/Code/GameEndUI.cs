using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
        [SerializeField] private Button nextButton;

        private Action nextButtonClickAction;

        private void Awake()
        {
            nextButton.onClick.AddListener(() => { nextButtonClickAction(); });
        }

        private void Start()
        {
            Lander.Instance.OnLanding += Lander_OnLanding;
            gameObject.SetActive(false);
        }

        private void Lander_OnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            if (args.LandingType == Lander.LandingType.Success)
            {
                titleText.text = "Successful Landing!";
                nextButtonTextMesh.text = "Continue";
                nextButtonClickAction = GameManager.Instance.GoToNextLevel;
            }
            else
            {
                titleText.text = "<color=#FF0000> CRASHED!</color>";
                nextButtonTextMesh.text = "Retry";
                nextButtonClickAction = GameManager.Instance.RetryLevel;
            }

            var landingSpeed = Mathf.Round(args.LandingSpeed) * 10;
            var landingAngle = Mathf.Round(args.LandingAngle * 100);
            var finalScore = GameManager.Instance.GetScore() + args.Score;

            statsText.text = landingSpeed + "\n" +
                             landingAngle + "\n" +
                             "x" + args.ScoreMultiplier + "\n" +
                             finalScore;

            gameObject.SetActive(true);
        }
    }
}