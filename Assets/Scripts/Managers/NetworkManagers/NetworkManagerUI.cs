using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button client;


    private void Awake()
    {
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

        });
    }
}
 