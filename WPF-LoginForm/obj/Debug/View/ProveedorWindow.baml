   M S B A M L   `   `   `  ����  WPF-LoginForm(    "WPF_LoginForm.View.ProveedorWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Gestión de Proveedores��5   	   $	��450��6(   $	��600��65    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�6.   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��2* �6   �� 5	      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgProveedores��5      $��0��6*      AutoGenerateColumns .     CanUserAddRows .     SelectionMode$ Single=�5   *   $��0,0,0,10q�5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 50 6Q   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *IdProveedor+6Q     5      $	 Nombre��6%   $
 * 6P    65   �� *Nombre+6P     5      $	 Contacto��6%   $
 * 6T    67   �� *
Contacto+6T   5      �� 5   
   $��1��6    �� Orientation$ Vertical=�6"   .+�5      �� $��Nombre del Proveedor:��6   $
��Bold"�65   �� 5      -   5         ��Name$ 	txtNombre��5      $��0,5,0,10q�6)   �� 5      $��	Contacto:��6   $
��Bold"�6)   �� 5      -   5        $ txtContacto��5      $��0,5,0,10q�6+   �� 5      $ 
Horizontal=�6   $��Right=�62   .+�5       �� -   5        $��Guardar��5       $	��100��6+   ���#27ae60
�������6e   �� 5!      -   5        $��Cancelar��5!      $	��100��6,   $��10,0,0,0q�68   6J   5"      5#      5$      5%      