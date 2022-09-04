 
using XmlCompiler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

public interface IComponentFactory
{
    public object Create(ComponentParams parameters, ComponentModel Parent, global::Report Root);
}

/// <summary>
/// Предоставляет сервисы сконфигурированные согласно окружению
/// </summary>
public class ComponentFactory: IComponentFactory
{
    /// <summary>
    /// Сериализуемые сборки
    /// </summary>
    private List<Assembly> imported = new List<Assembly>() {
        
        Assembly.GetExecutingAssembly(),
        Assembly.GetCallingAssembly(),
        Assembly.GetEntryAssembly(),
        
        typeof(System.Data.DataColumn).Assembly
    };

    public ComponentFactory()
    {
       
    }



    /// <summary>
    /// Анализ информации полученной из XML документа 
    /// для создание модели компонента представления    
    /// </summary>
    /// <param name="parameters">параметры управляющего потока</param>
    /// <param name="Parent">ссылка на поток данных</param>
    /// <returns></returns>
    public object Create(ComponentParams parameters, ComponentModel Parent, global::Report Root)
    {
        object result = null;
        try
        {                            
            string TypeName = ToCapitalStyle(parameters.Tag).Filter("#");
            
            WriteLine("Внедрение компонента\n "+new
            {
                type = TypeName,
                parameters = parameters,
                parent = Parent,
                root = Root
            }.ToJsonOnScreen());
          
             
            // по-идеи тут мы регистрируем фукнкцию доступа к данному экземпляру
            // но почему-то просто присваиваем ссылки
            if (parameters.GetNameAttribute() != null)
            {
                Console.WriteLine(parameters.GetNameAttribute());
                var parentTarget = Parent.GetTarget();
                if (parentTarget == null)
                    throw new Exception("Не проинициаллизирован родительский компонент для: "+ Parent.Attributes);
                Type parentType = parentTarget.GetType();
                string property = ToCapitalStyle(parameters.GetNameAttribute());
                PropertyInfo info = parentType.GetProperty(property);
                if( info == null)
                {
                    throw new Exception("Класс " + parentType.Name + " не обяьвляет свойство " + property);
                }
                object value = info.GetValue(Parent.GetTarget());
                if (value != null)
                {
                    return result=value;
                }
                else
                {
                    result = Create(TypeName, parameters.Attrs, Parent, Root);
                    info.SetValue(Parent.GetTarget(), result);
                    return result;
                }
            }

            if (Parent != null)
            {

                //если существует свойство с одноимённым идентификатором, то 
                //компонент является указателем на это свойство в случае
                //если свойство определено в единственном числе,
                //если свойство определено во множественном числе( т.е. как коллекция )
                //ссылка будет добавлена в коллекцию
                if (IsCollectionType(Parent.GetTarget().GetType()))
                {
                    result = Create(TypeName, parameters.Attrs, Parent, Root);
                    if (Parent.GetTarget() is DataColumnCollection)
                    {
                        DataColumnCollection col = (DataColumnCollection)Parent.GetTarget();
                        col.Add((DataColumn)result);
                        return result;
                    }
                    else
                    {
                        dynamic collection = Parent.GetTarget();
                        collection.Add(result);
                        return result;

                    }
                }
                PropertyInfo info = Parent.GetTarget().GetType().GetProperty(TypeName);
                if (info != null)
                {
                    if (IsCollectionType(Parent.GetTarget().GetType(), info.Name))
                    {
                        result = new List<object>();
                        info.SetValue(Parent.GetTarget(), result);
                        return result;

                    }
                    else
                    {
                        result = Create(TypeName, parameters.Attrs, Parent, Root);
                        if (Parent.GetTarget().GetType().Name.StartsWith("List"))
                        {
                            dynamic collection = info.GetValue(Parent.GetTarget());
                            collection.Add(result);
                            return result;
                        }
                        else
                        {

                            info.SetValue(Parent.GetTarget(), result);
                            return result;
                        }

                    }
                }
                else
                {
                    result = Create(TypeName, parameters.Attrs, Parent, Root);
                    if (Parent.GetTarget().GetType().Name.StartsWith("List"))
                    {
                        dynamic collection = Parent.GetTarget();
                        collection.Add(result);
                        return result;
                    }
                    else
                    {
                        throw new Exception("Свойство " + TypeName + " не определено в типе " + Parent.GetTarget().GetType().Name);
                    }

                }
            }
            else
            {
                result = Create(TypeName, parameters.Attrs, Parent, Root);
                
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(parameters) + " \n" + GetTags()+ex);
            /*throw new Exception("Ошибка: "+ 
                JsonConvert.SerializeObject(parameters) + " \n" + GetTags()
            );   */                    
        }
        finally
        {
            if (result == null && parameters.Tag != "#text")  
                throw new NullReferenceException($"При исп. функции тега {parameters.Tag}. Результат функции не должен вернуть ссылку на Null")  ;
        }
        return result;
    }

    private bool IsCollectionType(object p) => Typing.IsCollectionType(p.GetType());

    private object Create(string typeName, IDictionary<string, string> attrs, ComponentModel parent, Report root)
    {
        if (typeName != "text")
        {
            return typeName.ToType().New();
        }
        else
        {
            return null;
        }
    }

    private string ToCapitalStyle(string tag) => tag.ToCapitalStyle();

    private void WriteLine(object v) => LoggerFactory.Create((builder) => { builder.AddConsole(); }).CreateLogger(GetType().GetTypeName()).LogInformation(v.ToString());

