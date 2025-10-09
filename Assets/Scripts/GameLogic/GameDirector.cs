using NUnit.Framework.Constraints;
using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameState gameState;

    private void Start()
    {
        ChangeState("restart");
    }

    public void ChangeState(string state)
    {
        UnloadLastScene();
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
        SceneManager.sceneLoaded += MakeLastSceneActive;
    }

    private void MakeLastSceneActive(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(GetLastSceneIndex()));
    }

    private void MainMenu()
    {
        gameState = GameState.MainMenu;
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
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }

    private void NewGame()
    {
        gameState = GameState.NewGame;
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    private int GetLastSceneIndex() => SceneManager.sceneCount - 1;

    private void UnloadLastScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        var lastSceneIndex = GetLastSceneIndex();
        if (lastSceneIndex > 0) SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(lastSceneIndex));
    }

    public enum GameState
    {
        MainMenu,
        NewGame,
        UpgradeMenu,
        Fight,
        GameOver
    }

}
