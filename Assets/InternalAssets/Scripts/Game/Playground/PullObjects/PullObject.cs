using System.Collections.Generic;
using UnityEngine;

public class PullObject<T> : MonoBehaviour where T : MonoBehaviour
{

    public int startCount;

    static List<T> unpulledObjects;

    static bool initialized = false;
    static T sample;
    static Transform defaultParent;

    public void Init()
    {
        if (initialized && sample != null)
        {
            return;
        }

        gameObject.SetActive(false);
        unpulledObjects = new List<T>();
        sample = this as T;
        defaultParent = sample.transform.parent;

        unpulledObjects.Add(sample);

        if(startCount <= 0)
        {
            startCount = 1;
        }

        for (int i = 1; i < startCount; i++)
        {
            T pullObject = Instantiate(sample, transform.parent);
            pullObject.gameObject.SetActive(false);
            unpulledObjects.Add(pullObject);
        }

        initialized = true;
    }

    public T Pull()
    {
        Init();

        if(unpulledObjects == null)
        {
            unpulledObjects = new List<T>();
        }

        T pullObject;

        if (unpulledObjects.Count == 0)
        {
           pullObject = Instantiate(sample, transform.parent);
        }
        else
        {
            pullObject = unpulledObjects[0];
            unpulledObjects.RemoveAt(0);
        }

        pullObject.transform.position = sample.transform.position;
        pullObject.transform.rotation = sample.transform.rotation;
        pullObject.gameObject.SetActive(true);

        return pullObject;
    }

    public void Unpull(T pullObject)
    {
        Init();

        if (unpulledObjects == null)
        {
            unpulledObjects = new List<T>();
        }

        if(unpulledObjects.Count == 0 || !unpulledObjects.Contains(pullObject))
        {
            unpulledObjects.Add(pullObject);
            pullObject.gameObject.SetActive(false);

            if(pullObject.transform.parent != defaultParent)
            {
                pullObject.transform.SetParent(defaultParent);
            }
        }
    }

    public void Unpull(List<T> pullObjects)
    {
        if(pullObjects == null || pullObjects.Count == 0)
        {
            return;
        }

        foreach (T item in pullObjects)
        {
            Unpull(item);
        }
    }

    public void UnpullThis()
    {
        Unpull(this as T);
    }

    private void OnDestroy()
    {
        if(sample == this)
        {
            sample = null;
            initialized = false;
        }
    }
}