   M S B A M L   `   `   `  ����?  ;DBDT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DBDT.SQL.SQL_SELECTDBDT.SQL.SQL_SELECT  5      '    !DBDT.SQL.SQL_SELECT.MainWindowSQL   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0cc!clr-namespace:DBDT.SQL.SQL_SELECT     5      -   5          +� FocusedElement  5   "   ��  �� ElementName$ txtCode�� J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$
 Open��5      6(     5	   
   -   5        $
 Save��5	      6(     5
   
   -   5        $ cc:SqlHandler.ParseOpis��5
      6;     5   
   -   5        $  cc:SqlHandler.ParseCommand��5      6X     5   
   -   5        $" cc:SqlHandler.ExecuteCommand��5      6\     5   
   -   5        $" cc:SqlHandler.ConnectCommand��5      6@     5   
   -   5        $% cc:SqlHandler.DisconnectCommand��5      6C     5   
   -	   5        $ cc:SqlHandler.ParseTable��5      6Q   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5+   
   c� $��1��6    � 	Resources 5,       �� 
TargetType   buttonStyle �� Property �� Value	 �� RelativeSource
 �� Path �� Triggers �� Binding �� AncestorType �� AncestorLevel �� Value��%B  &	        '�� u     �� 5-      " ��6   .�5.      ��  �@�ToolTip 60   �� 	 �� *Self+$
 Command.Text�5/      �� 50      " ��6    51      �� 52       6&   �� 	 �� " ��$ 1��$
 	IsEnabled�	 False6�   .a�53      ��  �@J�Opacity$
 0.30Q�68   54      56      57      .@�58      �� $
��Open��6   #��[  6$   .��59      �� $��Images/folder_page.png��6   5:      �� 5;      $
��Save��6   #��[  6$   .��5<      �� $��Images/disk.png��6   5=      �� 5>      $��cc:SqlHandler.ParseOpis��6   #��[  67    � ToolTip$ Zapisz do bazy danych��6h   .��5?      �� $��Images/database.png��6   5@      �� 5A      �� 5B      $	��Cut��6   #��[  6#   .��5C      �� $��Images/cut.png��6   5D      �� 5E      $
��Copy��6   #��[  6$   .��5F      �� $ ��Images/page_white_copy.png��6   5G      �� 5H      $��Paste��6   #��[  6%   .��5I      �� $!��Images/page_white_paste.png��6   5J      �� 5K      �� 5L      $"��cc:SqlHandler.ConnectCommand��6   #��[  6<   $ Połącz��6m   .��5M      �� $!��Images/database_connect.png��6   5N      �� 5O      $%��cc:SqlHandler.DisconnectCommand��6   #��[  6?   $ 	Rozłącz��6p   .��5P      �� $��Images/disconnect.png��6   5Q      �� 5S      �� 5T      -
   5         ��Name$ 	b_wykonaj��5T      $"��cc:SqlHandler.ExecuteCommand��6(   #��[  6O   $ Wykonaj��6�   .��5U      �� $��Images/lightning.png��6   {�.  66   5V      �� 5W      $ ��cc:SqlHandler.ParseCommand��6   #��[  6:   $ Analizuj��6k   .��5X      �� $��Images/bug.png��6   5Y      �� 5Z      �� 5[      $��80��6   $y�Hidden=�6#   �� 5\      $��cc:SqlHandler.ParseTable��6   #��[  68   $  Włącz auto uzupełnianie��6i   .��5]      �� -   5         ��Name$ auto_on��5]      $%��/SQL/SQL_SELECT/Images/auto.png��6)   5^      �� 5_      �� 5`      -   5        $��B_EXIT��5`      #��[  6#   $ Zamknij��6T   .��5a      �� $��Images/exit.ico��6   5b      5c      �� 5d   
   -   5        $��txtCode��5d       �� AcceptsReturn .  �� 
AcceptsTab . $ �� HorizontalScrollBarVisibility$
 Auto=�5g      " �� VerticalScrollBarVisibility$
 Auto=�5h      $��5q�5i      $��Courier New)�5j      $��2��5k      $��14(�69    �� 	MaxLength$
 9000��6G   .��6Y    � ContextMenu 5l      �� 5m      -   5        $	��cxm��5m      .@�5n      m� -   5        $��
cxmItemCut��5n   /   $��Wytnij��6   6A   m� 5o      -   5        $��cxmItemCopy��5o   /   $��Kopiuj��6   6B   m� 5p      -   5        $��cxmItemPaste��5p   .   $��Wklej��6   6B   �� 5q      m� 5r      -   5        $��cxmItemSelectAll��5r   9   $��Zaznacz wszystko��6   6Q   m� 5s      -   5        $��cxmItemSelectLine��5s   >   $��Wybierz bieżącą linię��6   6W   �� 5t      m� 5u      -   5        $��cxmItemUndo��5u   >   $��Cofnij ostatnią akcję��6   6Q   m� 5v      -   5        $��cxmItemRedo��5v   ?   $��Powtórz ostatnią akcję��6   6R   �� 5w      m� 5x      -   5        $��cxmItemClear��5x   <   $��Wyczyść cały tekst��6   6P   �� 5y      m� 5z      $��W zaznaczonym fragmencie��6   .@�5{      m� -   5        $��cxmItemLike��5{   ;   $��zamień na LIKE��6#   6N   m� 5|      -   5        $��cxmItemLikeProc��5|   @   $��zamień na LIKE%...%��6#   6W   m� 5}      -   5        $��cxmItemLikeRow��5}   8   $��zamień na =��6#   6N   m� 5~      -   5        $��cxmItemLikeAnd��5~   :   $��zamień na AND��6#   6P   m� 5      -   5        $��cxmItemLikeOr��5   9   $��zamień na OR��6#   6N   �� 5�      m� 5�      -   5        $��cxmItemUpperText��5�   A   $��Zmień na DUŻE LITERY��6#   6Y   m� 5�      -   5        $��cxmItemLowerText��5�   A   $��Zmień na małe litery��6#   6Y   m� 5�      -   5        $��cxmItemColumsTable��5�   D   $��Kopiuj kolumny w tabeli��6#   6^   �� 5�      m� 5�      -   5        $��praca_procedura��5�   #   $"��Ustaw tryb pracy - procedura��6;   6a   5�      �� 5�      m� 5�      -    5        $��cxmSelecAllTable��5�   @   $��Pokaż wszystkie tabele��6   6X   5�      .��5�   #   5�      9� 5�   
   -!   5        $��errorsExpander��5�       $��3��6   $��Błędy��66   .��5�      &   System.Windows.Controls.DataGrid  -"   5        $��
errorsGrid��5�      $	��120��6)      AutoGenerateColumns .     RowStyle 5�      �� 5�      )  #System.Windows.Controls.DataGridRow"  6   .�5�      ��  � ��
 �  ��67    5�      P� 5�       P� Property �@ 
IsSelected P� Value . . �5�   "   ��  � ��
 �����6?   5�      5�         Columns 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Header$ Numer��6I   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� $
 Number�6I     5�      $ Poziom��6H    6)   �� $
 Class�6H     5�      $ Status��6H    6)   �� $
 State�6H     5�      $ Linia��6M    6)   �� $
 
LineNumber�6M     5�          Width5  /System.Windows.Controls.DataGridLengthConverter$  * 6J   $ Wiadomość��6T    6)   �� $
 Message�6T   5�      5�      �� 5�   
   -#   5        $��	statusBar��5�      $��4��6%   $��Stretch=�62   ��5�      �� 5�      .R�5�      � ! � ColumnDefinitions! 5�      �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   �� 5�      $
��Auto �6/   5�      5�      .@�5�      �� $��0��6   .��5�      �� -$   5        $��connStatusIcon��5�      5�      �� 5�      $��1��6   .��5�      �� -%   5        $��	txtStatus��5�      5�      �� 5�      $��2��6   .��5�      �� -&   5        $��txtConnection��5�      5�      5�      5�      5�      