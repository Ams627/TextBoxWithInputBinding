﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TextBoxWithInputBinding
{
    public static class TextBlockEx
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached(
                "Text",
                typeof(string),
                typeof(TextBlockEx),
                new PropertyMetadata(null, TextPropertyChanged));

        public static string GetText(DependencyObject obj)
        {
            return (string) obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        private static void TextPropertyChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = obj as TextBlock;

            if (textBlock != null)
            {
                var text = (string) e.NewValue;

                textBlock.Inlines.Clear();
                // textBlock.Inlines.Add(new Run(text));
                // add Runs and Underlines as necessary here
            }
        }
    }
}
