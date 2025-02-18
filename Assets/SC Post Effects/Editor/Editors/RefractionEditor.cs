using UnityEditor;
using UnityEditor.Rendering.PostProcessing;

namespace SCPE
{
    [PostProcessEditor(typeof(Refraction))]
    public sealed class RefractionEditor : PostProcessEffectEditor<Refraction>
    {
        SerializedParameterOverride normalMap;
        SerializedParameterOverride amount;
        SerializedParameterOverride tint;

        public override void OnEnable()
        {
            amount = FindParameterOverride(x => x.amount);
            tint = FindParameterOverride(x => x.tint);
            normalMap = FindParameterOverride(x => x.normalMap);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("refraction");

            SCPE_GUI.DisplaySetupWarning<RefractionRenderer>();

            PropertyField(amount);
            SCPE_GUI.DisplayIntensityWarning(amount);
            
            EditorGUILayout.Space();
            
            PropertyField(normalMap);
            SCPE_GUI.DisplayTextureOverrideWarning(normalMap.overrideState.boolValue);

            if (normalMap.overrideState.boolValue && normalMap.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a texture", MessageType.Info);
            }

            PropertyField(tint);

            EditorGUILayout.Space();

        }
    }
}