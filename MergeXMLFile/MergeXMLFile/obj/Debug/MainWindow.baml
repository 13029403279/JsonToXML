   M S B A M L   `   `   `  ����G  CMergeXMLFile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MergeXMLFileMergeXMLFile  5   	       MergeXMLFile.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:MergeXMLFile     5   	     � DataContext$   ��5   	    � Title$ 
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6   $��30��6J   $��80��6Y   $	��Top=�6g   $��	10,50,5,5q�6�   $
��Left=�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   ��6/   �� *	FileName1+6�   �� 5   
   -   5        $��0��5      $��1��6   $��Browse File��6.   $��	20,50,5,5q�6D   $��80��6W   $��40��6b   $
��Left=�6n   $	��Top=�6�   �� 5   
   $��1��6   $��0��6   $��30��6K   $��80��6Z   $��	10,50,5,5q�6h   $
��Left=�6{    J� 	AllowDrop . $	��Top=�6�   ��60   �� *	FileName2+6�   �� 5   
   -   5        $��1��5      $��1��6   $��Browse File��6.   $��	20,50,5,5q�6D   $��80��6W   $��40��6b   $
��Left=�6n   $	��Top=�6�   �� 5   
   -   5        $��2��5      $��0��6   $��Compare��6.   $��	10,50,5,5q�6@   $��80��6S   $��40��6^   $
��Left=�6j   $	��Top=�6�   �� 5   
   -   5        $��2��5      $��1��6   $��Save Result��6.   $��	20,50,5,5q�6D   $��80��6W   $��40��6b   $
��Left=�6n   $	��Top=�6�   5      5      