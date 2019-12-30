using System;
using System.IO;
using System.Reflection;

namespace NeoFaceCloudSample
{
    class Program
    {
        /// <summary>
        /// ファイルの内容をバッファに全て読み込む
        /// </summary>
        /// <param name="file">対象ファイル名</param>
        /// <returns>
        /// 読み込んだbyte配列
        /// null: 読み込み失敗
        /// </returns>
        private static byte[] ReadBytes(string file)
        {
            byte[] buff;
            try
            {
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    buff = new byte[fs.Length];
                    fs.Read(buff, 0, buff.Length);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            return buff;
        }

        /// <summary>
        /// プログラムの使い方の表示
        /// </summary>
        static void ShowHelp()
        {
            var exe = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            Console.WriteLine("以下の形式で実行してください\n"
                + $"{exe} register JpegFile UserID UserName \n"
                + $"{exe} auth JpegFile [UserID]");
        }

        /// <summary>
        /// 登録コマンドの実行
        /// </summary>
        /// <param name="userID">登録する顔認証対象者ID</param>
        /// <param name="userName">登録する顔認証対象者名</param>
        /// <param name="jpeg">登録する顔画像のJPEGデータ</param>
        static void DoRegister(string userID, string userName, byte[] jpeg)
        {
            var result = NfcRestApi.Register(userID, userName, jpeg).Result;
            if (result.ResultStatus == NfcRestApi.ResultStatus.Success)
            {
                Console.WriteLine($"顔認証対象者を登録しました (userOId:{result.UserOID})");
                return;
            }

            Console.WriteLine("顔認証対象者を登録できませんでした");
            switch (result.ResultStatus)
            {
                case NfcRestApi.ResultStatus.Conflict:
                    Console.WriteLine("指定した顔認証対象者は既に登録されています");
                    break;
                case NfcRestApi.ResultStatus.FaceNotDetected:
                    Console.WriteLine("画像から顔を検出できませんでした");
                    break;
                case NfcRestApi.ResultStatus.MultiFacesDetected:
                    Console.WriteLine("画像に複数の顔が映っています");
                    break;
            }
        }

        /// <summary>
        /// 認証コマンドの実行
        /// </summary>
        /// <param name="jpeg">認証する顔画像のJPEGデータ</param>
        /// <param name="userID">
        /// 顔認証対象者ID
        /// 指定あり: 1:1認証
        /// null: 1:N認証
        /// </param>
        static void DoAuth(byte[] jpeg, string userID)
        {
            var result = NfcRestApi.Auth(jpeg, userID).Result;
            if (result.ResultStatus == NfcRestApi.ResultStatus.Success)
            {
                var user = result.FaceMatches[0].UserMatches[0];
                Console.WriteLine($"顔認証に成功しました (ユーザ名:{user.MatchUser.UserName} 照合スコア:{user.Score})");
                return;
            }

            Console.WriteLine("顔認証に失敗しました");
            switch (result.ResultStatus)
            {
                case NfcRestApi.ResultStatus.IDUnregistered:
                    Console.WriteLine("指定したユーザは登録されていません");
                    break;
                case NfcRestApi.ResultStatus.FaceUnregistered:
                    Console.WriteLine("指定したユーザは顔画像が登録されていません");
                    break;
                case NfcRestApi.ResultStatus.FaceNotDetected:
                    Console.WriteLine("画像から顔を検出できませんでした");
                    break;
                case NfcRestApi.ResultStatus.MultiFacesDetected:
                    Console.WriteLine("画像に複数の顔が映っています");
                    break;
                case NfcRestApi.ResultStatus.FaceCheckInvalid:
                    Console.WriteLine("顔を検出しましたが、照合に必要な品質を満たしていません");
                    break;
                case NfcRestApi.ResultStatus.NotAvailableFeature:
                    Console.WriteLine("顔を検出しましたが、特徴量の抽出に失敗しました");
                    break;
                case NfcRestApi.ResultStatus.NotApplicable:
                    Console.WriteLine("顔を検出しましたが、該当する対象者が見つかりませんでした");
                    break;
                case NfcRestApi.ResultStatus.NotMatched:
                    Console.WriteLine("顔を検出しましたが、閾値を満たす対象者が見つかりませんでした");
                    break;
            }
        }

        /// <summary>
        /// Mainメソッド
        /// </summary>
        /// <param name="args">
        /// ShowHelp()を参照
        /// </param>
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowHelp();
                return;
            }

            var command = args[0];
            var file = args[1];
            var jpeg = ReadBytes(file);
            if (jpeg == null)
            {
                Console.WriteLine($"{file} を開けません");
                return;
            }

            switch (command)
            {
                case "register":
                    if (args.Length < 4)
                    {
                        ShowHelp();
                        return;
                    }
                    var userID = args[2];
                    var userName = args[3];
                    DoRegister(userID, userName, jpeg);
                    break;
                case "auth":
                    userID = null;
                    if (args.Length >= 3)
                    {
                        userID = args[2];
                    }
                    DoAuth(jpeg, userID);
                    break;
                default:
                    ShowHelp();
                    break;
            }
        }
    }
}
