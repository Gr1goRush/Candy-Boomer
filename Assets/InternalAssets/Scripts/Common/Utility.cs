using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static void InvokeRealtime(this MonoBehaviour monoBehaviour, GameAction action, float time)
    {
        monoBehaviour.StartCoroutine(Invoking(action, time));
    }

    private static IEnumerator Invoking(GameAction action, float time)
    {
        yield return new WaitForSecondsRealtime(time);

        action?.Invoke();
    }

    public static IEnumerable<T> GetRandomEnumerable<T>(IEnumerable<T> arr)
    {
        System.Random random = new System.Random();
        return arr.OrderBy(x => random.Next());
    }

    public static T GetRandomElement<T>(this T[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }

    public static Quaternion GetLookRotation(Vector3 position, Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - position;
        return GetLookRotation(dir);
    }

    public static Quaternion GetLookRotation(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}