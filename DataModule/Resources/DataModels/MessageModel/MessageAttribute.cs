 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

[EntityLabel("Атрибут сообщения")]
public class MessageAttribute: BaseEntity
{

    [Label("Наименование")]
    [NotNullNotEmpty("Необходимо указать наименование")]
    [RusText("Используйте символы русского алфавита")]
    public string Name { get; set; }

    [Label("Системный")]
    [HelpMessage("С системными данными работают информационные ресурсы, пользователи не учавствуют в обмене")]
    public bool IsSystem { get; set; } = false;

    [Label("Иконка")]
    [InputIcon()]
    public string Icon { get; set; } = "home";


    [Label("Тип данных")]
    [NotNullNotEmpty("Обязатльно укажите тип данных")]
    [SelectCsDataType()]
    public string CsType { get; set; } = "string";


    [Label("Тип данных")]
    [NotNullNotEmpty("Обязатльно укажите тип данных")]
 
    public string SqlType { get; set; } = "nvartchar(max)";


    [Label("Тип ввода")]
    [NotNullNotEmpty("Обязатльно укажите тип ввода")]
    [SelectInputType()]
    public string InputType { get; set; } = "text";


    [Label("Краткое описание")]
    [NotNullNotEmpty("Кратко опишите атррибут")]
    public string Description { get; set; }

 

    [Label("Методы проверки")]
    [NotMapped()]
    public List<ValidationModel> Validations { get; set; }

    [Label("Тип данных SQL Server")]
    public string SqlServerDataType { get; set; }

    [Label("Тип данных MySQL")]
    public string MySqlDataType { get; set; }

    [Label("Тип данных Postgre")]
    public string PostgreDataType { get; set; }

    [Label("Тип данных Oracle")]
    public string OracleDataType { get; set; }

    public MessageAttribute()
    {
    }

    public MessageAttribute(Dictionary<string, string> attributes)
    {

    }


  
}