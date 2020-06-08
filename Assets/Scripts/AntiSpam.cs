using UnityEngine;

public class AntiSpam : MonoBehaviour
{
    [SerializeField] private Controls ctrl;
    public AntiSpamSingleton instance;

    private void Awake()
    {
        lock (AntiSpamSingleton.Instance)
        {
            Debug.Log("Anti Spam running");
            AntiSpamSingleton.Instance.PlayerCtrl = ctrl;
            // AntiSpamSingleton.Instance.IsTransitioning = true;
        }
    }

    private void Update()
    {
        // Debug.Log($"State: {AntiSpamSingleton.Instance.IsTransitioning}");
    }
}