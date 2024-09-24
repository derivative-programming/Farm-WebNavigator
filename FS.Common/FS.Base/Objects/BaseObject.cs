using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Base.Objects
{
    public class BaseObject
    {
        private bool  _IsDirty = false;
        protected bool IsLoaded = false;
        private bool _IsObjectInvalidated = false;


        protected bool IsObjectInvalidated
        {
            get
            {
                return _IsObjectInvalidated;
            }
            set
            {
                _IsObjectInvalidated = value; 
            }
        }
        protected bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                if (IsLoaded)
                {
                    _IsDirty = value;
                }
            }
        }
        public bool IsDirtyObject()
        {
            return this.IsDirty;
        }
        public void SetIsDirty()
        {
            this._IsDirty = true;
        } 
        public void SetIsLoaded()
        {
            this.IsLoaded = true;
        }
        public void Copy(object obj)
        {
            IsLoaded = false;
            var properties1 = obj.GetType().GetProperties();
            var properties2 = this.GetType().GetProperties();

            for (var i = 0; i < properties1.Length; i++)
            {
                for (var j = 0; j < properties2.Length; j++)
                {
                    var property1 = properties1[i];
                    var property2 = properties2[j];

                    if (property1.Name == property2.Name)
                    {
                        if (property1.CanRead && property2.CanWrite)
                        {
                            var val = property1.GetValue(obj, null);

                            property2.SetValue(this, val, null);
                        }
                        break;
                    }
                }
            }
            IsLoaded = true;
            this.IsDirty = ((BaseObject)obj).IsDirtyObject();
        }
          
    }
}
 
