   M S B A M L   `   `   `  ����  WPF-LoginForm0    *WPF_LoginForm.View.ComprasRealizadasWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Compras Realizadas��5   	   $	��600��6$   $	��900��61    � WindowStartupLocation$ CenterScreen=�5   	   ���#f4f6f9.��5      � $��15q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Historial de Compras��6   $��24(�60   $
��Bold"�6>   ���#2c3e506P   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	dgCompras��5      $��1��6&      AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�5   %      RowBackground
 �����	   AlternatingRowBackground	 �#e7ebf0
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	ID Compra��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
IdCompra+6U     5      $ Fecha��6%   $	 150 6^    64   �� *Fecha+ �� StringFormat$ d��6^     5      $ 	Proveedor��6%   $	 200 6]    68   �� *Proveedor.Nombre+6]     5      $ 
Total (Bs)��6%   $	 120 6d    69   �� *Total+$ N2��6d   5      �� 5    
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,15,0,0q�6W   .+�5!      �� -   5        $��btnVerDetalles��5!      $��Ver Detalles��6+   $��5q�6_   $
��10,5q�6j   �� 5"      -   5        $��btnGenerarPdf��5"      $��Generar PDF��6*   $��5q�6\   $
��10,5q�6g   5#        5&   
   -   5        $ dgDetalleCompra��5&      $��3��6,   $��0,10,0,0q�69    .   .   . 
 �����	 �#e7ebf0
 5)        5*      $ Producto��6%   $	 200 6[    67   �� *Producto.Nombre+6[     5+      $ Cantidad��6%   $	 100 6T    67   �� *
Cantidad+6T     5,      $ Precio Unit.��6%   $	 120 6o    6;   �� *PrecioUnitario+$ N2��6o     5-      $ Subtotal��6%   $	 120 6e    67   �� *
SubTotal+$ N2��6e   5/      50      51      