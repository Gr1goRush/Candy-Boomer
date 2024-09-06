using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Levels List", fileName = "LevelsList", order = 1)]
public class LevelsList : ScriptableObject
{
    public int levelsCount = 5;
}