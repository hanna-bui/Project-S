using System.Collections.Generic;
using Characters;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] internal Button selectButton;
    [SerializeField] internal List<Button> charactersButtons;
    [SerializeField] internal SManager smanager;

    private int index = 0;

    private void Start()
    {
        foreach (var b in charactersButtons)
        {
            b.onClick.AddListener(delegate { Task(b); });
        }
        selectButton.onClick.AddListener(ChangeScene);
    }

    private void Task(Button b)
    {
        index = charactersButtons.IndexOf(b);
        Debug.Log(index);
    }
    
    private void ChangeScene()
    {
        smanager.playerPrefab = smanager.spawnPrefabs[index];
        smanager.ServerChangeScene("SampleScene2.0");
    }
}
