   M S B A M L   `   `   `  ����  WPF-LoginForm+    %WPF_LoginForm.View.UserManagementView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Gestión de Usuarios��5   	   $	��600��6%   $
��1050��62   ���#F4F6F9 � WindowStartupLocation$ CenterScreen=�5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� $��👤 Gestión de Usuarios��6   $��28(�5      $
��Bold"�5      ���#2C3E50$��0,0,0,10q�5      �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��	0,10,0,20q�6;   $
��Left=�6N   .+�5      �� $��0,0,15,0q�6   .+�5      �� $��Nombre de Usuario��6   $��SemiBold"�65   �� 5      -   5         ��Name$ txtNombreUsuario��5   2   $	��150��6   $��30��6&   62   5      �� 5      $��0,0,15,0q�6   .+�5      �� $��Contraseña��6   $��SemiBold"�6.   G� 5      -   5         G�Name$ txtContrasena��5   6   $	��150��6   $��30��6*   66   5       �� 5#      $��0,0,15,0q�6   .+�5$      �� $	��Rol��6   $��SemiBold"�6'   �� 5%      -   5         ��Name$ cmbRol��5%   3   $	��150��6   $��30��6'   .@�5&      �� $��Administrador��6#   �� 5'      $��Vendedor��6#   5(      5)      �� 5,      $��0,0,15,0q�6   .+�5-      �� $��ID Vendedor��6   $��SemiBold"�6/   �� 5.      -   5        $ cmbVendedor��5.      $	��200��5/      $��30��50       �� DisplayMemberPath$ Nombre��51       �� SelectedValuePath$ 
IdVendedor��52      	 �� SelectedValue	 53      �� *
IdVendedor+54      �� 57      $ 
Horizontal=�6   $��	10,20,0,0q�62   .+�58      �� -   5        
 ��Name$
 
btnAgregar��58      $��➕ Agregar��59      $	��100��5:      $��30��5;      ���#27AE60
�������$
��Bold"�5>      $��0,0,10,0q�5?      5@      �� 5B      -   5        $
 btnGestionarVendedores��5B      $��⚙ Gestionar Vendedores��5C      $	��160��5D      $��30��5E      ���#2980B9
�������$
��Bold"�5H      5I      5J      5K      &   System.Windows.Controls.DataGrid  5N   
   -   5          Name$ 
dgUsuarios��5N      $��2��5O         AutoGenerateColumns .     CanUserAddRows .  $��0,10,0,0q�5R         AlternatingRowBackground �#ECF0F1   HeadersVisibility$ Column=�5T      $��14(�5U         RowHeaderWidth$ 0Q�5V         
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�5X         Columns 5Z      0  *System.Windows.Controls.DataGridTextColumn  5[      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6O   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *	IdUsuario+6O     5\      $ Usuario��6%   $ * 6X    66   �� *NombreUsuario+6X     5]      $	 Rol��6%   $	 100 6J    62   �� *Rol+6J     5^      $ Vendedor��6%   $	 200 6[    67   �� *Vendedor.Nombre+6[   4  .System.Windows.Controls.DataGridTemplateColumn  5`      $ Acciones��6)   $	 150 6;      CellTemplate 5a      �� 5b      .R�5c      �� $ 
Horizontal=�6)   $��Center=�6B   .+�5d   "   �� -   5        $��✏️��5d   )   $��40��5e   )   $��25��5f   )   ���#F39C12
�������$	��5,0q�5i   )   5j   )   �� 5k   "   -	   5        $��🗑️��5k   )   $��40��5l   )   $��25��5m   )   ���#E74C3C
�������$	��5,0q�5p   )   5q   )   5r      5s      5u      5x      5y      5z      