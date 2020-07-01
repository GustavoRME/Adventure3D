using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private InputMaster input;

    private bool isOpen;

    [SerializeField]
    private GameObject menu;

    private void Awake()
    {
        input = new InputMaster();
        input.UI.Menu.performed += ctx => OpenMenu();
    }

    private void Start()
    {
        menu.SetActive(false);
        isOpen = false;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void OpenMenu()
    {
        isOpen = !isOpen;

        menu.SetActive(isOpen);

        if(isOpen)
        {
            Time.timeScale = .0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}
