//-----------------------------------------------------------------------
// <copyright file="OrderVerification.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Use this class to query order info from server,
    /// !!!注意， 下面的代码仅用于参考，并且下面的代码应该放在开发者自己的服务器上以保证AppSecret的安全，
    /// 开发者应该使用自己的代码跟自己的服务器通讯, 服务器上运行与下面类似的代码进行查询以确定最终的支付结果。
    /// </summary>
    public class OrderVerification
    {
        /// <summary>
        /// verification query status
        /// </summary>
        public enum QueryStatus
        {
            /// <summary>
            /// query success
            /// </summary>
            Success,

            /// <summary>
            /// The wrong parameter
            /// </summary>
            WrongParameter
        }

        /// <summary>
        /// the ALIPAY
        /// </summary>
        public enum PayType
        {
            /// <summary>
            /// The unknown pay type
            /// </summary>
            Unknown,

            /// <summary>
            /// The ALIPAY
            /// </summary>
            ALIPAY
        }

        /// <summary>
        /// Queries the specified application identifier.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="appSecret">The application secret.</param>
        /// <returns>the verification info.</returns>
        public VerificationInfo Query(string appId, string orderId, string appSecret)
        {
            const string QueryURL = "http://api.miglass.mi.com/queryOrder?";
            string parameters = "devAppId=" + appId;
            parameters += "&orderId=" + orderId;

            HMACSHA1 hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(appSecret));
            byte[] hashValue = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(parameters));
            parameters += "&signature=" + BitConverter.ToString(hashValue).Replace("-", string.Empty).ToLower();
            string finalUrl = QueryURL + parameters;

            Debug.Log("final url is " + finalUrl);

            WebRequest request = WebRequest.Create(finalUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            var response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Debug.Log("response is： " + responseFromServer);
            reader.Close();

            response.Close();

            var queryResult = JsonUtility.FromJson<QueryResult>(responseFromServer);

            VerificationInfo result = new VerificationInfo();

            if (queryResult.code == 0)
            {
                result.Status = QueryStatus.Success;
                if (queryResult.data != null)
                {
                    result.PayedAmount = queryResult.data.buyerPayAmount;
                    result.IsPayed = queryResult.data.paymentStatus.Equals("TRADE_SUCCESS");
                    if (queryResult.data.paymentType == "ALIPAY")
                    {
                        result.PayType = PayType.ALIPAY;
                    }
                    else
                    {
                        result.PayType = PayType.Unknown;
                    }
                }
            }
            else if (queryResult.code == 1353)
            {
                result.Status = QueryStatus.WrongParameter;
            }

            return result;
        }

        /// <summary>
        /// the verification info
        /// </summary>
        public struct VerificationInfo
        {
            /// <summary>
            /// Gets or sets the status.
            /// </summary>
            /// <value>
            /// The status.
            /// </value>
            public QueryStatus Status
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the total payed amount.
            /// </summary>
            /// <value>
            /// The payed amount, unit is 0.01yuan.
            /// </value>
            public int PayedAmount
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the user payed the order.
            /// </summary>
            /// <value>
            ///   <c>true</c> if user payed the order; otherwise, <c>false</c>.
            /// </value>
            public bool IsPayed
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the type of the pay.
            /// </summary>
            /// <value>
            /// The type of the pay.
            /// </value>
            public PayType PayType
            {
                get;
                set;
            }
        }

        #region Component generated code
        /// <summary>
        /// The detail info
        /// </summary>
        [Serializable]
        public class Detail
        {
            /// <summary>
            /// The order identifier
            /// </summary>
            public string orderId;

            /// <summary>
            /// The buyer pay amount
            /// </summary>
            public int buyerPayAmount;

            /// <summary>
            /// The payment status
            /// </summary>
            public string paymentStatus;

            /// <summary>
            /// The payment type
            /// </summary>
            public string paymentType;
        }

        /// <summary>
        /// The query result
        /// </summary>
        [Serializable]
        public class QueryResult
        {
            /// <summary>
            /// the message 
            /// </summary>
            public string msg;

            /// <summary>
            /// The code
            /// </summary>
            public int code;

            /// <summary>
            /// The data
            /// </summary>
            public Detail data;

        }
        #endregion
    }
}
