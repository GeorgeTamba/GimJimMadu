using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SCPE
{
    public sealed class RefractionRenderer : PostProcessEffectRenderer<Refraction>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Refraction);
        }
        
        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);
            
            sheet.properties.SetVector(ShaderParameters.Params, new Vector4(settings.amount.value, 0f, 0f, 0f));
            sheet.properties.SetColor("_Tint", settings.tint.value);
            sheet.properties.SetTexture("_RefractionNormal", settings.normalMap.value ? settings.normalMap.value : Texture2D.normalTexture);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}