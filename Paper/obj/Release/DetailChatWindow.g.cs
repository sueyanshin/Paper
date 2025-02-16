﻿#pragma checksum "..\..\DetailChatWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9CFDA6016D313AFB410FE94894B3D6ADBAB08511C100CB79A9C144F7883A6D23"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    /// DetailChatWindow
    /// </summary>
    public partial class DetailChatWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 110 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DocumentViewer PdfViewer;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ChatMessagesPanel;
        
        #line default
        #line hidden
        
        
        #line 202 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MessageInput;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FlashcardPanel;
        
        #line default
        #line hidden
        
        
        #line 257 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock QuestionText;
        
        #line default
        #line hidden
        
        
        #line 264 "..\..\DetailChatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock AnswerText;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\DetailChatWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Paper;component/detailchatwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DetailChatWindow.xaml"
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
            
            #line 93 "..\..\DetailChatWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PdfViewer = ((System.Windows.Controls.DocumentViewer)(target));
            return;
            case 3:
            this.ChatMessagesPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.MessageInput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.FlashcardPanel = ((System.Windows.Controls.Grid)(target));
            
            #line 252 "..\..\DetailChatWindow.xaml"
            this.FlashcardPanel.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.FlashcardPanel_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.QuestionText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.AnswerText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.SummaryContent = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

