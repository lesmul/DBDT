﻿#pragma checksum "..\..\..\DXF\UC_XMLXSLT.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "129191CDF5690296966F53B727BBB1F320E45685AAD29A8CB9D782C724CCC973"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using DBDT.DXF;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
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


namespace DBDT.DXF {
    
    
    /// <summary>
    /// UC_XMLXSLT
    /// </summary>
    public partial class UC_XMLXSLT : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xmlFilePathTextBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_numerSerii;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xsltFilePathTextBox;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar PROGRESX;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl TC_DANE;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xmlTextBox;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ICSharpCode.AvalonEdit.TextEditor xmlEditor;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WebBrowser webBrowser;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\DXF\UC_XMLXSLT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ParaMetry;
        
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
            System.Uri resourceLocater = new System.Uri("/DBDT;component/dxf/uc_xmlxslt.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DXF\UC_XMLXSLT.xaml"
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
            
            #line 23 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenXmlFile_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.xmlFilePathTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txt_numerSerii = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            
            #line 28 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveXmlFile_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 38 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenXsltFile_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.xsltFilePathTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 49 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.XsltFile20_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 50 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.XsltFile10_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.PROGRESX = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 10:
            this.TC_DANE = ((System.Windows.Controls.TabControl)(target));
            
            #line 57 "..\..\..\DXF\UC_XMLXSLT.xaml"
            this.TC_DANE.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TabControl_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.xmlTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            
            #line 64 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveToFile_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.xmlEditor = ((ICSharpCode.AvalonEdit.TextEditor)(target));
            return;
            case 14:
            this.webBrowser = ((System.Windows.Controls.WebBrowser)(target));
            return;
            case 15:
            this.ParaMetry = ((System.Windows.Controls.Label)(target));
            return;
            case 16:
            
            #line 95 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddParameter_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 96 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LoadXSLTParameter_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 97 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ExampleParameter_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 98 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Example2Parameter_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 99 "..\..\..\DXF\UC_XMLXSLT.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DelParameter_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
