using System.IO;
using System.Text;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Telve.EditorTools
{
    /// <summary>
    /// Programmatic EditMode test runner for use from Coplay's script executor,
    /// which cannot compile against UnityEditor.TestTools.TestRunner.Api
    /// directly (limited ad-hoc reference set). Results are written to disk
    /// since they also can't reliably read the live Editor's console buffer.
    /// </summary>
    public static class TestRunnerHelper
    {
        public static void RunEditModeTests(string resultPath)
        {
            var startedPath = resultPath + ".started";
            File.WriteAllText(startedPath, "started");
            if (File.Exists(resultPath)) File.Delete(resultPath);

            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            var filter = new Filter { testMode = TestMode.EditMode };
            api.RegisterCallbacks(new ResultCallbacks(resultPath));
            api.Execute(new ExecutionSettings(filter));
        }

        class ResultCallbacks : ICallbacks
        {
            readonly string _resultPath;

            public ResultCallbacks(string resultPath) => _resultPath = resultPath;

            public void RunStarted(ITestAdaptor testsToRun) { }

            public void RunFinished(ITestResultAdaptor result)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"SUMMARY pass={result.PassCount} fail={result.FailCount} inconclusive={result.InconclusiveCount} skip={result.SkipCount} duration={result.Duration}");
                AppendFailures(result, sb);
                File.WriteAllText(_resultPath, sb.ToString());
            }

            static void AppendFailures(ITestResultAdaptor node, StringBuilder sb)
            {
                if (node.TestStatus == TestStatus.Failed && !node.Test.IsSuite)
                {
                    sb.AppendLine($"FAILED: {node.FullName}");
                    sb.AppendLine($"  Message: {node.Message}");
                    sb.AppendLine($"  StackTrace: {node.StackTrace}");
                }

                if (node.Children != null)
                {
                    foreach (var child in node.Children) AppendFailures(child, sb);
                }
            }

            public void TestStarted(ITestAdaptor test) { }
            public void TestFinished(ITestResultAdaptor result) { }
        }
    }
}
