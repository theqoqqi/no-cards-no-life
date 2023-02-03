using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Components.Utils {
    public class EditorFullscreenSwitcher : MonoBehaviour {

        [SerializeField] private int tabHeight = 22;

        [SerializeField] private KeyCode toggleKey = KeyCode.F12;

#if UNITY_EDITOR
        private void Update() {
            if (Application.isEditor && Input.GetKeyDown(toggleKey)) {
                Toggle();
            }
        }

        private void Toggle() {
            if (EditorApplication.isPlaying) {
                FullScreenGameWindow();
            }
            else {
                CloseGameWindow();
            }
        }

        private EditorWindow GetMainGameView() {
            EditorApplication.ExecuteMenuItem("Window/General/Game");

            var type = Type.GetType("UnityEditor.PlayModeView,UnityEditor");
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
            var getMainGameView = type?.GetMethod("GetMainPlayModeView", bindingFlags);
            var gameView = getMainGameView?.Invoke(null, null);

            return (EditorWindow) gameView;
        }

        private void FullScreenGameWindow() {
            var gameView = GetMainGameView();

            var screenWidth = Screen.currentResolution.width;
            var screenHeight = Screen.currentResolution.height;
            var newPos = new Rect(0, 0 - tabHeight, screenWidth, screenHeight + tabHeight);

            gameView.titleContent = new GUIContent("Game (Stereo)");
            gameView.position = newPos;
            gameView.minSize = new Vector2(screenWidth, screenHeight + tabHeight);
            gameView.maxSize = gameView.minSize;
            gameView.position = newPos;
        }

        private void CloseGameWindow() {
            var gameView = GetMainGameView();

            gameView.Close();
        }
    }
#endif
}