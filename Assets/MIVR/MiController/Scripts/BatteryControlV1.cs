//-----------------------------------------------------------------------
// <copyright file="BatteryControlV1.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// the battery controller for v1
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class BatteryControlV1 : MonoBehaviour
    {
        /// <summary>
        /// Battery Level 0 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Sprite BatteryLevel0Sprite;

        /// <summary>
        /// Battery Level 1 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Sprite BatteryLevel1Sprite;

        /// <summary>
        /// Battery Level 2 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Sprite BatteryLevel2Sprite;

        /// <summary>
        /// Battery Level 3 Sprite.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Sprite BatteryLevel3Sprite;

        /// <summary>
        /// Battery Level
        /// </summary>
        private Image batteryLevel;

        /// <summary>
        /// Calc duration.
        /// </summary>
        private float duration = 99f;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            this.batteryLevel = this.GetComponent<Image>();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            this.SetBatterySprite();
        }

        /// <summary>
        /// Set Battery Sprite
        /// </summary>
        private void SetBatterySprite()
        {
            int battery = InputManager.ControllerState.BatteryPercentRemaining;
            Debug.Log("20170922 v1battery " + battery);
            if (battery >= 70)
            {
                this.batteryLevel.sprite = this.BatteryLevel3Sprite;
            }
            else if (battery >= 40 && battery < 70 && this.batteryLevel.sprite != this.BatteryLevel2Sprite)
            {
                this.batteryLevel.sprite = this.BatteryLevel2Sprite;
            }
            else if (battery >= 10 && battery < 40 && this.batteryLevel.sprite != this.BatteryLevel1Sprite)
            {
                this.batteryLevel.sprite = this.BatteryLevel1Sprite;
            }
            else if (battery < 10 && this.batteryLevel.sprite != this.BatteryLevel0Sprite)
            {
                this.batteryLevel.sprite = this.BatteryLevel0Sprite;
            }
        }
    }
}
