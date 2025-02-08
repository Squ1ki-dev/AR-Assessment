using UnityEngine;

public abstract class AttackRange : MonoBehaviour
{
    public TriggerObserver TriggerObserver;
    public virtual void TriggerEnter(Collider collider) {}
    public virtual void TriggerExit(Collider collider) {}
}