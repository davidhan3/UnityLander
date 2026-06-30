using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class Lander : MonoBehaviour
    {
        public static Lander Instance { get; private set; }

        private const float GRAVITY_SCALE = 0.7f;

        public event EventHandler OnUpForce;
        public event EventHandler OnLeftForce;
        public event EventHandler OnRightForce;
        public event EventHandler ResetForce;
        public event Action<int> OnCoinPickup;
        public event EventHandler<OnStateChangedArgs> OnStateChanged;

        public class OnStateChangedArgs : EventArgs
        {
            public GameState State;
        }

        public event EventHandler<OnLandedEventArgs> OnLanding;

        public class OnLandedEventArgs : EventArgs
        {
            public LandingType LandingType;
            public float LandingSpeed;
            public float LandingAngle;
            public int ScoreMultiplier;
            public int Score;
        }

        public enum LandingType
        {
            Success,
            WrongLandingArea,
            TooSteep,
            TooFast
        }

        private GameState currentPlayState;

        public enum GameState
        {
            WaitingToStart,
            Playing,
            GameEnded,
        }

        [SerializeField] private float mainThrust = 500f;
        [SerializeField] private float adjustThrust = 200f;
        [SerializeField] private float fuelConsumedPerSecond = 1f;
        [SerializeField] private float maxFuelAmount = 15f;
        private Rigidbody2D landerRigidBody2D;
        private float currentFuelAmount;

        private void Awake()
        {
            Instance = this;
            landerRigidBody2D = GetComponent<Rigidbody2D>();
            landerRigidBody2D.gravityScale = 0f;
            currentFuelAmount = maxFuelAmount;
            currentPlayState = GameState.WaitingToStart;
        }

        private void FixedUpdate()
        {
            ResetForce?.Invoke(this, EventArgs.Empty);

            switch (currentPlayState)
            {
                case GameState.Playing:
                    if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed ||
                        Keyboard.current.rightArrowKey.isPressed)
                    {
                        if (currentFuelAmount <= 0)
                        {
                            return;
                        }

                        ConsumeFuel();
                        landerRigidBody2D.gravityScale = GRAVITY_SCALE;
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

                    break;
                case GameState.WaitingToStart:
                    if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed ||
                        Keyboard.current.rightArrowKey.isPressed)
                    {
                        landerRigidBody2D.gravityScale = GRAVITY_SCALE;
                        SetGameState(GameState.Playing);
                    }

                    break;
                case GameState.GameEnded:
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            var landingType = LandingType.Success;
            var landingSpeed = collision2D.relativeVelocity.magnitude;
            var landingAngle = Math.Abs(Vector2.Dot(Vector2.up, transform.up));

            if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
            {
                landingType = LandingType.WrongLandingArea;
            }

            const float softLandingVelocityThreshold = 3f;

            if (landingSpeed > softLandingVelocityThreshold)
            {
                landingType = LandingType.TooFast;
            }

            const float softLandingAngleThreshold = .9f;
            if (landingAngle < softLandingAngleThreshold)
            {
                landingType = LandingType.TooSteep;
            }

            if (landingType != LandingType.Success)
            {
                SetGameState(GameState.GameEnded);
                OnLanding?.Invoke(this, new OnLandedEventArgs
                {
                    LandingType = landingType,
                    LandingSpeed = landingSpeed,
                    LandingAngle = landingAngle,
                    ScoreMultiplier = 0,
                    Score = 0
                });
                return;
            }

            const float maxScoreAmountLandingAngle = 100;
            const float scoreDotVectorMultiplier = 10;

            var landingAngleScore = maxScoreAmountLandingAngle -
                                    Mathf.Abs(landingAngle - 1f) * scoreDotVectorMultiplier *
                                    maxScoreAmountLandingAngle;
            const float maxScoreAmountLandingSpeed = 100;
            var landingSpeedScore = (softLandingVelocityThreshold - landingSpeed) * maxScoreAmountLandingSpeed;

            var landingPadScoreMultiplier = landingPad.GetScoreMultiplier();
            var totalScore = (int)(landingPadScoreMultiplier * (landingAngleScore + landingSpeedScore));
            OnLanding?.Invoke(this, new OnLandedEventArgs
            {
                LandingType = landingType,
                LandingSpeed = landingSpeed,
                LandingAngle = landingAngle,
                ScoreMultiplier = landingPadScoreMultiplier,
                Score = totalScore
            });
            SetGameState(GameState.GameEnded);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out FuelPickup fuelPickup))
            {
                IncreaseFuel(fuelPickup.GetFuelAmount());
                fuelPickup.DestroySelf();
            }
            else if (other.gameObject.TryGetComponent(out CoinPickup coinPickup))
            {
                OnCoinPickup?.Invoke(coinPickup.GetWorth());
                coinPickup.DestroySelf();
            }
        }

        private void SetGameState(GameState state)
        {
            currentPlayState = state;
            OnStateChanged?.Invoke(this, new OnStateChangedArgs
            {
                State = state
            });
        }

        private void ConsumeFuel()
        {
            currentFuelAmount -= fuelConsumedPerSecond * Time.deltaTime;
        }

        private void IncreaseFuel(float increaseAmount)
        {
            currentFuelAmount += increaseAmount;
            if (currentFuelAmount > maxFuelAmount)
            {
                currentFuelAmount = maxFuelAmount;
            }
        }

        public float GetCurrentFuelAmount()
        {
            return currentFuelAmount;
        }

        public float GetMaxFuelAmount()
        {
            return maxFuelAmount;
        }

        public float GetSpeedX()
        {
            return landerRigidBody2D.linearVelocityX;
        }

        public float GetSpeedY()
        {
            return landerRigidBody2D.linearVelocityY;
        }
    }
}