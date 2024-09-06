using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : AttackGridItem<Bomb>
{
    protected override void OnExplodeImpact()
    {
        AudioController.Instance.Sounds.PlayOneShot("bomb");

        VibrationManager.Instance.Vibrate();

       int randomNumber = Random.Range(0, 100);
        if(randomNumber < 20)
        {
            ExplodeCross();
            return;
        }

        if(randomNumber < 60)
        {
            ExplodeHorizontal();
            return;
        }

        ExplodeNeighbours();
    }

    private void ExplodeCross()
    {
        ExplodeHorizontal();
        ExplodeVertical();
    }

    private void ExplodeHorizontal()
    {
        List<IAttackGridItem> items = GameController.Instance.Playground.AttackGrid.FindItemsInRow(Position.y, Position.group);
        ExplodeItems(items);
    }

    private void ExplodeVertical()
    {
        List<IAttackGridItem> items = GameController.Instance.Playground.AttackGrid.FindItemsInColumn(Position.x, Position.group);
        ExplodeItems(items);
    }

    private void ExplodeNeighbours()
    {
        AttackGrid attackGrid = GameController.Instance.Playground.AttackGrid;

        IAttackGridItem attackGridItem = attackGrid.FindItem(Position.y, Position.x + 1, Position.group);
        attackGridItem?.Explode();

        attackGridItem = attackGrid.FindItem(Position.y, Position.x - 1, Position.group);
        attackGridItem?.Explode();

        attackGridItem = attackGrid.FindItem(Position.y + 1, Position.x, Position.group);
        attackGridItem?.Explode();

        attackGridItem = attackGrid.FindItem(Position.y - 1, Position.x, Position.group);
        attackGridItem?.Explode();
    }

    private void ExplodeItems(List<IAttackGridItem> items)
    {
        if(items == null || items.Count == 0)
        {
            return;
        }

        foreach (var item in items)
        {
            if (!item.Exploded)
            {
                item.Explode();
                GameController.Instance.AddScore();
            }
        }
    }
}
