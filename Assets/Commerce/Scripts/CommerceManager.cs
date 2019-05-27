//-----------------------------------------------------------------------
// <copyright file="CommerceManager.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// the commerce manager
    /// </summary>
    public class CommerceManager : MonoBehaviour
    {
        /// <summary>
        /// The app id.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Unity need this.")]
        public string AppId = string.Empty;

        /// <summary>
        /// The application key
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Unity need this.")]
        public string AppKey = string.Empty;

        /// <summary>
        /// The commerce manager
        /// </summary>
        private AndroidJavaObject nativeCommerceManager = null;

        /// <summary>
        /// The commerce callback
        /// </summary>
        private CommerceCallback commerceCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceManager"/> class.
        /// </summary>
        public CommerceManager()
        {
        }

        /// <summary>
        /// Occurs when [login finished].
        /// </summary>
        public event EventHandler<LoginFinishedEventArgs> LoginFinished;

        /// <summary>
        /// Occurs when [bill created].
        /// </summary>
        public event EventHandler<BillCreatedEventArgs> BillCreated;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CommerceManager Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Login with the default account.
        /// </summary>
        public void Login()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (this.nativeCommerceManager != null)
        {
            this.nativeCommerceManager.Call("login");
        }
#endif
        }

        /// <summary>
        /// Creates the bill.
        /// </summary>
        /// <param name="orderId">The order id which CP generated.</param>
        /// <param name="displayTitle">The display title of the pay content.</param>
        /// <param name="accountOpenId">The openId from account info.</param>
        /// <param name="totalPrice">The total price, the unit is 0.01yuan.</param>
        public void CreateBill(string orderId, string displayTitle, string accountOpenId, int totalPrice)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (this.nativeCommerceManager != null)
        {
            this.nativeCommerceManager.Call(
                "createPaymentBill",
                orderId,
                displayTitle,
                accountOpenId,
                totalPrice);
        }
#endif
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Instance != null)
        {
            return;
        }

        Instance = this;
        this.commerceCallback = new CommerceCallback();
        this.commerceCallback.LoginFinished += this.CommerceCallback_LoginFinished;
        this.commerceCallback.BillCreated += this.CommerceCallback_BillCreated;

        this.nativeCommerceManager = new AndroidJavaObject("com.mi.dlabs.sdk.commerce.CommerceManager");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Assert(!string.IsNullOrEmpty(this.AppId), "AppId must not be empty");
        Debug.Assert(!string.IsNullOrEmpty(this.AppKey), "AppKey must not be empty");

        this.nativeCommerceManager.Call("registerPayService", activity, this.AppId, this.AppKey, this.commerceCallback);
#endif
        }

        /// <summary>
        /// Handles the LoginFinished event of the CommerceCallback control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LoginFinishedEventArgs"/> instance containing the event data.</param>
        private void CommerceCallback_LoginFinished(object sender, LoginFinishedEventArgs e)
        {
            var handler = this.LoginFinished;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Handles the BillCreated event of the CommerceCallback control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BillCreatedEventArgs"/> instance containing the event data.</param>
        private void CommerceCallback_BillCreated(object sender, BillCreatedEventArgs e)
        {
            var handler = this.BillCreated;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }
    }
}
