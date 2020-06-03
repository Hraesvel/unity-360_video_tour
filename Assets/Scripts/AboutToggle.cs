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
    }

    public void toggleAboutPanel()
    {
          panel.SetActive(!panel.activeSelf);
    }
}
