   M S B A M L   `   `   `  ����  WPF-LoginForm+    %WPF_LoginForm.View.VentaDetalleWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Ventas Realizadas��5   	   $	��600��6#   $	��800��60    � WindowStartupLocation$ CenterScreen=�5   	   ���#f4f6f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� -   5         ��Name$ lblInfoVenta��5      $��20(�6*   $
��Bold"�68   ���#2c3e506J   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgDetalleVenta��5      $��1��6+      AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Producto��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6[   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Producto.Nombre+6[     5      $	 Cant.��6%   $
 80 6Q    64   �� *
Cantidad+6Q     5      $	 	Precio U.��6%   $	
 100 6[    68   �� *PrecioUnitario+6[     5      $	 Subtotal��6%   $	
 100 6T    67   �� *
Subtotal+6T   5      5      5      