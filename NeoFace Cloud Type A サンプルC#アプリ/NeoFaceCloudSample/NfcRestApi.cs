using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static NeoFaceCloudSample.Constants;

namespace NeoFaceCloudSample
{
    public static class Constants
    {
        public const string TenantID = "T8005270";
        public const string ManageApiKey = "l7801fdb64b7f6447e8d69457ccd456258";
        public const string AuthApiKey = "l7ab472fb5612645aba822eba69bc48284";
        public const string NfcBaseUri = @"https://api.cloud.nec.com/neoface/";
        public const string NfcManageUri = NfcBaseUri + @"v1/";
        public const string NfcAuthUri = NfcBaseUri + @"f-face-image/v1/action/auth";
    }

    public static class NfcRestApi
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// json文字列からStringContentを生成する
        /// </summary>
        /// <param name="json">json文字列</param>
        /// <returns>StringContent</returns>
        private static StringContent CreateJsonContent(string json)
        {
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Httpリクエストとレスポンスをエラー出力する。デバッグ用。
        /// </summary>
        /// <param name="response">Httpレスポンス</param>
        /// <param name="resBody">レスポンスBody</param>
        /// <param name="reqBody">リクエストBody</param>
        private static void DumpResult(HttpResponseMessage response, string resBody, string reqBody = null)
        {
            var uri = response.RequestMessage.RequestUri.ToString().Replace(NfcBaseUri, "/");
            var method = response.RequestMessage.Method;
            if (reqBody != null)
            {
                // 画像部分のデータ出力を省略
                reqBody = Regex.Replace(reqBody, @"Images"":\[""(?<digest>.{10}).+?""\]", @"Images"":\[""${digest}...""\]");
                reqBody = Regex.Replace(reqBody, @"Image"":""(?<digest>.{10}).+?""", @"Image"":""${digest}...""");
            }
            Console.Error.WriteLine($"{method} {uri} {reqBody} -> response: {(int)response.StatusCode} {response.ReasonPhrase} body:{resBody}");
        }

        /// <summary>
        /// byte配列をBase64文字列に変換
        /// </summary>
        /// <param name="data">変換するbyte配列</param>
        /// <returns>Base64文字列</returns>
        private static string ToBase64(byte[] data)
        {
            // URLセーフのBase64形式に変換
            return Convert.ToBase64String(data, Base64FormattingOptions.None).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// 共通的に使用する結果コード
        /// </summary>
        public enum ResultStatus
        {
            Success = 0,
            Failed,
            Conflict,
            FaceNotDetected,
            MultiFacesDetected,
            IDUnregistered,
            FaceUnregistered,
            NotFoundFace,
            FaceCheckInvalid,
            NotAvailableFeature,
            NotApplicable,
            NotMatched,
        }

        /// <summary>
        /// 顔認証対象者情報登録リクエストパラメータ
        /// </summary>
        [DataContract]
        class RegisterUserParam
        {
            [DataMember(Name = "userId")]
            public string UserID { get; set; }
            [DataMember(Name = "userName")]
            public string UserName { get; set; }
            [DataMember(Name = "userState")]
            public string UserState { get; set; }
        }

        /// <summary>
        /// 顔認証対象者情報登録レスポンス
        /// </summary>
        [DataContract]
        public class RegisterUserResult
        {
            [DataMember(Name = "userOId")]
            public int? UserOID { get; set; }
            public ResultStatus ResultStatus { get; set; }

            public override string ToString()
            {
                return $"Status:{ResultStatus} UserOID:{UserOID}";
            }
        }

        /// <summary>
        /// 顔認証対象者情報登録
        /// </summary>
        /// <param name="userID">登録する顔認証対象者ID</param>
        /// <param name="userName">>登録する顔認証対象者名</param>
        /// <returns>顔認証対象者登録結果</returns>
        private static async Task<RegisterUserResult> RegisterUser(string userID, string userName)
        {
            var result = new RegisterUserResult
            {
                ResultStatus = ResultStatus.Failed
            };
            var param = new RegisterUserParam
            {
                UserID = userID,
                UserName = userName,
                UserState = "enabled",
            };

            var reqJson = JsonConvert.SerializeObject(param);
            using (var content = CreateJsonContent(reqJson))
            {
                content.Headers.Add("X-NECCP-Tenant-ID", TenantID);
                content.Headers.Add("apiKey", ManageApiKey);
                using (var response = await client.PostAsync(NfcManageUri + "users", content))
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    DumpResult(response, resultJson, reqJson);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            result = JsonConvert.DeserializeObject<RegisterUserResult>(resultJson);
                            result.ResultStatus = ResultStatus.Success;
                            break;
                        case HttpStatusCode.Conflict:
                            result.ResultStatus = ResultStatus.Conflict;
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 顔認証対象者情報削除レスポンス
        /// </summary>
        [DataContract]
        class UnregisterUserResult
        {
            public ResultStatus ResultStatus { get; set; }

            public override string ToString()
            {
                return $"Status:{ResultStatus}";
            }
        }

        /// <summary>
        /// 顔認証対象者情報削除
        /// </summary>
        /// <param name="userOID">削除する顔認証対象ID</param>
        /// <returns></returns>
        private static async Task<UnregisterUserResult> UnregisterUser(int userOID)
        {
            var result = new UnregisterUserResult
            {
                ResultStatus = ResultStatus.Failed
            };

            using (var request = new HttpRequestMessage(HttpMethod.Delete, NfcManageUri + "users/" + userOID))
            {
                request.Headers.Add("X-NECCP-Tenant-ID", TenantID);
                request.Headers.Add("apiKey", ManageApiKey);
                request.Method = HttpMethod.Delete;

                using (var response = await client.SendAsync(request))
                {
                    DumpResult(response, await response.Content.ReadAsStringAsync());
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            result.ResultStatus = ResultStatus.Success;
                            break;
                        case HttpStatusCode.NotFound:
                            result.ResultStatus = ResultStatus.Failed;
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 顔認証対象者顔情報登録レスポンス
        /// </summary>
        [DataContract]
        class RegisterUserFaceResult
        {
            public ResultStatus ResultStatus { get; set; }

            public override string ToString()
            {
                return $"Status:{ResultStatus}";
            }
        }

        /// <summary>
        /// 顔認証対象者顔情報登録リクエストパラメータ
        /// </summary>
        [DataContract]
        class RegisterUserFaceParam
        {
            [DataMember(Name = "faceIndex")]
            public int? FaceIndex { get; set; }
            [DataMember(Name = "faceState")]
            public string FaceState { get; set; }
            [DataMember(Name = "faceImage")]
            public string FaceImage { get; set; }
        }

        /// <summary>
        /// 顔認証対象者顔情報登録
        /// </summary>
        /// <param name="userOID">登録対象の顔認証対象者</param>
        /// <param name="jpeg">顔画像のJPEGデータ</param>
        /// <returns></returns>
        private static async Task<RegisterUserFaceResult> RegisterUserFace(int userOID, byte[] jpeg)
        {
            var result = new RegisterUserFaceResult
            {
                ResultStatus = ResultStatus.Failed
            };

            var param = new RegisterUserFaceParam
            {
                FaceIndex = 0,
                FaceState = "enabled",
                FaceImage = ToBase64(jpeg),
            };

            var json = JsonConvert.SerializeObject(param);
            using (var content = CreateJsonContent(json))
            {
                content.Headers.Add("X-NECCP-Tenant-ID", TenantID);
                content.Headers.Add("apiKey", ManageApiKey);
                using (var response = await client.PostAsync(NfcManageUri + "users/" + userOID + "/faces", content))
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    DumpResult(response, resultJson, json);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            result.ResultStatus = ResultStatus.Success;
                            break;
                        case HttpStatusCode.Forbidden:
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Conflict:
                            result.ResultStatus = ResultStatus.Failed;
                            break;
                        case (HttpStatusCode)434:
                            result.ResultStatus = ResultStatus.FaceNotDetected;
                            break;
                        case (HttpStatusCode)435:
                            result.ResultStatus = ResultStatus.MultiFacesDetected;
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 顔認証リクエストパラメータ
        /// </summary>
        [DataContract]
        class AuthParam
        {
            [DataMember(Name = "userId")]
            public string UserID { get; set; }

            [DataMember(Name = "paramSetId")]
            public string ParamSetID { get; set; }

            [DataMember(Name = "queryImages")]
            public string[] QueryImages { get; set; }
        }

        /// <summary>
        /// 顔マッチ(顔認証レスポンスの内部クラス)
        /// </summary>
        [DataContract]
        public class FaceMatche
        {
            [DataMember(Name = "faceMatchIndex")]
            public int? FaceMatchIndex { get; set; }

            [DataMember(Name = "userMatches")]
            public UserMatche[] UserMatches { get; set; }
        }

        /// <summary>
        /// 対象者マッチ(顔認証レスポンスの内部クラス)
        /// </summary>
        [DataContract]
        public class UserMatche
        {
            [DataMember(Name = "score")]
            public double? Score { get; set; }

            [DataMember(Name = "matchUser")]
            public MatchUser MatchUser { get; set; }
        }

        /// <summary>
        /// マッチ対象者(顔認証レスポンスの内部クラス)
        /// </summary>
        [DataContract]
        public class MatchUser
        {
            [DataMember(Name = "userOId")]
            public int? UserOID { get; set; }

            [DataMember(Name = "userId")]
            public string UserId { get; set; }

            [DataMember(Name = "userName")]
            public string UserName { get; set; }
        }

        /// <summary>
        /// 顔認証レスポンス
        /// </summary>
        [DataContract]
        public class AuthResult
        {
            [DataMember(Name = "authId")]
            public string AuthID { get; set; }

            [DataMember(Name = "status")]
            public string Status { get; set; }

            [DataMember(Name = "statusCode")]
            public int? StatusCode { get; set; }

            [DataMember(Name = "statusMessage")]
            public string StatusMessage { get; set; }

            [DataMember(Name = "faceMatches")]
            public FaceMatche[] FaceMatches { get; set; }

            [DataMember(Name = "matchUser")]
            public MatchUser MatchUser { get; set; }

            public ResultStatus ResultStatus { get; set; }

            public override string ToString()
            {
                return $"Status:{ResultStatus} StatusCode:{StatusCode}";
            }
        }

        /// <summary>
        /// 顔認証リクエスト
        /// </summary>
        /// <param name="jpeg">顔画像のJPEGデータ</param>
        /// <param name="userID">
        /// 顔認証対象者ID
        /// 指定あり: 1:1認証
        /// null: 1:N認証
        /// </param>
        /// <returns>顔認証結果</returns>
        public static async Task<AuthResult> Auth(byte[] jpeg, string userID = null)
        {
            var result = new AuthResult
            {
                ResultStatus = ResultStatus.Failed
            };

            var param = new AuthParam
            {
                ParamSetID = "auth_30_02_0056",    // 1:N 閾値:中
                QueryImages = new string[] { ToBase64(jpeg) },
            };

            if (userID != null)
            {
                param.UserID = userID;
                param.ParamSetID = "auth_30_01_0050";    // 1:1 userId指定 閾値:中
            }

            var json = JsonConvert.SerializeObject(param);
            using (var content = CreateJsonContent(json))
            {
                content.Headers.Add("apiKey", AuthApiKey);
                using (var response = await client.PostAsync(NfcAuthUri, content))
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    DumpResult(response, resultJson, json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<AuthResult>(resultJson);
                        switch (result.StatusCode)
                        {
                            case 200:
                                result.ResultStatus = ResultStatus.Success;
                                break;
                            case 404:
                                result.ResultStatus = ResultStatus.IDUnregistered;
                                break;
                            case 424:
                                result.ResultStatus = ResultStatus.FaceUnregistered;
                                break;
                            case 434:
                                result.ResultStatus = ResultStatus.NotFoundFace;
                                break;
                            case 435:
                                result.ResultStatus = ResultStatus.MultiFacesDetected;
                                break;
                            case 436:
                                result.ResultStatus = ResultStatus.FaceCheckInvalid;
                                break;
                            case 443:
                                result.ResultStatus = ResultStatus.NotAvailableFeature;
                                break;
                            case 444:
                                result.ResultStatus = ResultStatus.NotApplicable;
                                break;
                            case 445:
                                result.ResultStatus = ResultStatus.NotMatched;
                                break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 顔認証対象者&顔画像登録
        /// </summary>
        /// <param name="userID">顔認証対象者ID</param>
        /// <param name="userName">顔認証対象者名</param>
        /// <param name="jpeg">顔画像のJPEGデータ</param>
        /// <returns>登録結果</returns>
        public static async Task<RegisterUserResult> Register(string userID, string userName, byte[] jpeg)
        {
            RegisterUserResult result;
            result = await RegisterUser(userID, userName);
            if (result.ResultStatus == ResultStatus.Success)
            {
                if (result.UserOID == null)
                {
                    result.ResultStatus = ResultStatus.Failed;
                    return result;
                }

                var userOid = (int)result.UserOID;
                var faceResult = await RegisterUserFace(userOid, jpeg);
                // 顔登録に失敗した場合は、顔認証対象者を削除する
                if (faceResult.ResultStatus != ResultStatus.Success)
                {
                    result.ResultStatus = faceResult.ResultStatus;
                    await UnregisterUser(userOid);
                }
            }

            return result;
        }
    }
}
