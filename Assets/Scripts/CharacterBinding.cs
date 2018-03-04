using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBinding {

    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();

    public TextMesh healthText;
    public Animator anim;
    public Transform prefab;
    public Character character;

    public CharacterBinding(Character character, TextMesh healthText, Animator anim, Transform prefab)
    {
        this.healthText = healthText;
        this.anim = anim;
        this.prefab = prefab;
        this.character = character;
    }

    public void Update()
    {
        UpdateTextBox();
    }

    public void UpdateTextBox()
    {
        this.healthText.text = Utils.ToDisplayText(character.health);
    }

}
