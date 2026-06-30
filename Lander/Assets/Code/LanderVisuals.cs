using System;
using UnityEngine;

namespace Code
{
    public class Thruster : MonoBehaviour
    {
        [SerializeField] private ParticleSystem leftThrusterParticle;
        [SerializeField] private ParticleSystem middleThrusterParticle;
        [SerializeField] private ParticleSystem rightThrusterParticle;
        [SerializeField] private GameObject explosionParticle;

        private Lander PlayerLander;

        private void Awake()
        {
            SetParticleSystem(leftThrusterParticle, false);
            SetParticleSystem(middleThrusterParticle, false);
            SetParticleSystem(rightThrusterParticle, false);

            PlayerLander = GetComponent<Lander>();
            PlayerLander.OnUpForce += PlayerLander_OnUpForce;
            PlayerLander.OnLeftForce += PlayerLander_OnLeftForce;
            PlayerLander.OnRightForce += PlayerLander_OnRightForce;
            PlayerLander.ResetForce += PlayerLander_ResetForce;
            PlayerLander.OnLanding += PlayerLander_OnLanding;
        }

        private void PlayerLander_OnLanding(object sender, Lander.OnLandedEventArgs args)
        {
            switch (args.LandingType)
            {
                case Lander.LandingType.WrongLandingArea:
                case Lander.LandingType.TooSteep:
                case Lander.LandingType.TooFast:
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                    break;
            }
        }

        private void PlayerLander_ResetForce(object sender, EventArgs e)
        {
            SetParticleSystem(leftThrusterParticle, false);
            SetParticleSystem(middleThrusterParticle, false);
            SetParticleSystem(rightThrusterParticle, false);
        }

        private void PlayerLander_OnRightForce(object sender, EventArgs e)
        {
            SetParticleSystem(leftThrusterParticle, true);
        }

        private void PlayerLander_OnLeftForce(object sender, EventArgs e)
        {
            SetParticleSystem(rightThrusterParticle, true);
        }

        private void PlayerLander_OnUpForce(object sender, EventArgs e)
        {
            SetParticleSystem(leftThrusterParticle, true);
            SetParticleSystem(middleThrusterParticle, true);
            SetParticleSystem(rightThrusterParticle, true);
        }

        private void SetParticleSystem(ParticleSystem ps, bool enableParticle)
        {
            ParticleSystem.EmissionModule emissionModule = ps.emission;
            emissionModule.enabled = enableParticle;
        }
    }
}