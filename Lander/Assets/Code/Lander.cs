using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Lander : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                Debug.Log("Up");
            }
        }
    }
}
