using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Telve.EditorTools
{
    /// <summary>
    /// Headless (-batchmode -executeMethod) package installer. Resolves each
    /// package to the registry's recommended version for this Editor build,
    /// same as the Package Manager window would, instead of hand-pinning
    /// version numbers in manifest.json.
    ///
    /// Installing a package can trigger a domain reload (e.g. it ships editor
    /// scripts), which wipes static fields and unsubscribes EditorApplication
    /// events. The remaining queue is persisted in EditorPrefs and
    /// [InitializeOnLoad] re-attaches the tick loop after every reload so the
    /// whole batch survives reloads instead of silently stalling.
    /// </summary>
    [InitializeOnLoad]
    public static class PackageInstaller
    {
        const string QueueKey = "Telve.PackageInstaller.Queue";

        static AddRequest _current;
        static string _currentPackage;

        static PackageInstaller()
        {
            if (EditorPrefs.HasKey(QueueKey))
            {
                EditorApplication.update += Tick;
            }
        }

        public static void Install()
        {
            var packages = new[]
            {
                "com.unity.2d.sprite",
                "com.unity.render-pipelines.universal",
                "com.unity.textmeshpro",
                "com.unity.ugui",
            };
            EditorPrefs.SetString(QueueKey, string.Join(",", packages));
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        /// <summary>Resume with only the packages not yet in manifest.json.</summary>
        public static void InstallRemaining()
        {
            var packages = new[]
            {
                "com.unity.render-pipelines.universal",
                "com.unity.textmeshpro",
                "com.unity.ugui",
            };
            EditorPrefs.SetString(QueueKey, string.Join(",", packages));
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        /// <summary>
        /// Coplay MCP bridge — installed from a git URL rather than the
        /// registry (it isn't a published UPM package). Unity 2022+
        /// per https://docs.coplay.dev/getting-started/installation.
        /// </summary>
        public static void InstallCoplay()
        {
            EditorPrefs.SetString(QueueKey, "https://github.com/CoplayDev/coplay-unity-plugin.git#beta");
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        static void Tick()
        {
            if (_current == null)
            {
                var remaining = EditorPrefs.GetString(QueueKey, "")
                    .Split(',')
                    .Where(s => s.Length > 0)
                    .ToArray();

                if (remaining.Length == 0)
                {
                    EditorApplication.update -= Tick;
                    EditorPrefs.DeleteKey(QueueKey);
                    Debug.Log("PACKAGE_INSTALL_DONE");
                    EditorApplication.Exit(0);
                    return;
                }

                _currentPackage = remaining[0];
                // Persist the queue MINUS this package before the request
                // starts, so a mid-flight domain reload resumes at the next
                // package instead of re-adding (or losing track of) this one.
                EditorPrefs.SetString(QueueKey, string.Join(",", remaining.Skip(1)));

                Debug.Log("PACKAGE_INSTALL_START:" + _currentPackage);
                _current = Client.Add(_currentPackage);
                return;
            }

            if (!_current.IsCompleted) return;

            if (_current.Status == StatusCode.Success)
            {
                Debug.Log("PACKAGE_INSTALL_OK:" + _current.Result.packageId);
            }
            else
            {
                Debug.LogError("PACKAGE_INSTALL_FAIL:" + _currentPackage + ":" + _current.Error.message);
            }

            _current = null;
        }
    }
}
