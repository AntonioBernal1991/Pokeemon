using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PokemonParty : MonoBehaviour
{

    
    

    [SerializeField] private List<Pokemon> pokemons;
    public const int MaxPokemonsInParty = 6;

    public List<Pokemon> Pokemons
      {


        get => pokemons;



      }
    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.InitPokemon();
        }
    }
    //Gets the first non fainted pokemon of the party.
    public Pokemon GetFirstAlivePokemon()
    {
       foreach (var pokemon in pokemons)
        {
            if (pokemon.HP > 0)
            {
                return pokemon;
            }

        }
        return null;
    }
    //Gets the position that occupies each pokemon of the party
    public int GetPositionFromPokemon(Pokemon pokemon)
    {
        for (int i = 0; i < Pokemons.Count; i++)
        {

            if (pokemon == Pokemons[i])
            {
                return  i;
            }
        }
        return -1;
    }

    //Adds a pokemon to the party after capturing it or adding it on unity editor.
    public void AddPokemonToParty(Pokemon pokemon)
    {
        if(pokemons.Count < MaxPokemonsInParty)
        {
            pokemons.Add(pokemon);
            
        }
        else
        {
            //Pc de Bill
        }


    }
   
    //Cures a Status and health of a fainted pokemon

    public void healFaintedPokemon()
    {
        foreach (var pokemon in pokemons)
        {
            if (pokemon.HP <= 0)
            {
                pokemon.HP = pokemon.MaxHp;
                pokemon.CureStatusCondition();
                pokemon.CureVolatileStatusCondition();
            }

        }
       
    }

    //Cures a Status and health of a  pokemon
    public void healPokemon()
    {
        foreach (var pokemon in pokemons)
        {
          
                pokemon.HP = pokemon.MaxHp;
                pokemon.CureStatusCondition();
                pokemon.CureVolatileStatusCondition();


        }

    }

}
