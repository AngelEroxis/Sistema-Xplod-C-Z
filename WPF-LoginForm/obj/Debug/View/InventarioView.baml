   M S B A M L   `   `   `  ����  WPF-LoginForm'    !WPF_LoginForm.View.InventarioView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ���#f4f6f9$��20q�5   #     � 	Resources  5         BoolToVisibilityConverter  ModernButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType��%�  &	        &	      �� 5   
   6'   �� 5	   
   " ��6*   .�5
      ��  � ��$ 5q�6'   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ 13(�6)   �� 5       � ��
 �����6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��
 ���� 6,   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5      ]� ��. .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $��0,0,0,10q�6C   .+�5       �� $!��📦 Gestión de Inventario��6   $��26(�68   $
��Bold"�6F   $��Center=�5!      ���#2c3e50$��0,0,20,0q�6H   �� 5#      -   5        $��➕ Agregar Producto��5#      ���#2980b9#��[ 5%      5&      �� 5(      -   5        $��🏬 Gestionar Almacenes��5(      ���#27ae60#��[ 5*      5+      5,      �� 5/   
   $ 
Horizontal=�6   $	��Top=�6.   $��0,0,0,10q�6C   .+�50      � $	��250��6   $��30��6   $��0,0,10,0q�6+   .+�51      �� -   5         ��Name$ 	txtBuscar��51      $��Center=�52      $	��5,2q�53      54      �� 55      $ ��🔍 Filtrar por nombre...��6   
�������$��8,0,0,0q�57      $��Center=�58       J� IsHitTestVisible .  y�5:      �� *IsBuscarVacio+ �� RelativeSource �� 	 �� AncestorType$	 UserControlL�
 �� 	Converter#
 [  5;      �� 5=      -   5        $��Buscar��5=      $��80��6&   ���#34495e#��[ 6F   5>      �� 5?      -   5        $��Limpiar filtro��5?      ���#7f8c8d#��[ 5@      6@   5A      &   System.Windows.Controls.DataGrid  5D   
   -   5          Name$ dgInventario��5D         AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    HeadersVisibility$ Column=�5H         AlternatingRowBackground �#ecf0f1   RowBackground
 �����$��14(�5K      $��0,0,0,10q�5L         Columns 5N      0  *System.Windows.Controls.DataGridTextColumn  5O      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
IdProducto+6P     5P      $ Nombre��6%   $ * 6P    65   �� *Nombre+6P     5Q      $ 	Proveedor��6%   $	 150 6]    68   �� *Proveedor.Nombre+6]     5R      $ Stock��6%   $ 80 6_    64   �� *Inventario.StockActual+6_     5S      $ Precio��6%   $	 100 5U       5T      �� *PrecioVenta+ �� StringFormat$ 
Bs. {0:N2}��5U      4  .System.Windows.Controls.DataGridTemplateColumn  5W      $ Acciones��6)   $	 120 6;      CellTemplate 5X      �� 5Y      .R�5Z      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5[   "   �� -   5        $��✏️��5[   )   $��35��66   $��28��6A   ���#f39c12#��[ 5\   )   6T   �� 5]   "   -   5        $��🗑️��5]   )   $��35��67   $��28��6B   ���#e74c3c#��[ 5^   )   6T   5_      5`      5b      5d      5f      5g      