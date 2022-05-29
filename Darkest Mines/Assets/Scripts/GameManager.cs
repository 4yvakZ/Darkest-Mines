using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Player> players = new List<Player>();
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    List<Character> characters = new List<Character>();

    [SerializeField] private GameObject fade;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text roundText;
    public int RoundIndex { get; private set; }

    private int characterIndex = 0;
    public bool IsBattleActive { private set; get; }
    public bool IsAttacking { get; private set; }
    public bool IsFightingAnim { get; private set; }

    private bool isGameOver;


    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        if (IsBattleActive)
        {
            if (IsFightingAnim)
            {
                Character target = characters[characterIndex].Target;
                if (characters[characterIndex].IsDone && (!target.isActiveAndEnabled || target.IsDone))
                {
                    characters[characterIndex].MoveToBack();
                    target.MoveToBack();
                    if (!target.isActiveAndEnabled && target is Player)
                    {
                        players.Remove(target as Player);
                        if (players.Count == 0)
                        {
                            text.text = "Game Over";
                            text.gameObject.SetActive(true);
                            isGameOver = true;
                            return;
                        }
                    }
                    fade.SetActive(false);
                    CheckCharacters();
                    IsFightingAnim = false;
                }
            }
            else
            {
                if (characters[characterIndex] is Enemy)
                {
                    Character target = players[Random.Range(0, players.Count)];
                    StartFight(target);
                }
                else if (characters[characterIndex] is Player && IsAttacking && Input.GetMouseButtonDown(0))
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

    /// <summary>
    /// Функция начала атаки персонажа
    /// Запускает анимации и выводит на передний план
    /// </summary>
    /// <param name="target">Цель атаки персанажа</param>
    private void StartFight(Character target)
    {
        characters[characterIndex].Attack(target);
        characters[characterIndex].MoveToForeground();
        target.MoveToForeground();
        fade.SetActive(true);
        IsFightingAnim = true;
    }

    private void CheckCharacters()
    {
        characters[characterIndex].Dehighlight();
        //Проверяемостались ли активные враги
        foreach (Character c in characters)
        {
            if (c is Enemy && c.isActiveAndEnabled)
            {
                //Ищем следующего персонажа на ход
                for (characterIndex++; characterIndex < characters.Count && !characters[characterIndex].isActiveAndEnabled; characterIndex++);
                if (characterIndex < characters.Count)
                {
                    characters[characterIndex].Highlight();
                    return;
                }
                //Конец очереди, убираем лишних и замешиваем
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
                characterIndex = 0;
                characters[characterIndex].Highlight();
                RoundIndex++;
                roundText.text = "Round " + RoundIndex;
                return;
            }
        }
        text.gameObject.SetActive(true);
        characters.Clear();
        roundText.gameObject.SetActive(false);
        IsBattleActive = false;
    }

    public void StartBattle()
    {
        if (IsBattleActive) return;
        IsBattleActive = true;
        RoundIndex = 1;
        roundText.text = "Round " + RoundIndex;
        roundText.gameObject.SetActive(true);

        text.gameObject.SetActive(false);
        SpawnEnemies();

        characters.AddRange(players);
        characters.AddRange(enemies);
        IListExtensions.Shuffle(characters);
        characters[characterIndex].Highlight();
    }


    private void SpawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.Activate();
        }
    }

    /// <summary>
    /// Функция активации режима атаки игроком
    /// Вызывается кнопкой Attack
    /// </summary>
    public void PlayerAttack()
    {
        if (!IsBattleActive) return;
        if (!(characters[characterIndex] is Player)) return;
        IsAttacking = true;
    }

    /// <summary>
    /// Функция пропуска хода
    /// Вызывается кнопкой Skip
    /// </summary>
    public void PlayerSkip()
    {
        if (!IsBattleActive) return;
        if (!(characters[characterIndex] is Player)) return;
        CheckCharacters();
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Перемешивает элементы списка
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
