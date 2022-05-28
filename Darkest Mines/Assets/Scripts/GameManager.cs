
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Player> players = new List<Player>();
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private TMP_Text text;
    List<Character> characters = new List<Character>();
    private int index = 0;
    public bool IsBattleActive { private set; get; }
    public bool IsAttacking { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsBattleActive)
        {
            if (characters[index] is Enemy)
            {
                Character target = players[Random.Range(0, players.Count)];
                characters[index].Attack(target);
                CheckCharacters();
            }
            else if (characters[index] is Player && IsAttacking && Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit && hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Character target = hit.collider.gameObject.GetComponent<Enemy>();
                    characters[index].Attack(target);
                    CheckCharacters();
                    IsAttacking = false;
                }
            }
        }
    }

    private void CheckCharacters()
    {
        
        //ѕровер€емостались ли активные враги
        foreach (Character c in characters)
        {
            if (c is Enemy && c.isActiveAndEnabled)
            {
                //»щем следующего персонажа на ход
                for (index++; index < characters.Count && !characters[index].isActiveAndEnabled; index++);
                if (index < characters.Count)
                {
                    characters[index].Hightlight();
                    return;
                }
                // онец очереди, убираем лишних и замешиваем
                for (int i = 0; i < characters.Count;)
                {
                    if (!characters[i].isActiveAndEnabled)
                    {
                        characters.RemoveAt(i);
                    }  
                    else
                    {
                        i++;
                    }
                }
                IListExtensions.Shuffle(characters);
                index = 0;
                return;
            }
        }
        IsBattleActive = false;
    }

    public void StartBattle()
    {
        if (IsBattleActive) return;
        IsBattleActive = true;

        text.text = "";
        SpawnEnemies();

        characters.AddRange(players);
        characters.AddRange(enemies);
        IListExtensions.Shuffle(characters);
        characters[index].Hightlight();
    }
    private void SpawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public void PlayerAttack()
    {
        if (!(characters[index] is Player)) return;
        IsAttacking = true;
    }

    public void PlayerSkip()
    {
        if (!(characters[index] is Player)) return;
        (characters[index]as Player).Skip();
        CheckCharacters();
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
