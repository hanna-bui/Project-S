using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour
{
    public void ChooseCharacter()
    {
        SceneManager.LoadSceneAsync("TestScene");
    }

    public void SpawnEnemy()
    {
        ///GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    public void SpawnItem()
    {

    }
}
