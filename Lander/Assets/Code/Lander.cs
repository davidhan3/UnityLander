using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Lander : MonoBehaviour
    {


        public event EventHandler OnUpForce;
        public event EventHandler OnLeftForce;
        public event EventHandler OnRightForce;
        public event EventHandler ResetForce;
        
        private Rigidbody2D landerRigidBody2D;
        [SerializeField] private float mainThrust = 500f;
        [SerializeField] private float adjustThrust = 200f;
        [SerializeField] private float fuelConsumedPerSecond = 1f;
        [SerializeField] private float maxFuelAmount = 15f;

        private float fuelAmount;

        private void Awake()
        {
            landerRigidBody2D = GetComponent<Rigidbody2D>();
            fuelAmount = maxFuelAmount;
        }

        private void FixedUpdate()
        {
            ResetForce?.Invoke(this, EventArgs.Empty);



            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                if (fuelAmount <= 0)
                {
                    Debug.Log("No fuel!");
                    return;
                }
                ConsumeFuel();
            }
            if (Keyboard.current.upArrowKey.isPressed)
            {
                landerRigidBody2D.AddForce(transform.up * (mainThrust * Time.deltaTime));
                OnUpForce?.Invoke(this, EventArgs.Empty);
            }
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                landerRigidBody2D.AddTorque(adjustThrust * Time.deltaTime);
                OnLeftForce?.Invoke(this, EventArgs.Empty);
            }
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                landerRigidBody2D.AddTorque(-adjustThrust * Time.deltaTime);
                OnRightForce?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
            {
                Debug.Log("Terrain Crashed");
                return;
            }
            
            const float softLandingVelocityThreshold = 3f;
            var landingSpeed = collision2D.relativeVelocity.magnitude;

            if (landingSpeed > softLandingVelocityThreshold)
            {
                Debug.Log("Hard Landing");
                return;
            }

            const float softLandingAngleThreshold = .9f;
            var dotVector = Math.Abs(Vector2.Dot(Vector2.up, transform.up));
            if (dotVector < softLandingAngleThreshold)
            {
                Debug.Log("Landing angle too steep");
                return;
            }
            Debug.Log("Successful Landing");

            const float maxScoreAmountLandingAngle = 100;
            const float scoreDotVectorMultiplier = 10;

            var landingAngleScore = maxScoreAmountLandingAngle -
                                      Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;
            const float maxScoreAmountLandingSpeed = 100;
            var landingSpeedScore = (softLandingVelocityThreshold - landingSpeed) * maxScoreAmountLandingSpeed;
            
            Debug.Log("Landing Angle Score: " + landingAngleScore);
            Debug.Log("Landing Speed Score: " + landingSpeedScore);

            var totalScore = landingPad.GetScoreMultiplier() * (landingAngleScore + landingSpeedScore);
            Debug.Log("Total Score: " + totalScore);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out FuelPickup fuel))
            {
                fuelAmount += fuel.GetFuelAmount();
                fuel.DestroySelf();
            }
        }

        private void ConsumeFuel()
        {
            fuelAmount -= fuelConsumedPerSecond * Time.deltaTime;
        }
    }
}
