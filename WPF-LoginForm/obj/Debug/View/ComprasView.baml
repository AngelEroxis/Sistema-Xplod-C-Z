   M S B A M L   `   `   `  ����  WPF-LoginForm$    WPF_LoginForm.View.ComprasView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ���#f4f6f9$��20q�5   #   .��5      �   � ColumnDefinitions  5   
   �� 5      $��2* �6   �� 5      $
��1.2* �6   .+�5   
   �� $��0��6   .+�5      �� $!��🛒 Productos para Comprar��6   $��24(�69   $
��Bold"�6G   ���#2c3e50$��0,0,0,20q�6n   ]� 5      $��0,0,0,10q�6   .+�5      �� -   5         ��Name$ txtBuscarProducto��5      $	��200��5      $��Center=�5      $��0,0,10,0q�5       � ToolTip$ Buscar producto��5      5      �� 5      -   5        $��Gestionar Proveedores��5      $	��160��5      $��0,0,10,0q�5      ���#8e44ad
�������$
��Bold"�5      �� 5      -   5         ��Name$ cbProveedorCompra��5      $	��200��5        �� DisplayMemberPath$ Nombre��5!       �� SelectedValuePath$ IdProveedor��5"      $ Selecciona un proveedor��5#      5$      &   System.Windows.Controls.DataGrid  5&      -   5          Name$ dgProductosCompra��5&         AutoGenerateColumns .     CanUserAddRows .  	   
IsReadOnly	 . $	��300��5*      $��0,0,0,10q�5+      
   GridLinesVisibility$

 None=�5,         HeadersVisibility$ Column=�5-      
�������   Columns 5/      0  *System.Windows.Controls.DataGridTextColumn  50      ,  &System.Windows.Controls.DataGridColumn   Header$ Producto��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *Nombre+6V     51      $ Precio Compra��6)   $	 120 6a    6@   �� *PrecioCompra+6a     52      $ Stock��6)   $ 80 6c    68   �� *Inventario.StockActual+6c   54      �� 56      -   5        $��➕ Agregar al carrito��56      $	��180��58      ���#2980b9
�������$
��Bold"�5;      $��5q�5<      $��0,0,0,0q�5=      5>      �� 5A   
   $��1��6   $��30,0,0,0q�6%   .+�5B      �� $��🛍️ Carrito de Compra��6   $��24(�5C      $
��Bold"�5D      ���#2c3e50$��0,0,0,20q�5F        5H      -   5        $ dgCarritoCompra��5H       .   .  $	��300��5K      
�������$

 None=�5M      $ Column=�5N      $��0,0,0,10q�5O       5P        5Q      $ Producto��6)   $ * 6_    6;   �� *Producto.Nombre+6_     5R      $ Cant.��6)   $ 80 6U    68   �� *
Cantidad+6U     5S      $ Precio U��6)   $	 100 6^    6;   �� *PrecioUnitario+6^     5T      $ Subtotal��6)   $	 120 6X    6;   �� *
Subtotal+6X   5V      �� 5X       �� Orientation$ 
Horizontal=�6   $��0,10,0,0q�62   .+�5Y      �� $��Total:��6   $��18(�5Z      $
��Bold"�5[      $��Center=�5\      ���#34495e5]      �� 5^      -   5         ��Name$ txtTotalCompra��5^      $��18(�5_      $
��Bold"�5`      $��10,0,0,0q�5a      ���#27ae605b      5c      � 5e      $��0,20,0,0q�6    � Orientation$ 
Horizontal=�6*   $
��Left=�6C   .+�5f      �� -   5        $��✅ Realizar Compra��5f      $	��150��5h      ���#27ae60
�������$
��Bold"�5k      $��0,0,10,0q�5l      �� 5n      -	   5        $��🧹 Limpiar Carrito��5n      $	��150��5p      $
��Bold"�5q      $��0,0,10,0q�5r      �� 5t      -
   5        $��📦 Ver Compras��5t      $	��150��5v      ���#3498db
�������$
��Bold"�5y      5z      5{      5|      5}      