using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  ReplaceObject: MonoBehaviour
{
    public GameObject PresentObject; // Объект в настоящем
    public GameObject PastObject; // Объект в настоящем
    public GameObject FutureObject; // Объект в будущем

    private KeyCode replaceKey_Future = KeyCode.E; // Клавиша для перехода в будущее
    private KeyCode replaceKey_Present = KeyCode.W; // Клавиша для перехода в настоящее
    private KeyCode replaceKey_Past = KeyCode.Q; // Клавиша для перехода в прошлое

    private void Start()
    {
        PastObject.SetActive(false);
        FutureObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(replaceKey_Future))
        {
            InFuture();
        }
        else if (Input.GetKeyDown(replaceKey_Present))
        {
            InPresent();
        }
        else if (Input.GetKeyDown(replaceKey_Past))
        {
            InPast();
        }
    }

    void InFuture()
    {
        if (PresentObject != null)
        {
            PresentObject.SetActive(false);
        }

        if (PastObject != null)
        {
            PastObject.SetActive(false);
        }

        if (FutureObject != null)
        {
            FutureObject.SetActive(true);
        }
    }

    void InPresent()
    {
        if (PresentObject != null)
        {
            PresentObject.SetActive(true);
        }

        if (PastObject != null)
        {
            PastObject.SetActive(false);
        }

        if (FutureObject != null)
        {
            FutureObject.SetActive(false);
        }
    }

    void InPast()
    {
        if (PresentObject != null)
        {
            PresentObject.SetActive(false);
        }

        if (PastObject != null)
        {
            PastObject.SetActive(true);
        }

        if (FutureObject != null)
        {
            FutureObject.SetActive(false);
        }
    }
}

public enum Times
{
    present,
    past,
    future
}