using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlatform : MonoBehaviour
{
    public bool Shooting { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] shootSockets;

    private int bulletsCount = 1;

    private Bullet shootedBullet = null;

    private void Start()
    {
        Shooting = false;

        if (PreferencesItemsManager.Instance.CanUseNextRoundItem(PreferenceItem.ExtraBulletsBoost))
        {
            bulletsCount = shootSockets.Length;
        }
        else
        {
            bulletsCount = 1;
        }
    }

    public Vector3 GetSize()
    {
        return spriteRenderer.bounds.size;
    }

    public void RotateAndShoot(Vector3 position)
    {
        if(Shooting || (shootedBullet != null && shootedBullet.gameObject.activeInHierarchy))
        {
            return;
        }

        Shooting = true;

       Vector3 eulers = Utility.GetLookRotation(transform.position, position).eulerAngles;
        eulers.z -= 90f;
        transform.rotation = Quaternion.Euler(eulers);

        this.OnAnimation(_animator, "Shooting", OnShootEnded, 0.99f);
        _animator.SetTrigger("Shoot");

        for (int i = 0; i < bulletsCount; i++)
        {
            Transform shootSocket = shootSockets[i];
            shootedBullet = GameController.Instance.Playground.BulletsSpawner.Spawn(shootSocket.position, shootSocket.rotation);
        }
    }

    private void OnShootEnded()
    {
        Shooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IAttackGridItem _))
        {
            return;
        }

        GameController.Instance.Lose();
    }
}
