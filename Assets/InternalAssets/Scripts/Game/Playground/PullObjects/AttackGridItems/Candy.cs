using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : AttackGridItem<Candy>
{
    protected override void OnExplodeImpact()
    {
        AudioController.Instance.Sounds.PlayOneShot("candy");

        GameController.Instance.AddScore();
    }
}
