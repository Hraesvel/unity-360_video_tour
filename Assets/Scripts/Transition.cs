using UnityEngine;

public class Transition : MonoBehaviour
{
    public bool Move { get; private set; }

    private void Start()
    {
        Move = false;
    }

    public bool FadeInFinish()
    {
        if (!Move)
            Move = true;
        return Move;
    }

    public void FadeOutStart()
    {
        Move = false;
    }
}