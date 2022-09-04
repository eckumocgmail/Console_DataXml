using COM;

using DataADO;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
 
namespace DataCommon
{
    public class DataTableService: MyValidatableObject,IDataTableService
    {

        public IEnumerable<string> GetColumnsNames(DataTable dataTable)
        {
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                columnNames.Add(column.ColumnName);
            }
            return columnNames.ToArray();
        }

        
        public IDictionary<string, string> GetColumnsCaptions(DataTable dataTable)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                result[column.ColumnName] = column.Caption;
            }
            return result;
        }

        public IDictionary<string, Type> GetDataTypes(DataTable dataTable)
        {
            IDictionary<string, Type> result = new Dictionary<string, Type>();
            foreach (DataColumn column in dataTable.Columns)
            {
                result[column.ColumnName] = column.DataType;
            }
            return result;
        }


        public IEnumerable<IDictionary<string, string>> GetTextData(DataTable dataTable)
        {
            var result = new List<IDictionary<string, string>>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDictionary<string, string> data = new Dictionary<string, string>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object value = row[column.ColumnName];
                    data[column.ColumnName] = value==null? "NULL": value.ToString();
                }
                result.Add(data);
            }
            return result.ToArray();
        }

        public IEnumerable<IDictionary<string, object>> GetRowsData(DataTable dataTable)
        {
            var result = new List<IDictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    object value = row[column.ColumnName];
                    data[column.ColumnName] = value;
                }
                result.Add(data);
            }
            return result.ToArray();
        }

        public IEnumerable<string> GetTextColumn(DataTable dataTable, string columnName)
        {
            List<string> result = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                object value = row[columnName];
                string text = value == null ? "NULL" : value.ToString();
                result.Add(text);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Выполняет установку значений свойств
        /// </summary>
        public class Setter
        {
            public static object FromText(object value, string propertyType)
            {
                switch (propertyType)
                {
                    case "String": { return value.ToString(); }
                    case "System.String": { return value.ToString(); }
                    case "Single": { return System.Single.Parse(value.ToString()); }
                    case "System.Single": { return System.Single.Parse(value.ToString()); }
                    case "Double": { return System.Double.Parse(value.ToString()); }
                    case "System.Double": { return System.Double.Parse(value.ToString()); }
                    case "Decimal": { return System.Decimal.Parse(value.ToString()); }
                    case "System.Decimal": { return System.Decimal.Parse(value.ToString()); }
                    case "Int16": { return System.Int16.Parse(value.ToString()); }
                    case "System.Int16": { return System.Int16.Parse(value.ToString()); }
                    case "Int32": { return System.Int32.Parse(value.ToString()); }
                    case "System.Int32": { return System.Int32.Parse(value.ToString()); }
                    case "Nullable<Int32>": { return System.Int32.Parse(value.ToString()); }
                    case "Nullable<System.Int32>": { return System.Int32.Parse(value.ToString()); }
                    case "Int64": { return System.Int64.Parse(value.ToString()); }
                    case "System.Int64": { return System.Int64.Parse(value.ToString()); }
                    case "UInt16": { return System.UInt16.Parse(value.ToString()); }
                    case "System.UInt16": { return System.UInt16.Parse(value.ToString()); }
                    case "UInt32": { return System.UInt32.Parse(value.ToString()); }
                    case "System.UInt32": { return System.UInt32.Parse(value.ToString()); }
                    case "UInt64": { return System.UInt64.Parse(value.ToString()); }
                    case "System.UInt64": { return System.UInt64.Parse(value.ToString()); }
                    default:
                        throw new Exception($"Тип  {propertyType} неподдрживается");
                }
            }
            public static void SetValue(object target, string property, object value)
            {
                var p = target.GetType().GetProperty(property);
                if (Typing.IsDateTime(p))
                {
                    DateTime? date = value.ToString().ToDate();
                    p.SetValue(target, date);
                }
                else if (Typing.IsNumber(p))
                {
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        if (Typing.IsNullable(p) && Typing.IsPrimitive(p.PropertyType) == false)
                        {
                            p.SetValue(target, null);
                        }
                        else
                        {
                            throw new Exception($"Свойство {property} не может хранить ссылку на null");
                        }
                    }
                    else
                    {
                        string propertyType = Typing.ParsePropertyType(p.PropertyType);
                        switch (propertyType)
                        {
                            case "Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                            case "System.Single": { p.SetValue(target, System.Single.Parse(value.ToString())); break; }
                            case "Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                            case "System.Double": { p.SetValue(target, System.Double.Parse(value.ToString())); break; }
                            case "Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                            case "System.Decimal": { p.SetValue(target, System.Decimal.Parse(value.ToString())); break; }
                            case "Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                            case "System.Int16": { p.SetValue(target, System.Int16.Parse(value.ToString())); break; }
                            case "Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                            case "System.Int32": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                            case "Nullable<Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                            case "Nullable<System.Int32>": { p.SetValue(target, System.Int32.Parse(value.ToString())); break; }
                            case "Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                            case "System.Int64": { p.SetValue(target, System.Int64.Parse(value.ToString())); break; }
                            case "UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                            case "System.UInt16": { p.SetValue(target, System.UInt16.Parse(value.ToString())); break; }
                            case "UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                            case "System.UInt32": { p.SetValue(target, System.UInt32.Parse(value.ToString())); break; }
                            case "UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                            case "System.UInt64": { p.SetValue(target, System.UInt64.Parse(value.ToString())); break; }
                            default:
                                throw new Exception($"Тип свойства {property} {propertyType} неподдрживается");
                        }


                    }
                    /*if (value != null && (value.GetType().Name == "Int64" || propertyTypeName == "Int32"))
                    {
                        value = Int32.Parse(value.ToString());
                    }*/
                }
                else if (Typing.IsText(p))
                {
                    p.SetValue(target, value.ToString());
                }
                else
                {

                    p.SetValue(target, value);
                }

            }
        }
        public IEnumerable<TRecord> GetResultSet<TRecord>(DataTable dataTable) where TRecord: class
        {
            Type type = typeof(TRecord);
            var properties = type.GetOwnPropertyNames();
            var result = new List<TRecord>();
            var columns = GetColumnsNames(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                object next = type.New();
                foreach(var name in properties)
                {
                    
                    string columnName = name.ToTSQLStyle();
                    string key = columns.Contains(columnName) ? columnName : columnName.ToCapitalStyle();
                    if (columns.Contains(key) == false)
                        continue;
                    try
                    {
                        object value = row[columnName];
                        if (value != null)
                        {
                            Setter.SetValue(next, name, value.ToString());
                        }
                    }
                    catch(Exception ex)
                    {
                        Error("Ошибка при разборе свойства "+ name, ex);
                        Error(ex);
                    }
                                     
                }
                
                
                
                if (next is MyValidatableObject)
                    ((MyValidatableObject)next).EnsureIsValide();
                result.Add((TRecord)next);
                
                    
                
                
            }
            return result.ToArray();
        }

        public JArray GetJArray(DataTable dataTable)
        {
            Dictionary<string, object> resultSet = new Dictionary<string, object>();
            List<Dictionary<string, object>> listRow = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> rowSet = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    rowSet[column.Caption] = row[column.Caption];
                }
                listRow.Add(rowSet);
            }
            resultSet["rows"] = listRow;

            JObject jrs = JObject.FromObject(resultSet);
            return (JArray)jrs["rows"];
        }

        public IEnumerable<dynamic> GetResultSet(DataTable dataTable, Type entity)
        {
            Type type = entity;
            var properties = type.GetOwnPropertyNames();
            var result = new List<dynamic>();
            var columns = GetColumnsNames(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                object next = type.New();
                foreach (var name in properties)
                {
                    string columnName = name.ToTSQLStyle();
                    string key = columns.Contains(columnName) ? columnName : columnName.ToCapitalStyle();
                    if (columns.Contains(key) == false)
                        continue;
                    try
                    {
                        object value = row[columnName];
                        if (value != null)
                        {
                            
                            Setter.SetValue(next, name, value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Error("Ошибка при разборе свойства " + name);
                        Error(ex);
                    }

                }



                if (next is MyValidatableObject)
                    ((MyValidatableObject)next).EnsureIsValide();
                result.Add((dynamic)next);




            }
            return result.ToArray();
        }
    }
}
