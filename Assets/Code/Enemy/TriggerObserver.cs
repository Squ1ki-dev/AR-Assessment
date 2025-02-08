using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerObserver : MonoBehaviour
{
    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;
    private void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);
    private void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);
}
