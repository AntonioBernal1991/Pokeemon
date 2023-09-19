using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base;
    public PokemonBase Base
    {
        get => _base;
    }

    [SerializeField] private int _level;

    public int Level
    {
        get => _level;
        set => _level = value;
    }

    private List<Move> _moves;

    public Move CurrentMove { get; set; }
    public List<Move> Moves
    {
        get => _moves;
        set => _moves = value;
    }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat,int> StatsBoosted { get; private set; }
    public StatusCondition StatusCondition { get;  set; }
    public int StatusNumTurns { get; set; }
    public StatusCondition VolatileStatusCondition { get; set; }
    public int VolatileStatusNumTurns { get; set; }
    public Queue<string> StatusChangeMessages {get; private set; }  = new Queue<string>();

    public event Action OnStatusConditionChanged;
    public bool HasHpChanged { get;  set; } = false;

    public int previousHPValue;

   

    private int _hp;

    public int HP
    {
        get => _hp;
        set {
            _hp = value;
            _hp = Mathf.FloorToInt(Mathf.Clamp(_hp, 0, MaxHp));
        }

    }
    private int _experience;
    public int Experience { get => _experience; set => _experience = value; }

    public Pokemon(PokemonBase pBase, int plevel)
    {
        _base = pBase;
        _level = plevel;
        InitPokemon();
    }
    //Sets a new pokemon with all its features.
    public void InitPokemon()
    {

      
        _experience = Base.GetNecessaryExpForLevel(_level);


        _moves = new List<Move>();

        foreach (var lmove in _base.LearnableMoves)
        {
            if (lmove.Level <= _level)
            {
                _moves.Add(new Move(lmove.Move));
            }
            if (Moves.Count >= PokemonBase.Number_Of_Learnable_Moves)
            {
                break;
            }
        }
        CalculateStats();
        _hp = MaxHp; 
        previousHPValue = MaxHp;
        HasHpChanged = true;

        resetBoostings();
        StatusCondition = null;
        VolatileStatusCondition = null;
    }
    //Resets stats after being weaked or boosted
    void resetBoostings()
    {
        StatusChangeMessages = new Queue<string>();
        StatsBoosted = new Dictionary<Stat, int>()
        {
            { Stat.Attack, 0 },
            { Stat.Defense, 0 },
            { Stat.SpAttack, 0 },
            { Stat.SpDefense, 0 },
            { Stat.Speed, 0 },
            { Stat.Accuracy, 0 },
            { Stat.Evasion, 0 },
        };
    }

    //Calculates the stats of each pokemon depending on its level.
    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((_base.Attack * _level) / 100.0f) + 2);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((_base.Defense * _level) / 100.0f) + 2);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((_base.SpAttack * _level) / 100.0f) + 2);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((_base.SpDefense * _level) / 100.0f) + 2);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((_base.Speed * _level) / 100.0f) + 2);
        
        MaxHp = Mathf.FloorToInt((_base.MaxHp * _level) / 20.0f) + 10;
    }
    //Gets the stat of the pokemon
    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];

        int boost = StatsBoosted[stat];
        float multiplier = 1.0f + Mathf.Abs(boost) / 2.0f;

        if (boost >= 0)
        {
           
            statValue = Mathf.FloorToInt(statValue * multiplier);
        }
        else
        {
           
            statValue = Mathf.FloorToInt(statValue / multiplier);
        }

        return statValue;
    }
    //Applies the boost to a stat.
    public void ApplyBoost(StatBoosting boost)
    {
        
        
       var stat = boost.stat;
       var value = boost.boost;

       StatsBoosted[stat] = Mathf.Clamp(StatsBoosted[stat] + value,-6,6);
        if (value > 0)
        {
            StatusChangeMessages.Enqueue($"{stat} of {Base.Name} rose.");
            new WaitForSecondsRealtime(1);
            
        }
        else if (value < 0)
        {
            StatusChangeMessages.Enqueue($"{stat} of {Base.Name} was reduced. ");
        }
        else
        {
            StatusChangeMessages.Enqueue($"{Base.Name} don´t notice any effect");
         
        }

        Debug.Log($"{stat} its modified to  {StatsBoosted[stat]}");
       
        
    }

    public int MaxHp  { get; private set; }
    public int Attack => GetStat(Stat.Attack);
    public int Defense => GetStat(Stat.Defense);
    public int SpAttack => GetStat(Stat.SpAttack);
    public int SpDefense => GetStat(Stat.SpDefense);
    public int Speed => GetStat(Stat.Speed);

    //Receives damage from an attacker , with concern to his stats , weakness , strenghts , type of damage...
    public DamageDescription ReceiveDamage(Pokemon attacker, Move move)
    {

        float critical = 1f;
        if (Random.Range(0f,100f)<8f)
        {
            critical = 2f;
        }

        float type = TypeMatrix.GetMultEffectiveness(move.Base.Type, this.Base.Type);

        var damageDesc = new DamageDescription()
        {
            Critical = critical,
            Type = type,
            Fainted = false
           
        };

        float attack = (move.Base.isSpecialMove ? attacker.SpAttack : attacker.Attack);
        float defense = (move.Base.isSpecialMove ? this.SpDefense : this.Defense);

        float modifiers = Random.Range(0.85f, 1.0f)*type * critical;


        float baseDamage = ((2 * attacker.Level / 5f + 2) * move.Base.Power * ((float)attack/defense)) / 50f + 2;
        int totalDamage = Mathf.FloorToInt(baseDamage * modifiers);
        UpdateHp(totalDamage);
        
        if (HP <= 0)
        {

            damageDesc.Fainted = true;
        }

        return damageDesc;
       
    }
    //Updates Health after receiving damage
    public void UpdateHp(int damage)
    {
        HasHpChanged = true;
        previousHPValue = HP;
        HP -= damage;
        if (HP <= 0)
        {

            HP = 0;    
        }
    }
    //Sets Status condition after altering it with an attack.
    public void SetConditionStatus(StatusConditionID id)
    {
        if (StatusCondition != null)
        {
            return;
        }
        StatusCondition = StatusConditionsFactory.StatusConditions[id];
        StatusCondition?.OnApplyStatusCondition?.Invoke(this);
        StatusChangeMessages.Enqueue($"{Base.Name} {StatusCondition.StartMessage}");
        OnStatusConditionChanged?.Invoke();
    }
    //Sets a temporal Status condition after altering it with an attack.
    public void SetVolatileConditionStatus(StatusConditionID id)
    {
        if (VolatileStatusCondition != null)
        {
            return;
        }
        VolatileStatusCondition = StatusConditionsFactory.StatusConditions[id];
        VolatileStatusCondition?.OnApplyStatusCondition?.Invoke(this);
        StatusChangeMessages.Enqueue($"{Base.Name} {VolatileStatusCondition.StartMessage}");
    }
    //Cures a volatile status 
    public void CureVolatileStatusCondition()
    {
        VolatileStatusCondition = null;
        
    }

    //Chooses a random attack for  the Enemy pokemon.
    public Move RandomMove()
    {
        var movesWithPp = Moves.Where(m => m.Pp > 0).ToList();
        if(movesWithPp.Count>0)
        {
            int randId = Random.Range(0, Moves.Count);
            Debug.Log(Moves[randId].Base.Name);
            return Moves[randId];
        }
       
         return null;
        
        
    }
   
    //Checks if the pokemon levels up and  cures the pokemon if yes.
    public bool NeedsToLevelUp()
    {
        if (Experience > Base.GetNecessaryExpForLevel(_level+1))
        {
            int currentMaxHP = MaxHp;
            _level++;
            HP += ( MaxHp - currentMaxHP);
            
            return true;
        }
        else
        {
            return false;
        }
           
    }
    //Checks the movements that a pokemon can learn on each level
    public LearnableMove GetLearnableMoveAtCurrentLevel()
    {
        return Base.LearnableMoves.Where(lm => lm.Level == _level).FirstOrDefault();
    }
    //Adds the movement that a pokemon can learn 
    public void LearnMove(LearnableMove learnableMove)
    {
        if (Moves.Count >= PokemonBase.Number_Of_Learnable_Moves)
        {
            return;
        }
        Moves.Add(new Move(learnableMove.Move));
        
    }
    //Cures the the different states of a pokemon 
    public void CureStatusCondition()
    {
        StatusCondition = null;
        OnStatusConditionChanged?.Invoke();
    }
    //Allows to perform a movement  or not at the beggining of a turn if the status condition that the pokemon suffers
    public bool OnStartTurn()
    {
        bool canPerformMovement = true;

        if (StatusCondition?.OnStartTurn != null)
        {
            if (!StatusCondition.OnStartTurn(this))
            {
                canPerformMovement = false;
            }
        }

        if (VolatileStatusCondition?.OnStartTurn != null)
        {
            if (!VolatileStatusCondition.OnStartTurn(this))
            {
                canPerformMovement = false;
            }
        }

        return canPerformMovement;
    }
    //Checks at the end of the  turn if the pokemon suffers a status conditions 
    public void OnFinishTurn()
    {
        StatusCondition?.OnFinishTurn?.Invoke(this);
        VolatileStatusCondition?.OnFinishTurn?.Invoke(this);
    }
    //Resets the stats of a pokemon after a battle.
    public void OnBattleFinish()
    {
        VolatileStatusCondition = null;
        resetBoostings();
    }





}

public class DamageDescription
{
    public float Critical { get; set; }
    public float Type { get; set; }
    public bool Fainted { get; set; }   


}

