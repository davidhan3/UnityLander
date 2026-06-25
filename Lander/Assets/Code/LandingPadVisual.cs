using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro textBox;
    
    private void Awake()
    {
        var landingPad = GetComponent<LandingPad>();
        textBox.text = "x"+landingPad.GetScoreMultiplier();
    }
}
