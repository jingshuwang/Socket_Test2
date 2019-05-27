//-----------------------------------------------------------------------
// <copyright file="MiVRMenu.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// MI VR Menu on unity editor
    /// </summary>
    public class MiVRMenu : MonoBehaviour
    {
        /// <summary>
        /// Opens the documentation.
        /// </summary>
        [MenuItem("MiVR/Documentation/Developer Site", false, 100)]
        private static void OpenDocumentation()
        {
            Application.OpenURL("http://dev.xiaomi.com/");
        }

        /// <summary>
        /// Opens the release notes.
        /// </summary>
        [MenuItem("MiVR/Documentation/Release Notes", false, 100)]
        private static void OpenReleaseNotes()
        {
            Application.OpenURL("http://dev.xiaomi.com/");
        }

        /// <summary>
        /// Opens the about.
        /// </summary>
        [MenuItem("MiVR/About Mi VR", false, 200)]
        private static void OpenAbout()
        {
            EditorUtility.DisplayDialog(
                "Mi VR SDK for Unity",
                "Version: " + VrManager.SDKVersion + "\n\n" + "Copyright: ©2016 XiaoMi Corporation. All rights reserved.\n",
                "OK");
        }
    }
}