using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public AudioSource menuMusic;
        [SerializeField] public AudioSource rainMusic;
        [SerializeField] public List<AudioSource> punchSFX;
        [SerializeField] public List<AudioSource> knifeSFX;
        [SerializeField] public List<AudioSource> spearSFX;
        [SerializeField] public List<AudioSource> rifleSFX;
        [SerializeField] public List<AudioSource> molotovSFX;
        [SerializeField] public AudioSource pickupSFX;

        public static AudioManager Instance { get; private set; }

        AudioManager()
        {
            Instance = this;
        } 

        public void PlayMenuMusic(bool play)          
        {
            if(play)
            {
                menuMusic.Play();
            }
            else
            {
                menuMusic.Pause();
            }
        }

        public void PlayRainMusic(bool play)          
        {
            if(play)
            {
                rainMusic.Play();
            }
            else
            {
                rainMusic.Pause();
            }
        }        

        public void PickupSFX()
        {
            if(pickupSFX != null)
            {
                pickupSFX.Play();
            }
            
        }

        public void PlayPunchSFX()
        {
            if(punchSFX.Count > 0)
            {
                punchSFX[Random.Range(0, punchSFX.Count)].Play();
            }
            
        }

        public void PlayKnifeSFX()
        {
            if(knifeSFX.Count > 0)
            {
                knifeSFX[Random.Range(0, knifeSFX.Count)].Play();
            }
            
        }

        public void PlaySpearSFX()
        {
            if(spearSFX.Count > 0)
            {
                spearSFX[Random.Range(0, spearSFX.Count)].Play();
            }
            
        }

        public void PlayRifleSFX()
        {
            if(rifleSFX.Count > 0)
            {
                rifleSFX[Random.Range(0, rifleSFX.Count)].Play();
            }
            
        }   

        public void PlayMolotovSFX()
        {
            if(molotovSFX.Count > 0)
            {
                molotovSFX[Random.Range(0, molotovSFX.Count)].Play();
            }
        }        
    }
}