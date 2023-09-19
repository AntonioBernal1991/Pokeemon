using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PartyHUD : MonoBehaviour
{
    [SerializeField] private Text messageText;
    private PartyMemberHUD[] memberHuds;

    private List<Pokemon> pokemons;
    
    public void InitPartyHUD()
    {
        memberHuds = GetComponentsInChildren<PartyMemberHUD>(true);
    }
        
    //Sets the info of your pokemon party.
    public void SetPartyData(List<Pokemon> pokemons)
     {
        
        messageText.text = "Select a Pokemon...";
        for (int i = 0; i < memberHuds.Length; i++)

        {
            
            if (i < pokemons.Count)
            {
                
                memberHuds[i].SetPokemonData(pokemons[i]);
                memberHuds[i].gameObject.SetActive(true);
            }
            else
            {
                memberHuds[i].gameObject.SetActive(false);
            }
        }

     }
    
    
  //Update the Selection of the pokemon in the pokemon party.
    public void UpdateSelectedPokemon(int selectedPokemon)
    {
        for (int i = 0; i < 6; i++)
        {
            memberHuds[i].SetSelectedPokemon(i == selectedPokemon);
            
        }
       

    }
    //Sets the text in the pokemon party menu.
    public void SetMessage(string message)
    {
        messageText.text = message;
    }







}
