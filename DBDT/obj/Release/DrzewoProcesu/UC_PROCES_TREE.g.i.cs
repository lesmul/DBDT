﻿#pragma checksum "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "82C5C7E3EA69F02615080857B2112A6EEDC0E69BDE4DC11C4DE90A6D80029FB1"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using DBDT.DrzewoProcesu;
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


namespace DBDT.DrzewoProcesu {
    
    
    /// <summary>
    /// UC_PROCES_TREE
    /// </summary>
    public partial class UC_PROCES_TREE : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView FolderView;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MINI_PODGLAD;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button B_ODSWIEZ;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CB_FIND;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button b_filtr;
        
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
            System.Uri resourceLocater = new System.Uri("/DBDT;component/drzewoprocesu/uc_proces_tree.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
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
            
            #line 8 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            ((DBDT.DrzewoProcesu.UC_PROCES_TREE)(target)).SizeChanged += new System.Windows.SizeChangedEventHandler(this.Size_Changen);
            
            #line default
            #line hidden
            return;
            case 2:
            this.FolderView = ((System.Windows.Controls.TreeView)(target));
            
            #line 20 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            this.FolderView.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.TreeViewItem_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            this.FolderView.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.PrevKeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 23 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_OnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 25 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Rename_File_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MINI_PODGLAD = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.B_ODSWIEZ = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            this.B_ODSWIEZ.Click += new System.Windows.RoutedEventHandler(this.click_refresh);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CB_FIND = ((System.Windows.Controls.ComboBox)(target));
            
            #line 57 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            this.CB_FIND.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmb_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.b_filtr = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\DrzewoProcesu\UC_PROCES_TREE.xaml"
            this.b_filtr.Click += new System.Windows.RoutedEventHandler(this.zastosuj_filtr);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

