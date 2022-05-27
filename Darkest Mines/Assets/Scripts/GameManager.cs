using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player[] players = new Player[4];
    [SerializeField] private Enemy[] enemies = new Enemy[4];
    List<Character> characters = new List<Character>();
    public bool IsBattleActive {private set; get; }
    // Start is called before the first frame update
    void Start()
    {
        IsBattleActive = false;
        StartBattle();
        players[0].Attack();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void StartBattle()
    {
        if (IsBattleActive) return;
        IsBattleActive = true;
        SpawnEnemies();

        characters.AddRange(players);
        characters.AddRange(enemies);
        IListExtensions.Shuffle(characters);
    }
    private void SpawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
