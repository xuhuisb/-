using System;
using System.Speech.Synthesis;
using System.Windows.Forms;
using ePS_Robot.公共函数;
using NAudio.Wave;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Data;
using ePS_Robot.人工咨询;

namespace ePS_Robot.药物咨询
{
    public partial class Inquire_Medicine : Form
    {
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private string resultStr;

        private bool isRecord = false;
        private string accessToken = null;

        private SpeechModel speechModel = new SpeechModel();
        private AutomaticSpeechRecognition testASR;
        SpeechSynthesizer synth = new SpeechSynthesizer();
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        private string fileName = string.Empty;
        int times = 0;
        int error_times;
        string medicine_name;
        //public string medicine_name = "";
        //public string requirment = "";

        //string selected_product = "";
        //string selected_spec = "";
        //string selected_manufactory = "";
        //bool timer = true;
        //bool is_button_clicked = false;
        DB_Utility db = new DB_Utility();
        Fun_Css g_cs = new Fun_Css();

        public Inquire_Medicine()
        {
            InitializeComponent();
            testASR = new AutomaticSpeechRecognition(speechModel);
        }

        private void Inquire_Medicine_Load(object sender, System.EventArgs e)
        {
            DataGridViewColumns();
            ColumnsWidth();
            dataGridView1.ClearSelection();
            synth.Speak(Lbl_Ask_Medicine_Name.Text);
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            if (!File.Exists(@".\token.dat"))
            {
                accessToken = testASR.GetStrAccess(); // token file does not exist, send a request
            }
            else
            {
                string[] tokenInfo = File.ReadAllLines(@".\token.dat");

                // check if the token has expired
                if (Convert.ToInt32(tokenInfo[1]) > ClassUtils.CurrentTime2Second())
                {
                    accessToken = tokenInfo[0];
                }
                else
                {
                    accessToken = testASR.GetStrAccess(); // expired, request again to refresh
                }
            }
            speechModel.APIAccessToken = accessToken; // update token in model
            Recorder_Controller();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartRecognize(string apiRecord)
        {
            WavInfo wav = ClassUtils.GetWavInfo(apiRecord);

            //数据量 = (采样频率 × 采样位数 × 声道数 × 时间) / 8
            // 非8k/16k, 16bit 位深, 单声道的，进行格式转换
            if (apiRecord.EndsWith(".mp3", StringComparison.CurrentCultureIgnoreCase)
                || int.Parse(wav.dwsamplespersec.ToString()) != 16000
                || int.Parse(wav.wbitspersample.ToString()) != 16
                || int.Parse(wav.wchannels.ToString()) != 1)
            {
                apiRecord = ClassUtils.Convert2Wav(apiRecord); // convert audio file to 16k，16bit wav
                tempStr = apiRecord;
            }

            //KeyValuePair<string, string> keyVal = (KeyValuePair<string, string>)comboBoxLan.SelectedItem;
            speechModel.APILanguage = "zh"; // fetch the value in comboBox

            if (backgroundWorker.IsBusy != true)
            {
                this.backgroundWorker.RunWorkerAsync(); // do the time consuming task
            }
        }



        #region Record audio
        private bool switchRecord = true;

        /// <summary>
        /// 开始录音/停止录音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recorder()
        {            
            if (switchRecord)
            {
                synth.SpeakAsyncCancelAll();
                switchRecord = false; // switch the record status                
                StartRec();               
            }
            else
            {
                switchRecord = true;                
                isRecord = true;                
                times = 0;
                StopRec();
                string filePath = Environment.CurrentDirectory + @"\record.wav";
                StartRecognize(filePath);
            }
        }
        #endregion

        private string tempStr = ""; // temporary directory to store the converted audio file


        #region Asynchronous work
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // indicate that this is recorded audio
            if (isRecord)
            {
                speechModel.APIRecord = System.Environment.CurrentDirectory + @"\record.wav";
                isRecord = false;
            }

            // indicate that conversion work has been done
            if (tempStr != "")
            {
                speechModel.APIRecord = tempStr;
            }
            resultStr = testASR.GetStrText() + "\r\n";
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int analizing_status;
            this.richTextBox1.Text = resultStr;
            // finish recognizing, delete temporary directory
            if (tempStr != "")
            {
                Directory.Delete(Path.GetDirectoryName(tempStr), true); // delete directory or sub-dir recursively
            }
            analizing_status = Analize_function();
            if (analizing_status == 0)
            {
                string _sql_medicine = @"
SELECT PreCheckSub.ProductName, Spec, Manufactory, Usage, Dosage, ProductEducation.SYZ, ProductEducation.ZYSX  FROM PreCheckSub
INNER JOIN ProductEducation ON PreCheckSub.ProductID = ProductEducation.ProductID
WHERE       ProductEducation.ProductName LIKE '%" + medicine_name + @"%'
GROUP BY PreCheckSub.ProductName, Spec, Manufactory, Usage, Dosage, ProductEducation.SYZ, ProductEducation.ZYSX";
                DataTable dt = db.Dtable(_sql_medicine).Tables[0];

                if (dt.Rows.Count == 0 || dt == null)
                {
                    MessageBox.Show("无该药品");
                }
                else
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Height = 40;
                    dataGridView1.ClearSelection();
                }
            }
            else if (analizing_status > 0 && analizing_status <= 3)
            {
                MessageBox.Show("无法识别，请重试");
                Recorder_Controller();
            }
            else
            {
                Inquire_Manual Inq_Man = new Inquire_Manual();
                Inq_Man.ShowDialog();
            }
            //if analizefunction == 0
            //  查找药名显示在datagridview上
            //else if analizefunction < 0 && analizefunction <= 3
            //   speakansy
            //   messagebox.show
            //   重新录音
            //else
            //   error_times = 0
            //   转到人工咨询
        }

