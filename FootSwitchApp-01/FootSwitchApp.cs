using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

using AForge.Video;
using AForge.Video.DirectShow;
using FaceDetectTools;
using System.IO;

using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace FootSwitchApp_01
{
    //public bool IsExitCapture { get; set; }
    public partial class FrmFootSwitchApp : Form
    {
        VideoCaptureDevice m_VideoSource = null;
        Bitmap m_LatestImage = null;
        public FrmFootSwitchApp()
        {
            InitializeComponent();
        }
        public bool DeviceExist = false;
        public FilterInfoCollection videoDevices;
        public VideoCaptureDevice videoSource = null;

        //接続されている全てのビデオデバイス情報を格納する変数
        // private FilterInfoCollection videoDevices;
        //使用するビデオデバイス
        // private VideoCaptureDevice videoDevice;
        // ビデオデバイスの機能を格納する配列
        // private VideoCapabilities[] videoCapabilities;

        //ウィンドウが生成された時
        private void FrmFootSwitchApp_Load(object sender, EventArgs e)
        {
            this.getCameraInfo();

            //videoSource = new VideoCaptureDevice(videoDevices[cmbCamera.SelectedIndex].MonikerString);
            //videoSource.NewFrame += new NewFrameEventHandler(videoRendering);
            //this.CloseVideoSource();

            //videoSource.Start();

            //FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //m_VideoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //m_VideoSource.NewFrame += new NewFrameEventHandler(VideoDevice_NewFrame);

            //m_VideoSource.Start();

            BtnCapture.Focus();
        }

        // カメラ情報の取得
        public void getCameraInfo() {
            try {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                cmbCamera.Items.Clear();

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices) {
                    cmbCamera.Items.Add(device.Name);
                    cmbCamera.SelectedIndex = 0;
                    DeviceExist = true;
                }
            } catch (ApplicationException) {
                DeviceExist = false;
                cmbCamera.Items.Add("Deviceが存在していません。");
            }
        }

        //新しいフレームが来た時
        void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            m_LatestImage = (Bitmap)img.Clone();
            PicPicture1.Image = img;
        }

        //ウィンドウを閉じる時
        private void FrmFootSwitchApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_VideoSource != null)
            {
                // 動画撮影停止
                m_VideoSource.Stop();

                if (m_VideoSource.IsRunning == true)
                {
                    m_VideoSource.SignalToStop();
                    m_VideoSource.WaitForStop();
                    m_VideoSource.NewFrame -= new NewFrameEventHandler(VideoDevice_NewFrame);
                }

                m_VideoSource = null;
            }

            if (m_LatestImage != null)
            {
                m_LatestImage.Dispose();
            }
        }

        // 開始or停止ボタン
        //private void button1_Click(object sender, EventArgs e) {
        //    if (button1.Text == "開始") {

        //        if (DeviceExist) {
        //            videoSource = new VideoCaptureDevice(videoDevices[cmbCamera.SelectedIndex].MonikerString);
        //            videoSource.NewFrame += new NewFrameEventHandler(videoRendering);
        //            this.CloseVideoSource();


        //            videoSource.Start();

        //            button1.Text = "停止";
        //            timer1.Enabled = true;
        //        } else {
        //            label1.Text = "デバイスが存在していません。";
        //        }
        //    } else {
        //        if (videoSource.IsRunning) {
        //            timer1.Enabled = false;
        //            this.CloseVideoSource();
        //            label1.Text = "停止中";
        //            button1.Text = "開始";

        //        }
        //    }
        //}

        // 描画処理
        private void videoRendering(object sender, NewFrameEventArgs eventArgs) {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            PicPicture1.Image = img;
        }
        // 停止の初期化
        private void CloseVideoSource() {
            if (!(videoSource == null))
                if (videoSource.IsRunning) {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
        // フレームレートの取得
        //private void timer1_Tick(object sender, EventArgs e) {
        //    label1.Text = videoSource.FramesReceived.ToString() + "FPS";
        //}

        private Mat GetCapture( int camidx ) {

            var camera = new VideoCapture(camidx) {
                // キャプチャする画像のサイズフレームレートの指定
                FrameWidth = 1280,
                FrameHeight = 1080,
                // Fps = 60
            };

            var img = new Mat();
            using (camera) {
                while (true) {
                    //if (this.IsExitCapture) {
                    //    this.Dispatcher.Invoke(() => this._Image.Source = null);
                    //    break;
                    //}

                    camera.Read(img); // Webカメラの読み取り（バッファに入までブロックされる

                    if ( ! img.Empty()) {
                        break;
                    }

                    //Cv2.ImShow("sample_show", img);
                    //Cv2.WaitKey(0);
                    //this.Dispatcher.Invoke(() => {
                    //    this._Image.Source = img.ToWriteableBitmap(); // WPFに画像を表示
                    //});
                }
            }
            return img;
        }

        public string captureDt = "";
        public string output;
        public DateTime dt;
        // ｢撮影｣クリック時に画像を保存
        private void BtnCapture_Click(object sender, EventArgs e)
        {
            //Cv2.ImWrite("capt1.jpg", GetCapture(0));
            //Cv2.ImWrite("capt2.jpg", GetCapture(1));

            //PicPicture2.ImageLocation = "capt1.jpg";
            //PicPicture3.ImageLocation = "capt2.jpg";

            //return;

            // 時刻取得
            captureDt = "";
            dt = DateTime.Now;
            captureDt += (dt.Year).ToString("0000");
            captureDt += (dt.Month).ToString("00");
            captureDt += (dt.Day).ToString("00");
            captureDt += (dt.Hour).ToString("00");
            captureDt += (dt.Minute).ToString("00");
            captureDt += (dt.Second).ToString("00");

            Cv2.ImWrite("C:\\record\\capture\\" + captureDt + "_0.bmp", GetCapture(0));
            Cv2.ImWrite("C:\\record\\capture\\" + captureDt + "_1.bmp", GetCapture(1));

            PicPicture1.ImageLocation = "C:\\record\\capture\\" + captureDt + "_0.bmp";
            PicPicture2.ImageLocation = "C:\\record\\capture\\" + captureDt + "_1.bmp";


            // 画像撮影と、ファイル名指定保存
            //m_LatestImage.Save("img" + captureDt + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //m_LatestImage.Save("C:\\record\\capture\\img" + captureDt + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            // 撮影時刻を画面に表示
            LblCapture.Text = dt + " に撮影しました。";

            //Bitmap bmp = new Bitmap("./img" + captureDt + ".bmp");
            Bitmap bmp = new Bitmap("C:\\record\\capture\\" + captureDt + "_" + cmbCamera.SelectedIndex + ".bmp");
            FaceDetectTools.FaceDetector fd = new FaceDetector("./haarcascade_frontalface_alt2.xml");
        
            var result = fd.Detect(bmp, 2.0);

            //Output Bitmap
            output = "";
            try {
                int i;
                for (i = 0; i < result.Count; i++) {
                    result[i].Save("./result/result_" + i + ".bmp");
                }
                //認証コマンドを実行する
                Process p = new Process();

                string wkFileName = string.Format(Properties.Settings.Default.FormatAuthFileName, Properties.Settings.Default.PathExe);
                string wkArguments = string.Format(Properties.Settings.Default.FormatAuthArguments, Properties.Settings.Default.PathImg);
                p.StartInfo.FileName = wkFileName;                  // 実行するファイル
                p.StartInfo.Arguments = wkArguments;                // 引数
                p.StartInfo.CreateNoWindow = true;                  // コンソールを開かない
                p.StartInfo.UseShellExecute = false;                // シェル機能を使用しない
                p.StartInfo.RedirectStandardOutput = true;          // 標準出力をリダイレクト
                p.Start();                                          // アプリの実行開始

                output = p.StandardOutput.ReadToEnd();       // 標準出力の読み取り
                LblMessage.Text = output;
            }
            catch {
                LblMessage.Text = "顔が検出されませんでした";
                output = "顔が検出できませんでした";
            }

            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter writer = new StreamWriter("C:\\record\\Record.csv", true, enc);
            writer.WriteLine(Convert.ToString(dt) + "," + captureDt + "_" + cmbCamera.SelectedIndex + ".bmp" + "," + output) ;
            writer.Close();

        }





    }
}
