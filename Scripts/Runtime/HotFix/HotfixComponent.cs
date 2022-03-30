using System.IO;
using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityGameFramework.Runtime
{
    public class HotfixComponent : GameFrameworkComponent
    {
        [SerializeField] private string url = "http://192.168.1.175/HotFix.dll";
        private static System.Reflection.Assembly s_HotfixAssembly;
        public static System.Reflection.Assembly HotfixAssembly => s_HotfixAssembly;
        private bool m_LoadedFlag;

        public async UniTask UpdateAndLoadHotfixDll()
        {
            await UpdateHotfixDll();
            LoadGameDll();
        }

        private void LoadGameDll()
        {
            //todo 使用ab包模式读取
            if (m_LoadedFlag)
                return;
            string gameDll = "";
#if UNITY_EDITOR
            gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            // 使用File.ReadAllBytes是为了避免Editor下gameDll文件被占用导致后续编译后无法覆盖
#else
            var persistPath = Path.Combine(Application.persistentDataPath, "HotFix.dll");
            var streamingPath = Path.Combine(Application.streamingAssetsPath, "HotFix.dll");
            gameDll = File.Exists(persistPath) ? persistPath : streamingPath;
#endif
            s_HotfixAssembly = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
            m_LoadedFlag = true;
        }

        private async UniTask UpdateHotfixDll()
        {
            var downloadComponent = GameEntry.GetComponent<DownloadComponent>();
            var downloadIndex = downloadComponent.AddDownload(Path.Combine(Application.persistentDataPath, "HotFix.dll"), url);
            await UniTask.WaitUntil(() => downloadComponent.GetDownloadInfo(downloadIndex).Status == TaskStatus.Done);
            //
            // var wr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            // wr.timeout = 10;
            // wr.downloadHandler = new DownloadHandlerFile(Path.Combine(Application.persistentDataPath, "HotFix.dll"));
            // Debug.Log($"下载开始url={url}");
            // wr.SendWebRequest();
            // await UniTask.WaitUntil(() => wr.isDone);
            // Debug.Log($"下载结束url={url}");
        }
    }
}