   M S B A M L   `   `   `  ���� ?  ;DBDT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DBDT.SQL.SQL_SELECTDBDT.SQL.SQL_SELECT  '    !DBDT.SQL.SQL_SELECT.MainWindowSQL   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     0cc!clr-namespace:DBDT.SQL.SQL_SELECT     -     +� FocusedElement  ��  �� ElementName$ txtCode�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$
 Open��  -   $
 Save��  -   $ cc:SqlHandler.ParseOpis��  -   $  cc:SqlHandler.ParseCommand��  -   $" cc:SqlHandler.ExecuteCommand��  -   $" cc:SqlHandler.ConnectCommand��  -   $% cc:SqlHandler.DisconnectCommand��  -	   $ cc:SqlHandler.ParseTable��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+�c� $��1�� � 	Resources  �� 
TargetType   buttonStyle �� Property �� Value	 �� RelativeSource
 �� Path �� Triggers �� Binding �� AncestorType �� AncestorLevel �� Value��%�   &	        '�� P     �� " ��.���  �@�ToolTip �� 	 �� *Self+$
 Command.Text��� " �� ��  �� 	 �� " ��$ 1��$
 	IsEnabled�	 False.a���  �@J�Opacity$
 0.30Q�.@��� $
��Open��#��[  .���� $��Images/folder_page.png���� $
��Save��#��[  .���� $��Images/disk.png���� $��cc:SqlHandler.ParseOpis��#��[   � ToolTip$ Zapisz do bazy danych��.���� $��Images/database.png���� �� $	��Cut��#��[  .���� $��Images/cut.png���� $
��Copy��#��[  .���� $ ��Images/page_white_copy.png���� $��Paste��#��[  .���� $!��Images/page_white_paste.png���� �� $"��cc:SqlHandler.ConnectCommand��#��[  $ Połącz��.���� $!��Images/database_connect.png���� $%��cc:SqlHandler.DisconnectCommand��#��[  $ 	Rozłącz��.���� $��Images/disconnect.png���� �� -
    ��Name$ 	b_wykonaj��$"��cc:SqlHandler.ExecuteCommand��#��[  $ Wykonaj��.���� $��Images/lightning.png��{�.  �� $ ��cc:SqlHandler.ParseCommand��#��[  $ Analizuj��.���� $��Images/bug.png���� �� $��80��$y�Hidden=��� $��cc:SqlHandler.ParseTable��#��[  $  Włącz auto uzupełnianie��.���� -    ��Name$ auto_on��$%��/SQL/SQL_SELECT/Images/auto.png���� �� -   $��B_EXIT��#��[  $ Zamknij��.���� $��Images/exit.ico���� -   $��txtCode�� �� AcceptsReturn .  �� 
AcceptsTab . $ �� HorizontalScrollBarVisibility$
 Auto=�" �� VerticalScrollBarVisibility$
 Auto=�$��5q�$��Courier New)�$��2��$��14(� �� 	MaxLength$
 9000��.�� � ContextMenu �� -   $	��cxm��.@�m� -   $��
cxmItemCut��$��Wytnij��m� -   $��cxmItemCopy��$��Kopiuj��m� -   $��cxmItemPaste��$��Wklej���� m� -   $��cxmItemSelectAll��$��Zaznacz wszystko��m� -   $��cxmItemSelectLine��$��Wybierz bieżącą linię���� m� -   $��cxmItemUndo��$��Cofnij ostatnią akcję��m� -   $��cxmItemRedo��$��Powtórz ostatnią akcję���� m� -   $��cxmItemClear��$��Wyczyść cały tekst���� m� $��W zaznaczonym fragmencie��.@�m� -   $��cxmItemLike��$��zamień na LIKE��m� -   $��cxmItemLikeProc��$��zamień na LIKE%...%��m� -   $��cxmItemLikeRow��$��zamień na =��m� -   $��cxmItemLikeAnd��$��zamień na AND��m� -   $��cxmItemLikeOr��$��zamień na OR���� m� -   $��cxmItemUpperText��$��Zmień na DUŻE LITERY��m� -   $��cxmItemLowerText��$��Zmień na małe litery��m� -   $��cxmItemColumsTable��$��Kopiuj kolumny w tabeli���� m� -   $��praca_procedura��$"��Ustaw tryb pracy - procedura���� m� -    $��cxmSelecAllTable��$��Pokaż wszystkie tabele��.��9� -!   $��errorsExpander��$��3��$��Błędy��.��&   System.Windows.Controls.DataGrid  -"   $��
errorsGrid��$	��120��   AutoGenerateColumns .     RowStyle �� )  #System.Windows.Controls.DataGridRow"  .���  � ��
 �  �� P�  P� Property �@ 
IsSelected P� Value . . ���  � ��
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Numer��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $
 Number�  $ Poziom�� �� $
 Class�  $ Status�� �� $
 State�  $ Linia�� �� $
 
LineNumber�      Width5  /System.Windows.Controls.DataGridLengthConverter$  * $ Wiadomość�� �� $
 Message��� -#   $��	statusBar��$��4��$��Stretch=����� .R�� ! � ColumnDefinitions! �� $
��Auto ��� $
��Auto ��� $
��Auto �.@��� $��0��.���� -$   $��connStatusIcon���� $��1��.���� -%   $��	txtStatus���� $��2��.���� -&   $��txtConnection��