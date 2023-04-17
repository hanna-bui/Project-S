using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public bool isOn;
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void Toggle()
    {
        isOn = !isOn;
        transform.GetChild(0).gameObject.SetActive(isOn);
    }
}
