using UnityEditor;
using UnityEditor.Rendering.PostProcessing;

namespace SCPE
{
    [PostProcessEditor(typeof(Transition))]
    public sealed class TransitionEditor : PostProcessEffectEditor<Transition>
    {
        SerializedParameterOverride gradientTex;
        SerializedParameterOverride progress;
        SerializedParameterOverride invert;
        SerializedParameterOverride color;

        public override void OnEnable()
        {
            gradientTex = FindParameterOverride(x => x.gradientTex);
            progress = FindParameterOverride(x => x.progress);
            invert = FindParameterOverride(x => x.invert);
            color = FindParameterOverride(x => x.color);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("transition");

            SCPE_GUI.DisplaySetupWarning<TransitionRenderer>();

            PropertyField(progress);
            SCPE_GUI.DisplayIntensityWarning(progress);
            
            EditorGUILayout.Space();
            
            PropertyField(gradientTex);
            SCPE_GUI.DisplayTextureOverrideWarning(gradientTex.overrideState.boolValue);

            if (gradientTex.overrideState.boolValue && gradientTex.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a gradient texture (pre-made textures can be found in the \"_SampleTextures\" package", MessageType.Info);
            }

            PropertyField(invert);
            PropertyField(color);
        }
    }
}