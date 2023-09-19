using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SelectionMovementUi : MonoBehaviour
{
    [SerializeField] private  Text[] movementTexts;
    [SerializeField] private Color selectedColor;
    

    private int currentSelectedMovement = 0;

   

    //Shows the different attacks on the battle attack menu.
    public void SetMovements(List<MoveBase> pokemonMoves, MoveBase newMove)
    {
        currentSelectedMovement = 0;
        for (int i = 0; i < pokemonMoves.Count ; i++)
        {
            movementTexts[i].text = pokemonMoves[i].Name;
        }
        movementTexts[pokemonMoves.Count].text = newMove.Name;
    }
    //Handles the moves you want to forget after leveling up.
    public void HandleForgetMoveSelection(Action<int> onSelected)
    {
       

        if (Input.GetAxisRaw("Vertical") !=0)
        {
            int direction = Mathf.FloorToInt(Input.GetAxisRaw("Vertical"));
            currentSelectedMovement -= direction;         
            onSelected?.Invoke(-1);  
        }
        currentSelectedMovement = Mathf.Clamp(currentSelectedMovement,
                0, PokemonBase.Number_Of_Learnable_Moves);
        UpdateColorForgetMoveSelection(currentSelectedMovement);

        if (Input.GetAxisRaw("Submit") != 0)
        {
            
            onSelected?.Invoke(currentSelectedMovement);
        }

    }
    //Paints the different forgetable attacks so you can navegate through them.
    public void UpdateColorForgetMoveSelection(int selectedMove)
    {
        for (int i = 0; i <= PokemonBase.Number_Of_Learnable_Moves; i++)
        {
            movementTexts[i].color = (i == selectedMove ? Color.red: Color.black);
        }
    }
}