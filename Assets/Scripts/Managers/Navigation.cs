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
        
        public void Respawn()
        {
            GameManager.instance.Respawn();
        }
        
        public void Play()
        {
            GameManager.instance.Play();
        }

        public void HowTo()
        {
            ///load overlay canvas with simple instructions
        }

        public void Solo()
        {
            SceneManager.LoadScene("GameSelect");
        }
        
        public void Lose()
        {
            GameManager.instance.Lose();
        }

        /*
     public void Solo()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
    
    public void ChooseLevel()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }
    */

        public void Next()
        {
            GameManager.instance.Next();
        }
        
        public void TestNextLvlUp()
        {
            GameManager.instance.Next();
        }

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