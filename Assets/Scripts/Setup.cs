using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Video;

public class Setup : MonoBehaviour
{
    public GameObject m_projection;
    void Awake()
    {
        var canvases = FindObjectsOfType<Canvas>();
        var videoPlayers = FindObjectsOfType<VideoPlayer>();
        foreach (var canvase in canvases)
        {
            if (canvase.gameObject.layer != 9)
                canvase.gameObject.SetActive(false);
        }

        foreach (var videoPlayer in videoPlayers)
        {
            videoPlayer.enabled = false;
        }

        if (m_projection != null)
        {
            m_projection.GetComponent<VideoPlayer>().enabled = true;
            m_projection.transform.Find("Canvas").gameObject.SetActive(true);
        }
        
        
        Destroy(gameObject);
        
    }
    
}
