using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    [SerializeField] GameObject upgradeDirectorPrefab;
    [SerializeField] GameObject fightDirectorPrefab;
    [SerializeField] GameObject uiButtonPrefab;
    [SerializeField] GameObject uiTextPrefab;
    [SerializeField] Canvas _messageCanvas;
    [SerializeField] UIPositionMarker _textPM;
    [SerializeField] UIPositionMarker _buttonPM;
    [SerializeField] List<AudioSource> _audioSources;
    private GameState _gameState;
    private int _wins = 0;

    private void Start()
    {
        Load("restart");
    }

    public void Load(string state)
    {
        StartCoroutine(ChangeState(state));
    }

    public IEnumerator ChangeState(string state)
    {
        _messageCanvas.enabled = false;
        SceneManager.sceneLoaded += MakeLastSceneActive;
        yield return UnloadLastScene();
        switch (state)
        {
            case "newgame":
                yield return NewGame();
                break;
            case "fight":
                yield return Fight();
                break;
            case "victory":
                yield return Victory();
                break;
            case "defeat":
                yield return Defeat();
                break;
            case "restart":
                yield return MainMenu();
                break;
            default:
                throw new InvalidOperationException("Invalid state passed");
        }
    }

    private void MakeLastSceneActive(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    private IEnumerator LoadScene(int index)
    {
        yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }

    private IEnumerator MainMenu()
    {
        _gameState = GameState.MainMenu;
        yield return StartCoroutine(LoadScene(1));
        _messageCanvas.enabled = true;
    }

    private IEnumerator DisplayMessage(string message, string buttontext)
    {
        _messageCanvas.enabled = true;
        var uiButtonObject = Instantiate(uiButtonPrefab, _messageCanvas.transform);
        var uiButton = uiButtonObject.GetComponent<UIButton>();
        uiButton.AssignCoorditates(_buttonPM.GetPosition());
        var uiTextObject = Instantiate(uiTextPrefab, _messageCanvas.transform);
        var uiText = uiTextObject.GetComponent<UIText>();
        uiText.AssignCoordinates(_textPM.GetPosition());
        uiText.AssignText(message);
        uiButton.AssignText(buttontext);
        bool buttonpressed = false;
        uiButton.AssignDelegate(delegate { buttonpressed = true; });
        while (!buttonpressed) yield return null;
        Destroy(uiButtonObject);
        Destroy(uiTextObject);
        _messageCanvas.enabled = false;
    }

    private IEnumerator Defeat()
    {
        _messageCanvas.enabled = true;
        yield return StartCoroutine(DisplayMessage("Вы погибли", "Начать заново"));
        yield return MainMenu();
        yield return null;
    }

    private IEnumerator Victory()
    {
        _wins += 1;
        _messageCanvas.enabled = true;
        if (_wins == 5)
        {
            _audioSources.Find(audio => audio.name == "BgMusic").Stop();
            _audioSources.Find(audio => audio.name == "WinAudio").Play();
            yield return StartCoroutine(DisplayMessage("Вы победили!", "Начать заново"));
            yield return MainMenu();
        }
        else
        {
            yield return StartCoroutine(DisplayMessage($"Враг побежден!\nПрогресс: {_wins}/5", "Продолжить"));
            _gameState = GameState.UpgradeMenu;
            yield return StartCoroutine(LoadScene(2));
            var director = Instantiate(upgradeDirectorPrefab).GetComponent<UpgradeDirector>();
            StartCoroutine(director.Begin(_gameState));
        }
    }

    private IEnumerator Fight()
    {
        _gameState = GameState.Fight;
        yield return StartCoroutine(LoadScene(3));
        var director = Instantiate(fightDirectorPrefab).GetComponent<FightDirector>();
        StartCoroutine(director.Begin(_wins));
    }

    private IEnumerator NewGame()
    {
        _gameState = GameState.NewGame;
        if (!_audioSources.Find(audio => audio.name == "BgMusic").isPlaying) _audioSources.Find(audio => audio.name == "BgMusic").Play();
        _wins = 0;
        yield return StartCoroutine(LoadScene(2));
        var director = Instantiate(upgradeDirectorPrefab).GetComponent<UpgradeDirector>();
        StartCoroutine(director.Begin(_gameState));
    }

    private int GetLastSceneIndex() => SceneManager.sceneCount - 1;

    private IEnumerator UnloadLastScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        var lastSceneIndex = GetLastSceneIndex();
        if (lastSceneIndex > 0) yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(lastSceneIndex));
    }

}
public enum GameState
{
    MainMenu,
    NewGame,
    UpgradeMenu,
    Fight,
    GameOver
}
