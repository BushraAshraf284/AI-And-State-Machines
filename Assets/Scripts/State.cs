using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public Action OnEnterAction, OnExitAction, ActiveAction;

    public State(Action active, Action onEnter, Action onExit)
    {
        OnEnterAction = onEnter;
        OnExitAction = onExit;
        ActiveAction = active;
    }

    public void Execute()
    {
        if(ActiveAction != null)
        {
            ActiveAction.Invoke();
        }
    }

    public void OnEnter()
    {
        if (OnEnterAction != null)
        {
            OnEnterAction.Invoke();
        }
    }
    public void OnExit()
    {
        if (OnExitAction != null)
        {
            OnExitAction.Invoke();
        }
    }
}
