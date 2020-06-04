using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutToggle : MonoBehaviour
{
    private GameObject panel;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
        if (panel.activeSelf)
            panel.SetActive(false);
    }

    public void ToggleAboutPanel()
    {
          panel.SetActive(!panel.activeSelf);
    }
}
