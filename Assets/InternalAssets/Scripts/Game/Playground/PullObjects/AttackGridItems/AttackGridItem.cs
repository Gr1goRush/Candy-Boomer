using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGridItem<T> : PullObject<T>, IAttackGridItem where T : MonoBehaviour
{
    public GridVector Position { get; private set; }

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public bool Exploded { get; private set; }

    public event Action<IAttackGridItem> OnExplodedEvent;

    private void Awake()
    {
        Exploded = false;
    }

    public Vector3 GetSize()
    {
        return spriteRenderer.bounds.size;
    }

    public void SetAnimatorController(RuntimeAnimatorController runtimeAnimatorController)
    {
        _animator.runtimeAnimatorController = runtimeAnimatorController;
    }

    public void SetDefault(GridVector position)
    {
        Position = position;

        Exploded = false;

        if (_animator != null)
        {
            _animator.Play("Idle");
        }
    }

    public void Explode()
    {
        if(Exploded)
        {
            return;
        }

        Exploded = true;

        if(_animator == null)
        {
            OnExplodeAnimation();
            return;
        }

        this.OnAnimation(_animator, "Explosion", OnExplodeAnimation, 0.99f);
        _animator.SetTrigger("Explode");
    }

    private void OnExplodeAnimation()
    {
        OnExplodeImpact();

        UnpullThis();

        OnExplodedEvent?.Invoke(this);
    }

    protected virtual void OnExplodeImpact()
    {

    }

    private void Reset()
    {
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
