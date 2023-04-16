using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{

    public class Navigation : MonoBehaviour
    {
        private PlayerSpawner ps;

        public static string main = "PlayDemo";

        public void PlayGame()
        {
            SceneManager.LoadScene("GameSetup");
        }
        public void HowTo()
        {
            SceneManager.LoadScene("HowTo");
        }

        public void Solo()
        {
            SceneManager.LoadScene("GameSelect");
        }
        
        public void Respawn()
        {
            GameManager.instance.Respawn();
        }
            
        public void Play()
        {
            GameManager.instance.Play();
        }
        
        public void Lose()
        {
            GameManager.instance.Lose();
        }

        public void Next()
        {
            GameManager.instance.Next();
        }
        
        public void Continue() {}    
        
        public void PlayAgain()
        {
            SceneManager.LoadScene("MainMenu");
        }


        /*
    public void Multi()
    {
        SceneManager.LoadScene("Starting");
    }
    */
    }
}