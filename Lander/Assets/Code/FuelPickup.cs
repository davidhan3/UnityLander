using UnityEngine;

namespace Code
{
    public class FuelPickup : MonoBehaviour
    {
        [SerializeField] private float fuelAmount;

        public float GetFuelAmount()
        {
            return fuelAmount;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
