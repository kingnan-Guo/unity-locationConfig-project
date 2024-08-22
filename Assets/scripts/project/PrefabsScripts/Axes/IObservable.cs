using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void Subscribe(System.Action onChanged);
    void Unsubscribe(System.Action onChanged);
}
