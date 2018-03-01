using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpellBinding
{
    public PlayerController playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    public Spell spell;
    private KeyCode key;
    private Text textBox;

	public SpellBinding(Spell spell, KeyCode key, Text textBox) {
        this.spell = spell;
        this.key = key;
        this.textBox = textBox;
	}

    public void updateTextBox()
    {
        this.textBox.text = playerController.textConverter(spell.getCooldown());
    }


    public KeyCode getKey()
    {
        return key;
    }

    // Use this for initialization
    void Start()
    {
    }

}
