// Updated by XamlIntelliSenseFileGenerator 6/30/2022 1:28:35 PM
#pragma checksum "..\..\..\Views\ParameterSearch.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0B5184FADD587D70C028A0EA60F5F14146DDC435AEE40C811602166026A48D75"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PQM.Views;
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


namespace PQM.Views
{


    /// <summary>
    /// ParameterSearch
    /// </summary>
    public partial class ParameterSearch : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PQM;component/views/parametersearch.xaml", System.UriKind.Relative);

#line 1 "..\..\..\Views\ParameterSearch.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.StackPanel colorPickerSP;
        internal Xceed.Wpf.Toolkit.ColorPicker colorPicker;
        internal System.Windows.Controls.Button setColor_btn;
        internal System.Windows.Controls.Label selectedStructureLabel;
        internal System.Windows.Controls.StackPanel xinterpSP;
        internal System.Windows.Controls.TextBox interpXtxt;
        internal System.Windows.Controls.StackPanel xinterpOutputSP;
        internal System.Windows.Controls.TextBlock interpXoutputY;
        internal System.Windows.Controls.TextBlock interpXoutputdY;
        internal System.Windows.Controls.StackPanel interpolateSP;
        internal System.Windows.Controls.TextBlock interpXoutputAUC;
        internal System.Windows.Controls.StackPanel aucInputSP;
        internal System.Windows.Controls.TextBox lowerBoundtxt;
        internal System.Windows.Controls.TextBox upperBoundtxt;
        internal System.Windows.Controls.StackPanel interpolateYSP;
        internal System.Windows.Controls.TextBox interpYtxt;
        internal System.Windows.Controls.StackPanel yinterpOutputSP;
        internal System.Windows.Controls.TextBlock interpYoutput;
    }
}
