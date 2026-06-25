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
            SetParticleSystem(leftThrusterParticle, false);
            SetParticleSystem(middleThrusterParticle, false);
            SetParticleSystem(rightThrusterParticle, false);
            
            playerLander = GetComponent<Lander>();
            playerLander.OnUpForce += PlayerLanderOnOnUpForce;
            playerLander.OnLeftForce += PlayerLanderOnOnLeftForce;
            playerLander.OnRightForce += PlayerLanderOnOnRightForce;
            playerLander.ResetForce += PlayerLanderOnResetForce;
        }

        private void PlayerLanderOnResetForce(object sender, EventArgs e)
        {
            SetParticleSystem(leftThrusterParticle, false);
            SetParticleSystem(middleThrusterParticle, false);
            SetParticleSystem(rightThrusterParticle, false);
        }

        private void PlayerLanderOnOnRightForce(object sender, EventArgs e)
        {
            SetParticleSystem(leftThrusterParticle, true);
        }

        private void PlayerLanderOnOnLeftForce(object sender, EventArgs e)
        {
            SetParticleSystem(rightThrusterParticle, true);
        }

        private void PlayerLanderOnOnUpForce(object sender, EventArgs e)
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
