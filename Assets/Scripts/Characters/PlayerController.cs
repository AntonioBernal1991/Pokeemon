using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Image transitionPanel;
    public string PlayerName => playerName;
    public Sprite PlayerSprite => playerSprite;

    private bool isMoving;
    public AudioClip caveClip, doorClip,healingClip;
    public Image CaveBackground;
    public float speed;

    private Vector2 input;
    public List<Hearts> heart;

    private Animator _animator;

    public LayerMask solidObjectsLayer, pokemonGrassLayer1, pokemonGrassLayer2, pokemonGrassLayer3, pokemonWaterLayer, Interactable, Door, Object, Object1, Object2, Object3;

    public event System.Action OnPokemonEncountered1; public event System.Action OnPokemonEncountered2; 
    public event System.Action OnPokemonEncountered3; public event System.Action OnPokemonEncountered4;

    public event System.Action<Collider2D> OnEnterTrainerFov;
    

    private float timeSinceLastClick;
    private float timeBetweenClicks = 0.25f;

    public PokemonParty playerParty;
    



    private void Awake()
    {
        _animator = GetComponent<Animator>();
       
    }
    
    //Receives the input of the keyboard to move and handles the action.
    public void HandleUpdate()
    {
        timeSinceLastClick += Time.deltaTime;
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0)
            {
                input.y = 0;
            }

            if(input != Vector2.zero)
            {  
                _animator.SetFloat("Move X",input.x);
                _animator.SetFloat("Move Y", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                if (isAvalaible(targetPosition))
                {
                    StartCoroutine(MoveTowards(targetPosition));
                }
               
                

            }
        }

        if (Input.GetAxisRaw("Submit") != 0)
        {

                if(timeSinceLastClick >= timeBetweenClicks)
                Interact();
            
        }
        if (Input.GetAxisRaw("Jump") != 0)
        {

            if (timeSinceLastClick >= timeBetweenClicks)
            Fish(); 
        }


    }
    //Sets the movements animation.
    private void LateUpdate()
    {
        
        
        if(input == Vector2.zero ) { { _animator.SetBool("Is Moving", false); } }
        else  
        {
             _animator.SetBool("Is Moving", true);
            _animator.SetBool("IsFishing", false);
        }


    }




    //transitions the movement of the player point by point so that the movement is not abrupt
    IEnumerator MoveTowards(Vector3 destination)
    {  
        isMoving = true;
        while(Vector3.Distance(transform.position,destination) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                destination,speed * Time.deltaTime);
            yield return null;  
        }
        transform.position = destination;
        isMoving = false;
     
       OnMoveFinish();
       


    }
    //checks different tasks.
    void OnMoveFinish()
    {
        CheckForPokemon1();
        CheckForPokemon2();
        CheckForPokemon3();
        CheckForInTrainerFoV();
        StartCoroutine(CheckForEntrances());
        CheckForObject();
    }


    //Checks if the path is blocked
    private bool isAvalaible(Vector3 target)
    {
        if (Physics2D.OverlapCircle(target,0.35f,solidObjectsLayer | Interactable| pokemonWaterLayer) != null)
        { return false; }
        return true;
    }

    //Allows to interact with different npcs
    private void Interact()
    {
        timeSinceLastClick = 0;

        var facingDirection = new Vector3(_animator.GetFloat("Move X"),
            _animator.GetFloat("Move Y"));
        var interactPosition = transform.position + facingDirection;

        Debug.DrawLine(transform.position, interactPosition,Color.magenta,1.0f);
        var collider = Physics2D.OverlapCircle(interactPosition,0.2f,Interactable);
        if (collider != null)
        {
            collider.GetComponent<InteractableInterface>()?.Interact(transform.position);

        }



    }

    [SerializeField] private float verticalOffset = 0.2f;

    //While walking over the grass , it runs a probabilty of encountering a pokemon.
    public void CheckForPokemon1()
    {
        if (Physics2D.OverlapCircle(transform.position-new Vector3(0,verticalOffset), 0.25f, pokemonGrassLayer1) != null)
        {
            if (Random.Range(0, 100) < 7)
            {
                input = Vector2.zero;
                OnPokemonEncountered1();
                
                
            }
            
        }
        
    }
    //While walking over the grass , it runs a probabilty of encountering a pokemon.
    public void CheckForPokemon2()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, pokemonGrassLayer2) != null)
        {
            if (Random.Range(0, 100) < 7)
            {
                input = Vector2.zero;
                OnPokemonEncountered2();
                
                
            }
        }
       
    }
    //While walking over the grass , it runs a probabilty of encountering a pokemon.
    public void CheckForPokemon3()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, pokemonGrassLayer3) != null)
        {
            if (Random.Range(0, 100) < 7)
            {
                input = Vector2.zero;
                OnPokemonEncountered3();
               
            }
        }
        
    }
    //While fishing , it runs a probabilty of encountering a  water pokemon.
    private void Fish()
    {


        _animator.SetBool("IsFishing", true);
        _animator.SetTrigger("Fish");

        timeSinceLastClick = 0;

        var facingDirection = new Vector3(_animator.GetFloat("Move X"),
            _animator.GetFloat("Move Y"));
        var interactPosition = transform.position + facingDirection;

        Debug.DrawLine(transform.position, interactPosition, Color.magenta, 1.0f);
        var collider = Physics2D.OverlapCircle(interactPosition, 0.2f, pokemonWaterLayer);
        if (collider != null)
        {
            
            if (Random.Range(0, 100) < 30)
            {
                OnPokemonEncountered4();

            }
        }
    }
    //While walking, it checks if we encounter a trainer battle.
    private void CheckForInTrainerFoV()
    {
        var collider = Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.8f, GameLayers.SharedInstance.FovLayer);

        if (collider != null)


        {
            input = Vector2.zero;
            OnEnterTrainerFov?.Invoke(collider);
           
        }
    }

    //Checks if theres a entrance to a cave or to a house
    IEnumerator CheckForEntrances()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, Door) != null)
        {
            SoundManager.SharedInstance.PlaySound(doorClip);
            yield return transitionPanel.DOFade(1.0f, 1.0f).WaitForCompletion();
            CaveBackground.gameObject.SetActive(true);
            transportPlayer(641.35f, -5.6f, 0f,1);
            SoundManager.SharedInstance.PlayMusic(caveClip);
            yield return transitionPanel.DOFade(0.0f, 1.0f).WaitForCompletion();
          

        }
        
    }



    //Checks if we cross with an object.
    public void CheckForObject()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, Object) != null)
        {

            this.playerParty.healPokemon();
            heart[0].DestroyGameObject();
             SoundManager.SharedInstance.PlaySound(healingClip);
        }
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, Object1) != null)
        {

            this.playerParty.healPokemon();
            heart[1].DestroyGameObject();
            SoundManager.SharedInstance.PlaySound(healingClip);
        }
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, Object2) != null)
        {

            this.playerParty.healPokemon();
            heart[2].DestroyGameObject();
            SoundManager.SharedInstance.PlaySound(healingClip);
        }
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0, verticalOffset), 0.25f, Object3) != null)
        {

            this.playerParty.healPokemon();
            heart[3].DestroyGameObject();
            SoundManager.SharedInstance.PlaySound(healingClip);
        }
    }


    //After loosing every pokemon, this function get us back to the beggining of the game.
    public void transportPlayer(float x,float y,float z,int dir)
    {
        this.transform.position = new Vector3(x, y, z);
        _animator.SetFloat("Move Y", dir);
    }

}
