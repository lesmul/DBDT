   M S B A M L   `   `   `  ???? ?  ;DBDT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DBDT.KonfiguracjaDBDT.Konfiguracja  -    'DBDT.Konfiguracja.WPF_KONFIG_CONNECTION   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:DBDT.Konfiguracja       ? Title$@  :Konfiguracja programu połączenia/katalogi/synchronizacja?? ? 
ResizeMode$ NoResize=? ? WindowStartupLocation$ CenterScreen=? ? Icon$ /Konfiguracja/iokno.png?? ? WindowStyle$ 
ToolWindow=?$	??637??$	??395??$??Stretch=?$??Stretch=?$
??Left=?$	??Top=?$??5,5,5,5q?$??-1,0,0,0q?.??? $??
-5,-5,-5,4q?.+??? $??Center=?$	??356??$	??Top=?$	??627??$??0,14,0,0q?.@??? $!??Połączenia do bazy danych??.??? ???	#FFF6F5F5.+??? -   $??Zakończ i zapisz??$??43??$??	0,186,0,0q?$	??601??$??1,1,1,1q?$	??Top=?$??Center=?$??Center=?$??Center=??? $??Nazwa serwera??$
??Left=?$??7,62,0,0q?$	??Top=??? -    ??Name$ TXT_NAZWA_SERWERA??$
??Left=?$??28??$??7,86,0,0q? ?? TextWrapping$
 Wrap=?$	??Top=?$	??388?? ? ToolTip$&  Podaj adre IP lub nazwę serwera?? ?? MaxLines$ 1??	 ?? 	MaxLength$		 250??
 ?? 	UndoLimit$
 1??$??80?? ?? 	IsEnabled . .????O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib      ?? 
TargetType" ?? ?? 	Resources    CueBannerBrush j? 
AlignmentX j? 
AlignmentY j? Stretch !? Visual??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj nazwę serwera??
??????? ?? Triggers P?  P? Property ? ?? ?? Empty P? Value# Z . ???  ?? Property ? ?? ?? Value# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $
??Test??$
??Left=?$??30??$??
400,31,0,0q?$	??Top=?$	??211???? -   $??Nazwa bazy danych??$
??Left=?$??	7,114,0,0q?$	??Top=??? -   $ TXT_NAZWA_BAZY??$
??Left=?$??28??$??
10,139,0,0q?$
 Wrap=?$	??Top=?$	??385??$ Podaj nazwę bazy danych??$ 1??$		 250??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%e   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj nazwę bazy danych??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -    ??Name$ CB_HIST_POL?? ?? DisplayMemberPath$ SerwerIBaza?? ?? SelectedValuePath$ SerwerIBaza??$
??Left=?$??30??$??	10,31,0,0q?$	??Top=?$	??385???? $&?? Historia zapisanych połączeń:??$
??Left=?$??7,5,0,0q?$	??Top=??? $)??#Konfiguracja lokalizacji katalogów?? ? Cursor$
 Hand??.??? ???	#FFFAFAFA.+??? -    ??Name$ 	SC_POZIOM?? ? SmallChange$ 1Q? ? Maximum$ 2Q? ? Minimum$ 0Q?  ? Value$  0Q?$
 Hand??! ?? Orientation$! 
Horizontal=?$??497,10,0,301q?$
??Left=?$??35??$< 6Przewiń aby dodać pozostałe wartości do katalogów??$??1,1,1,1q????	#FF0C0C0C???	#FF121212" K? ZIndex$" 1???? -   $??LBL_LK_1??$$??Lokalizacja - katalog głowny:??$
??Left=?$??7,10,0,0q?$	??Top=??? -	   $ TXT_LOKALIZACJA_PLIKOW_1??$
??Left=?$??28??$??7,37,0,0q?$
 Wrap=?$	??Top=?$	??380??$ Wklej ścieszkę w pole??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%v   &	        !? $
 Left=?$ Center=?$
 None=? ?? $/??)Podaj lokalizację plików do bazy wiedzy??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -
   $# TXT_LOKALIZACJA_PLIKOW_OPIS_1??$
??Left=?$??28??$??
392,37,0,0q?$
 Wrap=?$	??Top=?$	??140??$  Podaj opis widoczny w menu??$ 1??$	 25??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj opis w drzewie??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_KATALAGU_1??
# ? Tag$# T1??$??Zapisz??$
??Left=?$??28??$??
537,37,0,0q?$	??Top=?$??74???? -   $??LBL_LK_2??$$??Lokalizacja - katalog głowny:??$
??Left=?$??7,70,0,0q?$	??Top=??? -   $ TXT_LOKALIZACJA_PLIKOW_2??$
??Left=?$??28??$??7,95,0,0q?$
 Wrap=?$	??Top=?$	??380??$ Wklej ścieszkę w pole??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%v   &	        !? $
 Left=?$ Center=?$
 None=? ?? $/??)Podaj lokalizację plików do bazy wiedzy??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $# TXT_LOKALIZACJA_PLIKOW_OPIS_2??$
??Left=?$??28??$??
392,95,0,0q?$
 Wrap=?$	??Top=?$	??140??$  Podaj opis widoczny w menu??$ 1??$	 25??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj opis w drzewie??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_KATALAGU_2??$# T2??$??Zapisz??$
??Left=?$??28??$??
537,95,0,0q?$	??Top=?$??74???? -   $??LBL_LK_3??$$??Lokalizacja - katalog głowny:??$
??Left=?$??	7,130,0,0q?$	??Top=??? -   $ TXT_LOKALIZACJA_PLIKOW_3??$
??Left=?$??28??$??	7,155,0,0q?$
 Wrap=?$	??Top=?$	??380??$ Wklej ścieszkę w pole??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%v   &	        !? $
 Left=?$ Center=?$
 None=? ?? $/??)Podaj lokalizację plików do bazy wiedzy??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $# TXT_LOKALIZACJA_PLIKOW_OPIS_3??$
??Left=?$??28??$??392,155,0,0q?$
 Wrap=?$	??Top=?$	??140??$  Podaj opis widoczny w menu??$ 1??$	 25??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj opis w drzewie??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_KATALAGU_3??$# T3??$??Zapisz??$
??Left=?$??28??$??537,155,0,0q?$	??Top=?$??74???? -   $??LBL_LK_4??$$??Lokalizacja - katalog głowny:??$
??Left=?$??	7,190,0,0q?$	??Top=??? -   $ TXT_LOKALIZACJA_PLIKOW_4??$
??Left=?$??28??$??	7,215,0,0q?$
 Wrap=?$	??Top=?$	??380??$ Wklej ścieszkę w pole??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%v   &	        !? $
 Left=?$ Center=?$
 None=? ?? $/??)Podaj lokalizację plików do bazy wiedzy??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $# TXT_LOKALIZACJA_PLIKOW_OPIS_4??$
??Left=?$??28??$??392,215,0,0q?$
 Wrap=?$	??Top=?$	??140??$  Podaj opis widoczny w menu??$ 1??$	 25??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj opis w drzewie??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_KATALAGU_4??$# T4??$??Zapisz??$
??Left=?$??28??$??537,215,0,0q?$	??Top=?$??74???? -   $??LBL_LK_5??$$??Lokalizacja - katalog głowny:??$
??Left=?$??	7,250,0,0q?$	??Top=??? -   $ TXT_LOKALIZACJA_PLIKOW_5??$
??Left=?$??28??$??	7,275,0,0q?$
 Wrap=?$	??Top=?$	??380??$ Wklej ścieszkę w pole??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%v   &	        !? $
 Left=?$ Center=?$
 None=? ?? $/??)Podaj lokalizację plików do bazy wiedzy??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $# TXT_LOKALIZACJA_PLIKOW_OPIS_5??$
??Left=?$??28??$??392,275,0,0q?$
 Wrap=?$	??Top=?$	??140??$  Podaj opis widoczny w menu??$ 1??$	 25??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%a   &	        !? $
 Left=?$ Center=?$
 None=? ?? $??Podaj opis w drzewie??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_KATALAGU_5??$# T5??$??Zapisz??$
??Left=?$??28??$??537,275,0,0q?$	??Top=?$??74???? $??Synchronizacja programu??.??? .+??? $)??#Lokalizacja - pliku bazy źródło:??$
??Left=?$??	10,12,0,0q?$	??Top=??? -   $! TXT_LOKALIZACJA_PLIKU_MATKI??$
??Left=?$??28??$??	10,37,0,0q?$
 Wrap=?$	??Top=?$	??467??$# Wklej ścieszkę w pole pliki??$ 1??$		 500??$
 1??$??80?? . .????1&clr-namespace:System;assembly=mscorlibSystem ?? 6sys&clr-namespace:System;assembly=mscorlib     " ?? ??%~   &	        !? $
 Left=?$ Center=?$
 None=? ?? $7??1Podaj lokalizację plików bazy do synchronizacji??
??????? P?  ? ??# Z . ???  ? ??# [  P?  ? ?? Z? . ???  ? ??# [  P?  ?@J?IsKeyboardFocused . . ???  ? ??
 ?????.???? -   $??ZAPISZ_LOK_PLIKU_MATKI??$# T5??$??Zapisz lokalizację??$
??Left=?$??28??$??
512,37,0,0q?$	??Top=?$??99???? -   $??button_synchronizuj??$??Synchronizuj??$??Center=?$??28??$??0,80,0,0q?$	??Top=?$	??601???? -   $
??Left=?$??Verdana)?$??28??$??
482,37,0,0q?$	??Top=?$??28??$ Otwórz?????? $+??%/DrzewoProcesu/Images/folder-open.png??$
??Fill=????? $ ?? Color$$ White?????? " ??.???  ? ?? ?? % ?? 
TargetType"% ??.R??? #??z??._??? $??Center=?$??Center=? :? & :? RoutedEvent$& Button.MouseEnter??.[??? .???? .???? ' ?? TargetProperty$' Background.Color?	( ?? To$( 	#FF484A4DX?) g? Duration$) 0:0:0.1D?:? $& Button.MouseLeave??.[??? .???? .??I? .???? $' Background.Color?$( WhiteX?$) 0:0:0.1D??? -    $
??Left=?$??Verdana)?$??28??$??580,280,0,0q?$	??Top=?$??28??$M GZapisz jako domyślne ustawienia dla pozostałych baz do synchronizacji?????? $??/IKONY/save_all.ico??$
??Fill=????? $$ White?????? " ??.???  ? ?? ?? "% ??.R??? #??z??._??? $??Center=?$??Center=? :? $& Button.MouseEnter??.[??? .???? .???? $' Background.Color?$( YellowX?$) 0:0:0.1D?:? $& Button.MouseLeave??.[??? .???? .??I? .???? $' Background.Color?$( WhiteX?$) 0:0:0.1D?