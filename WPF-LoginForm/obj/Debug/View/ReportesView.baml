   M S B A M L   `   `   `  ����H  DWPF-LoginForm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WPF_LoginForm.ViewWPF_LoginForm.View  5      %    WPF_LoginForm.View.ReportesView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:WPF_LoginForm.View     5      .��5	      �� ���#f4f6f9  �� Padding$  30q�6"   ._�5
   
   ]� ��. .+�5      �� $	��Top=�6   $��0,0,0,20q�6.   .+�5      �� $��📊 Reportes del Sistema��6   $��26(�5      $
��Bold"�5      ���#2c3e50$��0,0,0,10q�5      �� 5      ���#bdc3c7$��1��61   5      �� 5      $��14(�6   $��SemiBold"�5      
����������#dcdcdc$��1q�5      $��10q�5      $��0q�5      .@�5       �� $��👥 Clientes��6   .��5!      ]� .+�5"      �� -   5        $��⬇ Exportar PDF��5"   !   $	��180��5$   !   $��35��5%   !   $��Right=�5&   !   $��0,0,0,10q�5'   !   ���#27ae60
�������$
��Bold"�5*   !   $	��Top=�5+   !   &   System.Windows.Controls.DataGrid  5-      -   5          Name$ dgClientesRpt��5-   #      AutoGenerateColumns .     
IsReadOnly . $��0q�50   #   
�������   HeadersVisibility$ Column=�52   #      GridLinesVisibility$ 
Horizontal=�53   #   54      55      �� 58      $��💰 Ventas��6   .��59      ]� .+�5:      �� -   5        $��⬇ Exportar PDF��5:   !   $	��180��5<   !   $��35��5=   !   $��Right=�5>   !   $��0,0,0,10q�5?   !   ���#2980b9
�������$
��Bold"�5B   !   $	��Top=�5C   !     5E      -   5        $ dgVentasRpt��5E   #    .   . $��0q�5H   #   
�������$ Column=�5J   #   $ 
Horizontal=�5K   #   5L      5M      �� 5P      $��📄 Créditos Pendientes��6   .��5Q      ]� .+�5R      �� -   5        $��⬇ Exportar PDF��5R   !   $	��180��5T   !   $��35��5U   !   $��Right=�5V   !   $��0,0,0,10q�5W   !   ���#8e44ad
�������$
��Bold"�5Z   !   $	��Top=�5[   !     5]      -   5        $ dgCreditosRpt��5]   #    .   . $��0q�5`   #   
�������$ Column=�5b   #   $ 
Horizontal=�5c   #   5d      5e      5f      5g      5h      5i      