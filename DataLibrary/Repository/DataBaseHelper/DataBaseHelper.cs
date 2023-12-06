using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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

                PropertyInfo property = typeof(T).GetProperty(columnName);

                if (property != null && value != DBNull.Value)
                {
                    // Check if the property is nullable and set its value
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                        property.SetValue(model, Convert.ChangeType(value, underlyingType), null);
                    }
                    else
                    {
                        property.SetValue(model, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }

            return model;
        }




    }
}
