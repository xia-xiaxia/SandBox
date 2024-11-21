using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public enum StateID
{
    Free_Mode,
    Creat_Mode,
}
//111111
public interface IState<T> where T : Enum
{
    T Id { get; }
    void OnEnterState();
    void OnUpdateState();

    void OnExitState();

    bool TransitionState(out T id);
}




