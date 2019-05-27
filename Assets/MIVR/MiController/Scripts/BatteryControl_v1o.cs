//-----------------------------------------------------------------------
// <copyright file="BatteryControl_v1o.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Battery Controller for v1o
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class BatteryControl_v1o : MonoBehaviour
    {
        /// <summary>
        /// Battery Level 0 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Texture2D BatteryLevel0Sprite;

        /// <summary>
        /// Battery Level 1 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Texture2D BatteryLevel1Sprite;

        /// <summary>
        /// Battery Level 2 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Texture2D BatteryLevel2Sprite;

        /// <summary>
        /// Battery Level 3 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Texture2D BatteryLevel3Sprite;

        /// <summary>
        /// Battery Level
        /// </summary>
        private MeshRenderer batteryLevel;

        /// <summary>
        /// use for initializing
        /// </summary>
        private void Start()
        {
            this.batteryLevel = this.GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            this.CheckBatteryStatus();
        }

        /// <summary>
        /// Check Battery Status
        /// </summary>
        private void CheckBatteryStatus()
        {
            int battery = InputManager.ControllerState.BatteryPercentRemaining;

            if (battery >= 75)
            {
                this.batteryLevel.material.mainTexture = this.BatteryLevel3Sprite;
            }
            else if (battery >= 50 && battery < 75 && this.batteryLevel.material.mainTexture != this.BatteryLevel2Sprite)
            {
                this.batteryLevel.material.mainTexture = this.BatteryLevel2Sprite;
            }
            else if (battery >= 15 && battery < 50 && this.batteryLevel.material.mainTexture != this.BatteryLevel1Sprite)
            {
                this.batteryLevel.material.mainTexture = this.BatteryLevel1Sprite;
            }
            else if (battery < 15 && this.batteryLevel.material.mainTexture != this.BatteryLevel0Sprite)
            {
                this.batteryLevel.material.mainTexture = this.BatteryLevel0Sprite;
            }
        }
    }
}
