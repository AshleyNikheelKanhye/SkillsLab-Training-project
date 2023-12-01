using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Repository.DataBaseHelper
{
    public static class DataBaseHelper
    {

        public static T MapUserFromReaders<T>(SqlDataReader reader) where T : new()
        {
            T model = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object value = reader.GetValue(i);

                // Use reflection or a dynamic mapping approach to set properties
                // Here, assuming your model class properties have the same names as database columns
                typeof(T).GetProperty(columnName)?.SetValue(model, value);
            }

            return model;
        }




    }
}
