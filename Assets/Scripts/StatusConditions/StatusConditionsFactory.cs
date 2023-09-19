using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatusConditionsFactory : MonoBehaviour
{

    public static void InitFactory()
    {
        foreach (var condition in StatusConditions)
        {
            var id = condition.Key;
            var scond = condition.Value;
            scond.id = id;
        }
    }
    //Dictionary with the different possible status of a pokemon.
    public static Dictionary<StatusConditionID, StatusCondition> StatusConditions { get; set; } =
        new Dictionary<StatusConditionID, StatusCondition>()
        { {
            StatusConditionID.psn,
            new StatusCondition()
            {
                Name = "Poison",
                Description = "Causes the pokemon to take damage each turn.",
                StartMessage = "was poisoned.",
                OnFinishTurn = PoisonEffect
            }
          },
          {
                 StatusConditionID.brn,
            new StatusCondition()
            {
                Name = "Burn",
                Description = "Causes the pokemon to take damage each turn",
                StartMessage = "was burned.",
                OnFinishTurn = BurnEffect
            }
          },
          {
            StatusConditionID.slp,
            new StatusCondition()
            {
                Name = "Sleep",
                Description = "Makes the Pokemon sleep for a fixed number of turns.",
                StartMessage = "has fallen asleep",
                OnApplyStatusCondition = (Pokemon pokemon) =>
                {
                    pokemon.StatusNumTurns = Random.Range(1,4);
                    Debug.Log($"The pokemon will sleep{pokemon.StatusNumTurns} turns");
                },
                OnStartTurn = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusNumTurns <= 0)
                    {
                        pokemon.CureStatusCondition();
                        pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} has awakened!");
                        return true;

                    }
                    pokemon.StatusNumTurns--;
                    pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is still asleep.");
                    return false;
                }
            }
          },
             {
                StatusConditionID.par,
                new StatusCondition()
                {
                    Name = "Paralyzed",
                    Description = "Causes the Pokemon to be paralyzed during the turn.",
                    StartMessage = "was paralyzed",
                    OnStartTurn = ParalyzedEffect
                }
            },
            {
                StatusConditionID.frz,
                new StatusCondition()
                {
                    Name = "Frozen",
                    Description = "Causes the Pokemon to be frozen, but can be healed randomly for one turn.",
                    StartMessage = "was frozen",
                    OnStartTurn = FrozenEffect
                }
            },
                       {
                StatusConditionID.conf,
                new StatusCondition()
                {
                    Name = "Confusión",
                    Description = "It causes the Pokemon to be confused and may attack itself.",
                    StartMessage = "was confused",
                    OnApplyStatusCondition = (Pokemon pokemon) =>
                    {
                        pokemon.VolatileStatusNumTurns = Random.Range(1, 6);
                        Debug.Log($"The pokemon will be confused for {pokemon.VolatileStatusNumTurns} turns");
                    },
                    OnStartTurn = (Pokemon pokemon) =>
                    {
                        if (pokemon.VolatileStatusNumTurns<=0)
                        {
                            pokemon.CureVolatileStatusCondition();
                            pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is not confused any more");
                            pokemon.StatusChangeMessages.Enqueue($"                      ");
                            return true;
                        }

                        pokemon.VolatileStatusNumTurns--;
                        pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is still confused.");

                        if (Random.Range(0, 2) == 0)
                        {
                            return true;
                        }
                        //Debemos dañarnos a nosotros mismos por la confusión
                        pokemon.UpdateHp(pokemon.MaxHp/6);
                        pokemon.StatusChangeMessages.Enqueue("So confused that he hurts himself!");
                        return false;
                    }
                }
            }
        };




    //Diferent effects of the status conditions.

    static void PoisonEffect(Pokemon pokemon)
    {
        pokemon.UpdateHp(pokemon.MaxHp / 8);
        pokemon.StatusChangeMessages.Enqueue($"The poison affects the health of {pokemon.Base.Name}.  ");
        
    }
    static void BurnEffect(Pokemon pokemon)
    {
        pokemon.UpdateHp(pokemon.MaxHp / 15);
        pokemon.StatusChangeMessages.Enqueue($"The burn affects the health of  {pokemon.Base.Name}.  ");

    }
    static bool ParalyzedEffect(Pokemon pokemon)
    {
        if (Random.Range(0, 100) < 25)
        {
            pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is paralyzed and cannot move.");
            return false;
        }
        return true;
    }

    static bool FrozenEffect(Pokemon pokemon)
    {
        if (Random.Range(0, 100) < 25)
        {
            pokemon.CureStatusCondition();
            pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is not frozen any more");
            return true;
        }

        pokemon.StatusChangeMessages.Enqueue($"{pokemon.Base.Name} is still frozen.");
        return false;
    }

}

public enum StatusConditionID
{
   none, brn,frz,par,psn,slp,conf
}
