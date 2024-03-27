using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int gridRows = 2;
    public int gridCols = 4;
    public float offsetX = 2f;
    public float offsetY = 2.5f;

    public float startTime = 20f;
    public bool timerOn = false;
    public bool flippable = true;

    private int score = 0;
    private int MAXscore = 0; //keeps track of max potential score

    // Bool for Time Attack on or off
    [SerializeField]
    public bool timeAttackMode = false;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text timer;

    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;

    [SerializeField] private Sprite[] alternateImages; //Second Card deck
    private bool usingAlternateImages = false;


    //serialize field for initializing deck of cards
    [SerializeField] int[] cardIdsForDeck = { 0, 1, 2, 3 };

    //SPRITE PACK FIELDS
    [SerializeField] private Sprite defaultCardBackSprite;
    [SerializeField] private Sprite alternateCardBackSprite;

    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite alternateBackground;
    [SerializeField] private SpriteRenderer backgroundRenderer; 


    [SerializeField] bool useAlternatePack = false;



    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        timerOn = false;//*******************************
        Time.timeScale = 1.0f;
    }

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
            timerOn = true;//*******************************
        }
        else
        {
            secondRevealed = card;
            //Debug.Log("Match? " + (firstRevealed.Id == secondRevealed.Id));
            StartCoroutine(CheckMatch());
        }
    }

    public void Timer()
    {
        // Set Minutes and seconds in seperate variables
        // Insures the time is displayed correctly
        float minutes = Mathf.FloorToInt(startTime / 60);
        float seconds = Mathf.FloorToInt(startTime % 60);

        // Updated to display minutes
        timer.text = string.Format("Timer: {0:00} : {1:00}", minutes, seconds);

        if (score == MAXscore)
        {
            // If Time Attack Mode is on, restart board and continue
            if (timeAttackMode)
            {
                ClearBoard();
                SetupBoard();
            }

        }

        if (timerOn)
        {
            if (startTime > 0 && score < MAXscore)
            {
                startTime -= Time.deltaTime;
            }
            else
            {
                startTime = 0;
                timerOn = false;
                Time.timeScale = 0.0f;
                flippable = false;
            }
        }
    }

    private IEnumerator CheckMatch()
    {
        if (firstRevealed.Id == secondRevealed.Id)
        {
            score++;
            scoreLabel.text = $"Score:  {score}";

            // If Time Attack, add to timer
            if (timeAttackMode)
            {
                startTime += 3f;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    public void SetupBoard()
    {

        Vector3 startPos = originalCard.transform.position;
        int[] numbers = ProduceDeck(cardIdsForDeck);
        numbers = ShuffleArray(numbers);
        ClearBoard(); // Ensure the board is cleared at the beginning.
        int cardsAdded = 0;

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                if (cardsAdded >= numbers.Length)
                {
                    continue;
                }

                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = cardsAdded;
                int id = numbers[index];
                card.SetCard(id, images[id]);
                card.Unreveal();

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
                cardsAdded++;
            }
        }
        // Correctly calculate MAXscore based on the number of unique cards.
        MAXscore += cardsAdded / 2;
        Debug.Log($"MAXscore updated to {MAXscore}.");
    }


    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }


    private int[] ProduceDeck(int[] numbers)
    {
        int[] newArray = new int[numbers.Length * 2];
        for (int i = 0; i < newArray.Length; i += 2)
        {
            newArray[i] = numbers[i / 2];
            newArray[i + 1] = numbers[i / 2];
        }
        return newArray;
    }


    // Method to shuffle an array of numbers
    // This is an implmentation of the Knuth shuffle algorithm
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int rand = Random.Range(i, newArray.Length);
            newArray[i] = newArray[rand];
            newArray[rand] = temp;
        }
        return newArray;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }
    private void ClearBoard()
    {
        // Find all card instances in the scene.
        MemoryCard[] cards = FindObjectsOfType<MemoryCard>();

        foreach (MemoryCard card in cards)
        {
            // Avoid destroying the original card template if it's in the scene.
            // Check if the card is the originalCard, and skip it if so.
            if (card != originalCard)
            {
                Destroy(card.gameObject);
            }
        }

    }

    public void SwitchCardSets()
    {
        usingAlternateImages = !usingAlternateImages; // Toggle the boolean value.
        // --DEBUG LINE-- Debug.Log($"Switching card sets. Using alternate images: {usingAlternateImages}"); 

        MemoryCard[] allCards = FindObjectsOfType<MemoryCard>();
        foreach (MemoryCard card in allCards)
        {
            int id = card.Id;
            Sprite newFrontSprite = usingAlternateImages ? alternateImages[id] : images[id]; //if else (I'm not consistent ik)
            card.SetCard(id, newFrontSprite);
            card.UpdateCardBack(usingAlternateImages);
        }
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        if (usingAlternateImages)
        {
            backgroundRenderer.sprite = alternateBackground;
        }
        else
        {
            backgroundRenderer.sprite = defaultBackground;
        }
    }


    public Sprite GetCurrentCardBackSprite()
    {
        
        return useAlternatePack ? alternateCardBackSprite : defaultCardBackSprite;
    }
}