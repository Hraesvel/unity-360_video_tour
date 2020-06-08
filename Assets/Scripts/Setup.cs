using System;
using UnityEngine;
using UnityEngine.Video;

public class Setup : MonoBehaviour
{
    public PrimaryInput m_InputStyle;
    public GameObject m_projection;

    private void Awake()
    {
        var canvases = FindObjectsOfType<Canvas>();
        var videoPlayers = FindObjectsOfType<VideoPlayer>();
        
        ConfigScene();
        
        foreach (var canvase in canvases)
        {
            if (canvase.gameObject.layer != 9)
                canvase.gameObject.SetActive(false);
        }

        foreach (var videoPlayer in videoPlayers) videoPlayer.enabled = false;

        if (m_projection != null)
        {
            m_projection.GetComponent<VideoPlayer>().enabled = true;
            m_projection.transform.Find("Canvas").gameObject.SetActive(true);
        }


        Destroy(gameObject);
    }

    void ConfigScene()
    {
        var cam = Camera.main;
        var autoclick = FindObjectsOfType<AutoClickInput>();
        foreach (var autoClickInput in autoclick)
        {
            // Debug.Log($"{autoClickInput.transform.parent.gameObject.name}");
            autoClickInput.primary = m_InputStyle;
        }

        switch (m_InputStyle)
        {
            case PrimaryInput.Controller:

                break;
            case PrimaryInput.Gaze:
                cam.GetComponent<RayInput>().enabled = true;
                cam.transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("RightHand").SetActive(false);
                break;
            case PrimaryInput.Quest:
                cam.GetComponent<RayInput>().enabled = false;
                cam.transform.GetChild(0).gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}