﻿#pragma checksum "..\..\ViewWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5F0BCFDE7C762A4986452DF18676284"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MassEffectPlyer {
    
    
    /// <summary>
    /// ViewWindow
    /// </summary>
    public partial class ViewWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button1;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button2;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBox1;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listView1;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image iconFile;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image iconMp3;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backButton;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\ViewWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button forwardButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MassEffectPlyer;component/viewwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ViewWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\ViewWindow.xaml"
            ((MassEffectPlyer.ViewWindow)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.customWindow_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.button1 = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\ViewWindow.xaml"
            this.button1.Click += new System.Windows.RoutedEventHandler(this.addInTrackList_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.button2 = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\ViewWindow.xaml"
            this.button2.Click += new System.Windows.RoutedEventHandler(this.close_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TextBox1 = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\ViewWindow.xaml"
            this.TextBox1.KeyDown += new System.Windows.Input.KeyEventHandler(this.textBox1_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.listView1 = ((System.Windows.Controls.ListView)(target));
            
            #line 32 "..\..\ViewWindow.xaml"
            this.listView1.PreviewMouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview_previewDoublClik);
            
            #line default
            #line hidden
            return;
            case 6:
            this.iconFile = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.iconMp3 = ((System.Windows.Controls.Image)(target));
            return;
            case 8:
            this.backButton = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\ViewWindow.xaml"
            this.backButton.Click += new System.Windows.RoutedEventHandler(this.backButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.forwardButton = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\ViewWindow.xaml"
            this.forwardButton.Click += new System.Windows.RoutedEventHandler(this.forwardButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

