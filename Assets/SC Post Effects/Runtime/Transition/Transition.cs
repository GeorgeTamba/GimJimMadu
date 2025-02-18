using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;

namespace SCPE
{
    [PostProcess(typeof(TransitionRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Screen/Transition", false)]
    [Serializable]
    public sealed class Transition : PostProcessEffectSettings
    {
        public TextureParameter gradientTex = new TextureParameter { value = null, defaultState = TextureParameterDefault.None };

        [Range(0f, 1f), Tooltip("Progress")]
        public FloatParameter progress = new FloatParameter { value = 0f };
        
        public BoolParameter invert = new BoolParameter { value = false };
        public ColorParameter color = new ColorParameter { value = Color.black };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value && progress.value > 0 && color.value.a > 0;
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
            shader = Shader.Find(ShaderNames.Transition);

            return wasSerialized;
        }
    }
}