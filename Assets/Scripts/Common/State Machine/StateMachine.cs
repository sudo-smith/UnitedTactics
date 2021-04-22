using UnityEngine;
using System.Collections;
public class StateMachine : MonoBehaviour
{
    public enum Phase
    {
        Movement,
        Combat
    };

    //todo something similar to transition function?
    //otherwise this is unnecessary
    public virtual Phase CurrentPhase
    {
        get { return _currentPhase; }
        set { _currentPhase = value; }
    }
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }
    protected State _currentState;
    protected Phase _currentPhase;
    protected bool _inTransition;
    public virtual T GetState<T>() where T : State
    {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }

    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    public virtual void ChangeState<T>(Phase p) where T : State
    {
        CurrentPhase = p;
        CurrentState = GetState<T>();
    }

    protected virtual void Transition(State value)
    {
        if (_currentState == value || _inTransition)
            return;
        _inTransition = true;

        if (_currentState != null)
            _currentState.Exit();

        _currentState = value;

        if (_currentState != null)
            _currentState.Enter();

        _inTransition = false;
    }
}
