using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Duse
{
    public partial class Form1 : Form
    {
        private CryptVerba cv;
        private string LOGPATH = Application.StartupPath + "\\LOGS";

        public Form1()
        {            
            InitializeComponent();
            int k = 0;

            cv=new CryptVerba();
            Process p;

            try
            {
                p = Process.Start("asrkeyw.exe", "s");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Внимание!",ex.Message);
                return;
            }
            p.WaitForExit();
        }

        //Пишем логи всех операций
        private void WriteLog(string text, string name)

        {
            if (Directory.Exists(LOGPATH) == false)
            {
                try
                {
                    Directory.CreateDirectory(LOGPATH);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Невозможно создать каталог " + LOGPATH + "\n" + e.Message);
                    return;

                }

            }
            StreamWriter sw = new StreamWriter(LOGPATH + "\\" + name + ".log", true, Encoding.GetEncoding(1251));
            sw.WriteLine(DateTime.Now.ToString("[dd.MM.yyyy HH:mm:ss]") + " " + text);
            txtLog.Text = txtLog.Text + DateTime.Now.ToLongTimeString() + " " + text + "\r\n";
            sw.Close();
        }


        //подпись файлов
        private void btnSign_Click(object sender, EventArgs e)
        {
            int k = cv.cInitKey("b:", "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }            
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            string dir_sign = @"Z:\work\develop\material\files\test\unsign\sign\";

            WriteLog("==== Начало подписи файлов ====================","sign");
            try 
            { 
                for (int i=0; i<FileList.Length; i++)
                {
                    string FileName = (new FileInfo(FileList[i]).Name);
                    k=cv.cSignFile(FileList[i],dir_sign + FileName,"3039");
                    if (k!=0)
                    {
                        WriteLog("Ошибка подписи " + FileName + " " + cv.GetError(k),"sign");
                    }
                    else
                    {
                        WriteLog("Файл " + FileName + " успешно подписан.","sign");
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message,"sign");
            }
            WriteLog("==== Конец подписи файлов ====================","sign");
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(),"sign");
            cv.cUnloadKeys();
        }

        //снятие подписи
        private void btnUnsign_Click(object sender, EventArgs e)
        {
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            int k = cv.cInitKey("b:", "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WriteLog("==== Начало снятия подписи файлов ====================", "unsign");
            try
            {
                for (int j = 0; j < FileList.Length; j++)
                {
                    k = cv.cDelSign(FileList[j]);
                    if (k != 0)
                    {
                        WriteLog("Ошибка снятия подписи " + (new FileInfo(FileList[j]).Name)+ " " + cv.GetError(k), "unsign");
                        cv.cUnloadKeys();
                        MessageBox.Show("Необходимо перезапустить программу!", "Ошибка инициализации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                        //return;
                    }
                    else
                    {
                        WriteLog("Подпись файла " + (new FileInfo(FileList[j]).Name) + " успешно снята.", "unsign");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "unsign");
            }
            
            WriteLog("==== Конец снятия подписи файлов ====================", "unsign");
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), "unsign");
            cv.cUnloadKeys();
        }

        //шифрование файлов

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            int k = cv.cInitKey("e:", "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            string dir_sign = @"Z:\work\develop\material\files\test\unsign\sign\";

            WriteLog("==== Начало шифрования файлов ====================", "encrypt");
            try
            {
                for (int i = 0; i < FileList.Length; i++)
                {
                    string FileName = (new FileInfo(FileList[i]).Name);
                    k = cv.cEncryptFile(FileList[i], dir_sign+FileName, "e:", "C:\\TSign\\OPENKEY\\942009", Convert.ToUInt16("1307"), Convert.ToUInt16("8020"), "");
                    if (k != 0)
                    {
                        WriteLog("Ошибка шифрования файла " + FileName + " " + cv.GetError(k), "encrypt");
                    }
                    else
                    {
                        WriteLog("Файл " + FileName + " успешно зашифрован.", "encrypt");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "encrypt");
            }
            WriteLog("==== Конец шифрования файлов ====================", "encrypt");
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), "encrypt");
            cv.cUnloadKeys();        
        }

        //расшифровка файлов
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            cv.cUnloadKeys();
            int k = cv.cInitKey("e:", "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show("Не удалось загрузить ключи с диска " + cv.GetError(k), "Вставьте диск с ключами и повторите попытку");
                return;
            }
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            string dir_sign = @"Z:\work\develop\material\files\test\unsign\sign\";

            WriteLog("==== Начало расшифровки файлов ====================", "decrypt");
            try
            {
                for (int i = 0; i < FileList.Length; i++)
                {

                    //File.Move(FileList[i], FileList[i] + ".tmp");
                    string FileName = (new FileInfo(FileList[i]).Name);
                    k = cv.cDecryptFile(FileList[i], dir_sign + FileName, Convert.ToUInt16("1307"), "e:", "C:\\TSign\\OPENKEY\\942009");
                    if (k > 0)
                    {
                        WriteLog("Ошибка расшифровки " + (new FileInfo(FileList[i]).Name) + " " + cv.GetError(k), "decrypt");
                        cv.cUnloadKeys();
                        MessageBox.Show("Необходимо перезапустить программу!", "Ошибка инициализации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;                        
                    }
                    else
                    {
                        //MessageBox.Show("Файл успешно расшифрован " + FileList[i]);
                        WriteLog("Файл " + FileName + " успешно расшифрован.", "decrypt");
                        //File.Delete(FileList[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "decrypt");
            }
            WriteLog("==== Конец расшифровки файлов ====================", "decrypt");
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), "decrypt");
            cv.cUnloadKeys();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            listView1.View = View.Details;
            listView1.Columns.Add("Имя",270);
            listView1.Columns.Add("Тип",55);
            listView1.Columns.Add("Дата создания",125);
            //listView1.Columns[listView1.Columns.Count - 1].Width = 10;
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            lb1.Text = "Всего файлов: "+FileList.Length.ToString();
            for (int i = 0; i < FileList.Length; i++)
            {
                FileInfo fi = new FileInfo(FileList[i]);
                ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(FileList[i]));
                LVI.SubItems.Add(fi.Extension);
                LVI.SubItems.Add(fi.CreationTime.ToShortDateString()+" "+fi.CreationTime.ToShortTimeString());
                                
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.SelectionLength = 0;
            txtLog.ScrollToCaret();
        }

        private void btnChkSign_Click(object sender, EventArgs e)
        {
            string[] FileList = Directory.GetFiles(@"Z:\work\develop\material\files\test\unsign", "*.*");
            int k = cv.cInitKey("b:", "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show("Не удалось загрузить ключи с диска " + cv.GetError(k), "Вставьте диск с ключами и повторите попытку");
                return;
            }
            WriteLog("==== Начало проверки подписи файлов ====================", "unsign");
            try
            {
                for (int j = 0; j < FileList.Length; j++)
                {
                    k = cv.cCheckSign(FileList[j]);
                    if (k != 0)
                    {
                        WriteLog("Файл " + (new FileInfo(FileList[j]).Name) + " " + cv.GetError(k), "unsign");
                    }
                    else
                    {
                        WriteLog("Файл " + (new FileInfo(FileList[j]).Name) + " подписан.", "unsign");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "unsign");
            }

            WriteLog("==== Конец проверки подписи файлов ====================", "unsign");
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), "unsign");
            cv.cUnloadKeys();
        }

    }
}
