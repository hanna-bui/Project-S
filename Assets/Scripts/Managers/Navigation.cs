using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        ///load overlay canvas with simple instructions
    }
    public void Solo()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    public void ChooseLevel()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void LevelSelect()
    {
        FindObjectOfType<SelectLevels>().Select();
    }
    
    public void RoomSelect()
    {
        FindObjectOfType<SelectRooms>().Select();
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

    public IEnumerator  ChooseCharacter()
    {
        var ao = SceneManager.LoadSceneAsync("PlayDemo");
        while (!ao.isDone)
        {
            yield return null;
        }
    }
}
