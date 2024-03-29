﻿

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// Иерархическая структура данных
/// </summary>
/// <typeparam name="T"></typeparam>
public class XmlNode<T>
{

    public new string ToString()
    {
        return $"<{ToKebabStyle(Name)}></{ToKebabStyle(Name)}>";
    }

    private string ToKebabStyle(string name)
    {
        return name;
    }


    /// <summary>
    /// Уникальное имя обьекта в родительском контексте
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Элемент
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Дочерние элементы
    /// </summary>
    public ConcurrentDictionary<string, XmlNode<T>> HierElements { get; set; }

 
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    public XmlNode(string name, T item, XmlNode<T> parent)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }
        if (item == null)
        {
            throw new ArgumentNullException("item");
        }
        Name = name;
        Item = item;
        Parent = parent;
        HierElements = new ConcurrentDictionary<string, XmlNode<T>>();
    }

    /// <summary>
    /// Удаление дочернего элемента
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Remove(string name)
    {
        XmlNode<T> output;
        return HierElements.TryRemove(name, out output);
    }


    /// <summary>
    /// Проверка наличия потомка с заданным именем
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Has(string name)
    {
        return HierElements.ContainsKey(name);
    }


    /// <summary>
    /// Добавление потомка
    /// </summary>
    /// <param name="pchild"></param>
    /// <returns></returns>
    public XmlNode<T> Append(XmlNode<T> pchild)
    {
        if (pchild == null)
        {
            throw new ArgumentNullException("pchild");
        }
        if (Has(pchild.Name))
        {
            return HierElements[pchild.Name];//throw new Exception($"Обьект с именем {pchild.Name} уже зарегистрирован в узле: {GetPath()}");
        }
        else
        {
            return HierElements[pchild.Name] = pchild;
        }
    }


    /// <summary>
    /// Ссылка на родительский элемент
    /// </summary>
    /// 
    [JsonIgnore]
    private XmlNode<T> _Parent { get; set; }


    /// <summary>
    /// Перемещение узла
    /// </summary>
    [JsonIgnore]
    public XmlNode<T> Parent
    {
        get
        {
            return _Parent;
        }
        set
        {
            if (_Parent != null)
            {
                _Parent.Remove(Name);
            }
            if (value != null)
            {
                _Parent = value;
                _Parent.Append(this);
            }

        }
    }


    /// <summary>
    /// Получение глубины иерархии
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        int level = 1;
        XmlNode<T> p = this;
        while (p.Parent != null)
        {
            p = p.Parent;
            level++;
        }
        return level;
    }

    /// <summary>
    /// Получение пути от истока
    /// </summary>
    /// <returns></returns>
    public List<string> GetPath()
    {
        if (Parent != null)
        {
            List<string> path = Parent.GetPath();
            path.Add(Name);
            return path;
        }
        return new List<string> { Name };
    }


    /// <summary>
    /// Получение абсолюного идентификатора
    /// </summary>
    /// <param name="separator">разделитель</param>
    /// <returns></returns>
    public string GetPath(string separator)
    {
        string path = "";
        foreach (string name in GetPath())
        {
            if (path.Length != 0)
            {
                path += separator + name;
            }
            else
            {
                path = name;
            }
        }
        return path;
    }


    /// <summary>
    /// Обработка узлов поддерева вертикально вниз
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void DoBroadcastToHierElements(Action<object> handle)
    {
        handle(this);
        foreach (XmlNode<T> pchild in HierElements.Values)
        {
            pchild.DoBroadcastToHierElements(handle);
        }
    }


    /// <summary>
    /// Обработка узлов поддерева снизу вверх
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void DoBroadcastFromHierElements<TNode>(Action<TNode> handle) where TNode : XmlNode<T>
    {
        foreach (XmlNode<T> pchild in HierElements.Values)
        {
            pchild.DoBroadcastFromHierElements<TNode>(handle);
        }
        handle((TNode)this);
    }


    /// <summary>
    /// Обход всей иерархии
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void DoBroadcastToParent<TNode>(Action<TNode> handle) where TNode : XmlNode<T>
    {
        handle((TNode)this);
        if (Parent != null)
        {
            Parent.DoBroadcastToParent<TNode>(handle);
        }
    }


    /// <summary>
    /// Обработка узлов поддерева сверху вниз
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="handle"></param>
    public void DoBroadcastToBrothers<TNode>(Action<TNode> handle) where TNode : XmlNode<T>
    {
        if (Parent == null)
        {
            handle((TNode)this);
        }
        foreach (XmlNode<T> pchild in HierElements.Values)
        {
            handle((TNode)pchild);
        }
        foreach (XmlNode<T> pchild in HierElements.Values)
        {
            pchild.DoBroadcastToBrothers<TNode>(handle);
        }
    }




}
