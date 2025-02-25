﻿#pragma checksum "..\..\DocumentWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "70BC858DC2F130786F0638C075EFD8653772AFDDD3191E54F3C0232CD457509E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Web.WebView2.Wpf;
using Paper;
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


namespace Paper {
    
    
    /// <summary>
    /// DocumentWindow
    /// </summary>
    public partial class DocumentWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 112 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Web.WebView2.Wpf.WebView2 pdfViewer;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ChatMessagesPanel;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LoadingIndicator;
        
        #line default
        #line hidden
        
        
        #line 248 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MessageInput;
        
        #line default
        #line hidden
        
        
        #line 303 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FlashcardPanel;
        
        #line default
        #line hidden
        
        
        #line 310 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionText;
        
        #line default
        #line hidden
        
        
        #line 319 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock AnswerText;
        
        #line default
        #line hidden
        
        
        #line 339 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PrevButton;
        
        #line default
        #line hidden
        
        
        #line 349 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CounterText;
        
        #line default
        #line hidden
        
        
        #line 356 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NextButton;
        
        #line default
        #line hidden
        
        
        #line 372 "..\..\DocumentWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SummaryContent;
        
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
            System.Uri resourceLocater = new System.Uri("/Paper;component/documentwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DocumentWindow.xaml"
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
            
            #line 96 "..\..\DocumentWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BackButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.pdfViewer = ((Microsoft.Web.WebView2.Wpf.WebView2)(target));
            return;
            case 3:
            this.ChatMessagesPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.LoadingIndicator = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.MessageInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 255 "..\..\DocumentWindow.xaml"
            this.MessageInput.KeyDown += new System.Windows.Input.KeyEventHandler(this.MessageInput_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 262 "..\..\DocumentWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SendButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FlashcardPanel = ((System.Windows.Controls.Grid)(target));
            
            #line 305 "..\..\DocumentWindow.xaml"
            this.FlashcardPanel.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.FlashcardPanel_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.QuestionText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.AnswerText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.PrevButton = ((System.Windows.Controls.Button)(target));
            
            #line 343 "..\..\DocumentWindow.xaml"
            this.PrevButton.Click += new System.Windows.RoutedEventHandler(this.PrevButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CounterText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.NextButton = ((System.Windows.Controls.Button)(target));
            
            #line 361 "..\..\DocumentWindow.xaml"
            this.NextButton.Click += new System.Windows.RoutedEventHandler(this.NextButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.SummaryContent = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

