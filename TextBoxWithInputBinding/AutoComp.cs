using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MvvmFoundation.Wpf;

namespace TextBoxWithInputBinding
{
    public class AutoComp : Control
    {
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
                }
                System.Diagnostics.Debug.WriteLine("Down Key Pressed");
            }, ()=>true);
            PressedCommand = new RelayCommand(() =>
            {
                System.Diagnostics.Debug.WriteLine("Pressed");
            }, ()=>true);
        }

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


        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    var textbox = GetTemplateChild("Part_TextBox") as TextBox;
        //    var textboxBindings = textbox?.InputBindings;

        //    var button = GetTemplateChild("Part_Button") as Button;

        //    textboxBindings[0].Command = button.Command;
        //    System.Diagnostics.Debug.WriteLine("Modified template");
        //}
    }
}
