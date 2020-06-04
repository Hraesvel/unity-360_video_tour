using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class UIActionsDev : MonoBehaviour
{
    private IEnumerator coroutine;

    public struct Projection
    {
        public GameObject Self;
        public Canvas Ui;
        public VideoPlayer VideoPlayer;

        public Projection(GameObject s, Canvas c, VideoPlayer vp)
        {
            Self = s;
            Ui = c;
            VideoPlayer = vp;
        }
    }

    
    public void TransitionV2(GameObject to)
    {
        var current = new Projection(gameObject, transform.Find("Canvas").GetComponent<Canvas>(),
            gameObject.GetComponent<VideoPlayer>());
        var target = new Projection(to, to.transform.Find("Canvas").GetComponent<Canvas>(),
            to.GetComponent<VideoPlayer>());

        var fadeIO = GameObject.Find("FadeIOPanel");

        StartCoroutine((new[]
        {
            FadeInRun_v2(fadeIO, current),
            FadeOut_v2(fadeIO, target)
        }).GetEnumerator());
        
    }
    
    private IEnumerator FadeInRun_v2(GameObject fadeIO, Projection cur)
    {
        var anim = fadeIO.GetComponent<Animator>();
        anim.Play("FadeIn", -1, 0f);
        while (fadeIO.GetComponent<Transition>().Move == false)
            yield return new WaitForSeconds(0.25f);
        cur.Ui.gameObject.SetActive(false);
        cur.VideoPlayer.enabled = false;
    }

    private IEnumerator FadeOut_v2(GameObject fadeIO, Projection to)
    {
        var anim = fadeIO.GetComponent<Animator>();
        to.VideoPlayer.enabled = true;
        // to.videoPlayer.Play();
        to.Ui.gameObject.SetActive(true);
        while (to.VideoPlayer.isPlaying == false)
            yield return new WaitForSeconds(0.25f);
        anim.SetTrigger("FadeIn");
    }

    
}
