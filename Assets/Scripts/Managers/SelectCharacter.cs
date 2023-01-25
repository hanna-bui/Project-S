using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Characters;
public class SelectCharacter : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform prevChar;
    public Transform selected;

    private void Start()
    {
        foreach (Character c in GameManager.instance.characters)
        {
            GameObject option = Instantiate(optionPrefab, transform);
            Button button = option.GetComponent<Button>();

            button.onClick.AddListener(() =>
            {
                GameManager.instance.SetCharacter(c);
                if (selected != null)
                {
                    prevChar = selected;
                }
                selected = option.transform;
            });

            Image image = option.GetComponentInChildren<Image>();
            ///chooses display image from sprite 
            ///image.sprite = c.sprite;
 
        }
    }

    private void Update()
    {
        if(selected != null)
        {
            selected.localScale = Vector3.Lerp(selected.localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime*10);
        }
        if (prevChar != null)
        {
            prevChar.localScale = Vector3.Lerp(prevChar.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10);
        }
    }
}
