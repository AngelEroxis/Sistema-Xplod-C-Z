   M S B A M L   `   `   `  ����  WPF-LoginForm%    WPF_LoginForm.View.ClientesView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ���#f5f7fb$��25q�5   #     � 	Resources  5         ModernButtonStyle �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   
BlurRadius   Color   Opacity �� 
TargetType	 �� CornerRadius
 �� Triggers P� Property P� Value��%q  &	        �� 5
   
   " ��6*   .�5      ��  � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 13(�6)   �� 5       � ��$
 12,8q�6(   �� 5       � ��$ 5,10,5,0q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       �@J�Effect 5        5      $ 0Q�6'   $ 8Q�67   $ #333333��6F   $	 0.2Q�6V   5      �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $	 6��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      
 5      P� 5       �@J�IsMouseOver . . �5   "   ��  �@J�Opacity$
 0.85Q�6<   5       P� 5!       � �� . . �5"   "   ��  �@J�Opacity$	 0.7Q�6<   5#      P� 5$       � {� .  . �5%   "   ��  �@J�Opacity$	 0.5Q�6<   5&      5(      5*      5+      5-      .��50      �  � RowDefinitions 51   
   �� 52      $
��Auto �6   �� 53      $��* �6   �� 54      $
��Auto �6   .+�58   
   �� $��👥 Gestión de Clientes��6   $��30(�59      $
��Bold"�6"   ���#2c3e50$��0,0,0,20q�5:   )   $��Segoe UI Semibold)�5;      &   System.Windows.Controls.DataGrid  5>   
   -   5          Name$ 
dgClientes��5>         AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�5@   %      HeadersVisibility$ Column=�5A      $��13(�6.      RowBackground
 �����   AlternatingRowBackground �#f0f2f7$��1��5C      $��0,0,0,15q�5D         Columns 5E      0  *System.Windows.Controls.DataGridTextColumn  5F      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6O   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *	IdCliente+6O     5G      $ Nombre��6%   $	 140 6P    65   �� *Nombre+6P     5H      $ CI��6%   $ 90 6H    61   �� *CI+6H     5I      $ Ítem��6%   $ 80 6O    63   �� *	NumItem+6O     5J      $ 	Domicilio��6%   $	 160 6V    68   �� *	Domicilio+6V     5K      $ Celular��6%   $	 100 6R    66   �� *	Celular+6R     5L      $ Empresa��6%   $	 150 6]    66   �� *EmpresaInstitucion+6]     5M      $ Garante��6%   $	 120 6R    66   �� *	Garante+6R     5N      $ Cel. Garante��6%   $	 110 6Z    6;   �� *
CelGarante+6Z     5O      $ Monto Total��6%   $	 110 5Q       5P      �� *
MontoTotal+ �� StringFormat$ 
Bs. {0:N2}��5Q        5R      $ Saldo Pendiente��6%   $	 120 5T       5S      �� *SaldoPendiente+$ 
Bs. {0:N2}��5T        5V      $ Estado Crédito��6%   $	 120 6_    6=   �� *EstadoCredito+6_   5X      � 5[   
   $��2��6   $
��Left=�6!   $��Center=�6<   .+�5]      �� -   5        $��➕ Agregar Cliente��5]      ���#3498db#��[  5`      �� 5b      -   5         ��Name$ btnEditarCliente��5b      $��✏️ Editar Cliente��5c      ���#2ecc71{�.  #��[  5g      �� 5i      -   5        $ btnEliminarCliente��5i      $��🗑️ Eliminar Cliente��5j      ���#e74c3c{�.  #��[  5n      �� 5p      -   5        $ btnVerDetalles��5p      $$��📋 Ver Detalles de Créditos��5q      ���#9b59b6{�.  #��[  5u      �� 5w      -   5        $ btnAsignarCredito��5w      $��💳 Asignar Crédito��5x      ���#e67e22{�.  #��[  5|      5}      5~      5      