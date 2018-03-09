using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter: IDeathObserver {

    private Stage stage;
    private List<Enemy> startingEnemies;

    /// <summary>
    /// Constructor for Encounter class.
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="startingEnemies"></param>
    public Encounter(Stage stage, List<Enemy> startingEnemies)
    {
        this.stage = stage;
        this.startingEnemies = startingEnemies;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemies"></param>
    private void SetEnemies(List<Enemy> enemies)
    {
        enemies.ForEach(e => e.RegisterDeathObserver(this));
        stage.SetEnemies(enemies);
        stage.SpawnEnemiesOffscreen(enemies);
    }

    private void AddEnemy(Enemy enemy)
    {
        enemy.RegisterDeathObserver(this);
        stage.AddEnemy(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemy.UnregisterDeathObserver(this);
        stage.RemoveEnemy(enemy);
    }

    public void DeathUpdate(Character character)
    {
        if (stage.enemies.Count == 0)
        {
            EndEncounter();
        }
    }

	public void StartEncounter () {
        SetEnemies(startingEnemies);
	}

    public void EndEncounter()
    {
        stage.EndEncounter();
    }

    //Enemy warrior = new Warrior(new Vector3(-0.3f + 1.43f * 0, -2.58f, 0));
    //Enemy mage = new Enemy("mage", 2, mageSpellbook, (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/warrior.prefab", typeof(Transform)), (TextMesh)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/enemy_text.prefab", typeof(TextMesh)),
    //    new Vector3(-0.3f + 1.43f * 2, -2.58f, 0), 2);
    //Enemy warrior2 = new Warrior(new Vector3(-0.3f + 1.43f * 1, -2.58f, 0));
    //enemies = new List<Enemy> { warrior, warrior2, mage };

    ///// <summary>
    ///// Initializes spells to be used for enemies.  Puts spells into spellbooks for the enemies.
    ///// </summary>
    //private void SetEnemySpells()
    //{
    //    Spell splash = new DamageSpell(4, 1, "Use1", target: "player");
    //    Spell frostbolt = new DamageSpell(6, 5, "Use2", target: "player");
    //    mageSpellbook = new List<Spell> { splash, frostbolt };
    //}

}
