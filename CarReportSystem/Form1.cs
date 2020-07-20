using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarReportSystem {
    public partial class Form1 : Form {
        BindingList<CarReport> _CarReports = new BindingList<CarReport>();
        Timer timerUpdateStatusStrip;
        

        public Form1() {
            InitializeComponent();
            

        }
        private void initButton() { //ボタンの非表示制御
            if (dgbCarReport.Rows.Count > 0) {
                btDataUpDate.Enabled = true;
                btDataDelete.Enabled = true;
            } else {
                btDataUpDate.Enabled = false;
                btDataDelete.Enabled = false;
            }
        }
        private void inputAllClear() {  //入力クリア
            dtpWriteDate.Value = System.DateTime.Now; //日付

            //ラジオボタン
            radioButtonClear();

            cbAuthorName.Text = "";
            cbCarName.Text = "";
            tbReport.Text = "";
            pbCarImage.Image = null;
        }
        private void radioButtonClear() {
            rbToyota.Checked = false;
            rbNissan.Checked = false;
            rbHonda.Checked = false;
            rbSubaru.Checked = false;
            rbForeignCar.Checked = false;
            rbEtCetera.Checked = false;
        }
        
        private void setCBAuthor(string Name) { //メーカーのコンボボックスの入力候補の登録
            if (!cbAuthorName.Items.Contains(Name)) {
                cbAuthorName.Items.Add(Name);
            }
        }
        private void setCBCarName(string carName) {
            if (!cbCarName.Items.Contains(carName)) {
                cbCarName.Items.Add(carName);
            }
        }
        private void Form1_Load(object sender, EventArgs e) {
            // TODO: このコード行はデータを 'infosys202010DataSet.CarReport' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            
            //dgbCarReport.Columns[0].Visible = false;

            initButton();
            DataCountLabel();
            timerUpdateStatusStrip = new Timer();
            timerUpdateStatusStrip.Interval = 100;
            timerUpdateStatusStrip.Enabled = true;
            timerUpdateStatusStrip.Tick += timerUpdateStatusStrip_Tick;

            for (int i = 0; i < dgbCarReport.RowCount; i++) {
                if (dgbCarReport.Rows[i].Cells[4].Value != null) {
                    setCBCarName(dgbCarReport.Rows[i].Cells[4].Value.ToString());
                }
            }
            for (int i = 0; i < dgbCarReport.RowCount; i++) {
                if (dgbCarReport.Rows[i].Cells[2].Value != null) {
                    setCBAuthor(dgbCarReport.Rows[i].Cells[2].Value.ToString());
                }
            }

        }

        private void timerUpdateStatusStrip_Tick(object sender, EventArgs e) {
            //現在の日時を更新する
            UpdateDateTimeStatusBar();
        }

        private void btExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void btDataAdd_Click(object sender, EventArgs e) {
            if (cbAuthorName.Text == "") {
                MessageBox.Show("記録者を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                carReportTableAdapter.Insert(

                    dtpWriteDate.Value,
                    cbAuthorName.Text,
                    ReMakerButton(),
                    cbCarName.Text,
                    tbReport.Text,
                    ImageToByteArray(pbCarImage.Image));
 
                //this._CarReports.Insert(0, obj);
                
                setCBAuthor(cbAuthorName.Text);
                setCBCarName(cbCarName.Text);
                //dgbCarReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            }
            
            
            inputAllClear();
            initButton();
            DataCountLabel();
            dgbCarReport.Refresh();
        }

        private void btDataDelete_Click(object sender, EventArgs e) {
            //carReportTableAdapter.Delete(dgbCarReport.CurrentRow.Cells[0].Value,
            //dgbCarReport.CurrentRow.Cells[2].Value, 
            //dgbCarReport.CurrentRow.Cells[1].Value, 
            //dgbCarReport.CurrentRow.Cells[3].Value,
            //dgbCarReport.CurrentRow.Cells[4].Value, 
            //dgbCarReport.CurrentRow.Cells[5].Value, 
            //dgbCarReport.CurrentRow.Cells[6].Value );
            initButton();
            DataCountLabel();
        }

        private void btDataUpDate_Click(object sender, EventArgs e) {
            dgbCarReport.CurrentRow.Cells[0].Value = dgbCarReport.CurrentRow.Cells[0].Value;
            dgbCarReport.CurrentRow.Cells[2].Value  = cbAuthorName.Text;
            dgbCarReport.CurrentRow.Cells[1].Value = dtpWriteDate.Value;
            dgbCarReport.CurrentRow.Cells[3].Value = ReMakerButton();
            dgbCarReport.CurrentRow.Cells[4].Value = cbCarName.Text;
            dgbCarReport.CurrentRow.Cells[5].Value = tbReport.Text;
            dgbCarReport.CurrentRow.Cells[6].Value = pbCarImage.Image;

            btDataSave_Click(sender,e);

            setCBAuthor(cbAuthorName.Text);
            setCBCarName(cbCarName.Text);
            dgbCarReport.Refresh();

            tsmiUpDataSave.Enabled = true;

        }
        private void dgbCarReport_CClick(object sender, EventArgs e) {
            //選択したレコードを取り出す
            //DataGridView で選択した行のインデックス
            if (dgbCarReport.CurrentRow.Cells[1].Value.ToString() != "") {

                dtpWriteDate.Value = DateTime.Parse(dgbCarReport.CurrentRow.Cells[1].Value.ToString());

                cbAuthorName.Text = dgbCarReport.CurrentRow.Cells[2].Value.ToString();
                cbCarName.Text = dgbCarReport.CurrentRow.Cells[4].Value.ToString();
                tbReport.Text = dgbCarReport.CurrentRow.Cells[5].Value.ToString();
                if (dgbCarReport.CurrentRow.Cells[6].Value.ToString() != "") {
                    pbCarImage.Image = ByteArrayToImage((byte[])dgbCarReport.CurrentRow.Cells[6].Value);
                } else {
                    pbCarImage.Image = null;
                }

                    switch (dgbCarReport.CurrentRow.Cells[3].Value.ToString()) {
                        case "DEFAULT":
                        radioButtonClear();
                        break;
                        case "トヨタ":
                        rbToyota.Checked = true;
                        break;
                        case "日産":
                        rbNissan.Checked = true;
                        break;
                        case "ホンダ":
                        rbHonda.Checked = true;
                        break;
                        case "スバル":
                        rbSubaru.Checked = true;
                        break;
                        case "外車":
                        rbForeignCar.Checked = true;
                        break;
                        case "その他":
                        rbEtCetera.Checked = true;
                        break;

                    }
                }
                initButton();
            
        }
        private string ReMakerButton() {
            string button= "";
            if (rbToyota.Checked) {
                button = "トヨタ";
            }else if (rbNissan.Checked) {
                button = "日産";
            }else if (rbHonda.Checked) {
                button = "ホンダ";
            }else if (rbSubaru.Checked) {
                button = "スバル";
            }else if (rbForeignCar.Checked) {
                button = "外車";
            }else if(rbEtCetera.Checked){
                button = "その他";
            } else {
                button = "DEFAULT";
            }

            return button;
        }
        private void CheckMakerButton(CarMaker cm) {
            switch (cm) {
                case CarMaker.DEFAULT:
                    radioButtonClear();
                    break;
                case CarMaker.トヨタ:
                    rbToyota.Checked = true;
                    break;
                case CarMaker.日産:
                    rbNissan.Checked = true;
                    break;
                case CarMaker.ホンダ:
                    rbHonda.Checked = true;
                    break;
                case CarMaker.スバル:
                    rbSubaru.Checked = true;
                    break;
                case CarMaker.外車:
                    rbForeignCar.Checked = true;
                    break;
                case CarMaker.その他:
                    rbEtCetera.Checked = true;
                break;

            }
        }

        private void btImageOpen_Click(object sender, EventArgs e) {
            if (ofdOpenImage.ShowDialog() == DialogResult.OK) {
                //選択した画像をピクチャーボックスに表示
                pbCarImage.Image = Image.FromFile(ofdOpenImage.FileName);
                //ピクチャーボックスのサイズに画像を調整
                pbCarImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) {
            this.Activate();
            pbCarImage.Image = Image.FromFile(ofdOpenImage.FileName);
        }

        private void btImageDelete_Click(object sender, EventArgs e) {
            if (pbCarImage.Image != null) {
                DialogResult dr = MessageBox.Show("画像を削除してもいいですか？", "確認", MessageBoxButtons.OKCancel);
                if (dr == System.Windows.Forms.DialogResult.OK) {
                    pbCarImage.Image = null;
                }
            }
        }

        private void btDataOpen_Click(object sender, EventArgs e) {
            
            this.carReportTableAdapter.Fill(this.infosys202010DataSet.CarReport);
            
        }

        private void btDataSave_Click(object sender, EventArgs e) {
            this.Validate();
            this.carReportBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202010DataSet);
        }
        private void UpdateDateTimeStatusBar() {
            //現在の日時を取得する
            DateTime nowTime = DateTime.Now;
            //日時を文字列に変換する
            string newText = nowTime.ToString();
            //表示する文字列が前と違う場合は、更新する
            if (!tsslTime.Text.Equals(newText,
                StringComparison.Ordinal)) {
                tsslTime.Text = newText;
            }
        }
        private void DataCountLabel() {
            tsslDataCount.Text = "データ件数：" + _CarReports.Count.ToString();
        }

        // バイト配列をImageオブジェクトに変換
        public static Image ByteArrayToImage(byte[] byteData) {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(byteData);
            return img;
        }

        // Imageオブジェクトをバイト配列に変換
        public static byte[] ImageToByteArray(Image img) {
            ImageConverter imgconv = new ImageConverter();
            byte[] byteData = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return byteData;
        }


    }
    
}
