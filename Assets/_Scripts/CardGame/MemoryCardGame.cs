using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class MemoryCardGame : Minigame
{
    [SerializeField]
    private SimpleCountdownTimer gameTimer;
    [SerializeField]
    private CardManager cardManager;
    [SerializeField]
    private GridGroupItemRandomizer gridRandomizer;
    [SerializeField]
    private GraphicRaycastManager graphicRaycastManager;
    [SerializeField]
    private SelectionManager selectionManager;
    [SerializeField]
    private float IncorrectPairResetTime = 0.5f;
    private bool isCardsResetting = false;

    private int completedPairCount = 0;

    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private GameObject LosePanel;

    private DifficultyManager difficultyManager;


    private List<GameObject> selectedObjectList = new List<GameObject>();

    public void SetupGame()
    {
        if (difficultyManager != null)
        {
            int Difficulty = DifficultyManager.instance.GetDifficulty() + 1;
            float gameDuration = Difficulty * 5;
            cardManager.SetCardCount(Difficulty);
            gameTimer.SetTimerDuration(gameDuration);
        }
    }

    public override void StartMinigame()
    {
        StartCoroutine(IInitializeBoard());
    }

    private IEnumerator IInitializeBoard()
    {
        SetupGame();
        cardManager.GenerateCards();
        gridRandomizer.RandomizeItemPosition(cardManager.GetCardContainer());
        yield return null;
    }

    public void Start()
    {
        difficultyManager = DifficultyManager.instance;
        graphicRaycastManager = GraphicRaycastManager.instance;
        selectionManager = SelectionManager.instance;

        StartMinigame();
    }

    #region Pairing check handlers

    public bool IsCorrectPairingSequence()
    {
        bool isCorrect = true;
        List<GameObject> selectionList = selectedObjectList;
        Card baseCard = selectionList[0].GetComponent<Card>();
        for (int i = 0; i < selectionList.Count; i++)
        {
            if (selectionList[i].GetComponent<Card>().Id != baseCard.Id)
            {
                isCorrect = false;
                break;
            }
        }
        return isCorrect;
    }

    public void OnCorrectPairing()
    {

    }

    #endregion

    #region Cancel pairing handlers

    public void OnCancelPairingSequence()
    {
        CancelSelectedCards();
    }

    public bool IsCancellingPairSequence()
    {
        bool isCancelling = false;
        List<GameObject> selectionList = selectedObjectList;
        if (selectionList.Count != selectionList.Distinct().Count())
        {
            isCancelling = true;
        }
        return isCancelling;
    }

    public void CancelSelectedCards()
    {
        StartCoroutine(ICancelSelectedCards());
    }

    public IEnumerator ICancelSelectedCards()
    {
        if (isCardsResetting) yield break;
        isCardsResetting = true;
        SelectionManager.instance.SetSelectState(false);
        for (int i = 0; i < selectedObjectList.Count; i++)
        {
            selectedObjectList[i].GetComponent<Selectable>().OnDeselect();
        }
        SelectionManager.instance.ClearSelection();
        yield return new WaitForSeconds(0.1f);
        SelectionManager.instance.SetSelectState(true);
        isCardsResetting = false;
        yield break;
    }

    #endregion

    #region Completed pairing handlers

    public void OnCompletedPairing()
    {
        StartCoroutine(IOnCompletedPairing());
    }

    private IEnumerator IOnCompletedPairing()
    {
        Debug.Log("COMPLETED CORRECT PAIR!");
        for (int i = 0; i < selectedObjectList.Count; i++)
        {
            selectedObjectList[i].GetComponent<Selectable>().SetActive(false);
            selectedObjectList[i].GetComponent<Selectable>().ForceDisable();
        }
        completedPairCount += 1;
        SelectionManager.instance.ClearSelection();
        yield break;
    }


    #endregion

    #region Reset card / pairing handlers
    public void ResetSelectedCards(float delay)
    {
        if (isCardsResetting) return;
        StartCoroutine(IResetSelectedCards(delay));
    }

    public IEnumerator IResetSelectedCards(float delay)
    {
        if (isCardsResetting) yield break;
        isCardsResetting = true;
        SelectionManager.instance.SetSelectState(false);
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < selectedObjectList.Count; i++)
        {
            selectedObjectList[i].GetComponent<Selectable>().OnDeselect();
        }
        SelectionManager.instance.ClearSelection();
        yield return new WaitForSeconds(0.1f);
        SelectionManager.instance.SetSelectState(true);
        isCardsResetting = false;
        yield break;
    }

    #endregion

    #region Game completion handlers

    private bool IsAllPairsCompleted()
    {
        bool isComplete = false;

        if (completedPairCount >= cardManager.GetCardCount())
        {
            isComplete = true;
        }

        return isComplete;
    }

    private void OnGameWin()
    {
        WinPanel.SetActive(true);
        gameTimer.PauseTimer();
        Debug.Log("GAME WIN!");
    }

    private void OnGameLose()
    {
        LosePanel.SetActive(true);
        Debug.Log("GAME LOSE!");
    }

    #endregion
    public void AddToPairingAttemptCount()
    {
        ClickCounter.instance.AddClick();
    }

    public void Update()
    {
        selectedObjectList = SelectionManager.instance.GetSelectedObjects();
        bool isPairingSequenceStarted = selectedObjectList.Count > 1;
        bool isCompletePair = selectedObjectList.Count == cardManager.GetCardPairingCount();

        if (SelectionManager.instance.IsListUpdated() && !IsCancellingPairSequence())
        {
            if (isPairingSequenceStarted)
            {
                if (IsCorrectPairingSequence())
                {
                    OnCorrectPairing();

                    if (isCompletePair)
                    {
                        OnCompletedPairing();

                        if (IsAllPairsCompleted() && WinPanel.activeSelf == false)
                        {
                            OnGameWin();
                        }
                    }
                }
                else if (!IsCorrectPairingSequence())
                {
                    ResetSelectedCards(IncorrectPairResetTime);
                    Debug.Log("INCORRECT PAIRING");
                }
            }
            AddToPairingAttemptCount();
        }
        else if (SelectionManager.instance.IsListUpdated() && IsCancellingPairSequence())
        {
            OnCancelPairingSequence();
            Debug.Log("PAIRING CANCELLED!");
            AddToPairingAttemptCount();
        }

        if(gameTimer.GetTimeRemaining() <= 0 && gameTimer.isTimerRunning)
        {
            OnGameLose();
        }

    }

    public override void RestartMinigame()
    {
        completedPairCount = 0;
        SelectionManager.instance.ClearSelection();
        StartCoroutine(IInitializeBoard());
        gameTimer.UnpauseTimer();
        gameTimer.RestartTimer();
        ClickCounter.instance.ResetClickCount();
    }

    public void PauseGame()
    {
        gameTimer.PauseTimer();
    }

    public void UnpauseGame()
    {
        gameTimer.UnpauseTimer();
    }



}
