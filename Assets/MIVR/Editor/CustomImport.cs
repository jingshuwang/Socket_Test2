//-----------------------------------------------------------------------
// <copyright file="CustomImport.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom import to add layer
    /// </summary>
    [InitializeOnLoad]
    public class CustomImport
    {
        /// <summary>
        /// Initializes static members of the <see cref="CustomImport"/> class.
        /// </summary>
        static CustomImport()
        {
            AddLayer("CustomUI");
        }

        /// <summary>
        /// Adds the layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private static void AddLayer(string layer)
        {
            if (!HasLayer(layer))
            {
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                Debug.Log(tagManager.ToString());
                SerializedProperty it = tagManager.GetIterator();
                while (it.NextVisible(true))
                {
                    if (it.name == "layers")
                    {
                        for (int i = 8; i < it.arraySize; i++)
                        {
                            SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                            if (string.IsNullOrEmpty(dataPoint.stringValue))
                            {
                                dataPoint.stringValue = layer;
                                tagManager.ApplyModifiedProperties();
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is has layer] [the specified layer].
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>whether has layer</returns>
        private static bool HasLayer(string layer)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                {
                    return true;
                }
            }

            return false;
        }
    }
}