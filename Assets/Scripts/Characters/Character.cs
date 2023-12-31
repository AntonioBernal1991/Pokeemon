using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed;
    private CharacterAnimator _animator;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<CharacterAnimator>();
    }

    //Moves character
   public  IEnumerator MoveTowards(Vector3 moveVector)
    {
        if (moveVector.x != 0 ) moveVector.y = 0;   

        _animator.MoveX = Mathf.Clamp(moveVector.x,-1,1);
        _animator.MoveY = Mathf.Clamp(moveVector.y, -1, 1);

        var targetPosition = transform.position;
        targetPosition.x += moveVector.x;
        targetPosition.y += moveVector.y;

        if (!IsPathAvalaible(targetPosition))
        {
            yield break;
        }


        _animator.IsMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        _animator.IsMoving = false;

        

       


    }
    //Checks if the path to walk is blocked.
    private bool IsPathAvalaible(Vector3 target)
    {

        var path = target - transform.position;
        var direction = path.normalized;

        return !Physics2D.BoxCast(transform.position+direction, 
            new Vector2(0.25f, 0.25f), 0f, direction, path.magnitude-1, GameLayers.SharedInstance.SolidObjectsLayer | GameLayers.SharedInstance.PlayerLayer);
            
      

    }
   
    //Makes the chacaracter look towards a target or direction.
    public void LookTowards(Vector3 target)
    {
        var diff = target - transform.position;
        var xdiff = Mathf.FloorToInt(diff.x);
        var ydiff = Mathf.FloorToInt(diff.y);

        if (xdiff == 0 || ydiff == 0)
        {
            _animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            _animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
        {
            _animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            _animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
    }
}
