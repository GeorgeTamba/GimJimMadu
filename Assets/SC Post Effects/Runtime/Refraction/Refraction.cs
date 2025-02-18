using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;

namespace SCPE
{
    [PostProcess(typeof(RefractionRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Screen/Refraction", true)]
    [Serializable]
    public sealed class Refraction : PostProcessEffectSettings
    {
        [FormerlySerializedAs("refractionTex")]
        [Tooltip("Takes a normal map to perturb the image")]
        public TextureParameter normalMap = new TextureParameter { value = null };

        [Range(0f, 1f), Tooltip("Amount")]
        public FloatParameter amount = new FloatParameter { value = 0f };
        public ColorParameter tint = new ColorParameter { value = new Color(1,1,1, 0.1f)};

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (amount == 0 || normalMap.value == null) { return false; }
                return true;
            }

            return false;
        }
        
        [SerializeField]
        public Shader shader;
        
        private void Reset()
        {
            SerializeShader();
        }
        
        private bool SerializeShader()
        {
            bool wasSerialized = !shader;
            shader = Shader.Find(ShaderNames.Refraction);

            return wasSerialized;
        }
    }
}