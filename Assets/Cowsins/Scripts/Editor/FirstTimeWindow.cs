using UnityEditor;
using UnityEngine;

namespace cowsins
{
    [InitializeOnLoad]
    public class FirstTimeWindow : EditorWindow
    {
        #region Variables

        private GUIStyle selectedStyle;
        private int selectedTab = 0;
        private string[] tabs = { "Start Up", "Preferences" };

        private Texture2D headerImage;
        private Texture2D reviewImage;
        private Texture2D supportImage;
        private Texture2D firstStepsImage;

        private bool showTutorials = false;
        private bool showDocumentation = false;
        private bool showSupport = false;
        private bool showFirstSteps = false;

        private Vector2 scrollPosition = Vector2.zero;

        #endregion

        #region Initialization

        static FirstTimeWindow()
        {
            EditorApplication.delayCall += ShowWindowOnce;
        }

        private static void ShowWindowOnce()
        {
            if (!SessionState.GetBool("First_Init", false))
            {
                if (EditorPrefs.GetBool("FirstTimeDontShowAgain")) return;
                ShowWindow();
                SessionState.SetBool("First_Init", true);
            }
        }

        [MenuItem("Cowsins/FPS Engine Startup")]
        public static void ShowWindow()
        {
            FirstTimeWindow window = GetWindow<FirstTimeWindow>();
            window.titleContent = new GUIContent("FPS Engine Startup");
            window.minSize = new Vector2(420, 600);
            window.maxSize = new Vector2(420, 600);
            window.Show();
        }

        private void OnEnable()
        {
            LoadResources();
        }

        private void LoadResources()
        {
            headerImage = Resources.Load<Texture2D>("CustomEditor/FPS_Engine_Logo_Startup");
            reviewImage = Resources.Load<Texture2D>("CustomEditor/leave-a-review");
            supportImage = Resources.Load<Texture2D>("CustomEditor/discord-support");
            firstStepsImage = Resources.Load<Texture2D>("CustomEditor/first-steps");
        }

        #endregion

        #region GUI

        private void OnGUI()
        {
            DefineSelectedStyle();

            float scrollViewWidth = 420f;
            float footerHeight = 20f;

            GUILayout.BeginArea(new Rect((Screen.width - scrollViewWidth) / 2, 20, scrollViewWidth, Screen.height - footerHeight - 20));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(scrollViewWidth));
            GUILayout.BeginVertical();

            DrawHeader();
            selectedTab = GUILayout.Toolbar(selectedTab, tabs);
            switch (selectedTab)
            {
                case 0: DrawStartUpTab(); break;
                case 1: DrawPreferencesTab(); break;
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            DrawFooter(footerHeight);
        }

        private void DrawHeader()
        {
            GUILayout.Label(headerImage, GUILayout.Width(400), GUILayout.Height(60));

            var headerStyle = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };
            GUILayout.Label("Welcome to FPS Engine!", headerStyle, GUILayout.Width(400));

            var descriptionStyle = new GUIStyle(GUI.skin.label) { wordWrap = true };
            GUILayout.Label("Thanks for purchasing FPS Engine! We are excited to help you bring your Dream Game to life!", descriptionStyle, GUILayout.Width(400));

            GUILayout.Space(10);
        }

