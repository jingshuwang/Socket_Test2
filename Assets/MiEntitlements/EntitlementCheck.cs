//-----------------------------------------------------------------------
// <copyright file="EntitlementCheck.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The EntitlementCheck class.
    /// </summary>
    public sealed class EntitlementCheck
    {
        /// <summary>
        /// Save vrlib java class.
        /// </summary>
        private static AndroidJavaClass nativeVrLibClass = null;

        /// <summary>
        /// Do entitlement check.
        /// </summary>
        /// <param name="finishActivityWhenFail">Whether finish app activity when entitlement check fail.</param>
        /// <returns>true if passed</returns>
        public static bool InitAndCheckEntitlement(bool finishActivityWhenFail = true)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if(nativeVrLibClass == null)
            {
                nativeVrLibClass = new AndroidJavaClass("com.mi.dlabs.vr.sdk.VrLib");
            }
            if (nativeVrLibClass != null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                string packageName = activity.Call<string>("getPackageName");
                bool result = nativeVrLibClass.CallStatic<bool>("checkProductionModePermission", activity, packageName, finishActivityWhenFail);
                Debug.Log("InitAndCheckEntitlement package name: " + packageName + ", result: " + result);
                return result;
            }
#endif
            return false;
        }
    }
}