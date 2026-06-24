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
    }
}
