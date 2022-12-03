///Scene manager
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Scene scene;
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
