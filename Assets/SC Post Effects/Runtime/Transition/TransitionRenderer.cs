using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;

namespace SCPE
{
    public sealed class TransitionRenderer : PostProcessEffectRenderer<Transition>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Transition);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetVector(ShaderParameters.Params, new Vector4(settings.progress.value, settings.invert.value ? 1 : 0, 0f, 0f));
            sheet.properties.SetTexture("_Gradient", settings.gradientTex.value == null ? Texture2D.whiteTexture : settings.gradientTex.value);
            sheet.properties.SetColor("_TransitionColor", settings.color.value);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}