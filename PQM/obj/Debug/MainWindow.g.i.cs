// Updated by XamlIntelliSenseFileGenerator 6/30/2022 1:28:12 PM
#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CF06A38C122B4A046B0D6986FA0C276D1A03EE853DF0E34D219D2F5A0164C318"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PQM.ViewModels;
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Converters;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Mag.Converters;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace PQM
{


    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {


#line 26 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid mainGrid;

#line default
#line hidden


#line 44 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label graphTitleLabel;

#line default
#line hidden


#line 50 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider xPosSlider;

#line default
#line hidden


#line 57 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CDbtn;

#line default
#line hidden


#line 59 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Efieldbtn;

#line default
#line hidden


#line 61 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SARbtn;

#line default
#line hidden


#line 64 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel selectSP;

#line default
#line hidden


#line 67 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button selectFilesBtn;

#line default
#line hidden


#line 71 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button selectFolderBtn;

#line default
#line hidden


#line 76 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel selectXrangeSP;

#line default
#line hidden


#line 80 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox setXMinTxt;

#line default
#line hidden


#line 84 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox setXMaxTxt;

#line default
#line hidden


#line 88 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button applyXrangeBtn;

#line default
#line hidden


#line 92 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox showRaw;

#line default
#line hidden


#line 97 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button exportBtn;

#line default
#line hidden


#line 106 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button selectbtn;

#line default
#line hidden


#line 111 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deselectbtn;

#line default
#line hidden


#line 115 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer structuresSV;

#line default
#line hidden


#line 117 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel structuresSP;

#line default
#line hidden

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
            System.Uri resourceLocater = new System.Uri("/PQM;component/mainwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\MainWindow.xaml"
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
            switch (connectionId)
            {
                case 1:
                    this.mainGrid = ((System.Windows.Controls.Grid)(target));
                    return;
                case 2:
                    this.graphTitleLabel = ((System.Windows.Controls.Label)(target));
                    return;
                case 3:
                    this.xPosSlider = ((System.Windows.Controls.Slider)(target));

#line 52 "..\..\MainWindow.xaml"
                    this.xPosSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.xPosSlider_ValueChanged);

#line default
#line hidden
                    return;
                case 4:
                    this.CDbtn = ((System.Windows.Controls.Button)(target));
                    return;
                case 5:
                    this.Efieldbtn = ((System.Windows.Controls.Button)(target));
                    return;
                case 6:
                    this.SARbtn = ((System.Windows.Controls.Button)(target));
                    return;
                case 7:
                    this.selectSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 8:
                    this.selectFilesBtn = ((System.Windows.Controls.Button)(target));

#line 70 "..\..\MainWindow.xaml"
                    this.selectFilesBtn.Click += new System.Windows.RoutedEventHandler(this.selectFilesBtn_Click);

#line default
#line hidden
                    return;
                case 9:
                    this.selectFolderBtn = ((System.Windows.Controls.Button)(target));

#line 75 "..\..\MainWindow.xaml"
                    this.selectFolderBtn.Click += new System.Windows.RoutedEventHandler(this.selectFolderBtn_Click);

#line default
#line hidden
                    return;
                case 10:
                    this.selectXrangeSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 11:
                    this.setXMinTxt = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 12:
                    this.setXMaxTxt = ((System.Windows.Controls.TextBox)(target));
                    return;
                case 13:
                    this.applyXrangeBtn = ((System.Windows.Controls.Button)(target));

#line 91 "..\..\MainWindow.xaml"
                    this.applyXrangeBtn.Click += new System.Windows.RoutedEventHandler(this.applyXrangeBtn_Click);

#line default
#line hidden
                    return;
                case 14:
                    this.showRaw = ((System.Windows.Controls.CheckBox)(target));

#line 95 "..\..\MainWindow.xaml"
                    this.showRaw.Checked += new System.Windows.RoutedEventHandler(this.showRaw_Checked);

#line default
#line hidden

#line 96 "..\..\MainWindow.xaml"
                    this.showRaw.Unchecked += new System.Windows.RoutedEventHandler(this.showRaw_Unchecked);

#line default
#line hidden
                    return;
                case 15:
                    this.exportBtn = ((System.Windows.Controls.Button)(target));

#line 99 "..\..\MainWindow.xaml"
                    this.exportBtn.Click += new System.Windows.RoutedEventHandler(this.exportBtn_Click);

#line default
#line hidden
                    return;
                case 16:
                    this.selectbtn = ((System.Windows.Controls.Button)(target));

#line 108 "..\..\MainWindow.xaml"
                    this.selectbtn.Click += new System.Windows.RoutedEventHandler(this.selectbtn_Click);

#line default
#line hidden
                    return;
                case 17:
                    this.deselectbtn = ((System.Windows.Controls.Button)(target));

#line 112 "..\..\MainWindow.xaml"
                    this.deselectbtn.Click += new System.Windows.RoutedEventHandler(this.deselectbtn_Click);

#line default
#line hidden
                    return;
                case 18:
                    this.structuresSV = ((System.Windows.Controls.ScrollViewer)(target));
                    return;
                case 19:
                    this.structuresSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 20:
                    this.colorPickerSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 21:
                    this.colorPicker = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
                    return;
                case 22:
                    this.setColor_btn = ((System.Windows.Controls.Button)(target));

#line 133 "..\..\MainWindow.xaml"
                    this.setColor_btn.Click += new System.Windows.RoutedEventHandler(this.setColor_btn_Click);

#line default
#line hidden
                    return;
                case 23:
                    this.selectedStructureLabel = ((System.Windows.Controls.Label)(target));
                    return;
                case 24:
                    this.xinterpSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 25:
                    this.interpXtxt = ((System.Windows.Controls.TextBox)(target));

#line 142 "..\..\MainWindow.xaml"
                    this.interpXtxt.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.interpXtxt_TextChanged);

#line default
#line hidden
                    return;
                case 26:
                    this.xinterpOutputSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 27:
                    this.interpXoutputY = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 28:
                    this.interpXoutputdY = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 29:
                    this.interpolateSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 30:
                    this.interpXoutputAUC = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 31:
                    this.aucInputSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 32:
                    this.lowerBoundtxt = ((System.Windows.Controls.TextBox)(target));

#line 166 "..\..\MainWindow.xaml"
                    this.lowerBoundtxt.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.auctxt_TextChanged);

#line default
#line hidden
                    return;
                case 33:
                    this.upperBoundtxt = ((System.Windows.Controls.TextBox)(target));

#line 169 "..\..\MainWindow.xaml"
                    this.upperBoundtxt.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.auctxt_TextChanged);

#line default
#line hidden
                    return;
                case 34:
                    this.interpolateYSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 35:
                    this.interpYtxt = ((System.Windows.Controls.TextBox)(target));

#line 176 "..\..\MainWindow.xaml"
                    this.interpYtxt.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.interpYtxt_TextChanged);

#line default
#line hidden
                    return;
                case 36:
                    this.yinterpOutputSP = ((System.Windows.Controls.StackPanel)(target));
                    return;
                case 37:
                    this.interpYoutput = ((System.Windows.Controls.TextBlock)(target));
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

