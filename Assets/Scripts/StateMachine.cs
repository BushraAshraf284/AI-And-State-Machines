using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Stack<State> states;
    private void Awake()
    {
        states = new Stack<State>();
    }

    private void Update()
    {
        if(GetCurrentState()!=null)
            GetCurrentState().Execute();
    }

    public void PushState(Action onEnter, Action onExit, Action activeAction)
    {
        PerformActionOnCurrent(ActionType.EXIT);

        var state = new State(activeAction, onEnter, onExit);
        states.Push(state);
        state.OnEnter();
    }

    public void PopState()
    {
        PerformActionOnCurrent(ActionType.EXIT);
        states.Pop();
        PerformActionOnCurrent(ActionType.ENTER);
      
    }



    public State GetCurrentState() { 

        return states.Count > 0? states.Peek() : null; 
    }

    public void PerformActionOnCurrent(ActionType type)
    {
        var current = GetCurrentState();
        if(current != null )
        {
            switch(type)
            {
                case ActionType.ENTER: 
                    current.OnExit(); break;
                case ActionType.EXIT: 
                    current.OnExit(); break;
                case ActionType.ACTIVE: 
                    current.Execute(); break;
            }
        }
    }

    
}

public enum ActionType
{
    ENTER,
    EXIT,
    ACTIVE
}
