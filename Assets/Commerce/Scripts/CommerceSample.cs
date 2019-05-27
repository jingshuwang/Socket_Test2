//-----------------------------------------------------------------------
// <copyright file="CommerceSample.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Commerce Sample
    /// </summary>
    public class CommerceSample : MonoBehaviour
    {
        /// <summary>
        /// The button text will be changed by steps.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Text ButtonText;

        /// <summary>
        /// For showing status.
        /// </summary>
        [SerializeField]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Text StatusText;

        /// <summary>
        /// whether is login
        /// </summary>
        private bool isLogining = false;

        /// <summary>
        /// The account information
        /// </summary>
        private AccountInfo accountInfo = null;

        /// <summary>
        /// The order identifier
        /// </summary>
        private string orderId = string.Empty;

        /// <summary>
        /// Called when [click].
        /// </summary>
        public void OnClick()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (object.ReferenceEquals(this.accountInfo, null) && !this.isLogining)
        {
            CommerceManager.Instance.Login();
            this.ButtonText.text = "Logining...";
            this.isLogining = true;
        }
        else if (!object.ReferenceEquals(this.accountInfo, null))
        {
            this.orderId = Guid.NewGuid().ToString();
            CommerceManager.Instance.CreateBill(
                this.orderId,
                "Sample Product",
                this.accountInfo.OpenId,
                1);
        }
#endif
        }

        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        CommerceManager.Instance.LoginFinished += this.Instance_LoginFinished;
        CommerceManager.Instance.BillCreated += this.Instance_BillCreated;
#endif
        }

        /// <summary>
        /// Handles the BillCreated event of the Instance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BillCreatedEventArgs"/> instance containing the event data.</param>
        private void Instance_BillCreated(object sender, BillCreatedEventArgs e)
        {
            this.StatusText.text = "BillCreated, code is " + e.Code + ", order Id is:" + e.OrderInfo.OrderId;
        }

        /// <summary>
        /// Handles the LoginFinished event of the Instance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LoginFinishedEventArgs"/> instance containing the event data.</param>
        private void Instance_LoginFinished(object sender, LoginFinishedEventArgs e)
        {
            if (e.Code == 0)
            {
                this.accountInfo = e.Account;
                this.ButtonText.text = "Pay";
                this.StatusText.text = "Login success! \n    OpenId: " + e.Account.OpenId + "    NickName: "
                    + e.Account.NickName + "\n    Avatar: " + e.Account.Avatar + "    Gender: " + e.Account.Gender;
            }
            else
            {
                this.ButtonText.text = "Login";
                this.StatusText.text = "Login failed! \n    ErrorCode: " + e.Code;
            }

            this.isLogining = false;
        }

        /// <summary>
        /// Verifies the payment.
        /// </summary>
        private void VerifyPayment()
        {
            // !!!注意， 下面的代码仅用于参考，并且下面的代码应该放在开发者自己的服务器上以保证AppSecret的安全，
            // 开发者应该使用自己的代码替换下面的代码跟自己的服务器通讯, 服务器上运行与下面类似的代码进行查询以确定最终的支付结果。
            // AppId 和 AppSecret 可以在开发者后台找到
            // The code below is only for reference, the AppId and AppSecert strings should replaced with your
            // real values on the mi development site. It is not safe to transfer AppSecert value from the app logic,
            // developers should make it a server-to-server transformation.
            OrderVerification verification = new OrderVerification();
            var result = verification.Query("AppId", this.orderId, "AppSecert");

            foreach (System.Reflection.PropertyInfo prop in typeof(OrderVerification.VerificationInfo).GetProperties())
            {
                Debug.Log(string.Format("{0} = {1}", prop.Name, prop.GetValue(result, null)));
            }

            this.orderId = string.Empty;

            if (result.IsPayed)
            {
                this.ButtonText.text = "Payed";
                this.StatusText.text = "You have payed " + result.PayedAmount + " cent(RMB).";
            }
            else
            {
                this.ButtonText.text = "Pay error";
                this.StatusText.text = "Pay error!";
            }
        }

        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            CommerceManager.Instance.LoginFinished -= this.Instance_LoginFinished;
            CommerceManager.Instance.BillCreated -= this.Instance_BillCreated;
        }

        /// <summary>
        /// Called when [application pause].
        /// </summary>
        /// <param name="pasued">if set to <c>true</c> [pasued].</param>
        private void OnApplicationPause(bool pasued)
        {
            // 在应用程序进行支付的时候，会跳到第三方的支付进程，此进程有可能被系统回收，从而无法收到callback，
            // 在应用重启的时候需要主动查询进行支付验证。
            // It is possible that your app is killed by android system while doing payment in another activity,
            // so you should confirm the payment when app is restart or resumed.
            if (pasued)
            {
                PlayerPrefs.SetString("orderId", this.orderId);
                PlayerPrefs.Save();
            }
            else
            {
                this.orderId = PlayerPrefs.GetString("orderId");
                if (!string.IsNullOrEmpty(this.orderId))
                {
                    this.VerifyPayment();
                    PlayerPrefs.SetString("orderId", string.Empty);
                }
            }
        }
    }
}
