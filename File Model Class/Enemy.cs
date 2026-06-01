using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField] private float damage;
    [SerializeField] private float detectionRange;
    [SerializeField] private Transform target;
    public float Damage => damage;
    public float DetectionRange => detectionRange;
    public Transform Target => target;

    public abstract override void Attack();

    public abstract void Patrol();
}