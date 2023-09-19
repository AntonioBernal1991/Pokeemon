using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class BattleUnit : MonoBehaviour
{
  public PokemonBase _base;
  private MoveBase _move;

 

    public List<ParticleSystem> particles;
    public List<ParticleSystem> attackParticles;


    public int _level;
  
  [SerializeField] bool isPlayer;
  [SerializeField] BattleHud hud;
  


  [SerializeField] BattleUnit attacker;
    public Pokemon Pokemon { get; set; }

    public bool IsPlayer => isPlayer;
  
    public BattleHud Hud => hud;


    private Image pokemonImage;

    public List<Image> attackBackGroundImage;
   


    private Vector3 initialPosition;

    private Color initialColor;

    [SerializeField]
    float startTimeAnim , attackTimeAnim , dieTimeAnim  , capturedTimeAnim = 0.6f,
    hitTimeAnim ; 

    
    private void Awake()
    {
         

         pokemonImage = GetComponent<Image>();
        initialPosition = pokemonImage.transform.localPosition;
        initialColor = pokemonImage.color; 
    }
    //Sets up the pokemon sprite thats going to battle.
    public void SetUpPokemon(Pokemon pokemon)
    {
        Pokemon = pokemon;
  
        pokemonImage.sprite =
            (isPlayer ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite);
        pokemonImage.color = initialColor;
        pokemonImage.transform.localPosition = initialPosition;

        hud.gameObject.SetActive(true);
        hud.SetPokemonData(pokemon);
      
       


        PlayStartAnimation();
    }
    // Clears up the Hud
    public void ClearHUD()
    {
        hud.gameObject.SetActive(false);
    }
    //Plays the animation of the two pokemon that are going to fight.
    public void  PlayStartAnimation()
    {
     
        
           pokemonImage.transform.localPosition = 
            new Vector3( initialPosition.x+(isPlayer? - 1:1) * 20,initialPosition.y);

        pokemonImage.transform.DOLocalMoveX(initialPosition.x, 1.5f);
    }

    //Plays the differents animations of each attack, depending on its type or name.
    public void PlayAttackAnimation()
    {
        var seq = DOTween.Sequence();



        if (this.Pokemon.CurrentMove.Base.MoveType == MoveType.Special && this.Pokemon.CurrentMove.Base.Type != PokemonType.Psychic)
        {

            seq.Append(attackBackGroundImage[0].DOFade(1, 0.5f)).WaitForCompletion();
            if (this.Pokemon.CurrentMove.Base.Name == "Flamethrower") { attackParticles[0].Play(); }
            else if (this.Pokemon.CurrentMove.Base.Name == "Bubble") { attackParticles[1].Play(); }
            else if (this.Pokemon.CurrentMove.Base.Name == "Water Gun") { attackParticles[2].Play(); }
            
        }


        else if (this.Pokemon.CurrentMove.Base.Name == "Swift") 
        {
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x + (isPlayer ? 1.5f : -1) * 2, 0.03f));
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x, 0.03f));
        }
        else if (this.Pokemon.CurrentMove.Base.Name == "Tail Whip")
        {
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x + (isPlayer ? 0.5f : - 0.5f) * 2, 0.35f));           
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x + (isPlayer ? - 0.5f : + 0.5f) * 2, 0.35f));
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x, 0.35f)).WaitForCompletion();
        }
        else if (this.Pokemon.CurrentMove.Base.Type == PokemonType.Psychic)
        {
            if (this.Pokemon.CurrentMove.Base.Name == "Psychic")
            {
                seq.Append(attackBackGroundImage[1].DOFade(1, 0.5f)).WaitForCompletion();
            }


        }

        else
        {
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x + (isPlayer ? 0.5f : -0.5f) * 2, 0.35f));
            seq.Append(pokemonImage.transform.DOLocalMoveX(initialPosition.x, 0.35f)).WaitForCompletion();
        }


    }
    public void ReceiveAttackAnimation()
    {
      
        if (attacker.Pokemon.CurrentMove.Base.Name == "Poison Sting") { particles[2].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Vine Whip") { particles[4].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Razor Leaf") { particles[5].Play(); }
    
        if (attacker.Pokemon.CurrentMove.Base.Name == "Sleep Powder") { particles[7].Play(); }
   
        if (attacker.Pokemon.CurrentMove.Base.Name == "Thunderbolt" || attacker.Pokemon.CurrentMove.Base.Name == "Thunder") { particles[9].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Poison Powder") { particles[10].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Thunder Wave") { particles[11].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Thunder Shock") { particles[15].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Acid Spray" || attacker.Pokemon.CurrentMove.Base.Name == "Acid") { particles[16].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Solar Beam") { particles[17].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Name == "Stun Spore") { particles[28].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Fire) { particles[19].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Flying) { particles[20].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Bug) { particles[22].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Ice) { particles[24].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Rock || attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Ground) { particles[26].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Dark) { particles[27].Play(); }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Psychic) { particles[13].Play(); }



        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Dragon)
        {
            if (attacker.Pokemon.CurrentMove.Base.Name == "Hyper Beam") 
            {
                particles[12].Play();
            }

            else
            { 
              particles[21].Play();
            }
        }
        

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Ghost)
        {
            if (attacker.Pokemon.CurrentMove.Base.Name == "Night Shade")
            {
                particles[0].Play();
            }
            else
            {
                particles[23].Play();
            }

        }

        
        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Water)
        {

            
                particles[18].Play();
            

        }

        if (attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Normal || attacker.Pokemon.CurrentMove.Base.Type == PokemonType.Fight)
        {
            if (attacker.Pokemon.CurrentMove.Base.Name == "Supersonic")
            {
                particles[13].Play();
            }
            else if (attacker.Pokemon.CurrentMove.Base.Name == "Slash")
            { 
                particles[14].Play(); 
            }
            else if (attacker.Pokemon.CurrentMove.Base.Name == "Smokescreen")
            {
                particles[3].Play();
            }

            else if (attacker.Pokemon.CurrentMove.Base.Name == "Swift")
            {
                particles[1].Play();
            }
            else if (attacker.Pokemon.CurrentMove.Base.Name == "Double-Edge")
            {
                particles[6].Play();

            }

            else
            {

                particles[25].Play();

            }
        }
           
       //Changes the background to black for special attacks.

        var seq = DOTween.Sequence();
        seq.Append(pokemonImage.DOColor(Color.black, hitTimeAnim));
        seq.Append(pokemonImage.DOColor(initialColor, hitTimeAnim));
        seq.Append(pokemonImage.DOColor(Color.black, hitTimeAnim));
        seq.Append(pokemonImage.DOColor(initialColor, hitTimeAnim));
        seq.Append(pokemonImage.DOColor(Color.black, hitTimeAnim));
        seq.Append(pokemonImage.DOColor(initialColor, hitTimeAnim)).WaitForCompletion();
       
        seq.Append(attackBackGroundImage[0].DOFade(0, 0.5f)).WaitForCompletion();
        seq.Append(attackBackGroundImage[1].DOFade(0, 0.5f)).WaitForCompletion();
       

    }


    //Plays the faint animation with a fade effect.
    public void PlayFaintAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Append(pokemonImage.transform.DOLocalMoveY(initialPosition.y - 5, dieTimeAnim));
        seq.Join(pokemonImage.DOFade(0f, 0.5f));
    }

    //Plays the capturing animation.
    public IEnumerator PlayCapturedAnimation()

    {
        var seq = DOTween.Sequence();
        seq.Append(pokemonImage.DOFade(0, 0.5f));
        seq.Join(transform.DOScale(new Vector3(0.01f,0.01f,0.01f),0.6f));
        seq.Join(transform.DOLocalMoveY(initialPosition.y + 2f, 0.6f));
        
        seq.Append(transform.DOScale(new Vector3(0.03f, 0.03f, 0.03f), 0.6f));
        yield return seq.WaitForCompletion();
        
    }
    //Plays the escaping animation.
    public IEnumerator PlayEscapeAnimation()

    {
        var seq = DOTween.Sequence();
        seq.Append(pokemonImage.DOFade(1, 0.5f));
        seq.Join(transform.DOScale(new Vector3(0.03f, 0.03f, 0.03f), 0.6f));
        seq.Join(transform.DOLocalMoveY(initialPosition.y, 0.6f));
        yield return seq.WaitForCompletion();
    }
}
