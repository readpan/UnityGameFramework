using System.IO;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class HotfixComponent : GameFrameworkComponent
    {
        //todo Manager统一写入GF框架中
        private static System.Reflection.Assembly s_HotfixAssembly;
        public static System.Reflection.Assembly HotfixAssembly => s_HotfixAssembly;
        protected override void Awake()
        {
            base.Awake();
            LoadGameDll();
        }

        private void LoadGameDll()
        {
#if UNITY_EDITOR
            string gameDll = Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll";
            // 使用File.ReadAllBytes是为了避免Editor下gameDll文件被占用导致后续编译后无法覆盖
#else
        string gameDll = Application.streamingAssetsPath + "/HotFix.dll";
#endif
            s_HotfixAssembly = System.Reflection.Assembly.Load(File.ReadAllBytes(gameDll));
        }
    }
}