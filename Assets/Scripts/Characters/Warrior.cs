using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Enemy {
    
    public Warrior(Vector3 position, float health = 1)
        : base("warrior", 3, new List<Spell> { new DamageSpell(4, 1, "Use1", target: "player"), new DamageSpell(6, 5, "Use2", target: "player") },
            (Transform)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/warrior.prefab", typeof(Transform)), (TextMesh)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/healthbar_sprite", typeof(Transform)), 
            position,
            health)
    {
        
    }

}
