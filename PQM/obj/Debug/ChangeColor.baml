   M S B A M L   `   `   `  ����>  :PQM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PQMPQM  5   	       PQM.ChangeColor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:PQM     5   	   X TXceed.Wpf.Toolkit, Version=4.3.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  ChangeColor��5	   	   $	��400��6   $	��300��6*    � 
ResizeMode$ NoResize=�66   .��5
      �  � ColumnDefinitions 5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��2* �6   �� 5      $
��1.5* �6   .+�5   
   �� $��Structure Name Here��6   $��0��6.   $��0��6;   $��2��5      $��Center=�6$   �� 5   
   -   5         ��Name$ 	ColorRect��5       �� RadiusX$ 5��5       �� RadiusY$ 5��6    $��1��5      $��0��6!   $��2��61   $	��100��5      $��50��6    
�������5       #  Xceed.Wpf.Toolkit.ColorPicker  5"   
   -   5          Name$ colorPicker��5"      $	��150��60   $��30��6<   $��2��5#      $��0��6(   $��2��68   $	��Top=�5$      $��0, 10, 0, 0q�63   5%      �� 5'   
   $��0��6   $��3��6%   $��2��62    �� Orientation$ 
Horizontal=�5(      $��Center=�6.   $��25��5)      .+�5*      �� $��R:��6   $��10(�6!   61   �� 5+      -   5        	 ��Name$
	 rtxt��5.      $��20��5+      $��50��6$   $��5, 0, 20, 0q�5,      ���#ECECEC5/   F   �� 50      $��G:��6   $��10(�6!   61   �� 51      -   5        $
	 gtxt��53      $��20��51      $��50��6$   $��5, 0, 20, 0q�52      ���#ECECEC54   /   �� 55      $��B:��6   $��10(�6!   61   �� 56      -   5        $
	 btxt��58      $��20��56      $��50��6$   $��5, 0, 20, 0q�57      ���#ECECEC59   /   5:      5=      5>      