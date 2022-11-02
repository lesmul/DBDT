using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace DBDT.Interaction
{

    internal static class EventCommand
    {

        public readonly static MethodInfo HandlerMethod = typeof(EventCommand).GetMethod("OnEvent", BindingFlags.NonPublic | BindingFlags.Static);

        public readonly static DependencyProperty EventProperty = DependencyProperty.RegisterAttached("Event", typeof(RoutedEvent), typeof(EventCommand), new PropertyMetadata(null, OnEventChanged));
        public readonly static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null));

        public static void SetEvent(UIElement element, RoutedEvent value)
        {
            element.SetValue(EventProperty, value);
        }

        public static RoutedEvent GetEvent(UIElement element)
        {
            return (RoutedEvent)element.GetValue(EventProperty);
        }

        public static void SetCommand(UIElement element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(UIElement element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void CommonClickHandler(UIElement element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.OldValue == null))
            {
                var evt = d.GetType().GetEvent(((RoutedEvent)e.OldValue).Name);
                evt.RemoveEventHandler(d, Delegate.CreateDelegate(evt.EventHandlerType, HandlerMethod));
            }

            if (!(e.NewValue == null))
            {
                var evt = d.GetType().GetEvent(((RoutedEvent)e.NewValue).Name);
                evt.AddEventHandler(d, Delegate.CreateDelegate(evt.EventHandlerType, HandlerMethod));
            }
        }

        private static void OnEvent(object sender, EventArgs args)
        {

            var command = GetCommand((UIElement)(DependencyObject)sender);

            if (command is null)
            {
                return;
            }

            if (command != null & command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

    }
}