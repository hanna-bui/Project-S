using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public bool isOn;
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    
    public void ToggleOn()
    {
        isOn = true;
        transform.GetChild(0).gameObject.SetActive(isOn);
    }
    
    public void ToggleOff()
    {
        isOn = false;
        transform.GetChild(0).gameObject.SetActive(isOn);
    }

    public void Toggle()
    {
        isOn = !isOn;
        transform.GetChild(0).gameObject.SetActive(isOn);
    }
}
