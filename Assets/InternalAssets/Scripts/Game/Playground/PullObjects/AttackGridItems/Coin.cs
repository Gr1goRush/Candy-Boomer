using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Coin : AttackGridItem<Coin>
{
    protected override void OnExplodeImpact()
    {
        AudioController.Instance.Sounds.PlayOneShot("coin");

        GameController.Instance.AddCoin();
        GameController.Instance.AddScore(2);
    }
}