        //语义分析
        public int Analize_function()
        {
            if (richTextBox1.Text == "" || richTextBox1.Text == "识别错误！\n")
            {
                error_times += 1;
                //识别错误计数
                return error_times;
            }
            else
            {
                var client = new Baidu.Aip.Nlp.Nlp("GRccY9WQTzSTmwT6MWhQhpHt", "xnACRllOLKNpYhIMZBxe3BtzbjGKqtcV");
                client.Timeout = 60000;
                //JObject result = JObject.Parse(client);
                JObject result = client.Lexer(richTextBox1.Text);
                //JObject result = client.Lexer(analyzeText);
                string result_json = result.ToString();
                JObject jsons = JObject.Parse(result_json);
                int num = jsons["items"].Count();
                for (int i = 0; i < num; i++)
                {
                    if (jsons["items"][i]["pos"].ToString() == "nz")
                    {
                        if (medicine_name == null || medicine_name == "")
                        {
                            medicine_name = jsons["items"][i]["item"].ToString();
                            //Console.WriteLine(medicine_name);
                        }
                    }   
                }
                return 0;
            }
        }

        #endregion

        public void StartRec()
        {
            waveSource = new WaveIn();
            waveSource.WaveFormat = new NAudio.Wave.WaveFormat(16000, 16, 1); // 16bit,16KHz,Mono的录音格式
            //this.

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            //waveSource.
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            waveFile = new WaveFileWriter(@".\record.wav", waveSource.WaveFormat);
            //waveSource.DataAvailable += waveIn_DataAvailable;
            waveSource.StartRecording();
        }

        /// <summary>
        /// 停止录音
        /// </summary>
        public void StopRec()
        {
            waveSource.StopRecording();

            // Close Wave(Not needed under synchronous situation)
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        /// <summary>
        /// 录音结束后保存的文件路径
        /// </summary>
        /// <param name="fileName">保存wav文件的路径名</param>
        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 开始录音回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
                int s = Math.Abs(BitConverter.ToInt16(e.Buffer, 0));
                if (s < 1000)
                {
                    times += 1;
                }
                else
                {
                    times = 0;
                }
                if (times >= 30)
                {
                    waveSource.DataAvailable -= new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                    times = 0;
                    Recorder();
                }
            }
        }

        /// <summary>
        /// 录音结束回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        public void Recorder_Controller()
        {
            Recorder();
            while (true)
            {
                if (Delay(3))
                {
                    Recorder();
                    break;
                }
            }
        }

        public static bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Seconds;
                Application.DoEvents();
                //Console.WriteLine(s);
            }
            while (s < delayTime);
            return true;
        }

        private void DataGridViewColumns()
        {
            DataGridViewCheckBoxColumn dgvcb = new DataGridViewCheckBoxColumn();
            /*
            dgvcb.Name = "check";
            dgvcb.HeaderText = "";
            dataGridView1.Columns.Add(dgvcb);
            */
            dataGridView1.Columns.Add("RowID", "");
            dataGridView1.Columns["RowID"].DataPropertyName = "RowID";            

            dataGridView1.Columns.Add("ProductName", "药品名称");
            dataGridView1.Columns["ProductName"].DataPropertyName = "ProductName";

            dataGridView1.Columns.Add("Spec", "规格");
            dataGridView1.Columns["Spec"].DataPropertyName = "Spec";

            dataGridView1.Columns.Add("Manufactory", "生产厂商");
            dataGridView1.Columns["Manufactory"].DataPropertyName = "Manufactory";

            dataGridView1.Columns.Add("Usage", "用法");
            dataGridView1.Columns["Usage"].DataPropertyName = "Usage";

            dataGridView1.Columns.Add("Dosage", "用量");
            dataGridView1.Columns["Dosage"].DataPropertyName = "Dosage";

            dataGridView1.Columns.Add("SYZ", "适应症");
            dataGridView1.Columns["SYZ"].DataPropertyName = "SYZ";

            dataGridView1.Columns.Add("ZYSX", "注意事项");
            dataGridView1.Columns["ZYSX"].DataPropertyName = "ZYSX";
            
            dataGridView1.Font = new System.Drawing.Font("宋体", 20F);
            dataGridView1.ColumnHeadersHeight = 35;
            
            dataGridView1.Rows.Add(1);
            dataGridView1.Rows[0].Cells["RowID"].Value = "-";
            //dataGridView1.Rows[0].Cells["FlushDate"].Value = "共计0";
            
            g_cs.DataGridViewWidth(dataGridView1);
        }

        private void ColumnsWidth()
        {
            try
            {
                //dataGridView1.Columns["check"].Width = Convert.ToInt32(dataGridView1.Width * 0.023);
                dataGridView1.Columns["RowID"].Width = Convert.ToInt32(dataGridView1.Width * 0.03);
                dataGridView1.Columns["ProductName"].Width = Convert.ToInt32(dataGridView1.Width * 0.1);
                dataGridView1.Columns["Spec"].Width = Convert.ToInt32(dataGridView1.Width * 0.1);
                dataGridView1.Columns["Manufactory"].Width = Convert.ToInt32(dataGridView1.Width * 0.17);
                dataGridView1.Columns["Usage"].Width = Convert.ToInt32(dataGridView1.Width * 0.1);
                dataGridView1.Columns["Dosage"].Width = Convert.ToInt32(dataGridView1.Width * 0.1);
                dataGridView1.Columns["SYZ"].Width = Convert.ToInt32(dataGridView1.Width * 0.2);
                dataGridView1.Columns["ZYSX"].Width = Convert.ToInt32(dataGridView1.Width * 0.2);
            }
            catch { }
        }
    }
}
