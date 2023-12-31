using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberHUD : MonoBehaviour
{
    public Text nameText, lvlText;  
    public HealthBar healthBar;
    public Image pokemonImage;

    private Pokemon _pokemon;

    [SerializeField] private Color selectedColor = Color.red;

    //Sets the pokemon info of the pokemon in the pokemon party.
    public void SetPokemonData(Pokemon pokemon )
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.Name;
        lvlText.text = $"Lv{pokemon.Level}";
        healthBar.SetHp((float)pokemon.HP/pokemon.MaxHp);
        pokemonImage.sprite = pokemon.Base.FrontSprite;
    }
    //Draws with color the pokemon thats gonna be chosen.
    public void SetSelectedPokemon(bool selected)
    {

        if (selected)
        {
            nameText.color = selectedColor;
        }
        else
        {
            nameText.color = Color.black;
        }

    }
}
