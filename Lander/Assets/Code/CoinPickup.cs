using UnityEngine;

namespace Code
{
    public class CoinPickup : MonoBehaviour
    {
        [SerializeField] private int worth = 100;

        public int GetWorth()
        {
            return worth;
        }
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
