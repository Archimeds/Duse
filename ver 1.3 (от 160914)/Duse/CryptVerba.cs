using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Duse
{
    class CryptVerba
    {
        private  const int NAME_LEN = 12;
        private const int ALIAS_LEN = 120;

        private string curKeyPath = "";


        public struct USR_KEYS_INFO
        {

            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 11)]
            public string num;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string nump;
            public UInt16 keys_status;
            public byte version_high;
            public byte version_low;
            public UInt32 KeySlotNumber;
            /*
            public USR_KEYS_INFO(int size)
            {
                num = new char[11];
                nump = new char[13];
                keys_status = 0;
                version_high = 0;
                version_low = 0;
                KeySlotNumber = 0;
                

            }*/

        }
        public struct USR_KEYS_INFO_EX
        {
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 11)]
            public char[] num;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 13)]
            public char[] nump;
            public UInt16 keys_status;
            public byte version_high;
            public byte version_low;
            public UInt32 KeySlotNumber;
            string alg;
            UInt32 Reserved;
            /*
            public USR_KEYS_INFO_EX(int size)
            {
                num = new char[11];
                nump = new char[13];
                keys_status = 0;
                version_high = 0;
                version_low = 0;
                KeySlotNumber = 0;
                alg = "";
                Reserved = 0;

            }*/
        }

        /*+ структура в которой возвращаются результаты подписи +*/
        public struct Check_Status
        {

            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 13)]
            public string Name;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 121)]
            public string Alias;
            public byte Position;
            public byte Status;
            public UInt32 Date;
           /*
            public Check_Status(int sz)
            {
                Name = new char[13];
                Alias = new char[121];
                Position = 0;
                Status = 0;
                Date = 0;

            }*/
        }



        /*+ структуры в списке ключей для функций SprList и CheckSpr +*/
        public struct Spr_List
        {
            public char[] key_id; /*+ идентификатор открытого ключа +*/
            public char key_type;  /*+ тип: рабочий, резервный или скомпрометированный +*/
            public char key_status;/*+ статус проверки открытого ключа +*/
            public Spr_List(int sz)
            {
                key_id = new char[13];
                key_type = '0';  /*+ тип: рабочий, резервный или скомпрометированный +*/
                key_status = '0';/*+ статус проверки открытого ключа +*/

            }

        }
        public struct sign_info
        {
            public char[] nump;
            public char[] reg_num;
            public sign_info(int sz)
            {
                nump = new char[13];
                reg_num = new char[8];
            }

        }

        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int GetDrvInfo( [Out,MarshalAs(UnmanagedType.LPArray)]  USR_KEYS_INFO[] keys, ref ulong count);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int ResetKeyEx(string key,ushort flag);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int FreeMemory(ref UInt16[] lpMemory);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int DelSign(string file_name, byte count);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int check_file_sign(string src_file_name, ref byte count, [Out,MarshalAs(UnmanagedType.LPArray)] Check_Status[] stat_array);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int SignFile(string src_file_name, string dst_file_name, string name);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int SignLogIn(string path);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int SignLogOut();
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int SignInit(string path, string base_path);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int SignDone();
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int InitKey(string key_dev, string key_num);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int CryptoInit(string path, string base_path);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int CryptoDone();
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int EnCryptFile(string file_in, string file_out, UInt16 node_from, [MarshalAs(UnmanagedType.LPArray)]  ushort[] node_to, string ser);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int DeCryptFile(string file_in, string file_out, UInt16 abonement);
        [DllImport("wbotho.dll", ExactSpelling = true)]
        internal static extern int TGetCryptKeyF(string file_in, string file_out, ref UInt16 abonets, ref UInt16[] user_list, string ser);

        public CryptVerba()
        {
        }


        public int cInitKey(string key_dev, string key_name)
        {
            curKeyPath = key_dev + "\\";
            return InitKey(key_dev, key_name);
        }
        //выгружаем ключ
        public int cResetKeys(string key)
        {
            curKeyPath = "";
            return ResetKeyEx(key,0);
        }
        //Выгружаем все ключи
        public int cUnloadKeys()
        {
            string[] keys_arr=new string[16];
            int count = 0;
            int err_code = cGetDrvInfo(ref keys_arr,ref count);
            if (err_code > 0)
                return err_code;
            for (int i = 0; i <count; i++)
            {
                err_code=ResetKeyEx(keys_arr[i], 1);
            }
            return 0;


        }
        //Получаем информацию о загруженных ключах
        public int cGetDrvInfo(ref string[] key_arr,ref int count)
        {
                ulong keys_count=0;
                USR_KEYS_INFO[] keys_list=new USR_KEYS_INFO[16];
               // for (int i = 0; i < keys_list.Length; i++)
                //    keys_list[i] = new USR_KEYS_INFO(1);
                

                int err_code=GetDrvInfo(keys_list,ref keys_count);

                if(err_code>0)
                    return err_code;
                count = Convert.ToInt32(keys_count);
                for(int i=0;i<Convert.ToInt32(keys_count);i++)
                {
                    key_arr[i]=keys_list[i].num.ToString();
                 }
                return 0;
                

        }
        //проверяем подпись на файле
        public int cCheckSign(string in_file)
        {
            Check_Status[] status_array = new Check_Status[16];
         
            byte count_s = 0;
            int err_code;
           //инициализация подписи
            err_code = SignInit(curKeyPath, curKeyPath);
            if(err_code>0)
            {
              return err_code;
            }
            err_code = check_file_sign(in_file, ref count_s,status_array);
            if(err_code>0)
            {
                 return err_code;
            }
            err_code=SignDone();
            if (err_code > 0)
                return err_code;
            
             return 0;
        }
        //подписать файл
        public int cSignFile(string in_file,string out_file,string s_node_from)
        {
            int err_code = SignInit(curKeyPath, curKeyPath);

             if(err_code>0)
             {
                    return err_code ;
            }
            err_code = SignFile(in_file, out_file, s_node_from);
            if(err_code>0)
            {
                    
                    return err_code;
            }
            SignDone();
            return err_code;
            

        }
        public int cSignFiles(string[] in_file, string[] out_file, string s_node_from)
        {
            int err_code = SignInit(curKeyPath, curKeyPath);

            if (err_code>0)
            {
                return err_code;
            }
            for (int i = 0; i < in_file.Length; i++)
            {
                err_code = SignFile(in_file[i],out_file[i], s_node_from);
                if (err_code > 0)
                {

                    return err_code;
                }
            }
            SignDone();
            return err_code;


        }
       
        //удалить подпись
        public int cDelSign(string in_file)
        {
            int err_code = SignInit(curKeyPath, curKeyPath);
            int count=1;
            if(err_code>0)
            {
            
                return err_code; 
            }
            err_code=DelSign(in_file,Convert.ToByte(count));
            if(err_code>0)
            {
                return err_code;
            }
            SignDone();
            return err_code;

        }
        //Зашифровываем файл
        public int cEncryptFile(string in_file,string out_file,string key_path,string cr_pub_key_path,ushort own_node,ushort nodes2,string ser )
        {
            
           int err_code = CryptoInit(key_path,cr_pub_key_path);
           ushort[] nodes=new ushort[1];
           nodes[0] = nodes2;
           if(err_code>0)
           {
                return err_code;
           }

            err_code = EnCryptFile(in_file, out_file, own_node,nodes, ser);
            if(err_code>0)
            {
                return err_code;
            }
            CryptoDone();
            return 0;
        
        }
        //Зашифровываем файлы
        public int cEncryptFiles(string[] in_file, string key_path, string cr_pub_key_path, ushort own_node, ushort nodes2, string ser)
        {

            int err_code = CryptoInit(key_path, cr_pub_key_path);
            ushort[] nodes = new ushort[1];
            nodes[0] = nodes2;
            if (err_code > 0)
            {
                return err_code;
            }
            //обрабатываем все файлы
            for (int i = 0; i < in_file.Length; i++)
            {
                err_code = EnCryptFile(in_file[i], in_file[i], own_node, nodes, ser);

                if (err_code > 0)
                {
                    return err_code;
                }
            }
            CryptoDone();
            return 0;

        }
        //Расшифровываем файл
        public int cDecryptFile(string in_file,string out_file,ushort own_node,string key_path,string cr_pub_key_path)
        {
            int err_code = CryptoInit(key_path, cr_pub_key_path);
            if(err_code>0)
            {
                //printf("CryptoInit error : %d\n",err_code);
                return err_code;
            }
            err_code =   DeCryptFile(in_file, out_file, own_node);
            if(err_code>0)
            {
                return err_code;
            }
            CryptoDone();
            return err_code;

        }
        public string GetError(int err_code)
        {
            string ret = "" ;
            switch (err_code)
            {
                case 1:
                    ret=  "Недостаточно памяти";
                    break;
                case 2:
                     ret= "Ошибка управления";
                     break;
                case 3:
                     ret= "Ошибка функции драйвера";
                     break;
                case 5:
                    ret= "Критическая ошибка";
                    break;
                case 6:
                     ret= "Ключ не найден или искажен";
                     break;
                case 7:
                     ret= "Ошибка параметров";
                     break;
                                    
                case 8:
                    ret= "Ошибка инициализации";
                    break;
                case 21:
                     ret= "Ошибка открытия файла источника";
                     break;
                case 22:
                     ret= "Ошибка открытия файла приемника";
                     break;
               case 23:
                    ret= "Ошибка записи в файл";
                    break;
                case 24:
                     ret= "Ошибка чтения файла";
                     break;
                case 25:
                     ret= "Ошибка переименования файла";
                     break;
                case 26:
                    ret= "Ошибка размера файла";
                    break;
                case 27:
                     ret= "Ошибка источника SRC";
                     break;
                case 29:
                     ret= "Файл не зашифрован";
                     break;
                case 30:
                    ret= "Файл не подписан";
                    break;
                case 31:
                     ret= "Ошибка поиска в файле";
                     break;
                case 32:
                     ret= "Ошибка закрытия файла";
                     break;
                case 33:
                    ret= "Ошибка удаления файла";
                    break;
                
                case 36:
                    ret= "Ошибка устройства";
                    break;
                case 37:
                     ret= "Ключ уже загружен";
                     break;
                case 38:
                     ret= "Нет свободных слотов";
                     break;
                case 39:
                    ret= "Ключ не установлен";
                    break;
                case 42:
                     ret= "Неизвестный формат";
                     break;
                case 100:
                     ret= "Ошибка базового справочника";
                     break;
                case 101:
                    ret= "Num не соответствует считанному из драйвера";
                    break;
                case 102:
                     ret= "Ошибка HASH";
                     break;
                case 103:
                     ret= "Ошибка открытия справочников";
                     break;
                case 104:
                    ret= "Ошибка открытия файла имитовставки";
                    break;
                case 105:
                     ret= "Ошибка чтения UZ";
                     break;
                case 106:
                     ret= "Ошибка чтения CKD ";
                     break;
                case 107:
                    ret= "Длины файлов не соответствуют друг другу (ошибка имитовставки)";
                    break;
                case 108:
                     ret= "Ошибка чтения справочников";
                     break;
                case 109:
                     ret= "Ошибка записи справочника";
                     break;
                case 110:
                    ret= "Ошибка чтения имитовставки";
                    break;
                case 111:
                     ret= "неверная имитовставка";
                     break;
                case 112:
                     ret= "Ключ скомпрометирован";
                     break;
                case 113:
                    ret= "Ошибка создания каталога";
                    break;
                case 114:
                     ret= "ошибка создания файла .IMM или .SPR";
                     break;
                case 115:
                     ret= "в заданном каталоге уже есть файл .SPR";
                     break;
                case 116:
                    ret= "Ошибка записи имитовставки";
                    break;
                case 117:
                     ret= "в справочнике нет заданного открытого ключа";
                     break;
                case 118:
                     ret= "неверная длина файла .SPR или .IMM";
                     break;                    
                case 119:
                    ret= "Ошибка открытия временного файла";
                    break;
                case 120:
                     ret= "Справочник открытых ключей пуст";
                     break;
                case 121:
                     ret= "Искажен заголовок открытого ключа";
                     break;
                case 122:
                    ret= "Ошибка поиска в справочнике ключей";
                    break;
                case 123:
                     ret= "открытй ключ не является резервным";
                     break;
                case 124:
                     ret= "искажен заголовок имитовставок";
                     break;
                case 125:
                    ret= "нет имитовставки на открытый ключ";
                    break;
                case 126:
                     ret= "нет имитовставки с указанным номером";
                     break;
                case 127:
                     ret= "Ошибка флопа";
                     break;
                case 128:
                    ret= "Справочник не найден";
                    break;
                case 129:
                     ret= "Плохой ключ";
                     break;
                case 130:
                     ret= "Ошибка буфера запаковки";
                     break;
                case 131:
                    ret= "Имитовставка на справочник выработана на ключе другой серии";
                    break;
                case 132:
                     ret= "Ошибка неверный тип ключа";
                     break;
                case 133:
                     ret= "вставлен другой носитель";
                     break;
                case 0x1000:
                    ret= "Функция не поддерживается";
                    break;
                case 0x1001:
                     ret= "Устройство не подключено или занято";
                     break;
                case 0x1002:
                     ret= "Отсутствует носитель в считывателе";
                     break;
                case 0x1003:
                    ret= "Носитель не может быть использован в качестве ключевого или структура искажена";
                    break;
                case 0x1004:
                     ret= "Носитель защищен от записи";
                     break;
                case 0x1005:
                     ret= "Введен неправильный пароль";
                     break;
                case 0x1006:
                    ret= "Отказ от выполнения операции";
                    break;
                case 0x1007:
                     ret= "Нет места на носителе";
                     break;
                case 0x1008:
                     ret= "Версия старше 5.0 не поддерживается";
                     break;               
                  
            }
            
            return ret;
        }
               
    }
}
