using System;
using UnityEngine;

namespace Code
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput Instance { get; private set; }
        private InputActions inputActions;

        private void Awake()
        {
            Instance = this;
            inputActions = new InputActions();
            inputActions.Enable();
        }

        private void OnDestroy()
        {
            inputActions.Disable();
        }

        public bool IsAnyKeyPressed()
        {
            return IsUpActionPressed() || IsLeftActionPressed() || IsRightActionPressed();
        }

        public bool IsUpActionPressed()
        {
            return inputActions.Player.LanderUp.IsPressed();
        }

        public bool IsLeftActionPressed()
        {
            return inputActions.Player.LanderLeft.IsPressed();
        }

        public bool IsRightActionPressed()
        {
            return inputActions.Player.LanderRight.IsPressed();
        }
    }
}