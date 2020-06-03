using System;
using UnityEngine;

public class Transition : MonoBehaviour
{

    private bool _move;
    public bool Move { get=> _move; }

    private void Start()
    {
        _move = false;
    }
    
    public bool FadeInFinish()
    {
        if (!_move)
            _move = true;
        return _move;
    }

    public void FadeOutStart()
    {
        _move = false;
    }
}