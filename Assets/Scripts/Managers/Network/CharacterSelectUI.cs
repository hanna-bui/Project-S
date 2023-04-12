using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] internal Button selectButton;
    [SerializeField] internal List<Button> charactersButtons;
    // [SerializeField] internal SManager smanager;

    public int index = 0;
    
    public static CharacterSelectUI instance;

    void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        foreach (var b in charactersButtons)
        {
            b.onClick.AddListener(delegate { Task(b); });
        }
        // selectButton.onClick.AddListener(ChangeScene);
    }

    private void Task(Button b)
    {
        index = charactersButtons.IndexOf(b);
        Debug.Log(index);
    }
    
    public void ChangeScene()
    {
        // smanager.ServerChangeScene("SampleScene2.0");
    }
}
