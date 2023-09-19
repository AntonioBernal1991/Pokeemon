using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;

    //Sets the life of a pokemon 
    public void SetHp(float normalizedValue)
    {
        healthBar.transform.localScale = new Vector3(normalizedValue, 0.1f,1.0f);
    }
    //Sets the life of pokemon with smooth transition.
    public IEnumerator SetSmoothHP(Pokemon pokemon)
    {
        float normalizedValue = (float)pokemon.HP / pokemon.MaxHp;
        yield return new WaitForSeconds(0.3f);
        var seq = DOTween.Sequence();
        seq.Append(healthBar.transform.DOScaleX(normalizedValue, 1f));
        new WaitForSeconds(0.5f);
     
      
        yield return seq.WaitForCompletion();


    }
}
