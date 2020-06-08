using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoClickInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler,
    IPointerDownHandler
{
    [SerializeField] private bool advancing;

    private Button button;
    public float ChannelTime = 5f;
    private IEnumerator decraseRoutin;
    private IEnumerator incraseRoutin;

    public PrimaryInput primary;
    [SerializeField] private Slider progressBar;
    public float ProgressRate = 1f;
    public float RegressRate = .1f;
    [SerializeField] private bool retreating;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("trigger down");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (primary == PrimaryInput.Controller)
            return;
        if (incraseRoutin == null)
            incraseRoutin = AddProgress(ProgressRate / ChannelTime, eventData);
        if (retreating)
        {
            StopCoroutine(decraseRoutin);
            decraseRoutin = null;
            retreating = false;
        }

        if (button.interactable)
            StartCoroutine(incraseRoutin);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (primary == PrimaryInput.Controller)
            return;
        if (decraseRoutin == null)
            decraseRoutin = DecreaseProgress(ProgressRate * RegressRate / ChannelTime);
        if (advancing)
        {
            StopCoroutine(incraseRoutin);
            incraseRoutin = null;
            advancing = false;
        }

        StartCoroutine(decraseRoutin);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("trigger up");
    }

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private IEnumerator AddProgress(float amount, PointerEventData eventData)
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
        ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerClickHandler);
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
        incraseRoutin = null;
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
        decraseRoutin = null;
        progressBar.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitTillFadeOver());
    }

    private IEnumerator WaitTillFadeOver()
    {
        while (AntiSpamSingleton.Instance.IsTransitioning)
            yield return null;

        yield return new WaitForSeconds(1f);

        button.interactable = true;
    }

    private void OnDisable()
    {
        progressBar.value = 0;
        progressBar.gameObject.SetActive(false);
        if (incraseRoutin != null)
            StopCoroutine(incraseRoutin);
        advancing = retreating = false;
        incraseRoutin = decraseRoutin = null;
        if (button != null)
            button.interactable = false;
    }
}