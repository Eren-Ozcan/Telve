using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Telve.EditorTools
{
    /// <summary>
    /// One-shot project configuration run via -batchmode -executeMethod:
    /// creates the URP asset/renderer, assigns it as the default render
    /// pipeline, and switches the mobile orientation to portrait
    /// (ROADMAP.md Faz 1: "2D URP, portre mod").
    /// </summary>
    public static class ProjectSetup
    {
        const string RendererDataPath = "Assets/Settings/Telve_URPRenderer.asset";
        const string PipelineAssetPath = "Assets/Settings/Telve_URPAsset.asset";

        public static void Configure()
        {
            ConfigureUrp();
            ConfigurePortraitOrientation();
            PlayerSettings.productName = "Telve";

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("PROJECT_SETUP_DONE");
            EditorApplication.Exit(0);
        }

        static void ConfigureUrp()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Settings"))
            {
                AssetDatabase.CreateFolder("Assets", "Settings");
            }

            var rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
            AssetDatabase.CreateAsset(rendererData, RendererDataPath);

            var pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
            AssetDatabase.CreateAsset(pipelineAsset, PipelineAssetPath);

            GraphicsSettings.defaultRenderPipeline = pipelineAsset;
            QualitySettings.renderPipeline = pipelineAsset;

            Debug.Log("PROJECT_SETUP_URP_ASSET:" + PipelineAssetPath);
        }

        static void ConfigurePortraitOrientation()
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            Debug.Log("PROJECT_SETUP_ORIENTATION:Portrait");
        }
    }
}
