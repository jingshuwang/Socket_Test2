//-----------------------------------------------------------------------
// <copyright file="CommerceCallback.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;
    using UnityEngine;

    /// <summary>
    /// the commerce callback
    /// </summary>
    public class CommerceCallback : AndroidJavaProxy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceCallback" /> class.
        /// </summary>
        public CommerceCallback() : base("com.mi.dlabs.sdk.commerce.IPaymentCallback")
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
        /// callback for the login finished.
        /// </summary>
        /// <param name="errorCode">The error code.
        /// 0: Success
        /// 1: Invalid parameter
        /// 2: MI account not login.
        /// 10: Login to server error.
        /// -102: Service not available.
        /// -101: Login twice, wait for previous.
        /// -100: Unknown error
        /// </param>
        /// <param name="accountInfo">The account information.</param>
        public void OnLoginFinished(int errorCode, AndroidJavaObject accountInfo)
        {
            var handler = this.LoginFinished;
            if (!object.ReferenceEquals(handler, null))
            {
                AccountInfo info = null;
                if (!object.ReferenceEquals(accountInfo, null))
                {
                    info = new AccountInfo()
                    {
                        OpenId = accountInfo.Call<string>("getOpenId"),
                        SessionId = accountInfo.Call<string>("getSessionId"),
                        NickName = accountInfo.Call<string>("getNickName"),
                        Avatar = accountInfo.Call<string>("getAvatar"),
                        Gender = accountInfo.Call<string>("getGender")
                    };
                }

                ErrorCode code = ErrorCode.UnknownError;
                switch (errorCode)
                {
                    case 0:
                        code = ErrorCode.Success;
                        break;
                    case 1:
                        code = ErrorCode.InvalidParameter;
                        break;
                    case 2:
                        code = ErrorCode.NotLogin;
                        break;
                    case 10:
                        code = ErrorCode.LoginError;
                        break;
                    case -102:
                        code = ErrorCode.LoginError;
                        break;
                    default:
                        code = ErrorCode.UnknownError;
                        break;
                }

                handler.Invoke(
                    this,
                    new LoginFinishedEventArgs()
                    {
                        Code = code,
                        Account = info
                    });
            }
        }

        /// <summary>
        /// callback for create bill finished.
        /// </summary>
        /// <param name="status">The status.
        /// 102: Success.
        /// 100: Create bill failed.
        /// 101: Cancelled by user.
        /// 103: Pay failed.
        /// 104: Unknown error
        /// </param>
        /// <param name="orderInfo">The order information.</param>
        public void OnCreateBillFinished(int status, AndroidJavaObject orderInfo)
        {
            var handler = this.BillCreated;
            if (!object.ReferenceEquals(handler, null))
            {
                OrderInfo info = null;
                if (!object.ReferenceEquals(orderInfo, null))
                {
                    info = new OrderInfo()
                    {
                        OrderId = orderInfo.Call<string>("getCpOrderId"),
                        AppId = orderInfo.Call<string>("getAppId"),
                        AppKey = orderInfo.Call<string>("getAppKey"),
                        OpenId = orderInfo.Call<string>("getOpenId"),
                        LocalCreatedTime = orderInfo.Call<long>("getLocalCreatedTime"),
                        Quantity = orderInfo.Call<int>("getAmounts"),
                        CurrencyType = orderInfo.Call<string>("getCurrencyType"),
                        ProductName = orderInfo.Call<string>("getProductName"),
                        ExtraData = orderInfo.Call<string>("getCpExtraData")
                    };
                }

                ErrorCode code = ErrorCode.UnknownError;
                switch (status)
                {
                    case 102:
                        code = ErrorCode.Success;
                        break;
                    case 100:
                        code = ErrorCode.CreateBillFailed;
                        break;
                    case 101:
                        code = ErrorCode.CanceledByUser;
                        break;
                    case 103:
                        code = ErrorCode.PayError;
                        break;
                    default:
                        code = ErrorCode.UnknownError;
                        break;
                }

                handler.Invoke(
                    this,
                    new BillCreatedEventArgs()
                    {
                        Code = code,
                        OrderInfo = info
                    });
            }
        }
    }
}