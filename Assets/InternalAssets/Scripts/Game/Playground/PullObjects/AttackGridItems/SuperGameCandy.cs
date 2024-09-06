using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGameCandy : AttackGridItem<SuperGameCandy>
{
    protected override void OnExplodeImpact()
    {
        GameController.Instance.SuperGame();
        GameController.Instance.AddScore(4);
    }
}
