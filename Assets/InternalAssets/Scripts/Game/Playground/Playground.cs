using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    public BulletsSpawner BulletsSpawner => bulletsSpawner;
    [SerializeField] private BulletsSpawner bulletsSpawner;

    public AttackGrid AttackGrid => attackGrid;
    [SerializeField] private AttackGrid attackGrid;

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}