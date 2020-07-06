using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarReportSystem {
    public partial class Form1 : Form {
        BindingList<CarReport> _CarReports = new BindingList<CarReport>();

        public Form1() {
            InitializeComponent();
           
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
            rbToyota.Checked = false;
            rbNissan.Checked = false;
            rbHonda.Checked = false;
            rbSubaru.Checked = false;
            rbForeignCar.Checked = false;
            rbEtCetera.Checked = false;

            cbAuthorName.Text = "";
            cbCarName.Text = "";
            tbReport.Text = "";
            pbCarImage = null;
        }
        
        private void setConboBoxMaker(string Name) { //メーカーのコンボボックスの入力候補の登録
            if (!cbAuthorName.Items.Contains(Name)) {
                cbAuthorName.Items.Add(Name);
            }

        }

        private void Form1_Load(object sender, EventArgs e) {
            initButton();       
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {

        }

        private void btExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void btDataAdd_Click(object sender, EventArgs e) {
            if (cbAuthorName.Text == "") {
                MessageBox.Show("記録者を入力してください", "記録エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                CarReport obj = new CarReport {
                    Date = dtpWriteDate.Value,
                    Author = cbAuthorName.Text,
                    Maker = gbMaker.Controls.

                };
                setConboBoxMaker(cbAuthorName.Text);
                //this._Cars.Insert(0, obj);
                //dgvCarData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }

            inputAllClear();
            initButton();
        }
    }
}
