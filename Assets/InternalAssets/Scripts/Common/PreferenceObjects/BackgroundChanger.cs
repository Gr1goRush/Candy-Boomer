using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : PreferenceObjectChanger<RuntimeAnimatorController>
{
    [SerializeField] private Animator _animator;

    protected override void Set(RuntimeAnimatorController obj)
    {
        _animator.runtimeAnimatorController = obj;
    }
}
