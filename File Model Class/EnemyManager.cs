using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void SortEnemyByHealth()
    {
        enemies.Sort((a, b) => b.Health.CompareTo(a.Health));
    }

    public void ShowEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            Debug.Log(enemy.name + " HP: " + enemy.Health);
        }
    }
}