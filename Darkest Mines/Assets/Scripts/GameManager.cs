
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
    public bool IsFightingAnim { get; private set; }

    [SerializeField]private GameObject fade;


    // Update is called once per frame
    void Update()
    {
        if (IsBattleActive)
        {
            if (IsFightingAnim)
            {
                Character target = characters[index].Target;
                if (characters[index].IsDone && (!target.isActiveAndEnabled || target.IsDone))
                {
                    characters[index].MoveToBack();
                    target.MoveToBack();
                    fade.SetActive(false);
                    CheckCharacters();
                    IsFightingAnim = false;
                }
            }
            else
            {
                if (characters[index] is Enemy)
                {
                    Character target = players[Random.Range(0, players.Count)];
                    StartFight(target);
                }
                else if (characters[index] is Player && IsAttacking && Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit && hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        Character target = hit.collider.gameObject.GetComponent<Enemy>();
                        StartFight(target);
                        IsAttacking = false;
                    }
                }
            }
        }
    }

    private void StartFight(Character target)
    {
        characters[index].Attack(target);
        characters[index].MoveToForeground();
        target.MoveToForeground();
        fade.SetActive(true);
        IsFightingAnim = true;
    }

    private void CheckCharacters()
    {
        characters[index].Dehighlight();
        //ѕровер€емостались ли активные враги
        foreach (Character c in characters)
        {
            if (c is Enemy && c.isActiveAndEnabled)
            {
                //»щем следующего персонажа на ход
                for (index++; index < characters.Count && !characters[index].isActiveAndEnabled; index++);
                if (index < characters.Count)
                {
                    characters[index].Highlight();
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
                characters[index].Highlight();
                return;
            }
        }
        text.gameObject.SetActive(true);
        IsBattleActive = false;
    }

    public void StartBattle()
    {
        if (IsBattleActive) return;
        IsBattleActive = true;

        text.gameObject.SetActive(false);
        SpawnEnemies();

        characters.AddRange(players);
        characters.AddRange(enemies);
        IListExtensions.Shuffle(characters);
        characters[index].Highlight();
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
        if (!IsBattleActive) return;
        if (!(characters[index] is Player)) return;
        IsAttacking = true;
    }

    public void PlayerSkip()
    {
        if (!IsBattleActive) return;
        if (!(characters[index] is Player)) return;
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
