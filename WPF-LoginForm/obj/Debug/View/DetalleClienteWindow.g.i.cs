﻿#pragma checksum "..\..\..\View\DetalleClienteWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AEDCDAA07D5BCAD6EB5FCA624817F7C0160915E77247314E60565ED9F3CE8ABA"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
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


namespace WPF_LoginForm.View {
    
    
    /// <summary>
    /// DetalleClienteWindow
    /// </summary>
    public partial class DetalleClienteWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 63 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtNombreCliente;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtInfoCliente;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtMontoTotal;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtSaldoPendiente;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtTotalCuotas;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtEstado;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgCreditos;
        
        #line default
        #line hidden
        
        
        #line 173 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgCuotas;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgPagos;
        
        #line default
        #line hidden
        
        
        #line 255 "..\..\..\View\DetalleClienteWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgProductos;
        
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
            System.Uri resourceLocater = new System.Uri("/WPF-LoginForm;component/view/detalleclientewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\DetalleClienteWindow.xaml"
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
            this.txtNombreCliente = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.txtInfoCliente = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            
            #line 70 "..\..\..\View\DetalleClienteWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnVolver_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtMontoTotal = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txtSaldoPendiente = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txtTotalCuotas = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.txtEstado = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.dgCreditos = ((System.Windows.Controls.DataGrid)(target));
            
            #line 150 "..\..\..\View\DetalleClienteWindow.xaml"
            this.dgCreditos.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgCreditos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.dgCuotas = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            this.dgPagos = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 11:
            
            #line 218 "..\..\..\View\DetalleClienteWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnRegistrarPago_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.dgProductos = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

