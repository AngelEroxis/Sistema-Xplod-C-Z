   M S B A M L   `   `   `  ����  WPF-LoginForm-    'WPF_LoginForm.View.AsignarCreditoWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Asignar Crédito��5   	   $	��600��6!   $	��800��6.    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ 	CanResize=�6-   ���#f0f3f7.��5      � $��20q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $ ��Asignar Crédito a Cliente��6   $��24(�65   $
��Bold"�6C   ���#2c3e50$��0,0,0,20q�5   )   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��3* �6#   .+�5      �� $��Productos Disponibles��6   $
��Bold"�66   $��0,0,10,0q�6H   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ dgProductos��5         AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly . 	   SelectionMode$	 Single=�5   -   
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6X   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *
IdProducto+6X     5      $ Nombre��6-   $ * 6X    6=   �� *Nombre+6X     5      $ Precio��6-   $	 100 6]    6=   �� *PrecioVenta+   ElementStyle 5       �� 5!   "    �� 
TargetType" ��6(   .�5"   &   ��  �� Property � �� �� Value 6=   �� *PrecioVenta+ �� StringFormat$ 	Bs {0:N2}��5#   #   5%      4  .System.Windows.Controls.DataGridTemplateColumn  5(      $ Acción��61   $ 80 6A      CellTemplate 5)      �� 5*   "   .R�5+   &   �� -   5        $��Agregar��5+   -   $��4q�6`   5,   #   5.      50      51      �� 54      $��Carrito de Crédito��6   $
��Bold"�63   $��10,0,0,0q�6E   $��1��6W   .��55      �� .+�56        -   5        $ 	dgCarrito��56       .   .   . $��0,0,0,10q�57   1   
 58        59      $ Nombre��61   $ * 6\    6A   �� *Nombre+6\     5:      $ Precio��61   $	 100 6a    6A   �� *PrecioVenta+ 5;   "   �� 5<   &   " ��6,   .�5=   *   ��  � �� 6A   �� *PrecioVenta+$ 	Bs {0:N2}��5>   '   5@        5C      $ Eliminar��65   $ 80 6G    5D   "   �� 5E   &   .R�5F   *   �� -   5        $��Quitar��5F   1   $��4q�6b   5G   '   5I      5K      �� 5M       �� Orientation$ 
Horizontal=�6!   $��0,5,0,5q�6:   .+�5N      �� $��Total: ��6$   $
��Bold"�63   $��14(�6E   �� 5O      -   5         ��Name$ txtTotal��5O   $   $��14(�66   $��10,0,0,0q�6D   5P      �� 5R      $ 
Horizontal=�6!   $��0,5,0,5q�6:   .+�5S      �� $��Cantidad de Cuotas:��6$   $��Center=�6?   �� 5T      -   5         ��Name$ txtCantidadCuotas��5T   "   $��60��6=   $��10,0,0,0q�6H   5U      �� 5W      $ 
Horizontal=�6!   $��0,5,0,10q�6:   .+�5X      �� $��Monto por Cuota: ��6$   $��Center=�6=   �� 5Y      -   5        $ txtMontoCuota��5Y   $   $
��Bold"�6;   $��10,0,0,0q�6M   5Z      �� 5\      -   5        $��Calcular Cuotas��5\      ���#27ae60
�������$��10q�5]   E   $
��Left=�5^      $��0,0,0,10q�68   �� 5`      -	   5        $��Confirmar Crédito��5`      ���#2980b9
�������$��10q�5a   E   $
��Left=�5b      5c      5d      5e      �� 5g   
   -
   5        $��2��5g      $��Cancelar��6   ���#c0392b
�������$��10q�5h   9   $��Right=�5i      $��0,20,0,0q�6-   5j      5k      