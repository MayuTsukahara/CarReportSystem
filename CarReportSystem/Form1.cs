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

        public Form1() {
            InitializeComponent();
            dgbCarReport.DataSource = _CarReports;

        }
        private void initButton() { //ボタンの非表示制御
            if (_CarReports.Count > 0) {
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
            initButton();
        }
        private void btExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void btDataAdd_Click(object sender, EventArgs e) {
            if (cbAuthorName.Text == "") {
                MessageBox.Show("記録者を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                CarReport obj = new CarReport {
                    Date = dtpWriteDate.Value,
                    Author = cbAuthorName.Text,
                    Maker =  ReMakerButton(),
                    CarName = cbCarName.Text,
                    Report = tbReport.Text,
                    Picture = pbCarImage.Image

                };
                
                this._CarReports.Insert(0, obj);
                setCBAuthor(cbAuthorName.Text);
                setCBCarName(cbCarName.Text);
                dgbCarReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            }
            
            inputAllClear();
            initButton();
        }

        private void btDataDelete_Click(object sender, EventArgs e) {
            _CarReports.RemoveAt(dgbCarReport.CurrentRow.Index);
            initButton();
        }

        private void btDataUpDate_Click(object sender, EventArgs e) {
            CarReport selectedReport = _CarReports[dgbCarReport.CurrentRow.Index];
            selectedReport.Author = cbAuthorName.Text;
            selectedReport.Date = dtpWriteDate.Value;
            selectedReport.Maker = ReMakerButton();
            selectedReport.CarName = cbCarName.Text;
            selectedReport.Report = tbReport.Text;
            selectedReport.Picture = pbCarImage.Image;
            setCBAuthor(cbAuthorName.Text);
            setCBCarName(cbCarName.Text);
            dgbCarReport.Refresh();

        }
        private void dgbCarReport_CClick(object sender, EventArgs e) {
            //選択したレコードを取り出す
            //DataGridView で選択した行のインデックス
            CarReport selectedCarReport = _CarReports[dgbCarReport.CurrentRow.Index];
            if (selectedCarReport.Date >= dtpWriteDate.MinDate &&selectedCarReport.Date <= dtpWriteDate.MaxDate) {
                dtpWriteDate.Value = selectedCarReport.Date;
            }
            cbAuthorName.Text = selectedCarReport.Author;
            CheckMakerButton(selectedCarReport.Maker);
            cbCarName.Text = selectedCarReport.CarName;
            tbReport.Text    = selectedCarReport.Report;
            pbCarImage.Image = selectedCarReport.Picture;
            initButton();

        }
        private CarMaker ReMakerButton() {
            var RadioButtonChecked_InGroup = gbMaker.Controls.OfType<RadioButton>().SingleOrDefault(rb => rb.Checked == true);
            string str = "";
            if (RadioButtonChecked_InGroup == null) {
                str = "DEFAULT";
            } else {
                str = RadioButtonChecked_InGroup.Text;
            }
            CarMaker cm = (CarMaker)Enum.Parse(typeof(CarMaker), str);
            return cm;
            
            //先生の
            //ラジオボタンのタグに数字を登録して、それで判断してる
            //RadioButton selectMaker = (gbMaker.Controls.Cast<RadioButton>().FirstOrDefault(rb => rb.Checked));
            //return (CarMaker)int.Parse(selectMaker.Tag.ToString());
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
            pbCarImage.Image = null;
        }

        private void btDataOpen_Click(object sender, EventArgs e) {
            if (ofdOpenData.ShowDialog() == DialogResult.OK) {
                using (FileStream fs = new FileStream(ofdOpenData.FileName, FileMode.Open, FileAccess.Read)) {
                    try {

                        BinaryFormatter formatter = new BinaryFormatter();
                        //逆シリアル化して読み込む
                        _CarReports = (BindingList<CarReport>)formatter.Deserialize(fs);
                        //データグリッドビューに再設定
                        dgbCarReport.DataSource = _CarReports;
                        //選択されてる箇所の表示
                        dgbCarReport_CClick(sender, e);
                    } catch (SerializationException a) {

                        Console.WriteLine("Failed to deserialize. Reason: " + a.Message);
                        throw;
                    }
                }
                dgbCarReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

        private void btDataSave_Click(object sender, EventArgs e) {
            if (sfdSaveData.ShowDialog() == DialogResult.OK) {

                BinaryFormatter formatter = new BinaryFormatter();
                //ファイルストリームを生成
                using (FileStream fs = new FileStream(sfdSaveData.FileName, FileMode.Create)) {
                    try {
                        //シリアル化して保存
                        formatter.Serialize(fs, _CarReports);
                    } catch (SerializationException a) {
                        Console.WriteLine("Failed to serialize. Reason: " + a.Message);
                        throw;
                    }
                }
            }
        }
    }
    
}
