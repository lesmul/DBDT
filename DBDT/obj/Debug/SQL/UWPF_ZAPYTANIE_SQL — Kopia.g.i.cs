﻿#pragma checksum "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5FBC201A983206A6303F75D0E52880DDD454B4C39B531F9016E2FE3A4F278F01"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
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


namespace DBDT.SQL {
    
    
    /// <summary>
    /// UWPF_ZAPYTANIE_SQL
    /// </summary>
    public partial class UWPF_ZAPYTANIE_SQL : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LB_HIST_ZAPYTAN_SQL;
        
        #line default
        #line hidden
        
        
        #line 6 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonSzukaj;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtWhere;
        
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
            System.Uri resourceLocater = new System.Uri("/DBDT;component/sql/uwpf_zapytanie_sql%20%e2%80%94%20kopia.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
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
            
            #line 3 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
            ((DBDT.SQL.UWPF_ZAPYTANIE_SQL)(target)).Loaded += new System.Windows.RoutedEventHandler(this.load_data);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LB_HIST_ZAPYTAN_SQL = ((System.Windows.Controls.ListBox)(target));
            
            #line 5 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
            this.LB_HIST_ZAPYTAN_SQL.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.mouse_dbl_clikck);
            
            #line default
            #line hidden
            return;
            case 3:
            this.buttonSzukaj = ((System.Windows.Controls.Button)(target));
            
            #line 6 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
            this.buttonSzukaj.Click += new System.Windows.RoutedEventHandler(this.buttonSzukaj_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtWhere = ((System.Windows.Controls.TextBox)(target));
            
            #line 7 "..\..\..\SQL\UWPF_ZAPYTANIE_SQL — Kopia.xaml"
            this.txtWhere.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

