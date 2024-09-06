using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PullObject<Bullet>
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float flySpeed;

    private void Update()
    {
        if (Mathf.Abs(transform.position.y) > 10f)
        {
            UnpullThis();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.position += flySpeed * Time.fixedDeltaTime * (Vector2)transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.TryGetComponent(out IAttackGridItem explodable))
        {
            return;
        }

        explodable.Explode();
        UnpullThis();
    }

    private void OnBecameInvisible()
    {
        UnpullThis();
    }
}