    private bool IsCollectionType(Type type, string name)
    {
        var propertyType = type.GetProperty(name);
        if (Typing.IsPrimitive(propertyType.PropertyType) == true)
            return false;
        var propertyInterfaces = propertyType.PropertyType.GetInterfaces();
        var interfacesNames = propertyInterfaces.Select(i => i.Name);
        interfacesNames.ToJsonOnScreen().WriteToConsole();
        return interfacesNames.ToHashSet().Contains(nameof(IEnumerable));
    }

    
    /// <summary>
    /// Определяем признак числено
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsCollectionType(Type type)
    {
        Type p = type;
        while (p != typeof(Object) && p != null)
        {
            if ((from pinterface in new List<Type>(p.GetInterfaces()) where pinterface.Name.StartsWith("ICollection") select p).Count() > 0)
            {
                return true;
            }
            p = p.BaseType;
        }
        return false;
    }

    /// <summary>
    /// Получение значения из свойства с указанным именем
    /// </summary>
    /// <param name="ключ"></param>
    /// <param name="значение"></param>
    /// <returns></returns>
    public object GetValue(object ключ, string значение)
    {
        PropertyInfo propertyInfo = ключ.GetType().GetProperty(значение);
        FieldInfo fieldInfo = ключ.GetType().GetField(значение);
        return
            fieldInfo != null ? fieldInfo.GetValue(ключ) :
            propertyInfo != null ? propertyInfo.GetValue(ключ) :
            null;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="PropertyName"></param>
    /// <param name="Attrs"></param>
    /// <param name="Parent"></param>
    /// <returns></returns>
    private object Create(string PropertyName, Dictionary<string, string> Attrs, ComponentModel Parent, global::Report Root)
    {
        
        Type type = FindTypeForShortName(PropertyName);
        if (type == null)
        {
            throw new Exception("Не найден класс с именем " + PropertyName);
        }
        else
        {            
            object Target = CreateWithDefaultConstructor<object>(type);
            Target = Apply(Target, Attrs, Parent, Root);
            return Target;
        }
        throw new Exception("Не найден класс с именем " + PropertyName);
    }


    /// <summary>
    /// Получаем новый экземпляр исп. конструктор по умолчанию
    /// </summary>
    private T CreateWithDefaultConstructor<T>(Type type)
    {
        var constructor = type.GetConstructors().FirstOrDefault();
        if (constructor==null)
            throw new Exception("Требуется реализовать конструктор типа "+type.Name);
        //constructor.GetParameters()
        return (T)constructor.Invoke(new object[0]);
    }


    /// <summary>
    /// Поиск типа в сборках звестных приложению
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    private Type FindTypeForShortName(string typeName)
    {
        foreach(var mod in imported)
        {
            Type t = (from p in mod.GetTypes() where p.Name == typeName select p).SingleOrDefault();         
            if (t != null)
            {
                return t;
            }
        }
        throw new Exception($"Не найден тип "+ typeName);   
    }


    /// <summary>
    /// Ключи для компиляции
    /// </summary>
    private string[] GetTags( )
    {

        var tags = new List<string>();
        foreach (var mod in imported)
        {
            WriteLine(((from p in mod.GetTypes() where IsEng(p.Name) select ToKebabStyle(p.Name)).ToList()));
           //tags.AddRange().ToList()); ;
        }
        return tags.ToArray();
    }

    private string ToKebabStyle(string name)
    {
        return name;
    }

    private bool IsEng(string name)
    {
        return true;
    }


    /// <summary>
    /// Создание предварительных параметров связывания
    /// </summary>
    /// <param name="target"></param>
    /// <param name="attrs"></param>
    /// <param name="parent"></param>
    private static object Apply(object target, Dictionary<string, string> attrs, ComponentModel parent, Report Root)
    {
        if(attrs != null)
        {

            Console.WriteLine(target);
            /*if (target is ViewItem)
            {
                ((ViewItem)target).Bindings = Collections.Expect(attrs, "name", "id", "Name", "Id");
                //((ViewItem)target).Init();
                ((ViewItem)target).Compile();
            }*/









            
            /*
            if (target.GetType().Name == nameof(AgregatedDataSet))
            {
                target.GetType().GetProperty("TableName").SetValue(target, attrs["TableName"]);
            }
            
            if (target.GetType().Name == "OdbcDataSource")
            {
                OdbcDataSource ds = ((OdbcDataSource)target);
                ds.Alias = attrs["Alias"];
                ds.connectionString = attrs["ConnectionString"];

            }
            if (target.GetType().Name == "DataTable")
            {
                string TableName = attrs["TableName"];
                string DataSource = attrs["DataSource"];
                object pds = (from p in Root.DataSources where ((OdbcDataSource)p).Alias == DataSource select p).SingleOrDefault();
                OdbcDataSource ds = ((OdbcDataSource)pds);
                System.Data.DataTable table = ds.CreateDataTable("select * from "+ TableName);
                table.TableName = TableName;
                return table;
            }
            if ( target.GetType().Name=="DataList" )
            {
                int x = 0;
                string dataset = attrs["Dataset"];
                string bind = attrs["Bind"];
                object table = (from p in Root.DataSets where ((DataTable)p).TableName == dataset select p).SingleOrDefault();
                if(table is AgregatedDataSet)
                {
                    ((DataList)target).Dataset = ((AgregatedDataSet)table).GetData();
                    ((DataList)target).Bindings = JsonConvert.DeserializeObject<Dictionary<string, string>>(bind);
                }
                else
                {
                    System.Data.DataTable dataTable = ((DataTable)table);
                    JArray jarray = convert(dataTable);
                    ((DataList)target).Dataset = jarray;
                    ((DataList)target).Bindings = JsonConvert.DeserializeObject<Dictionary<string, string>>(bind);
                }
            }*/
        }


        
        return target;
    }



    /// <summary>
    /// Вытягиваем данные из 
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public JArray ReadData(System.Data.DataTable dataTable)
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
}

