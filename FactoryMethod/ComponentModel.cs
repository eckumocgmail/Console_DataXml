
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
/// <summary>
/// Функции связывания элементов
/// </summary>
public interface IComponentModel
{
    public void PreLinking();
    public void PostLinking();
    public void OnError();
    public void OnMessage();
    public void OnInit();
    public void OnChanges();
    public void OnUpdate();
    public void OnDestroy();
}
public class ComponentModel : IComponentModel
{
    public ComponentParams Parameters { get; set; }

    
    event EventHandler<ComponentEventArgs> Input = (sender, evt) => { };

    event EventHandler<ComponentEventArgs> Output = (sender, evt) => { };


    public object Target { get; set; }
    public string[] Attributes { get; set; }


    //public EventEmitter Output { get; set; }
    //public ViewItem View { get; set; }
    //public BaseEntity Model { get; set; }
    //public Controller Ctrl { get; set; }


    /// <summary>
    /// Инициаллизация компонента - связывание разметки
    /// </summary>
    public void OnInit()
    {

    }

    public void OnChanges()
    {
    }

    public void OnUpdate()
    {
    }

    public void OnDestroy()
    {
    }

    public void PreLinking()
    {
    }

    public void PostLinking()
    {
    }

    public void OnError()
    {
    }

    public void OnMessage()
    {
    }

    internal object GetTarget() => Target;

    internal void SetTarget(object targetComponent)
    {
        this.Target = targetComponent;
    }
}