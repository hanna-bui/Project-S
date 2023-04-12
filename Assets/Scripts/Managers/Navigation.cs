using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameSetup");
    }
    public void HowTo()
    {
        ///load overlay canvas with simple instructions
    }
    public void Solo()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }
    public void Multi()
    {
        SceneManager.LoadScene("Starting");
    }
}
