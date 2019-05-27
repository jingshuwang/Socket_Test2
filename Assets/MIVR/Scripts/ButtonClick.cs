//-----------------------------------------------------------------------
// <copyright file="ButtonClick.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// the sample button click script
    /// </summary>
    public class ButtonClick : MonoBehaviour
    {
        /// <summary>
        /// Fixed forveated level.
        /// </summary>
        private VrManager.FovLevel fovLevel = VrManager.FovLevel.Fov0;

        /// <summary>
        /// refreshRate mode.
        /// </summary>
        private VrManager.RefreshRateMode refreshRate = VrManager.RefreshRateMode.Fps60;

        /// <summary>
        /// Called when [click].
        /// </summary>
        public void OnClick()
        {
            Debug.Log("**** OnClick.");
            this.transform.GetComponentInChildren<Text>().text = (Random.value * 100).ToString();

            /* new api test
            fovLevel++;
            if (fovLevel > VrManager.FovLevel.Fov3)
            {
                fovLevel = VrManager.FovLevel.Fov0;
            }

            VrManager.SetFovMode(fovLevel);
            //*/

            this.refreshRate++;
            if (this.refreshRate > VrManager.RefreshRateMode.Fps72)
            {
                this.refreshRate = VrManager.RefreshRateMode.Fps60;
            }

            VrManager.SetRefreshRateMode(this.refreshRate);
        }

        /// <summary>
        /// Called when [pointer down].
        /// </summary>
        public void OnPointerDown()
        {
            Debug.Log("**** OnPointerDown.");
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
        }
    }
}
