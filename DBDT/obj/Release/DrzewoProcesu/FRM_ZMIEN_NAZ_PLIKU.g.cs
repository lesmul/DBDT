﻿#pragma checksum "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AD0FE4B5D0B3B48E2031A188B1B9BC146807B6F17438C4C2E4CB9D3D14A6C8AC"
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
    /// FRM_ZMIEN_NAZ_PLIKU
    /// </summary>
    public partial class FRM_ZMIEN_NAZ_PLIKU : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXT_NOWA_NAZWA;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXT_ORYGINALNA_NAZWA;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button B_ZMIEN;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button B_KOPIUJ;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button B_ANULUJ;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TXT_INFO;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button del_file;
        
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
            System.Uri resourceLocater = new System.Uri("/DBDT;component/drzewoprocesu/frm_zmien_naz_pliku.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
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
            this.TXT_NOWA_NAZWA = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.TXT_ORYGINALNA_NAZWA = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.B_ZMIEN = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
            this.B_ZMIEN.Click += new System.Windows.RoutedEventHandler(this.b_zmien);
            
            #line default
            #line hidden
            return;
            case 4:
            this.B_KOPIUJ = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
            this.B_KOPIUJ.Click += new System.Windows.RoutedEventHandler(this.b_kopiuj);
            
            #line default
            #line hidden
            return;
            case 5:
            this.B_ANULUJ = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
            this.B_ANULUJ.Click += new System.Windows.RoutedEventHandler(this.b_anuluj);
            
            #line default
            #line hidden
            return;
            case 6:
            this.TXT_INFO = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.del_file = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\DrzewoProcesu\FRM_ZMIEN_NAZ_PLIKU.xaml"
            this.del_file.Click += new System.Windows.RoutedEventHandler(this.Del_file_click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

