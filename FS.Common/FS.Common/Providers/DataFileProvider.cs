using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HM.Common.Providers
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
            _file = HM.Common.Configuration.ApplicationSetting.ReadApplicationSetting("DataFolder", System.IO.Path.GetTempPath()) + baseFileName;
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
            HM.Common.Reflection.Functions.SetPropertyValue(ref obj,
                this._primaryKeyPropertyName, id.ToString());
            this._dataFileInfo.Data.Add(data);
            this._dataFileInfo.NextID++;
            Save();
            return id;
        }
        public bool Exists(int id)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if (( HM.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                    return true;
            }
            return false;
        }
        public bool Exists(string code)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((HM.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], "Code")) == code)
                    return true;
            }
            return false;
        }
        public List<T> GetAll()
        {
            return this._dataFileInfo.Data;
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
                HM.Common.Reflection.Functions.SetPropertyValue(ref obj,
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
                if (( HM.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], this._primaryKeyPropertyName)) == id.ToString())
                    return this._dataFileInfo.Data[i];
            }
            return null;
        }
        public object Get(string code)
        {
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((HM.Common.Reflection.Functions.GetPropertyValue(
                    this._dataFileInfo.Data[i], "Code")) == code)
                    return this._dataFileInfo.Data[i];
            }
            return null;
        }
        public void Update(T data)
        {
            int id = System.Convert.ToInt32(HM.Common.Reflection.Functions.GetPropertyValue(
                    data, this._primaryKeyPropertyName));
            for (int i = 0; i < this._dataFileInfo.Data.Count; i++)
            {
                if ((HM.Common.Reflection.Functions.GetPropertyValue(
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
                if (( HM.Common.Reflection.Functions.GetPropertyValue(
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
            HM.Common.Serialize.Functions.SerializeBinary(this._dataFileInfo, this._file);
        }
        private void Load()
        {
            this._dataFileInfo = HM.Common.Serialize.Functions.DeSerializeBinary<DataFileInfo>(this._file);
        }
    } 
}
