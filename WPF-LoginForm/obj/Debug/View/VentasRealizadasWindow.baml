   M S B A M L   `   `   `  ����  WPF-LoginForm/    )WPF_LoginForm.View.VentasRealizadasWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Ventas Realizadas��5   	   $	��600��6#   $	��800��60    � WindowStartupLocation$ CenterScreen=�5   	   ���#f4f6f9.��5      � $��20q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� $��Historial de Ventas��6   $��24(�6/   $
��Bold"�6=   ���#2c3e506O   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgVentas��5      $��1��6%      AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID Venta��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 80 6S   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� *	IdVenta+6S     5      $ Fecha��6%   $		 150 6N   
 64   �� *Fecha+6N     5      $ Método Pago��6%   $		 120 6Y   
 6:   �� *
MetodoPago+6Y     5      $ 
Total (Bs)��6%   $		 100 6S   
 69   �� *Total+6S     5      $ Vendedor��6%   $		 150 6[   
 67   �� *Vendedor.Nombre+6[   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Detalles��6)   $		 100 6;      CellTemplate 5      �� 5      .R�5      �� -   5        $	��Ver��5   %   
 � Tag 6P   �� *	IdVenta+5      5      5      5      5       