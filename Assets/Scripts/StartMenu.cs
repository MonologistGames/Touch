using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Touch.PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    public PlayerController PlayerController;
    public CanvasGroup CanvasGroup;
    public CinemachineVirtualCamera StartMenuCamera;
    public GameObject Title;
    private bool _isStarted = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !_isStarted)
        {
            PlayerController.enabled = true;
            StartMenuCamera.enabled = false;
            Title.SetActive(false);
            CanvasGroup.alpha = 1;
            _isStarted = true;
        }
    }
}
