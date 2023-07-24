using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using SVLib;

public class SVWindow : EditorWindow {

    private ConversationView conversationView;
    private ActionView actionView;
    private ImageView imageView;
    private MaterialView materialView;
    private ModelView modelView;
    private MusicView musicView;
    private VoiceView voiceView;

    private PromptView currentView;

    private LoginView loginView;

    [MenuItem("Tools/Solace Vision")]
    public static void ShowExample() {
        System.Type[] types = { System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll") };

        SVWindow wnd = GetWindow<SVWindow>(types);
        wnd.titleContent = new GUIContent("SVWindow");
    }

    public void LoadViews(VisualElement root) {
        conversationView = new ConversationView(root);
        actionView = new ActionView(root);
        imageView = new ImageView(root);
        materialView = new MaterialView(root);
        modelView = new ModelView(root);
        musicView = new MusicView(root);
        voiceView = new VoiceView(root);

        currentView = conversationView;

        loginView = new LoginView(root);
    }

    public void SetButtonBehaviors(VisualElement root) {
        Button conversationMenuButton = (Button) root.Query("toolbar").First().Query("conversation-menu").First();
        Button actionMenuButton = (Button) root.Query("toolbar").First().Query("action-menu").First();
        Button imageMenuButton = (Button) root.Query("toolbar").First().Query("image-menu").First();
        Button materialMenuButton = (Button) root.Query("toolbar").First().Query("material-menu").First();
        Button modelMenuButton = (Button) root.Query("toolbar").First().Query("model-menu").First();
        Button musicMenuButton = (Button) root.Query("toolbar").First().Query("music-menu").First();
        Button voiceMenuButton = (Button) root.Query("toolbar").First().Query("voice-menu").First();

        conversationMenuButton.clicked += () => { 
            SwitchView(conversationView);
        };
        actionMenuButton.clicked += () => { 
            SwitchView(actionView);
        };
        imageMenuButton.clicked += () => { 
            SwitchView(imageView);
        };
        materialMenuButton.clicked += () => { 
            SwitchView(materialView);
        };
        modelMenuButton.clicked += () => { 
            SwitchView(modelView);
        };
        musicMenuButton.clicked += () => { 
            SwitchView(musicView);
        };
        voiceMenuButton.clicked += () => { 
            SwitchView(voiceView);
        };
    }

    public void SwitchView(PromptView newView) {

        currentView.Hide();
        newView.Show();
        currentView = newView;
        
    }

    public void CreateGUI() {
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.solacevision.assistant/Editor/SVWindow.uxml");
        VisualElement uxmlUI = visualTree.Instantiate();
        root.Add(uxmlUI);
        
        LoadViews(root);
        SetButtonBehaviors(root);
        
    }
}