//-----------------------------------------------------------------------
// <copyright file="BuildScripts.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// MI VR Menu on unity editor
    /// </summary>
    public class BuildScripts : MonoBehaviour
    {
        /// <summary>
        /// Builds the android player.
        /// </summary>
        /// <exception cref="Exception">
        /// UNITY_BUILD_TARGET -system property not defined, aborting.
        /// or
        /// Build failed:  + error
        /// </exception>
        public static void BuildAndroidPlayer()
        {
            string[] scenes = { "Assets/MIVR/Scenes/360ViewController.unity" };

            string error = BuildPipeline.BuildPlayer(scenes, "sample.apk", BuildTarget.Android, BuildOptions.None);

            if (!string.IsNullOrEmpty(error) && error.Length > 0)
            {
                throw new Exception("Build failed: " + error);
            }
        }

        /// <summary>
        /// Exports the core SDK.
        /// </summary>
        public static void ExportCoreSDK()
        {
            string[] coreAssets =
                {
            "Assets/MIVR",
            "Assets/Plugins/Android/AndroidManifest.xml",
            "Assets/Plugins/Android/libvrcore.so",
            "Assets/Plugins/Android/libVRPlugin.so",
            "Assets/Plugins/Android/libvrapi.so",
            "Assets/Plugins/Android/vrlib.aar",
            "Assets/Plugins/Android/assets/donotdelete.txt"
            };
            AssetDatabase.ExportPackage(coreAssets, "core.unityPackage", ExportPackageOptions.Recurse);
        }

        /// <summary>
        /// Exports the commerce SDK.
        /// </summary>
        public static void ExportCommerceSDK()
        {
            string[] coreAssets =
                {
            "Assets/Commerce/Scenes/Commerce.unity",
            "Assets/Commerce/Scripts",
            "Assets/Plugins/Android/paysdk.aar",
            "Assets/Plugins/Android/commerce.aar"
            };
            AssetDatabase.ExportPackage(coreAssets, "commerce.unityPackage", ExportPackageOptions.Recurse);
        }

        /// <summary>
        /// Exports the commerce SDK for OVR.
        /// </summary>
        public static void ExportCommerceSDKForOVR()
        {
            string[] coreAssets =
                {
            "Assets/Commerce/Scenes/CommerceSample.unity",
            "Assets/Commerce/Scripts",
            "Assets/Plugins/Android/paysdk.aar",
            "Assets/Plugins/Android/commerce.aar",
            "Assets/MIVR/Materials/InSideSphere.mat",
            "Assets/MIVR/Textures/ReticleCenter.png",
            "Assets/MIVR/Textures/sphere.jpg",
            "Assets/Plugins/Android/vrlib.aar",
            "Assets/Entitlements",
            };
            AssetDatabase.ExportPackage(coreAssets, "platform.unityPackage", ExportPackageOptions.Recurse);
        }
    }
}