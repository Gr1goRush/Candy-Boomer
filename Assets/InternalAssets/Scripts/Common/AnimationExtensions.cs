using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate void GameAction();

public static class AnimationExtensions
{
    public static void OnAnimation(this MonoBehaviour monoBehaviour, Animator animator, string name, GameAction call, float targetTime)
    {
        monoBehaviour.StartCoroutine(AnimationWatch(animator, name, call, targetTime));
    }

    static IEnumerator AnimationWatch(Animator animator, string name, GameAction call, float targetTime)
    {
        bool isAnimation = false;
        while (!isAnimation)
        {
            isAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName(name);
            yield return null;
        }

        isAnimation = true;
        while (isAnimation)
        {
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            isAnimation = animatorStateInfo.IsName(name);
            if (animatorStateInfo.normalizedTime >= targetTime)
            {
                break;
            }
            yield return null;
        }

        call?.Invoke();
    }
}