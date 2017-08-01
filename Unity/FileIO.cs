using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Unity
{
    internal class FileIO<T>
    {
        public string ErrMsg {  get;  set; }

        
        public T ReadData(string FileName)
        {
            T obj = default(T);
            ErrMsg = string.Empty;
            if (File.Exists(FileName))
            {
                try
                {
                    using (Stream serializationStream = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite))
                        obj = (T)new BinaryFormatter().Deserialize(serializationStream);
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                }
            }
            else
                ErrMsg = "File not found.";
            return obj;
        }

        
        public bool DeleteFile(string FileName)
        {
            bool flag = false;
            if (File.Exists(FileName))
            {
                try
                {
                    File.Delete(FileName);
                    flag = true;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                }
            }
            else
                ErrMsg = string.Format("File {0} does not exist.", FileName);
            return flag;
        }

        
        public bool WriteData(T data, string FileName, string FilePath)
        {
            bool flag = false;
            ErrMsg = string.Empty;
            try
            {
                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            try
            {
                using (Stream serializationStream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    new BinaryFormatter().Serialize(serializationStream, data);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            return flag;
        }
    }
}
