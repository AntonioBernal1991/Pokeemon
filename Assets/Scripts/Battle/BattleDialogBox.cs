using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;


    [SerializeField]  GameObject actionSelect;
    [SerializeField]  GameObject movementSelect;
    [SerializeField]  GameObject movementDesc;
    [SerializeField] private GameObject yesNoBox;

    [SerializeField] List<Text> actionText;
    [SerializeField] List<Text> movementTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;
    [SerializeField] Text yesText, noText;

    public float charactersPerSecond = 120.0f;
    [SerializeField] Color selectedColor = Color.red;

    public bool isWriting  = false;

    //Shows the Dialog during the battle phase.
    public IEnumerator SetDialog(string message)
    {
        isWriting = true;
        dialogText.text = "";
        foreach (var character in message)
        {
            dialogText.text += character;
            yield return new WaitForSeconds(1 / charactersPerSecond);
        }
        yield return new WaitForSeconds(1f);
        isWriting = false;
    }
    //Activates the Dialog box.
    public void ToggleDialogText(bool activated)
    {
        dialogText.enabled = activated;
    }

    //Activates the Actions box 
    public void ToggleActions(bool activated)
    {
        actionSelect.SetActive(activated);
    }

    //Activates the attacks box.
    public void ToggleMovements(bool activated)
    {
        movementSelect.SetActive(activated);
        movementDesc.SetActive(activated);
    }

    //Activates the box to accept or denie actions.
    public void ToggleYesNoBox(bool activated)
    {
        yesNoBox.SetActive(activated);
    }

    //Changes color of the diferent actions to choose one.
    public  void SelectAction(int selectedAction)
    {
        for (int i = 0; i < actionText.Count; i++)
        {
            actionText[i].color = (i == selectedAction ? selectedColor : Color.black);
         

        }
    }
    //Shows the differents pokemon attacks of each pokemon.
    public void SetPokemonMovements(List<Move> moves)
    {
        for (int i = 0; i < movementTexts.Count; i++)
        {
            if (i < moves.Count)
            {
                movementTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                movementTexts[i].text = "---";
            }

        }

    }

    //Changes color of the diferent attacks to choose one.
    public void SelectMovement(int selectedMovement, Move move)
    {
        for (int i = 0; i < movementTexts.Count; i++)
        {
            movementTexts[i].color = (i == selectedMovement ? selectedColor : Color.black);

        }
        ppText.text = $"PP:{move.Pp}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString().ToUpper();

        ppText.color = (move.Pp <= 0 ? Color.red:Color.black);
    }
    
    //Changes color of the yes and No answer to choose one.
    public void SelectYesNoAction(bool yesSelected)
    {
        if(yesSelected)
        {
            yesText.color = Color.red;
            noText.color = Color.black;
        }
        else
        {
            yesText.color = Color.black;
            noText.color = Color.red;
        }
    }




}