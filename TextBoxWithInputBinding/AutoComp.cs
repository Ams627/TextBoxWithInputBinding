using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmFoundation.Wpf;

namespace TextBoxWithInputBinding
{
    public class AutoComp : Control, ICommandSource
    {
        private TextBox _theTextBox;
        private ListBox _theListBox;

        static AutoComp()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoComp), new FrameworkPropertyMetadata(typeof(AutoComp)));
        }

        private enum Modes
        {
            Typing,
            Choosing
        }

        private Modes _mode;
        
        public AutoComp()
        {
            _mode = Modes.Typing;
            Typing = "type something";

            DownCommand = new RelayCommand(() =>
            {
                if (_mode == Modes.Typing)
                {
                    _mode = Modes.Choosing;
                    ShowListBox = true;
                    _theListBox.SelectedIndex = 0;
                    var lbItem = _theListBox.SelectedItem;
                    var listBoxItem = (ListBoxItem)_theListBox
                        .ItemContainerGenerator
                        .ContainerFromItem(lbItem);

                    listBoxItem.Focus();
                }
                System.Diagnostics.Debug.WriteLine("Down Key Pressed");
            }, ()=>true);

            PressedCommand = new RelayCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("Pressed");
            }, ()=>true);
            EnterCommand = new RelayCommand(() =>
            {
                if (Command != null)
                {
                    var command = Command as RoutedCommand;

                    if (command != null)
                    {
                        command.Execute(CommandParameter, CommandTarget);
                    }
                    else
                    {
                        ((ICommand)Command).Execute(CommandParameter);
                    }
                }
            });
        }


        public RelayCommand EnterCommand
        {
            get { return (RelayCommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(RelayCommand), typeof(AutoComp), new PropertyMetadata(null));


        public RelayCommand DownCommand
        {
            get { return (RelayCommand)GetValue(DownCommandProperty); }
            set { SetValue(DownCommandProperty, value); }
        }

        public static readonly DependencyProperty DownCommandProperty =
            DependencyProperty.Register("DownCommand", typeof(RelayCommand), typeof(AutoComp), new PropertyMetadata(null));

        public RelayCommand PressedCommand
        {
            get { return (RelayCommand)GetValue(PressedCommandProperty); }
            set { SetValue(PressedCommandProperty, value); }
        }

        public static readonly DependencyProperty PressedCommandProperty =
            DependencyProperty.Register("PressedCommand", typeof(RelayCommand), typeof(AutoComp), new PropertyMetadata(null));

        public string Typing
        {
            get { return (string)GetValue(TypingProperty); }
            set { SetValue(TypingProperty, value); }
        }

        public static readonly DependencyProperty TypingProperty =
            DependencyProperty.Register("Typing", typeof(string), typeof(AutoComp), new PropertyMetadata(""));
        
        public bool ShowListBox
        {
            get { return (bool)GetValue(ShowListBoxProperty); }
            set { SetValue(ShowListBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowListBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowListBoxProperty =
            DependencyProperty.Register("ShowListBox", typeof(bool), typeof(AutoComp), new PropertyMetadata(false));

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(AutoComp), new PropertyMetadata(null));

        public string MemberPath
        {
            get { return (string)GetValue(MemberPathProperty); }
            set { SetValue(MemberPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MemberPathProperty =
            DependencyProperty.Register("MemberPath", typeof(string), typeof(AutoComp), new PropertyMetadata(""));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoComp), new PropertyMetadata(null));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoComp), new PropertyMetadata(null));

        public string SelectedItemPath
        {
            get { return (string)GetValue(SelectedItemPathProperty); }
            set { SetValue(SelectedItemPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemPathProperty =
            DependencyProperty.Register("SelectedItemPath", typeof(string), typeof(AutoComp), new PropertyMetadata(""));


        private EventHandler canExecuteChangedHandler;

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AutoComp), new PropertyMetadata((ICommand)null,
                new PropertyChangedCallback(CommandChanged)));

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoComp cs = (AutoComp)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }


        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            // If oldCommand is not null, then we need to remove the handlers.
            if (oldCommand != null)
            {
                RemoveCommand(oldCommand, newCommand);
            }
            AddCommand(oldCommand, newCommand);
        }

        // Remove an old command from the Command Property.
        private void RemoveCommand(ICommand oldCommand, ICommand newCommand)
        {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }

        // Add the command.
        private void AddCommand(ICommand oldCommand, ICommand newCommand)
        {
            EventHandler handler = new EventHandler(CanExecuteChanged);
            canExecuteChangedHandler = handler;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += canExecuteChangedHandler;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {

            if (this.Command != null)
            {
                RoutedCommand command = this.Command as RoutedCommand;

                // If a RoutedCommand.
                if (command != null)
                {
                    if (command.CanExecute(CommandParameter, CommandTarget))
                    {
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.IsEnabled = false;
                    }
                }
                // If a not RoutedCommand.
                else
                {
                    if (Command.CanExecute(CommandParameter))
                    {
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.IsEnabled = false;
                    }
                }
            }
        }


        public object CommandParameter { get; }
        public IInputElement CommandTarget { get; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _theTextBox = GetTemplateChild("Part_TextBox") as TextBox;
            _theListBox = GetTemplateChild("Part_ListBox") as ListBox;
            System.Diagnostics.Debug.WriteLine("Modified template");
        }
    }
}
