using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class HUD : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _promotion;
    [SerializeField] private string[] _texts;

    [SerializeField] private List<Transform> _transforms = new List<Transform>();

    private bool _isInitialized;

    private void Start()
    {
        _promotion.transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    private void UpdateScore()
    {
        _promotion.text = _texts[Random.Range(0, _texts.Length)];
        _promotion.transform.DOScale(1, 0.4f).SetLink(_promotion.gameObject).SetEase(Ease.OutBack);
        _promotion.transform.DOScale(0, 0.4f).SetLink(_promotion.gameObject).SetEase(Ease.InBack).SetDelay(0.6f);
    }
    public void Initialize()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        Show();

        _isInitialized = true;
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Hide;
        GameState.Instance.GamePaused += Hide;
        GameState.Instance.GameUnpaused += Show;

        GameState.Instance.ScoreAdded += UpdateScore;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Hide;
        GameState.Instance.GamePaused -= Hide;
        GameState.Instance.GameUnpaused -= Show;

        GameState.Instance.ScoreAdded -= UpdateScore;
    }
    private void Show()
    {
        _panel.SetActive(true);

        foreach (Transform transform in _transforms)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, Random.Range(0.2f, 0.7f)).SetEase(Ease.OutBack).SetLink(transform.gameObject).SetUpdate(true);
        }
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnButtonPauseClicked()
    {
        GameState.Instance.PauseGame();
    }
    public void OnExitButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
}