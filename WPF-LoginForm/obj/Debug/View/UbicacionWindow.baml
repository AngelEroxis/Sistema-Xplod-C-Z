   M S B A M L   `   `   `  ����  WPF-LoginForm(    "WPF_LoginForm.View.UbicacionWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Ubicaciones de Inventario��5   	   $	��450��6+   $	��600��68    � WindowStartupLocation$ CenterOwner=�6D   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      �� -   5        $��Agregar Ubicación��5      ���#2980b9
�������6Y   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgUbicaciones��5      $��1��6*      AutoGenerateColumns .     CanUserAddRows .     RowBackground
 �����   AlternatingRowBackground �#ecf0f1$��14(�5   L   	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 
Ubicación��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *	Ubicacion+6V     5      $
 	Productos��6%   $	 250 68    5      �� *ProductosResumen+4  .System.Windows.Controls.DataGridTemplateColumn  5      $
 Acciones��6)   $	 120 6;      CellTemplate 5      �� 5      .R�5      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5   "   �� -   5        $��✏️��5   )   $��40��66   $	��5,0q�6A   5   )   �� 5   "   -   5        $��🗑️��5   )   $��40��67   $	��5,0q�6B   5   )   5      5      5       5"      5#      5$      