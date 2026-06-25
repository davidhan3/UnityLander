using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Lander : MonoBehaviour
    {
        public float mainThrust = 500;
        public float adjustThrust = 200;
        
        private Rigidbody2D landerRigidBody2D;

        private void Awake()
        {
            landerRigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                landerRigidBody2D.AddForce(transform.up * (mainThrust * Time.deltaTime));
            }
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                landerRigidBody2D.AddTorque(adjustThrust * Time.deltaTime);
            }
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                landerRigidBody2D.AddTorque(-adjustThrust * Time.deltaTime);
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
    }
}