        private void DrawFooter(float footerHeight)
        {
            Rect footerRect = new Rect(0, position.height - footerHeight, position.width, footerHeight);
            EditorGUI.DrawRect(footerRect, new Color(0.15f, 0.15f, 0.15f));

            GUILayout.BeginArea(footerRect);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            bool dontShowAgain = EditorPrefs.GetBool("Unity6EditorWindowDontShowAgain", false);
            dontShowAgain = GUILayout.Toggle(dontShowAgain, "Don't show again");
            EditorPrefs.SetBool("Unity6EditorWindowDontShowAgain", dontShowAgain);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        #endregion

        #region Tabs

        private void DrawStartUpTab()
        {
            GUILayout.BeginVertical();
            var headerStyle = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };
            var descriptionStyle = new GUIStyle(GUI.skin.label) { wordWrap = true };

            // Tutorials Section
            DrawFoldoutSection(ref showTutorials, "TUTORIALS", "FPS Engine Tutorials can be found at: ", "Tutorials", "https://cowsinss-organization.gitbook.io/fps-engine-documentation/before-we-start/list-of-tutorials");

            // Documentation Section
            DrawFoldoutSection(ref showDocumentation, "DOCUMENTATION", "FPS Engine Documentation can be found at: ", "Documentation", "https://cowsinss-organization.gitbook.io/fps-engine-documentation");

            // Support Section
            DrawFoldoutSection(ref showSupport, "SUPPORT", "Do you need help? Get access to support by Cowsins on our Discord Server!", supportImage, "Join Discord Server", "https://discord.gg/759gSeTT9m");

            // First Steps Section
            DrawFirstStepsSection();

            // Review Section
            DrawReviewSection();

            GUILayout.EndVertical();
        }

        private void DrawFoldoutSection(ref bool toggle, string label, string description, string buttonText, string url)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
            toggle = EditorGUILayout.Foldout(toggle, label, true);
            if (toggle)
            {
                GUILayout.Space(5);
                GUILayout.Label(description, new GUIStyle(GUI.skin.label) { wordWrap = true }, GUILayout.Width(400));
                if (GUILayout.Button(buttonText)) Application.OpenURL(url);
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFoldoutSection(ref bool toggle, string label, string description, Texture2D image, string buttonText, string url)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
            toggle = EditorGUILayout.Foldout(toggle, label, true);
            if (toggle)
            {
                GUILayout.Space(5);
                GUILayout.Label(image, GUILayout.Width(400), GUILayout.Height(101));
                GUILayout.Label(description, new GUIStyle(GUI.skin.label) { wordWrap = true }, GUILayout.Width(400));
                if (GUILayout.Button(buttonText)) Application.OpenURL(url);
                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFirstStepsSection()
        {
            EditorGUILayout.BeginVertical(GUI.skin.GetStyle("HelpBox"));
            showFirstSteps = EditorGUILayout.Foldout(showFirstSteps, "FIRST STEPS", true);
            if (showFirstSteps)
            {
                GUILayout.Label(firstStepsImage, GUILayout.Width(400), GUILayout.Height(250));

                Rect imageRect = GUILayoutUtility.GetLastRect();
                float buttonSize = 50;
                float buttonX = imageRect.x + (imageRect.width / 2) - (buttonSize / 2);
                float buttonY = imageRect.y + (imageRect.height / 2) - (buttonSize / 2);

                var playButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.white } };
                GUI.backgroundColor = new Color(0, 0, 0, 0.5f);

                if (GUI.Button(new Rect(buttonX, buttonY, buttonSize, buttonSize), "▶", playButtonStyle))
                {
                    Application.OpenURL("https://www.youtube.com/watch?v=0UXrm3-1rlE");
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawReviewSection()
        {
            GUILayout.Space(10);
            GUILayout.Label(reviewImage, GUILayout.Width(400), GUILayout.Height(101));
            GUILayout.Label("Please consider leaving a review on the Unity Asset Store to help Cowsins provide Free Support and Free Updates to FPS Engine!", new GUIStyle(GUI.skin.label) { wordWrap = true }, GUILayout.Width(400));
            if (GUILayout.Button("Leave a Review"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/fps-engine-218594");
            }
            GUILayout.Space(20);
        }

        private void DrawPreferencesTab()
        {
            GUILayout.Space(10);
            bool newToggleValue = GUILayout.Toggle(DraggableButtonInSceneView.showDraggableButton, "Show Draggable Button in Scene View");
            if (newToggleValue != DraggableButtonInSceneView.showDraggableButton)
            {
                DraggableButtonInSceneView.SetShowDraggableButton(newToggleValue);
            }
        }

        #endregion

        #region Styles

        private void DefineSelectedStyle()
        {
            selectedStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f)) },
                active = { background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 1f)) }
            };
        }

        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        #endregion
    }
}
