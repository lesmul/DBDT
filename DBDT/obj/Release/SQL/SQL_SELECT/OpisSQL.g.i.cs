﻿#pragma checksum "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2546E77373A245D02A97DD449C6F62C450A79EC96D184FE86351635D4C20A60A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using DBDT.SQL.SQL_SELECT;
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


namespace DBDT.SQL.SQL_SELECT {
    
    
    /// <summary>
    /// OpisSQL
    /// </summary>
    public partial class OpisSQL : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXT_OPIS_ZAPYTANIA_SQL;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom1;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom2;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom3;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom4;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom5;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CBpoziom6;
        
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
            System.Uri resourceLocater = new System.Uri("/DBDT;component/sql/sql_select/opissql.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
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
            this.TXT_OPIS_ZAPYTANIA_SQL = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            
            #line 34 "..\..\..\..\SQL\SQL_SELECT\OpisSQL.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CBpoziom1 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.CBpoziom2 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.CBpoziom3 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.CBpoziom4 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.CBpoziom5 = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.CBpoziom6 = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
