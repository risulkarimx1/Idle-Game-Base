using System.IO;
using Services.DataFramework;
using Services.LogFramework;
using UnityEditor;
using UnityEngine;
using Debug = Services.LogFramework.Debug;


namespace _Scripts.EditorUtil.Editor
{
    public class DataEditorTool
    {
        [MenuItem("Tools/Clear Data")]
        public static void ClearData()
        {
            var path = Path.Combine(Application.persistentDataPath, DataManager.Key);
            
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Debug.Log($"Folder {path} has been removed.", LogContext.DataManager);
            }
            else
            {
                Debug.Log($"Folder {path} does not exist.", LogContext.DataManager);
            }
        }
    }
}
