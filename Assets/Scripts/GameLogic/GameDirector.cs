using NUnit.Framework.Constraints;
using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameState gameState;
    private GameObject _player;

    private void Start()
    {
        MainMenu();
    }

    public void ChangeState(string state)
    {
        switch (state)
        {
            case "newgame":
                NewGame();
                break;
            case "fight":
                Fight();
                break;
            case "victory":
                Victory();
                break;
            case "defeat":
                Defeat();
                break;
            case "restart":
                MainMenu();
                break;
            default:
                throw new InvalidOperationException("Invalid state passed");
        }
    }

    private void MainMenu()
    {
        gameState = GameState.MainMenu;
        UnloadLastScene();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void Defeat()
    {
        throw new NotImplementedException();
    }

    private void Victory()
    {
        throw new NotImplementedException();
    }

    private void Fight()
    {
        gameState = GameState.Fight;
        UnloadLastScene();
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }

    private void NewGame()
    {
        gameState = GameState.NewGame;
        UnloadLastScene();
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    private void UnloadLastScene()
    {
        var lastSceneIndex = SceneManager.sceneCount - 1;
        if (lastSceneIndex > 0) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(lastSceneIndex));
    }

    public void SetPlayerReference(GameObject player) => _player = player;

    public enum GameState
    {
        MainMenu,
        NewGame,
        UpgradeMenu,
        Fight,
        GameOver
    }

}
