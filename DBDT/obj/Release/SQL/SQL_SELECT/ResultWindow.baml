   M S B A M L   `   `   `  ����   DBDT&     DBDT.SQL.SQL_SELECT.ResultWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Wynik :)��$	��768��$
��1024�� � SizeToContent$ Width=� � WindowStartupLocation$ CenterScreen=�$	��610��$	��300��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��	10,0,10,0q�.+�� $	��180��$
��Left=�$��24��.+��� $	��190��$
��Left=�$��	0,-1,0,-2q�$��12(�$
��Bold"�.��!Sposób wyświetlania wyników:	� -   $��copy_memory��$��Kopiuj do schowka��$��185,5,-136,-5q�	� -   $��
new_window��$��Otwórz w nowym oknie SQL�� d� 	IsChecked$
 TrueY�$��309,5,-299,-2q��� -   $��Wyczyść schowek��$��484,1,-495,2q�$��Center=�$
��Left=�9� -   $��errorsExpander��$��1��$��Filtry��.���� $��1��$��5,5,5,5q�.+��  �� $
��Auto ��� $��* �.+��� -   $��CB_FILTR��$��0��$��5,5,5,5q� �� Text$  ��$��23��$��2�� �� 
IsEditable . $
��Left=�$	��200��	� -    	�Name$ ruwnum��$��0��$��	216,4,0,3q�$
 TrueY�$��Center=�$��	15,-1,0,0q�$
��Left=�.��=	� -   $
 like��$��1��$��	216,0,0,0q�$
��Left=�.��	Like %%�� -    ��Name$ 	TXT_FILTR��$��0��$��28��	 �� TextWrapping$
	 Wrap=�$	��300��
 �� 	UndoLimit$
 10�� �� MaxLines$ 1�� �� 	MaxLength$ 25�� J� RenderTransformOrigin$	 1,1&�$��Center=�$
��Left=�$��	284,2,0,3q�&   System.Windows.Controls.DataGrid  -	   $��
resultGrid��$��2��   AutoGenerateColumns .    SelectionUnit$
 Cell=�$��0,1,0,1q�   
IsReadOnly .  � ContextMenu �� .@�m� -
   $"��Kopiuj i usuń polskie znaki��m� -   $!��Kopiuj i usuń białe znaki��m� -   $��Kopiuj oryginał���� m� -   $��Kopiuj z warunkiem where���� m� -   $(��"Kopiuj z warunkiem select -> where���� m� $ ��Utwórz warunek UPDATE SET��.@�m� -   $��Kolumny UPDATE��m� -   $��Kolumny WHERE���� m� -   $&�� Kopiuj nazwy zaznaczonych kolumn���� m� $��Automatyzacja skryptów��.@�m� -   $&�� Kopiuj nazwy zaznaczonych kolumn���� m� -   $#��Kopiuj nazwy wartości --> OR���� m� -   $$��Kopiuj nazwy wartości --> AND���� -    ��Name$ LBL_INFO��$��3��$��Informacja......��$��23��$��Center=�$��	10,2,10,2q�$��Center=�$��	0,0,219,0q��� -   $ 
LBL_INFO_2��$��3��$��Ilość zaznaczonych: 0��$��23��$��Center=�$��	10,2,10,2q�$��Center=�$��	645,0,0,0q�$��Right=