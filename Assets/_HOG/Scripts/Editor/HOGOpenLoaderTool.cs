using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class HOGOpenLoaderTool
{
    private static bool shouldReturnToLastScene = false;

    static HOGOpenLoaderTool()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            shouldReturnToLastScene = true;
        }
        else if (state == PlayModeStateChange.EnteredEditMode && shouldReturnToLastScene)
        {
            shouldReturnToLastScene = false;
            ReturnToLastScene();
        }
    }

    [MenuItem("HOG/PlayGame")]
    public static void OpenLoader()
    {
        string currentSceneName = "GameScene";
        File.WriteAllText(".lastScene", currentSceneName);
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Hog/Scenes/Loader.unity");
        EditorApplication.isPlaying = true;
    }

    [MenuItem("HOG/LoadEditedScene")]
    public static void ReturnToLastScene()
    {
        string lastScene = File.ReadAllText(".lastScene");
        EditorSceneManager.OpenScene($"{Directory.GetCurrentDirectory()}/Assets/_Hog/Scenes/{lastScene}.unity");

        OpenSpecificObjectsInScene();
    }

    private static void OpenSpecificObjectsInScene()
    {
        GameObject gameScreen = GameObject.Find("GameScreen");
        if (gameScreen != null)
        {
            EditorGUIUtility.PingObject(gameScreen);

            Transform characterLeft = gameScreen.transform.Find("CharacterLeft3D");
            Transform characterRight = gameScreen.transform.Find("CharacterRight3D");

            if (characterLeft != null)
            {
                EditorGUIUtility.PingObject(characterLeft.gameObject);
            }

            if (characterRight != null)
            {
                EditorGUIUtility.PingObject(characterRight.gameObject);
            }
        }
    }
}
