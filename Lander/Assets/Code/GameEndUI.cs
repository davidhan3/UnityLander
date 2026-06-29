using TMPro;
using UnityEngine;

namespace Code
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statsText;

        private void Start()
        {
            Lander.Instance.OnLanding += Lander_OnLanding;
            gameObject.SetActive(false);
        }

        private void Lander_OnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            Debug.Log("On Landing triggered");
            if (args.LandingType == Lander.LandingType.Success)
            {
                titleText.text = "Successful Landing!";
            }
            else
            {
                titleText.text = "<color=#FF0000> CRASHED!</color>";
            }

            var landingSpeed = Mathf.Round(args.landingSpeed) * 10;
            var landingAngle = Mathf.Round(args.landingAngle * 100);
            var finalScore = GameManager.Instance.GetScore() + args.Score;

            statsText.text = landingSpeed + "\n" +
                             landingAngle + "\n" +
                             "x" + args.scoreMultiplier + "\n" +
                             finalScore;

            gameObject.SetActive(true);
        }
    }
}