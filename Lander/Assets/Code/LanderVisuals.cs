using System;
using UnityEngine;

namespace Code
{
    public class Thruster : MonoBehaviour
    {

        [SerializeField] private ParticleSystem leftThrusterParticle;
        [SerializeField] private ParticleSystem middleThrusterParticle;
        [SerializeField] private ParticleSystem rightThrusterParticle;

        private Lander playerLander;
        
        private void Awake()
        {
            SetEnabledParticleSystem(leftThrusterParticle, false);
            SetEnabledParticleSystem(middleThrusterParticle, false);
            SetEnabledParticleSystem(rightThrusterParticle, false);
            
            playerLander = GetComponent<Lander>();
            playerLander.OnUpForce += PlayerLanderOnOnUpForce;
            playerLander.OnLeftForce += PlayerLanderOnOnLeftForce;
            playerLander.OnRightForce += PlayerLanderOnOnRightForce;
            playerLander.ResetForce += PlayerLanderOnResetForce;
        }

        private void PlayerLanderOnResetForce(object sender, EventArgs e)
        {
            SetEnabledParticleSystem(leftThrusterParticle, false);
            SetEnabledParticleSystem(middleThrusterParticle, false);
            SetEnabledParticleSystem(rightThrusterParticle, false);
        }

        private void PlayerLanderOnOnRightForce(object sender, EventArgs e)
        {
            SetEnabledParticleSystem(leftThrusterParticle, true);
        }

        private void PlayerLanderOnOnLeftForce(object sender, EventArgs e)
        {
            SetEnabledParticleSystem(rightThrusterParticle, true);
        }

        private void PlayerLanderOnOnUpForce(object sender, EventArgs e)
        {
            SetEnabledParticleSystem(leftThrusterParticle, true);
            SetEnabledParticleSystem(middleThrusterParticle, true);
            SetEnabledParticleSystem(rightThrusterParticle, true);
        }

        private void SetEnabledParticleSystem(ParticleSystem ps, bool enableParticle)
        {
            ParticleSystem.EmissionModule emissionModule = ps.emission;
            emissionModule.enabled = enableParticle;
        }
    }
}
