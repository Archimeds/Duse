using System;
using System.Linq;
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
using Microsoft.Win32;
using System.Security.Permissions;


namespace Duse
{
    public partial class StartForm : Form
    {
        private CryptVerba cv;
        private string LOGPATH = Application.StartupPath + "\\LOGS";
        private string TMPATH = Application.StartupPath + "\\TMP";

        FolderBrowserDialog FBD = new FolderBrowserDialog();        
        OpenFileDialog OFD = new OpenFileDialog();
        OpenFileDialog OFD_1 = new OpenFileDialog();
        
        public string diskA = "b:";
        public string diskAkey = "3039";
        public string diskApubkey = "c:\\TSign\\OPENKEY\\941007";
        public string diskB = "e:";
        public string diskBkey = "2307";
        public string diskBpubkey = @"c:\TSign\OPENKEY\941009\";
        public string BIK = "040702734";
        public string regNum = "2232";
        public string numFil = "0004";
        public string FS = "0020";
        public string path_311p = "x:\\material\\files\\311p\\04";                
        public string path_365p_in = "x:\\material\\files\\365p\\in";
        public string path_365p_out = "x:\\material\\files\\365p\\out";
        public string path_365p_arch_in = "x:\\material\\files\\365p\\arch\\in";
        public string path_365p_arch_out = "x:\\material\\files\\365p\\arch\\out";        
        public string path_IZ_FTS = @"z:\work\develop\material\files\RVK_5_0\IZ_FTS";        
        public string path_B_FTS = @"z:\work\develop\material\files\RVK_5_0\B_FTS";
        public string path_arj32 = @"C:\Windows\system32\arj32.exe";
        public string path_PTKPSD = @"c:\PTK PSD\Post\Post";
        public string path_364 = @"x:\material\files\RVK_5_0\B_FTS\PS_EI";
        public string path_tmp = "x:\\material\\files\\tmp";

        public const int MOD_ALT = 0x1;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_SHIFT = 0x4;
        public const int MOD_WIN = 0x8;
        public const int NONE = 0x0;

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("kernel32.dll")]
        public static extern Int16 GlobalAddAtom(string name);
        [DllImport("kernel32.dll")]
        public static extern Int16 GlobalDeleteAtom(Int16 nAtom);
        //Int16 atom = GlobalAddAtom("PFight");
                                
        public StartForm()
        {            
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

            //bool state = RegisterHotKey(this.Handle, atom, MOD_SHIFT | MOD_ALT, (uint)Keys.R);
            //bool state = RegisterHotKey(this.Handle, atom, MOD_WIN, (uint)Keys.W);
            //RegisterHotKey(this.Handle, 553, NONE, (uint)Keys.F5);
            RegisterHotKey(this.Handle, 553, MOD_ALT, (uint)Keys.D1);
            RegisterHotKey(this.Handle, 554, MOD_WIN, (uint)Keys.W);

            InitializeComponent();           
            
        }
        

        private void StartForm_Load(object sender, EventArgs e)
        {            
            btnVFL.Visible = cmBox365.Visible = false;
            LoadSet();            
            cmBoxType.Text = cmBoxType.Items[0].ToString();            
            //FileWatcher(@"X:\material\files\test\unsign", "*.txt");
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCheckBox();
            SaveSet();
            UnregisterHotKey(this.Handle, 553);
            UnregisterHotKey(this.Handle, 554);
            //GlobalDeleteAtom(atom);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int HotKeyID = m.WParam.ToInt32();
                if (HotKeyID==554)
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else if (this.WindowState == FormWindowState.Minimized)
                    {
                        notifyIcon.Visible = false; this.Show();
                        this.WindowState = FormWindowState.Normal;
                    }
                }
                if (HotKeyID == 553)
                {
                    switch (cmBoxType.Text)
                    {                        
                        case "365-П":

                        switch (cmBox365.Text)
                        {
                            case "Входящие":
                                lvLoad365(txt365_in.Text, "*.vrb");
                                break;
                            case "Исходящие":
                                lvLoad365(txt365_out.Text, "*.*");
                            break;
                        }                        
                        break;
                    }
                }
            }            
            base.WndProc(ref m);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x0312 && m.WParam.ToString() == atom2.ToString())            
        //    {
        //        if (this.WindowState == FormWindowState.Normal)
        //        {
        //            this.WindowState = FormWindowState.Minimized;
        //        }
        //        else if (this.WindowState == FormWindowState.Minimized)
        //        {
        //            notifyIcon.Visible = false; this.Show();
        //            this.WindowState = FormWindowState.Normal;
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        private void btnStart_Click(object sender, EventArgs e)
        {            
            switch (cmBoxType.Text)
            {
                case "311-П":
                    RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");

                    try
                    {
                        string folder_name = dTPick.Value.ToString("_ddMMyy");
                        string format_date = dTPick.Value.ToString("yyMMdd");
                        if (chkTR1.Checked == true)
                        {
                            if (!Directory.Exists(tsLB3.Text + "\\" + folder_name))
                            {
                                Directory.CreateDirectory(tsLB3.Text + "\\" + folder_name);
                            }
                            lViewFCopy(tsLB3.Text + "\\" + folder_name);
                        }
                        if (chkTR2.Checked == true)
                        {
                            if (!Directory.Exists(tsLB3.Text + "\\" + folder_name))
                            {
                                Directory.CreateDirectory(tsLB3.Text + "\\" + folder_name);
                            }
                            lViewFCopy(tsLB3.Text + "\\" + folder_name);
                        }
                        if (checkSign.Checked)
                            SignFiles(tsLB3.Text,"*.xml","sign_311");
                        if (checkCrypt.Checked)
                            CryptFiles(tsLB3.Text, "*.xml", "crypt_311");
                        if (checkPack.Checked)
                        {
                            if (chkTR1.Checked == true)
                            {                                
                                string FileName = "AN" + txtBIK.Text.Substring(4, 5) + format_date + myNumUpD.Text.Substring(0, 4) + ".arj";
                                PackFilesARJ(txtPTK_PSD.Text, "m -e", FileName, tsLB3.Text, "*.xml");                                
                                myNumUpD.Value++;
                                reg.SetValue("numUPD_311_TR1", myNumUpD.Text);
                                                                
                                lvLoad311(tsLB3.Text, "*.xml");
                            }
                            if (chkTR2.Checked == true)
                            {                                
                                string FileName = "BN" + txtBIK.Text.Substring(4, 5) + format_date + myNumUpD.Text.Substring(0, 4) + ".arj";
                                PackFilesARJ(txtPTK_PSD.Text, "m -e", FileName, tsLB3.Text, "*.xml");
                                myNumUpD.Value++;
                                reg.SetValue("numUPD_311_TR2", myNumUpD.Text);
                                 
                                lvLoad311(tsLB3.Text, "*.xml");
                            }
                            
                        }
                    }
                    catch (Exception EX)
                    { MessageBox.Show(EX.Message); }
                    break;
                case "364-П":
                    try
                    {
                        string folderName = DateTime.Now.ToString("_ddMMyy");
                        if (checkSign.Checked)
                        {                            
                            if (!Directory.Exists(tsLB3.Text + "\\" + folderName))
                            {
                                Directory.CreateDirectory(tsLB3.Text + "\\" + folderName);
                            }
                            lViewFCopy(tsLB3.Text + "\\" + folderName);
                            SignFiles(tsLB3.Text, "*.xml", "sign_364");
                        }                            
                        if (checkCrypt.Checked)
                            CryptFiles(tsLB3.Text, "*.xml", "crypt_364");
                        if (checkPack.Checked)
                        {
                            string[] filelist = Directory.GetFiles(tsLB3.Text, "*.arj");
                            foreach (string f in filelist)
                            {
                                string fName = Path.GetFileName(f);
                                WriteLog("====|||======================================", "file_copy");
                                File.Copy(Path.Combine(tsLB3.Text, fName), Path.Combine(txtPTK_PSD.Text, fName), true);
                                WriteLog(fName + " скопирован в " + txtPTK_PSD.Text, "file_copy");
                                WriteLog("====|||======================================", "file_copy");
                                File.Move(Path.Combine(tsLB3.Text, fName), Path.Combine(tsLB3.Text + "\\" + folderName, fName));
                                WriteLog(fName + " перенесен в " + tsLB3.Text + "\\" + folderName, "file_move");
                                WriteLog("====|||======================================", "file_move");                                
                            }
                            string[] flist = Directory.GetFiles(tsLB3.Text, "*.xml");
                            foreach (string s in flist)
                            {
                                File.Delete(s);
                            }
                            listView1.Clear();
                        }
                    }
                    catch {}
                    break;
                case"364-П(Декл.)":
                    try
                    {
                        if (checkSign.Checked)
                            SignFiles(tsLB3.Text,"*.xml","sign_364");
                        if (checkDecrypt.Checked)
                            DecryptFiles("*.xml","decrypt_364");
                        if (checkUnsign.Checked)
                            UnsignFiles("*.xml","unsign_364");
                        if (checkPack.Checked)
                        {
                            string folderName = DateTime.Now.ToString("_ddMMyy");
                            string dt_pik_year = dTPick.Value.ToString("yyyy");
                            string dt_pik_month = dTPick.Value.ToString("MM");
                            string dt_pik_day = dTPick.Value.ToString("dd");
                            string xml_in_DT = txt_IZ_FTS.Text + @"\DT\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day + "\\";
                            string xml_in_KESDT = txt_B_FTS.Text + @"\KESDT\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day + "\\";
                            if (chkDT.Checked)
                            {
                                if (!Directory.Exists(tsLB3.Text + "\\" + folderName))
                                {
                                    Directory.CreateDirectory(tsLB3.Text + "\\" + folderName);
                                }
                                lViewFMove(tsLB3.Text + "\\" + folderName);                                
                                lvLoad364(xml_in_DT, "*.xml");
                            }
                            if (chkKESDT.Checked)
                            {
                                if (!Directory.Exists(tsLB3.Text + "\\" + folderName))
                                {
                                    Directory.CreateDirectory(tsLB3.Text + "\\" + folderName);
                                }
                                lViewFMove(tsLB3.Text + "\\" + folderName);
                                string[] filelist = Directory.GetFiles(tsLB3.Text, "*.arj");
                                foreach (string f in filelist)
                                {
                                    string fName = Path.GetFileName(f);
                                    //if (!File.Exists(Path.Combine(xml_in_KESDT, fName)))
                                    WriteLog("====|||======================================", "pack_create");
                                    File.Copy(Path.Combine(tsLB3.Text, fName), Path.Combine(tsLB3.Text +"\\" + folderName, fName), true);
                                    WriteLog(fName + " скопирован в " + tsLB3.Text + folderName, "pack_create");
                                    File.Move(Path.Combine(tsLB3.Text, fName), Path.Combine(txtPTK_PSD.Text, fName));
                                    WriteLog(fName + " перенесен в " + txtPTK_PSD.Text, "pack_create");
                                    WriteLog("====|||======================================", "pack_create");
                                }
                                lvLoad364(xml_in_KESDT, "*.xml");
                            }
                            
                        }
                    }
                    catch{}
                    break;
                 case"365-П":
                    try
                    {
                        switch (cmBox365.Text)
                        { 
                            case "Исходящие":
                                string format_path = DateTime.Now.ToString("ddMMyy");
                                if (!Directory.Exists(txt365_arch_out.Text + "\\" + format_path))
                                    {
                                        Directory.CreateDirectory(txt365_arch_out.Text + "\\" + format_path);
                                    }
                                        lViewFCopy(txt365_arch_out.Text + "\\" + format_path);
                                if (checkSign.Checked)
                                    SignFiles(tsLB3.Text,"*.*","sign_365");
                                if (checkCrypt.Checked)
                                    CryptFiles(tsLB3.Text, "*.vrb", "crypt_365");                                               
                                if (checkPack.Checked)
                                    {
                                        if (!Directory.Exists(TMPATH))
                                    {
                                        Directory.CreateDirectory(TMPATH);
                                    }
                                        string format_date=dTPick.Value.ToString("yyyyMMdd");
                                        string FileName = "AFN_" + txtBIK.Text.Substring(2, 7) + "_MIFNS00_" + format_date + "_" + myNumUpD.Text.Substring(1, 3) + ".arj";
                                        PackFilesARJ(TMPATH, "m -e", FileName, tsLB3.Text, "*.*");
                                        SignFiles(TMPATH,"*.arj","sign_365_arj");
                                        string[] filelist = Directory.GetFiles(TMPATH,"*.arj");
                                        foreach (string f in filelist)
                                            {
                                                string fName = Path.GetFileName(f);
                                                //if (!File.Exists(Path.Combine(xml_in_KESDT, fName)))
                                                WriteLog("====|||======================================", "pack_create");
                                                File.Copy(Path.Combine(TMPATH, fName), Path.Combine(txt365_arch_out.Text+"\\"+format_path, fName), true);
                                                WriteLog(fName + " скопирован в " + txt365_arch_out.Text + "\\" + format_path, "pack_create");                                                
                                                File.Move(Path.Combine(TMPATH, fName), Path.Combine(txtPTK_PSD.Text, fName));
                                                WriteLog(fName + " перенесен в " + txtPTK_PSD.Text, "pack_create");
                                                WriteLog("====|||======================================", "pack_create");
                                            }
                                            myNumUpD.Value++;
                                            lvLoad365(tsLB3.Text, "*.vrb");
                                    }
                                        break;
                            case "Входящие":
                                        if (checkDecrypt.Checked)
                                        {
                                            DecryptFiles("*.vrb", "decrypt_365");
                                            listView1.Clear();
                                            InsertABS();
                                        }
                                            break;
                        } 
                        
                            
                    }
                    catch{}
                    break;
                case "Other":
                    try
                    {
                        if (checkSign.Checked)
                            SignFiles(tsLB3.Text, "*.*", "sign_other");
                        if (checkDecrypt.Checked)
                            DecryptFiles("*.*", "decrypt_other");
                        if (checkUnsign.Checked)
                            UnsignFiles("*.*", "unsign_other");
                        if (checkCrypt.Checked)
                            CryptFiles(tsLB3.Text, "*.*", "crypt_other");
                    }
                    catch {}
                    break;
            }           
        }

        
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.SelectionLength = 0;
            txtLog.ScrollToCaret();
        }

        private void btnDef_Click(object sender, EventArgs e)
        {
            DefSet();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSet();
        }


        private void cmBoxType_DropDown(object sender, EventArgs e)
        {
            SaveCheckBox();
        }

        private void cmBoxType_TextChanged(object sender, EventArgs e)
        {
            LoadCheckBox();
        }

        private void cmBoxType_DropDownClosed(object sender, EventArgs e)
        {
            LoadCheckBox();
        }

        private void cmBox365_TextChanged(object sender, EventArgs e)
        {
            Load365cmb();
        }                 

        //Ожидание события над файлом
        private void OnChanged(object source, FileSystemEventArgs e)
        {            
            //MessageBox.Show("File "+e.FullPath+""+e.ChangeType);
            // Specify what is done when a file is changed, created, or deleted.
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            //try
            //{
            //    WriteLog("Файл: " + e.FullPath + " " + e.ChangeType, "123");
            //}
            //catch ( Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}                        
        }

        //Ожидание события над файлом
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            //MessageBox.Show("File "+e.OldFullPath+" renamed "+e.FullPath);
            // Specify what is done when a file is renamed.
            //Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            //try
            //{
            //    WriteLog("Файл: " + e.FullPath + " " + e.ChangeType, "123");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

                                
        //проверка наличия файлов в каталоге
        private void FileWatcher(string patch, string filter)
        {
            FileSystemWatcher watcher=new FileSystemWatcher();
            watcher.Path = patch;
            watcher.Filter = filter;
            //watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
 
        }

        private void Load365cmb()
        {
            switch (cmBox365.Text)
            { 
                case "Входящие":
                    checkPack.Enabled = checkPack.Checked = false;
                    checkDecrypt.Enabled = checkDecrypt.Checked = true;
                    checkUnsign.Enabled = checkUnsign.Checked = checkSign.Enabled = checkSign.Checked = checkCrypt.Enabled = checkCrypt.Checked = false;
                    lvLoad365(txt365_in.Text, "*.vrb");
                    break;
                case "Исходящие":
                    checkDecrypt.Enabled = checkDecrypt.Checked = checkUnsign.Enabled = checkUnsign.Checked = false;
                    checkSign.Enabled = checkSign.Checked = checkCrypt.Enabled = checkCrypt.Checked = checkPack.Enabled = checkPack.Checked = true;
                    lvLoad365(txt365_out.Text, "*.*");
                    break;
            }
        }

        //загрузка значений Checkbox-ов
        private void LoadCheckBox()
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");

            switch (cmBoxType.Text)
            {
                case "364-П":                    
                    myNumUpD.Visible = false;                    
                    btnVFL.Visible = true;
                    cmBox365.Visible = false;
                    splitContainer1.Panel1.Enabled = chkDT.Checked = chkKESDT.Checked = chkTR1.Checked = chkTR2.Checked = false;
                    listView1.Clear();
                    checkPack.Text = "В архив";
                    checkPack.Enabled=true;
                    checkPack.Checked = false;
                    checkSign.Enabled =checkSign.Checked= true;
                    checkCrypt.Enabled =checkCrypt.Checked= true;
                    checkUnsign.Enabled = checkUnsign.Checked = false;
                    checkDecrypt.Enabled = checkDecrypt.Checked = false;
                    break;
                case "364-П(Декл.)":                    
                    checkPack.Text = "В архив";
                    btnVFL.Visible = cmBox365.Visible = false;
                    splitContainer1.Panel1.Enabled = true;
                    chkDT.Enabled = chkKESDT.Enabled = true;
                    chkDT.Checked = chkKESDT.Checked = chkTR1.Enabled = chkTR1.Checked = chkTR2.Enabled = chkTR2.Checked = false;
                    myNumUpD.Visible = false;
                    listView1.Clear();
                    break;
                case "365-П":
                    try { myNumUpD.Text=reg.GetValue("numUPD_365").ToString();}
                    catch { myNumUpD.Text = "0001"; }
                    checkPack.Text = "Архивировать";
                    //checkPack.Enabled = checkDecrypt.Enabled = checkUnsign.Enabled =checkSign.Enabled=checkCrypt.Enabled= true;
                    myNumUpD.Visible = true;
                    btnVFL.Visible = false;
                    cmBox365.Visible = true;
                    cmBox365.Text = cmBox365.Items[0].ToString();
                    Load365cmb();
                    splitContainer1.Panel1.Enabled = chkDT.Checked = chkKESDT.Checked = chkTR1.Checked = chkTR2.Checked = false;
                    break;
                case "311-П":
                //default:           
                    try { myNumUpD.Text = reg.GetValue("numUPD_311_TR1").ToString(); }
                    catch { myNumUpD.Text = "0001"; } 
                    try { checkSign.Checked = reg.GetValue("sign311").ToString() == "false" ? false : true; }
                    catch { checkSign.Checked = true; }
                    try { checkCrypt.Checked = reg.GetValue("crypt311").ToString() == "false" ? false : true; }
                    catch { checkCrypt.Checked = true; }
                    try { checkPack.Checked = reg.GetValue("pack311").ToString() == "false" ? false : true; }
                    catch { checkPack.Checked = true; }                    
                    myNumUpD.Visible = true;                    
                    checkPack.Text = "Архивировать";
                    checkPack.Enabled = true;
                    checkSign.Enabled = true;
                    checkCrypt.Enabled = true;
                    checkUnsign.Enabled = checkUnsign.Checked = false;
                    checkDecrypt.Enabled = checkDecrypt.Checked = false;
                    splitContainer1.Panel1.Enabled = true;
                    chkTR1.Enabled = chkTR2.Enabled = true;
                    chkDT.Checked = chkKESDT.Checked = chkDT.Enabled = chkKESDT.Enabled = chkTR1.Checked = chkTR2.Checked = false;
                    chkTR1.Checked = chkTR2.Checked = false;
                    btnVFL.Visible = cmBox365.Visible = false;
                    listView1.Clear();
                    //btnVFL.Visible = true;
                    //listView1.Items.Clear();
                    //tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();
                    //tsLB3.Text = "";
                    break;
                case "Other..":
                    myNumUpD.Visible = false;
                    btnVFL.Visible = true;
                    cmBox365.Visible = false;
                    splitContainer1.Panel1.Enabled = chkDT.Checked = chkKESDT.Checked = chkTR1.Checked = chkTR2.Checked = false;
                    listView1.Clear();
                    checkPack.Text = "Архивировать";
                    checkPack.Enabled=checkPack.Checked = false;
                    checkSign.Enabled =checkSign.Checked= true;
                    checkCrypt.Enabled =checkCrypt.Checked= true;
                    checkUnsign.Enabled = checkUnsign.Checked = true;
                    checkDecrypt.Enabled = checkDecrypt.Checked = true;
                    break;
            }

            reg.Close();
        }

        //сохранение значений checkbox-ов
        private void SaveCheckBox()
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");
            switch (cmBoxType.Text)
            {                
                case "365-П":
                    reg.SetValue("numUPD_365", myNumUpD.Text);
                    break;
                case "311-П":                    
                    reg.SetValue("sign311", checkSign.Checked ? "true" : "false");
                    reg.SetValue("crypt311", checkCrypt.Checked ? "true" : "false");
                    reg.SetValue("pack311", checkPack.Checked ? "true" : "false");                                        
                    break;
            }
            reg.Close();
        }

        //сохранение настроек
        private void SaveSet()
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");                        
            reg.SetValue("year_dtPick", dTPick.Value.Year);
            reg.SetValue("month_dtPick", dTPick.Value.Month);
            reg.SetValue("day_dtPick", dTPick.Value.Day);
            reg.SetValue("diskA", txtdiskA.Text);
            reg.SetValue("diskAkey", txtdiskAkey.Text);
            reg.SetValue("diskApubkey", txtdiskApubkey.Text);
            reg.SetValue("diskB", txtdiskB.Text);
            reg.SetValue("diskBkey", txtdiskBkey.Text);
            reg.SetValue("diskBpubkey", txtdiskBpubkey.Text);
            reg.SetValue("BIK", txtBIK.Text);
            reg.SetValue("regNum", txtregNum.Text);
            reg.SetValue("numFil", txtnumFil.Text);
            reg.SetValue("FS", txtFS.Text);
            reg.SetValue("path_311p", txt311p.Text);
            reg.SetValue("path_IZ_FTS", txt_IZ_FTS.Text);
            reg.SetValue("path_B_FTS", txt_B_FTS.Text);
            reg.SetValue("path_365p_in", txt365_in.Text);
            reg.SetValue("path_365p_out", txt365_out.Text);
            reg.SetValue("path_365_arch_in", txt365_arch_in.Text);
            reg.SetValue("path_365_arch_out", txt365_arch_out.Text);
            reg.SetValue("path_PTK_PSD", txtPTK_PSD.Text);
            reg.SetValue("path_arj32", txtArj32.Text);
            reg.SetValue("path_run.bat", txt_insABS.Text);
            reg.SetValue("path_364",txt_364.Text);            
            reg.Close();
        }

        //загрузка настроек
        private void LoadSet()
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");
                        
            string year_dtPick=null, month_dtPick=null, day_dtPick=null;
            try { year_dtPick = reg.GetValue("year_dtPick").ToString(); }
            catch { reg.SetValue("year_dtPick", DateTime.Now.Year); year_dtPick = DateTime.Now.Year.ToString(); }
            try { month_dtPick = reg.GetValue("month_dtPick").ToString(); }
            catch { reg.SetValue("month_dtPick", DateTime.Now.Month); month_dtPick = DateTime.Now.Month.ToString(); }
            try { day_dtPick = reg.GetValue("day_dtPick").ToString(); }
            catch { reg.SetValue("day_dtPick", DateTime.Now.Day); day_dtPick = DateTime.Now.Day.ToString(); }
            try { txtdiskA.Text = reg.GetValue("diskA").ToString(); }
            catch { txtdiskA.Text = diskA; }
            try { txtdiskAkey.Text = reg.GetValue("diskAkey").ToString(); }
            catch { txtdiskAkey.Text = diskAkey; }
            try { txtdiskApubkey.Text = reg.GetValue("diskApubkey").ToString(); }
            catch { txtdiskApubkey.Text = diskApubkey; }
            try { txtdiskB.Text = reg.GetValue("diskB").ToString(); }
            catch { txtdiskB.Text = diskB; }
            try { txtdiskBkey.Text = reg.GetValue("diskBkey").ToString(); }
            catch { txtdiskBkey.Text = diskBkey; }
            try { txtdiskBpubkey.Text = reg.GetValue("diskBpubkey").ToString(); }
            catch { txtdiskBpubkey.Text = diskBpubkey; }
            try { txtBIK.Text = reg.GetValue("BIK").ToString(); }
            catch { txtBIK.Text = BIK; }
            try { txtregNum.Text = reg.GetValue("regNum").ToString(); }
            catch { txtregNum.Text = regNum; }
            try { txtnumFil.Text = reg.GetValue("numFil").ToString(); }
            catch { txtnumFil.Text = numFil; }
            try { txtFS.Text = reg.GetValue("FS").ToString(); }
            catch { txtFS.Text = FS; }
            try { txt311p.Text = reg.GetValue("path_311p").ToString(); }
            catch { txt311p.Text = path_311p; }
            try { txt_IZ_FTS.Text = reg.GetValue("path_IZ_FTS").ToString(); }
            catch { txt_IZ_FTS.Text = path_IZ_FTS; }
            try { txt_B_FTS.Text = reg.GetValue("path_B_FTS").ToString(); }
            catch { txt_B_FTS.Text = path_B_FTS; }
            try { txt365_in.Text = reg.GetValue("path_365p_in").ToString(); }
            catch { txt365_in.Text = path_365p_in; }
            try { txt365_out.Text = reg.GetValue("path_365p_out").ToString(); }
            catch { txt365_out.Text = path_365p_out; }
            try { txt365_arch_in.Text = reg.GetValue("path_365_arch_in").ToString(); }
            catch { txt365_arch_in.Text = path_365p_arch_in; }
            try { txt365_arch_out.Text = reg.GetValue("path_365_arch_out").ToString(); }
            catch { txt365_arch_out.Text = path_365p_arch_out; }
            try { txtArj32.Text = reg.GetValue("path_arj32").ToString(); }
            catch { txtArj32.Text = path_arj32; }
            try { txt_insABS.Text = reg.GetValue("path_run.bat").ToString(); }
            catch { txt_insABS.Text = path_arj32; }
            try { txtPTK_PSD.Text = reg.GetValue("path_PTK_PSD").ToString(); }
            catch { txtPTK_PSD.Text = path_PTKPSD; }
            try { txt_364.Text = reg.GetValue("path_364").ToString(); }
            catch { txt_364.Text = path_364; }
            reg.Close();
            dTPick.Value = new DateTime(Convert.ToInt32(year_dtPick), Convert.ToInt32(month_dtPick),Convert.ToInt32(day_dtPick));
        }
        

        //настройки по дефолту
        private void DefSet()
        {
            txtdiskA.Text=diskA;
            txtdiskAkey.Text=diskAkey;
            txtdiskApubkey.Text=diskApubkey;
            txtdiskB.Text=diskB;
            txtdiskBkey.Text=diskBkey;
            txtdiskBpubkey.Text=diskBpubkey;
            txtBIK.Text=BIK;
            txtregNum.Text=regNum;
            txtnumFil.Text=numFil;
            txtFS.Text=FS;
            txt311p.Text = path_311p;
            txt_IZ_FTS.Text = path_IZ_FTS;
            txt_B_FTS.Text = path_B_FTS;
            txt365_in.Text = path_365p_in;
            txt365_out.Text = path_365p_out;
            txt365_arch_in.Text = path_365p_arch_in;
            txt365_arch_out.Text = path_365p_arch_out;
            txtArj32.Text = path_arj32;
            txtPTK_PSD.Text = path_PTKPSD;
            txt_364.Text = path_364;
        }

        //Загрузка данных в ListView 311-П
        public void lvLoad311(string path_name,string file_ex)
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("Имя", 300);
            listView1.Columns.Add("Тип", 40);
            listView1.Columns.Add("Дата создания", 100);
            listView1.Columns.Add("Размер", 65);
            string[] FileList = Directory.GetFiles(path_name, file_ex);
            tsLB3.Text = path_name.ToString();

            foreach (string fi in FileList)
            {
                FileInfo fiz = new FileInfo(fi);
                ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(fiz.Name));
                LVI.SubItems.Add(fiz.Extension.ToUpper());
                LVI.SubItems.Add(fiz.CreationTime.ToShortDateString() + " " + fiz.CreationTime.ToShortTimeString());
                LVI.SubItems.Add(fiz.Length.ToString() + " байт");
            }
            tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();            
        }

        //Загрузка данных в ListView 364-П(не декларации)
        private void lvload364p(string path_name)
        {            
            OFD_1.Title = "Выбери файл(ы) для обработки";
            OFD_1.InitialDirectory = path_name;
            OFD_1.Filter = "xml files (*.xml)|*.xml";
            OFD_1.Multiselect = true;
            OFD_1.FilterIndex = 1;
            OFD_1.RestoreDirectory = false;

            if (OFD_1.ShowDialog() == DialogResult.OK)
            {
                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Имя", 240);
                listView1.Columns.Add("Тип", 50);
                listView1.Columns.Add("Дата создания", 125);
                listView1.Columns.Add("Размер", 80);
                FileInfo FN = new FileInfo(OFD_1.FileName);
                tsLB3.Text = FN.DirectoryName;
                foreach (string Flist in OFD_1.FileNames)
                {
                    FileInfo FI = new FileInfo(Flist);
                    ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(FI.Name));
                    LVI.SubItems.Add(FI.Extension.ToUpper());
                    LVI.SubItems.Add(FI.CreationTime.ToShortDateString() + " " + FI.CreationTime.ToShortTimeString());
                    LVI.SubItems.Add(FI.Length.ToString() + " байт");
                }
                tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();
            }
        }

        //Загрузка данных в ListView 364-П
        private void lvLoad364(string path_name, string file_ex)
        {            
            listView1.Clear();
            tsLB3.Text = path_name.ToString();
            listView1.View = View.Details;
            listView1.Columns.Add("Имя", 312);
            listView1.Columns.Add("Тип", 37);
            listView1.Columns.Add("Дата создания", 95);
            listView1.Columns.Add("Размер", 65);
            string[] FileList = Directory.GetFiles(path_name, file_ex);
                                    
            foreach (string fi in FileList)
            {
                FileInfo fiz = new FileInfo(fi);
                ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(fiz.Name));
                LVI.SubItems.Add(fiz.Extension.ToUpper());
                LVI.SubItems.Add(fiz.CreationTime.ToShortDateString() + " " + fiz.CreationTime.ToShortTimeString());
                LVI.SubItems.Add(fiz.Length.ToString() + " байт");
            }
            tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();

        }

        //Загрузка данных в ListView 365-П
        private void lvLoad365(string path_name, string file_ex)
        {
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("Имя", 240);
            listView1.Columns.Add("Тип", 50);
            listView1.Columns.Add("Дата создания", 125);
            listView1.Columns.Add("Размер", 80);
            string[] FileList = Directory.GetFiles(path_name, file_ex);            
            tsLB3.Text = path_name.ToString();
                        
            //IEnumerable<string> files = from file in Directory.EnumerateFiles(path_name, file_ex, System.IO.SearchOption.TopDirectoryOnly) select file;            
                foreach (string fi in FileList)
            {
               FileInfo fiz = new FileInfo(fi);                                
               ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(fiz.Name));
               LVI.SubItems.Add(fiz.Extension.ToUpper());
               LVI.SubItems.Add(fiz.CreationTime.ToShortDateString() + " " + fiz.CreationTime.ToShortTimeString());
               LVI.SubItems.Add(fiz.Length.ToString() + " байт");               
            }               
               tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();
                
         }

        //Загрузка данных в ListView других файлов
        private void lvloadOther(string path_name)
        {
            OFD.Title = "Выбери файл(ы) для обработки";
            OFD.InitialDirectory = path_name;
            OFD.Filter = "all files (*.*)|*.*";
            OFD.Multiselect = true;
            OFD.FilterIndex = 2;
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                listView1.Clear();
                listView1.View = View.Details;
                listView1.Columns.Add("Имя", 240);
                listView1.Columns.Add("Тип", 50);
                listView1.Columns.Add("Дата создания", 125);
                listView1.Columns.Add("Размер", 80);
                FileInfo FN = new FileInfo(OFD.FileName);                
                tsLB3.Text = FN.DirectoryName;
                foreach (string Flist in OFD.FileNames)
                {
                    FileInfo FI = new FileInfo(Flist);
                    ListViewItem LVI = listView1.Items.Add(Path.GetFileNameWithoutExtension(FI.Name));
                    LVI.SubItems.Add(FI.Extension.ToUpper());
                    LVI.SubItems.Add(FI.CreationTime.ToShortDateString() + " " + FI.CreationTime.ToShortTimeString());
                    LVI.SubItems.Add(FI.Length.ToString() + " байт");
                }
                tsLB1.Text = "Всего файлов: " + listView1.Items.Count.ToString();
            }
        }

        //Проверка и открытие нового дня
        private void Open_update()
        {
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");           
                reg.SetValue("numUPD_311_TR1", "0001");
                reg.SetValue("numUPD_311_TR2", "0001");
                reg.SetValue("numUPD_365", "0001");
                myNumUpD.Text = "0001";
                dTPick.Value = DateTime.Now;
                txtLog.Clear();
        }

        //Запуск записи в АБС
        private void InsertABS()
        {
            try
            {
                Process _insABS = new Process();
                ProcessStartInfo start_insABS = new ProcessStartInfo();
                start_insABS.FileName = txt_insABS.Text;
                FileInfo runBat = new FileInfo(txt_insABS.Text);
                start_insABS.WorkingDirectory = runBat.DirectoryName;
                start_insABS.UseShellExecute = true;
                start_insABS.RedirectStandardError = false;
                start_insABS.CreateNoWindow = true;
                start_insABS.WindowStyle = ProcessWindowStyle.Normal;
                _insABS.StartInfo = start_insABS;
                _insABS.Start();
            }
            catch { }
            
        }

        //Архивирование файлов
        private void PackFilesARJ(string work_dir, string key, string filename, string patchpack, string file_ex)
        {                        
            try
            {
                Process _process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = work_dir;
                startInfo.Arguments = "" + key + " " + filename + " " + patchpack + " " + file_ex + "";
                startInfo.FileName = txtArj32.Text;
                startInfo.UseShellExecute = true;
                //startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = false;
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                _process.StartInfo = startInfo;
                _process.Start();
                _process.WaitForExit();                
            }
            catch (Exception ex)
            {
                WriteLog("====???======================================", "pack_err");
                WriteLog("Не удалось создать архив " + ex.Message, "pack_err");
                WriteLog("====???======================================", "pack_err");
                return;
            }
            if (!File.Exists(work_dir + "\\" + filename))
            {
                WriteLog("====???======================================", "pack_err");
                WriteLog("Архив не создан " + filename, "pack_err");
                WriteLog("====???======================================", "pack_err");
                return;
            }
            else
            {
                WriteLog("====|||======================================", "pack_create");
                WriteLog("Создан архив " + filename, "pack_create");
                WriteLog("====|||======================================", "pack_create");
            }                        
        }

        //Копирование файлов
        private void CopyFiles(string[] file_in, string[] file_out)
        {
            for (int i = 0; i < file_in.Length; i++)
            {
                try
                {
                    File.Copy(file_in[i], file_out[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Невозможно скопировать файлы \n" + e.Message);
                }
            }
        }

        //Переименование файлов
        private void RnmFiles(string[] file_in, string[] file_out)
        {
            for (int i = 0; i < file_in.Length; i++)
            {
                try
                {
                    File.Move(file_in[i], file_out[i]+".tmp");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Невозможно переименовать файлы \n"+e.Message);
                }
            }
        }

        //Копирование файлов
        private void lViewFCopy(string path_file)
        {
            List<string> add = new List<string>(); //Объявляем List в который будем добавлять значения            
            foreach (ListViewItem item in listView1.Items) // Перебираем все items
            {
                add.Add(item.SubItems[0].Text + item.SubItems[1].Text);

                //if (item.Selected) //Проверяем выделен ли он
                //{
                //    add.Add(item.SubItems[3].Text); // Добавляем в List<>
                //}
            }
            WriteLog("===Копирование файлов ====================", "file_copy");
            foreach (string listC in add)
            {
                if (!File.Exists(path_file + "\\" + listC))
                {
                    File.Copy(tsLB3.Text + "\\" + listC, path_file + "\\" + listC);
                    WriteLog(listC + " скопирован в архив.", "file_copy");
                }
                else
                {
                    string ddate = DateTime.Now.ToString("_yyMMdd_HHmm");
                    File.Copy(tsLB3.Text + "\\" + listC, path_file + "\\" + listC + ddate);
                    WriteLog(listC + " cкопирован в архив.", "file_copy");
                }
                //File.Copy(tsLB3.Text + "\\" + listC, path_tmp + "\\" + listC);
            }
            WriteLog("==== Копирование завершено ====================", "file_copy");
            WriteLog("Всего обработанных файлов:\t" + listView1.Items.Count, "file_copy");
        }

        //Перемещение файлов
        private void lViewFMove(string path_file)
        {
            List<string> add = new List<string>(); //Объявляем List в который будем добавлять значения            
            foreach (ListViewItem item in listView1.Items) // Перебираем все items
            {
                add.Add(item.SubItems[0].Text + item.SubItems[1].Text);

                //if (item.Selected) //Проверяем выделен ли он
                //{
                //    add.Add(item.SubItems[3].Text); // Добавляем в List<>
                //}
            }
            WriteLog("==== Начало переноса файлов ====================", "file_move");
            foreach (string listC in add)
            {
                if (!File.Exists(path_file + "\\" + listC))
                {
                    File.Move(tsLB3.Text + "\\" + listC, path_file + "\\" + listC);
                    WriteLog(listC + " перенесен в архив.", "file_move");
                }
                else
                {
                    string ddate = DateTime.Now.ToString("_yyMMdd_HHmm");                    
                    File.Move(tsLB3.Text + "\\" + listC, path_file + "\\" + listC + ddate);
                    WriteLog(listC + " перенесен в архив.", "file_move");
                }                
                //File.Copy(tsLB3.Text + "\\" + listC, path_tmp + "\\" + listC);
            }
            WriteLog("==== Конец переноса файлов ====================", "file_move");
            WriteLog("Всего обработанных файлов:\t" + listView1.Items.Count, "file_move");
        }
       

        //Создание временного каталога
        private void CreateTMP()
        {
            if (Directory.Exists(TMPATH) == false)
            {
                try
                {
                    Directory.CreateDirectory(TMPATH);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Невозможно создать временный каталог" + TMPATH + "\n" + e.Message);
                    return;
                }
            }

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
            txtLog.Text +=DateTime.Now.ToString("dd.MM.yyyy  HH:mm:ss") + "\t" + text + "\r\n";
            sw.Close();            
        }

                  
        //подпись файлов
        private void SignFiles(string path, string file_ex, string log_name)
        {
            int k = cv.cInitKey(txtdiskA.Text, "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }            
            string[] Filetxt = Directory.GetFiles(path, file_ex);
            RnmFiles(Filetxt, Filetxt);
            string[] Filetmp = Directory.GetFiles(path, "*.tmp*");

            WriteLog("==== Начало подписи файлов ====================", log_name);
            try
            {
                for (int i = 0; i < Filetmp.Length; i++)
                {
                    string FileName = (new FileInfo(Filetxt[i]).Name);
                    k = cv.cSignFile(Filetmp[i], Filetxt[i], txtdiskAkey.Text);
                    File.Delete(Filetmp[i]);                    
                    if (k != 0)
                    {
                        WriteLog("Ошибка подписи " + FileName + " " + cv.GetError(k), log_name);
                    }
                    else
                    {
                        WriteLog(FileName + " подписан.", log_name);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "sign");
            }
            WriteLog("==== Конец подписи файлов ====================", log_name);
            WriteLog("Всего обработанных файлов:\t" + Filetxt.Length.ToString(), log_name);
            cv.cUnloadKeys();
        }
               
        //шифрование файлов
        private void CryptFiles(string path_file, string file_ex, string log_name)
        {
            int k = cv.cInitKey(txtdiskB.Text, "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] FileList = Directory.GetFiles(path_file, file_ex);

            WriteLog("==== Начало шифрования файлов ====================", log_name);
            try
            {
                for (int i = 0; i < FileList.Length; i++)
                {
                    string FileName = (new FileInfo(FileList[i]).Name);
                    k = cv.cEncryptFile(FileList[i], FileList[i], txtdiskB.Text, txtdiskBpubkey.Text, Convert.ToUInt16(txtdiskBkey.Text), Convert.ToUInt16(txtFS.Text), "");
                    if (k != 0)
                    {
                        WriteLog("Ошибка шифрования " + FileName + " " + cv.GetError(k), log_name);
                    }
                    else
                    {
                        WriteLog(FileName + " зашифрован.", log_name);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, log_name);
            }
            WriteLog("==== Конец шифрования файлов ====================", log_name);
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), log_name);
            cv.cUnloadKeys(); 
        }

        //снятие подписи
        private void UnsignFiles(string file_ex,string log_file)
        {            
            int k = cv.cInitKey(txtdiskA.Text, "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show(cv.GetError(k), "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] FileList = Directory.GetFiles(tsLB3.Text, file_ex);
            WriteLog("==== Начало снятия подписи файлов ====================", log_file);
            try
            {
                for (int j = 0; j < FileList.Length; j++)
                {
                    k = cv.cDelSign(FileList[j]);
                    if (k != 0)
                    {
                        WriteLog("Ошибка снятия подписи " + (new FileInfo(FileList[j]).Name) + " " + cv.GetError(k), log_file);
                        cv.cUnloadKeys();
                        MessageBox.Show("Необходимо перезапустить программу!", "Ошибка инициализации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                        //return;
                    }
                    else
                    {
                        WriteLog("Подпись " + (new FileInfo(FileList[j]).Name) + " снята.", log_file);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, log_file);
            }

            WriteLog("==== Конец снятия подписи файлов ====================", log_file);
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), log_file);
            cv.cUnloadKeys();
        }
        //расшифровка файлов
        private void DecryptFiles(string file_ex, string log_file)
        {
            cv.cUnloadKeys();
            int k = cv.cInitKey(txtdiskB.Text, "");
            if ((k != 0) && (k != 37))
            {
                MessageBox.Show("Не удалось загрузить ключи с диска " + cv.GetError(k), "Вставьте диск с ключами и повторите попытку");
                return;
            }
            string[] FileList = Directory.GetFiles(tsLB3.Text, file_ex);

            WriteLog("==== Начало расшифровки файлов ====================", log_file);
            try
            {
                for (int i = 0; i < FileList.Length; i++)
                {

                    //File.Move(FileList[i], FileList[i] + ".tmp");
                    string FileName = (new FileInfo(FileList[i]).Name);
                    //k = cv.cDecryptFile(FileList[i], dir_sign + FileName, Convert.ToUInt16(txtdiskBkey.Text), txtdiskB.Text, txtdiskBpubkey.Text);
                    k = cv.cDecryptFile(FileList[i], FileList[i], Convert.ToUInt16(txtdiskBkey.Text), txtdiskB.Text, txtdiskBpubkey.Text);
                    if (k > 0)
                    {
                        WriteLog("Ошибка расшифровки " + (new FileInfo(FileList[i]).Name) + " " + cv.GetError(k), log_file);
                        cv.cUnloadKeys();
                        MessageBox.Show("Необходимо перезапустить программу!", "Ошибка инициализации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("Файл успешно расшифрован " + FileList[i]);
                        WriteLog(FileName + " расшифрован.", log_file);
                        //File.Delete(FileList[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, "decrypt");
            }
            WriteLog("==== Конец расшифровки файлов ====================", log_file);
            WriteLog("Всего обработанных файлов:\t" + FileList.Length.ToString(), log_file);
            cv.cUnloadKeys();
        }        

        private void btnVFL_Click(object sender, EventArgs e)
        {
            switch (cmBoxType.Text)
            { 
                case "364-П":
                    lvload364p(txt_364.Text);
                    break;
                case "Other..":
                    lvloadOther(tsLB3.Text);
                    break;
            }

         
            //lvLoad311(txt311p.Text);
        }
        
        private void chkDT_CheckedChanged(object sender, EventArgs e)
        {              
            if (chkDT.Checked == true)
            {
                chkKESDT.Checked = false;
                checkPack.Enabled = checkDecrypt.Enabled = checkDecrypt.Checked = checkUnsign.Enabled = checkUnsign.Checked = true;
                checkSign.Enabled = checkSign.Checked = checkCrypt.Enabled = checkCrypt.Checked = checkPack.Checked = false;
                string dt_pik_year = dTPick.Value.ToString("yyyy");
                string dt_pik_month = dTPick.Value.ToString("MM");
                string dt_pik_day = dTPick.Value.ToString("dd");
                string xml_in_DTKA = txt_IZ_FTS.Text + @"\DT_KA\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day+"\\";
                string xml_in_DT = txt_IZ_FTS.Text + @"\DT\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day + "\\";
                //checkPack.Checked = checkDecrypt.Checked = checkUnsign.Checked = true;               
                
                try
                {
                    if (Directory.Exists(xml_in_DTKA))
                    {
                        string[] xml_DTKAFiles = Directory.GetFiles(xml_in_DTKA, "*.xml");
                        string fdName = DateTime.Now.ToString("_yyMMdd");
                        if (!Directory.Exists(xml_in_DT))
                        {
                            Directory.CreateDirectory(xml_in_DT);
                        }
                        if (!Directory.Exists(Path.Combine(xml_in_DTKA, fdName)))
                        {
                            Directory.CreateDirectory(Path.Combine(xml_in_DTKA, fdName));
                        }
                        foreach (string f in xml_DTKAFiles)
                        {
                            string fName = f.Substring(xml_in_DTKA.Length);
                            //if (!File.Exists(Path.Combine(xml_in_DT, fName)))
                                File.Copy(Path.Combine(xml_in_DTKA, fName), Path.Combine(xml_in_DT, fName), true);
                                File.Move(Path.Combine(xml_in_DTKA, fName), Path.Combine(xml_in_DTKA+fdName, fName));
                        }                        
                        lvLoad364(xml_in_DT, "*.xml");
                    }
                    else
                    {
                        listView1.Clear();
                    }
                    
                }
                    
                catch(Exception ex)
                {
                    listView1.Clear();
                    MessageBox.Show(ex.Message,"Внимание!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void chkKESDT_CheckedChanged(object sender, EventArgs e)
        {
            
            //RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");            
            //try { checkSign.Checked = reg.GetValue("sign364").ToString() == "false" ? false : true; }
            //catch { checkSign.Checked = true; }
            //try { checkPack.Checked = reg.GetValue("pack364").ToString() == "false" ? false : true; }
            //catch { checkPack.Checked = false; }
            if (chkKESDT.Checked == true)            
            {
                chkDT.Checked = false;
                checkSign.Enabled = checkSign.Checked = checkPack.Enabled = true;
                checkCrypt.Enabled = checkCrypt.Checked = checkDecrypt.Enabled = checkDecrypt.Checked = checkUnsign.Enabled = checkUnsign.Checked = checkPack.Checked = false;
                string dt_pik_year = dTPick.Value.ToString("yyyy");
                string dt_pik_month = dTPick.Value.ToString("MM");
                string dt_pik_day = dTPick.Value.ToString("dd");                
                //checkSign.Checked = checkSign.Enabled=true;                 
                //checkPack.Checked = false;                
                string xml_in_KVIT = txt_B_FTS.Text+@"\KVIT\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day + "\\";
                string xml_in_KESDT = txt_B_FTS.Text + @"\KESDT\" + dt_pik_year + "\\" + dt_pik_month + "\\" + dt_pik_day + "\\";
                try
                {
                    if (Directory.Exists(xml_in_KVIT))
                    {
                        string fdName = DateTime.Now.ToString("_yyMMdd");
                        string[] xml_KVITFiles = Directory.GetFiles(xml_in_KVIT, "*.xml");
                        if (!Directory.Exists(xml_in_KESDT))
                        {
                            Directory.CreateDirectory(xml_in_KESDT);
                        }
                        if (!Directory.Exists(Path.Combine(xml_in_KVIT, fdName)))
                        {
                            Directory.CreateDirectory(Path.Combine(xml_in_KVIT, fdName));
                        }
                        foreach (string f in xml_KVITFiles)
                        {                            
                            string fName = f.Substring(xml_in_KVIT.Length);
                            //if (!File.Exists(Path.Combine(xml_in_KESDT, fName)))
                            File.Copy(Path.Combine(xml_in_KVIT, fName), Path.Combine(xml_in_KESDT, fName), true);
                            File.Move(Path.Combine(xml_in_KVIT, fName), Path.Combine(xml_in_KVIT + fdName, fName));                     
                        }

                        lvLoad364(xml_in_KESDT, "*.xml");
                    }
                    else
                    {
                        listView1.Clear();
                    }
                }
                catch (Exception ex)
                {
                    listView1.Clear();
                    MessageBox.Show(ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkTR1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");
            string path_TR1 = dTPick.Value.ToString("ddMMyy")+"\\04\\TR1";
            if (chkTR1.Checked==true)
            {
                try { myNumUpD.Text = reg.GetValue("numUPD_311_TR1").ToString(); }
                catch { myNumUpD.Text = "0001"; }                
                chkTR2.Checked = false;
                lvLoad311(txt311p.Text + "\\"+path_TR1, "*.xml");
            }
            
        }

        private void chkTR2_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\duse\\settings");
            string path_TR2 = dTPick.Value.ToString("ddMMyy")+"\\04\\TR2";
            if (chkTR2.Checked == true)
            {
                try { myNumUpD.Text = reg.GetValue("numUPD_311_TR2").ToString(); }
                catch { myNumUpD.Text = "0001"; } 
                chkTR1.Checked = false;
                lvLoad311(txt311p.Text + "\\"+path_TR2, "*.xml");
            }            
        }

        //private void numUpDown_ValueChanged(object sender, EventArgs e)
        //{
        //    if (numUpDown.Value < 10)
        //    {
        //        txtUpDown.Text = "000" + numUpDown.Value;
        //    }
        //    else
        //    {
        //        txtUpDown.Text = "00" + numUpDown.Value;
        //    }
        //}

        private void checkSign_MouseMove(object sender, MouseEventArgs e)
        {
            checkSign.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));                       
        }

        private void checkSign_MouseLeave(object sender, EventArgs e)
        {
            checkSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkCrypt_MouseMove(object sender, MouseEventArgs e)
        {
            checkCrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkCrypt_MouseLeave(object sender, EventArgs e)
        {
            checkCrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkDecrypt_MouseMove(object sender, MouseEventArgs e)
        {
            checkDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkDecrypt_MouseLeave(object sender, EventArgs e)
        {
            checkDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkUnsign_MouseMove(object sender, MouseEventArgs e)
        {
            checkUnsign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkUnsign_MouseLeave(object sender, EventArgs e)
        {
            checkUnsign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkPack_MouseMove(object sender, MouseEventArgs e)
        {
            checkPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }

        private void checkPack_MouseLeave(object sender, EventArgs e)
        {
            checkPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }       

        private void checkPack_Click(object sender, EventArgs e)
         {
            if (cmBoxType.Text == "364-П(Декл.)")
            {
                checkUnsign.Checked = checkDecrypt.Checked = checkSign.Checked = false;
            }
            if (cmBoxType.Text == "364-П")
            {
                checkSign.Checked = checkCrypt.Checked = false;
            }
        }

        
        private void txtArj_DoubleClick(object sender, EventArgs e)
        {
            OFD.Title = "Укажи путь к arj32.exe";
            OFD.InitialDirectory = path_arj32;
            OFD.Filter = "exe files (*.exe)|*.exe";
            OFD.Multiselect = false;
            OFD.FilterIndex = 2;
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                txtArj32.Text = OFD.FileName;
            }
        }

        private void txt311p_DoubleClick(object sender, EventArgs e)
        {            
            FBD.Description = "Каталог 311-П";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt311p.Text = FBD.SelectedPath;
            }
        }

        private void txt365_in_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 365-П Входящие";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt365_in.Text = FBD.SelectedPath;
            }
        }

        private void txt365_out_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 365-П Исходящие";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt365_out.Text = FBD.SelectedPath;
            }
        }

        private void txt365_arch_in_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 365-П Архив (Входящие)";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt365_arch_in.Text = FBD.SelectedPath;
            }
        }

        private void txt365_arch_out_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 365-П Архив (Исходящие)";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt365_arch_out.Text = FBD.SelectedPath;
            }
        }

        private void txt_IZ_FTS_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 364-П Входящие";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt_IZ_FTS.Text = FBD.SelectedPath;
            }
        }

        private void txt_B_FTS_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 364-П Исходящие";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt_B_FTS.Text = FBD.SelectedPath;
            }
        }

        private void txtPTK_PSD_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог ПТК ПСД";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txtPTK_PSD.Text = FBD.SelectedPath;
            }
        }               

        private void txt_insABS_DoubleClick(object sender, EventArgs e)
        {
            OFD.Title = "Укажи путь к run.bat";
            OFD.InitialDirectory = "";
            OFD.Filter = "bat files (*.bat)|*.bat";
            OFD.Multiselect = false;
            OFD.FilterIndex = 2;
            OFD.RestoreDirectory = true;

            if (OFD.ShowDialog() == DialogResult.OK)
            {
                txt_insABS.Text = OFD.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InsertABS();
        }

        private void txt_364_DoubleClick(object sender, EventArgs e)
        {
            FBD.Description = "Каталог 364-П";
            FBD.SelectedPath = "";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                txt_364.Text = FBD.SelectedPath;
            }
        }
                
        private void Open_new_day_Click(object sender, EventArgs e)
        {
            Open_update();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            notifyIcon.Visible = false; this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void StartForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true; this.Hide();
            }
        }

        private void cmBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
                                                                                                                                                           
    }
}
