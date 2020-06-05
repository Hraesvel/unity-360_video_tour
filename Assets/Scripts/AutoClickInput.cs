using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using Slider = UnityEngine.UI.Slider;

public class AutoClickInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    public float ChannelTime = 5f;
    public float ProgressRate = 1f;
    public float RegressRate = .1f;
    [SerializeField] private Slider progressBar;
    [SerializeField] private bool advancing;
    [SerializeField] private bool retreating;

    private IEnumerator incraseRoutin;
    private IEnumerator decraseRoutin;

    void Start()
    {
        incraseRoutin = AddProgress(ProgressRate / ChannelTime);
        decraseRoutin = DecreaseProgress(ProgressRate * RegressRate / ChannelTime);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (retreating)
        {
            StopCoroutine(decraseRoutin);
            decraseRoutin = DecreaseProgress(ProgressRate * RegressRate / ChannelTime);
            retreating = false;
        }

        Debug.Log("progressing");
        StartCoroutine(incraseRoutin);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (advancing)
        {
            StopCoroutine(incraseRoutin);
            incraseRoutin = AddProgress(ProgressRate / ChannelTime);
            advancing = false;
        }

        StartCoroutine(decraseRoutin);
    }

    private IEnumerator AddProgress(float amount)
    {
        advancing = true;
        retreating = false;
        progressBar.gameObject.SetActive(true);
        while (progressBar.value < 1)
        {
            progressBar.value += amount;
            yield return new WaitForSeconds(amount);
        }

        Debug.Log("Done: some callback or event trigger here");

        advancing = false;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("hard reset");

        advancing = false;
        retreating = false;
        progressBar.value = 0f;
        incraseRoutin = AddProgress(ProgressRate / ChannelTime);
        decraseRoutin = DecreaseProgress(ProgressRate * RegressRate / ChannelTime);
    }
    

    private IEnumerator DecreaseProgress(float amount)
    {
        retreating = true;
        advancing = false;
        while (progressBar.value > 0)
        {
            progressBar.value -= amount;
            yield return new WaitForSeconds(amount);
        }

        Debug.Log("back to zero");

        retreating = false;
        decraseRoutin = DecreaseProgress(amount);
        progressBar.gameObject.SetActive(false);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("trigger up");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("trigger down");

    }
}