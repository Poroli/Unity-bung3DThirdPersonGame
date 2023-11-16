using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static bool GamePause;

    [SerializeField] private string tagCollectables;
    [SerializeField] private GameObject WinScreen;
    private GameObject[] collectables;
    private int collected;

    private void HowManyToCollect()
    {
        collectables = GameObject.FindGameObjectsWithTag(tagCollectables);

        foreach (GameObject collectable in collectables)
        {
            collectable.gameObject.AddComponent<GLClient>().GLogic = this;
        }
    }

    public void Collect()
    {
        collected++;
        CheckQuestComplete();
    }

    private void CheckQuestComplete()
    {
        if (collected < collectables.Length)
        {
            return;
        }
        WinScreen.SetActive(true);
        GamePause = true;
    }
    private void Start()
    {
        HowManyToCollect();
    }
}

public class GLClient: MonoBehaviour
{
    public GameLogic GLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GLogic.Collect();
        }
    }
}
