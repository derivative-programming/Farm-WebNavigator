using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Common.Providers
{
    public  class DataFileProvider<T>
    {
        [Serializable]
        private class DataFileInfo
        {
            public int NextID = 0;
            public List<T> Data = new List<T>();
        }

        private DataFileInfo _dataFileInfo = null;
        private string _file = string.Empty;
        private string _primaryKeyPropertyName = string.Empty;
        private System.Collections.Hashtable _objByIDs = new System.Collections.Hashtable();

        public DataFileProvider(string baseFileName, string primaryKeyPropertyName)
        {
            _file = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("DataFolder", System.IO.Path.GetTempPath()) + baseFileName;
            _primaryKeyPropertyName = primaryKeyPropertyName;
            if (System.IO.File.Exists(_file))
            {
                Load();
            }
            else
            {
                this._dataFileInfo = new DataFileProvider<T>.DataFileInfo();
            }
        }


        public int Add(T data)
        {
            if (this._dataFileInfo.NextID == 0)
                this._dataFileInfo.NextID++;
            int id = this._dataFileInfo.NextID;
            object obj = (object)data;
            FS.Common.Reflection.Functions.SetPropertyValue(ref obj,
                this._primaryKeyPropertyName, id.ToString());
            this._dataFileInfo.Data.Add(data);
            this._dataFileInfo.NextID++;
            Save();
            return id;
        }

        public void Add(T data, int id)
        {  
            object obj = (object)data;
            FS.Common.Reflection.Functions.SetPropertyValue(ref obj,
                this._primaryKeyPropertyName, id.ToString());
            this._dataFileInfo.Data.Add(data); 
            Save(); 
        }

        public int GetNextPKID()
        {
            if (this._dataFileInfo.NextID == 0)
                this._dataFileInfo.NextID++;
            int result = this._dataFileInfo.NextID;
             this._dataFileInfo.NextID++;
             return result;
        }
        public bool Exists(int id)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if (( FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                    return true;
            }
            return false;
        }
        public bool Exists(string code)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], "Code")) == code)
                    return true;
            }
            return false;
        }
        public List<T> GetAll()
        {
            return this._dataFileInfo.Data;
        } 

        public int GetLastID()
        {
            return (this._dataFileInfo.NextID - 1);
        }
        public int GetCount()
        {
            return  this._dataFileInfo.Data.Count;
        }
        public void DeleteAll()
        {
            this._dataFileInfo.Data.Clear();
            Save();
        }

        public List<T> AddAll(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T data = list[i];
                if (this._dataFileInfo.NextID == 0)
                    this._dataFileInfo.NextID++;
                int id = this._dataFileInfo.NextID;
                object obj = (object)data;
                FS.Common.Reflection.Functions.SetPropertyValue(ref obj,
                    this._primaryKeyPropertyName, id.ToString());
                this._dataFileInfo.Data.Add(data);
                this._dataFileInfo.NextID++; 
            }
            Save();
            return list;
        }
        public object Get(int id)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if (( FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                    return this._dataFileInfo.Data[i];
            }
            return null;
        }
        public object Get(string code)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], "Code")) == code)
                    return this._dataFileInfo.Data[i];
            }
            return null;
        }
        public void Update(T data)
        {
            int id = System.Convert.ToInt32(FS.Common.Reflection.Functions.GetPropertyValue(
                    data, this._primaryKeyPropertyName));
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                {
                    this._dataFileInfo.Data[i] = data;
                    break;
                }
            }
            Save();
        }
        public void Delete(int id)
        {
            int val = -1;
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if (( FS.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                {
                    val = i;
                    break;
                }
            }
            if (val != -1)
            {
                this._dataFileInfo.Data.RemoveAt(val);
                Save();
            }
        }
        public void Save()
        {
            FS.Common.Serialize.Functions.SerializeBinary(this._dataFileInfo, this._file);
        }
        private void Load()
        {
            this._dataFileInfo = FS.Common.Serialize.Functions.DeSerializeBinary<DataFileInfo>(this._file);
        }
    } 
}
