======================================================================
NeoFace Cloud Type A サンプルC#アプリ ソースコード
======================================================================

■説明
NeoFace Cloud Type A (画像転送型)のREST APIの使用例です。
画像ファイルを指定して顔認証を実施します。


■ビルド方法

1.展開
  NeoFaceCloudSampleフォルダを任意の場所に展開します。
  
2. プロジェクトを開く
  Visual Studio 2017 を使用してNeoFaceCloudSample.slnを開きます。

3. APIキーを設定
  NfcRestApi.csを開き、テナントIDとAPIキーをご利用の環境のものに修正します。
  
--------------------------------------------------------------------
    public static class Constants
    {
        public const string TenantID = "テナントIDを記載";
        public const string ManageApiKey = "APIキー(管理用)を記載";
        public const string AuthApiKey = "APIキー(顔認証用)を記載";
        public const string NfcBaseUri = @"https://api.cloud.nec.com/neoface/";
        public const string NfcManageUri = NfcBaseUri + @"v1/";
        public const string NfcAuthUri = NfcBaseUri + @"f-face-image/v1/action/auth";
    }
--------------------------------------------------------------------

4. ビルド
  Visual Studio 2017によりビルドします。


■アプリの利用方法
コマンドプロンプトより実行します。
標準エラー出力に、実行したREST APIのリクエストとレスポンスが出力され、
標準出力に結果が出力されます。

[登録]
NeoFaceCloudSample.exe register JpegFile UserID UserName
UserIDとUserNameの内容で顔認証登録対象者を登録し、JpegFileの顔画像を登録します。

<実行例>
>NeoFaceCloudSample.exe register master.jpg nec001 日電太郎
顔認証対象者を登録しました (userOId:1)


[1:1認証]
NeoFaceCloudSample.exe auth JpegFile UserID
指定したUserIDの顔認証登録対象者に対して、JpegFileの顔画像で認証します。

<実行例>
>NeoFaceCloudSample.exe auth test1.jpeg nec001
顔認証に成功しました (ユーザ名:日電太郎 照合スコア:0.902)


[1:N認証]
NeoFaceCloudSample.exe auth JpegFile
JpegFileの顔画像で認証します。

<実行例>
>NeoFaceCloudSample.exe auth test2.jpeg
顔認証に成功しました (ユーザ名:日電次郎 照合スコア:0.894)


■使用ライブラリ
以下のNuGetパッケージを使用しています。
(自動的にダウンロードされます。)

・Newtonsoft.Json
  ライセンスファイル: packages\Newtonsoft.Json.10.0.3\LICENSE.md


■ライセンス
本ソースコードは、CC0 1.0 全世界 のもとで提供されます。

CC0 1.0 全世界
http://creativecommons.org/publicdomain/zero/1.0/deed.ja


■更新履歴

2018/04/10 Version 1.0
  初版

2018/06/14 Version 1.0.0.1
  paramSetIdの誤りを修正
