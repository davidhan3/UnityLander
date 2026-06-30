using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statsMesh;
        [SerializeField] private GameObject speedLeftArrow;
        [SerializeField] private GameObject speedRightArrow;
        [SerializeField] private GameObject speedUpArrow;
        [SerializeField] private GameObject speedDownArrow;
        [SerializeField] private Image fuelBar;

        private void Awake()
        {
            speedLeftArrow.SetActive(false);
            speedRightArrow.SetActive(false);
            speedUpArrow.SetActive(false);
            speedDownArrow.SetActive(false);
        }

        private void Update()
        {
            UpdateStatsTextMesh();
        }

        private void UpdateStatsTextMesh()
        {
            var score = GameManager.Instance.GetScore();
            var time = Mathf.Round(GameManager.Instance.GetTime());
            var speedX = Mathf.Abs(Mathf.RoundToInt(Lander.Instance.GetSpeedX()) * 10);
            var speedY = Mathf.Abs(Mathf.RoundToInt(Lander.Instance.GetSpeedY()) * 10);

            SetSpeedArrows(speedX, speedRightArrow, speedLeftArrow);
            SetSpeedArrows(speedY, speedUpArrow, speedDownArrow);

            fuelBar.fillAmount = Lander.Instance.GetCurrentFuelAmount() / Lander.Instance.GetMaxFuelAmount();

            statsMesh.text = GameManager.Instance.GetLevelNumber() + "\n" +
                             score + "\n" +
                             time + "\n" +
                             speedX + "\n" +
                             speedY + "\n";
        }

        //TODO is this too confusing?
        private void SetSpeedArrows(int speed, GameObject posArrow, GameObject negArrow)
        {
            if (speed > 0)
            {
                posArrow.SetActive(true);
                negArrow.SetActive(false);
            }
            else if (speed < 0)
            {
                posArrow.SetActive(false);
                negArrow.SetActive(true);
            }
            else
            {
                posArrow.SetActive(false);
                negArrow.SetActive(false);
            }
        }
    }
}