   M S B A M L   `   `   `  ����Q  MSecondaryStructureTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:SecondaryStructureTool.DataModel SecondaryStructureTool.DataModel  5   	   '    !SecondaryStructureTool.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Atoolkit-http://schemas.microsoft.com/wpf/2008/toolkit     5      D	DataModel.clr-namespace:SecondaryStructureTool.DataModel     5   	   -   5          �Name$  PrimaryWindow��5   J    � Title$' !Secondary Structure Analysis Tool��5   	   $	��800��63   $
��1224��6@    � WindowStartupLocation$ CenterScreen=�5       � 	Resources 5	         BackGroundGray��%+   &	        ��5
   
   	#FFDEDEDE��62   5      ��5      �� 5   
    �� Color# � ��6   .��5      (� -   5         (�Name$ PrimaryView��5       (� Stretch$ Uniform=�6#    J� RenderTransformOrigin$ 0.5,0.5&�65   $
��Left=�6U   $��10,10,0,-2.856q�6p   $
��1204��6�   z�5   
   V� 5      .~�5      �� �� 5      �� 5      T� 5      5      .��5   
   �� $ 0.5,0.5&�6   $��780.313��61   $��1229.869��6B   $
��Left=�6S   z�5      V� 5      .~�5      �� �� 5      �� 5      T� 5       5!      .+�5#      � -   5         �Name$ Title��5#      ���	#FF1EB80F$��712.151��6:   $	��200��6K   $
��Left=�6W   $ 0.5,0.5&�6r   
	 �� Top$	 15.851��6�   z�5$      V� 5%      .~�5&      �� �� 5'      �� 5(      T� 5)      5*      .+�5,      �� $'��!Secondary Structure Analysis Tool��6   $
��Wrap=�6E   $��20,10,0,199q�6Y   $
��Bold"�6n   $��Showcard Gothic)�6�   $��21.333(�6�   �� 5-      $ ��Written By: Clinton Walker��6   $��8(�6>   $��	10,0,0,17q�6K   $
��Left=�6^   $��Bottom=�6y   �� 5.      $'��!Bioinformatics Final Project 2015��6   $��8(�6E   $��10,0,0,6.36q�6R   $
��Left=�6g   $��Bottom=�6�   �� 5/      $��Ver: 1.0��6   $��8(�6,   $��168.065,0,0,6.36q�69   $
��Left=�6S   $��Bottom=�6n   51      � 52      -   5        $ Setup��52      $	��567��6"   
 �� Left$
 213.645��6/   $��288.858��6E   $
��Left=�6U   $ 0.5,0.5&�6p    � DataContext$  ��6�   $	 148.108��6�    � RowDefinitions 53      �� 54      $
��226* �6$   �� 55      $
��341* �6$   z�57      V� 58      .~�59      �� �� 5:      �� 5;      T� 5<      5=      .+�5?      �� $
��Left=�6   $��369.848��67   
���   �$	��Top=�6W   $��280.858��6o   $��-114.029,107.5,0,0q�6   $��0��6�    �� RadiusX$ 10��6�    �� RadiusY$ 12��6�   $��2��6�   ��5@      �� 5A      # � ��6*   5C      �� 5D      $
��Left=�6   $��477.348��67   
���   �$	��Top=�6W   $��280.858��6o   $��-3.53,0,0,0q�6   $��0��6�   ���	#FFB3B3B3$ 10��6�   $ 10��6�   $��2��6�   �� 5E      -   5         ��Name$ AccessionNumber��5E      $#��-104.506,203.07,304.364,2.663q�6T   $��8(�6{   $��Center=�6�   $��Center=�6�   $��89��6�   $��20.267��6�   ���	#FFDEDEDE��63   �� *AccessionNumber+6�   �� 5F      -   5         ��Name$ Submit��5F      $��Submit��6)   $
��Left=�6:   $��208.273,211.137,0,0q�6U   $	��Top=�6r   $��49��6�   $��20��6�   ���	#FF1EB80F$��Arial Black)�6�   $��10.667(�6�   $
��Bold"�6�   $��1��6  �� 5G      -   5        $ JobID��5G      $#��26.429,35.414,173.429,171.379q�6@   $��8(�6g   $��Center=�6t   $��Center=�6�   $��89��6�   $��19.319��6�   ���	#FFD1D1D1$y�Hidden=�6�   ��6)   �� *JobID+6�   �� 5H      $��Job ID��6   $
��Left=�6)   $��3.5,20.281,0,0q�6D   $	��Top=�6\   $��20��6t   $��45��6�   $��8(�6�   $ 0.894,0.532&�6�   $y�Hidden=�6�   �� 5I      $��Accession Number��6   $
��Left=�63   $��-109.53,183.07,0,0q�6N   $	��Top=�6j   $��20��6�   $��79��6�   $��8(�6�   �� 5J      $��Enter Sequence or FASTA:��6   $
��Left=�6;   $��-109.53,2.337,0,0q�6V   $	��Top=�6q   $��20.098��6�   $	��106��6�   $��8(�6�   $��1��6�   �� 5K      $��Query Setup��6   $
��Left=�6.   $��5.752,-0.5,0,0q�6I   $	��Top=�6a   $
��25.5��6y   $
��96.5��6�   $��Arial Black)�6�   $��9.333(�6�   �� 5L      -   5        $ 
SeqOrFASTA��5L      $$��-104.506,22.379,21.535,134.751q�6T    �� TextWrapping$
 Wrap=�6|   $��8(�6�   $��Center=�6�   $��Center=�6�   $��183.758��6�   $��371.829��6�   {�.  �� 	UndoLimit$
 1000��6   �� AcceptsReturn . $��1��6-  ��6.   �� *SequenceOrFASTAInput+.��6:  ��5M      �� 5N      $ 	#FFDEDEDE��6*   .��5O   *   5P      �� 5Q      -	   5         ��Name$ AccessionNumber_Copy��5Q      $��15,203,21.858,3q�6O   $��8(�6h   $��Center=�6u   $��Center=�6�   $	��252��6�   $��20��6�   ���	#FFDEDEDE��66   �� *Name+6�   �� 5R      $��Name of Sequence��6   $
��Left=�63   $��10,183.07,0,0q�6N   $	��Top=�6e   $��20��6}   $��79��6�   $��8(�6�   �� 5S      -
   5         ��Name$ BadSequenceData��5S      $�� ��64   $
��Left=�6�   $��-104.506,191.094,0,0q�6�   $��1��6�   $	��Top=�6�   $y�Hidden=�6   d� 	IsChecked 6?   �� *BadSequence+5U      �� 5V      -   5         ��Name$ DataBaseDropdown��5V      $#��-104.506,162.43,297.183,41.61q�65   $y�Hidden=�6\   ��5W      �� 5X       �� EndPoint$	 0,1&�6.    �� 
StartPoint$	 0,0&�6=   .��5Y      �  � Color$ 	#FFF0F0F0��6+    � Offset$ 0Q�6=   � 5Z      $ 	#FFDEDEDE��6+   $ 1Q�6=   5[      5_      �� 5`      $��Choose Database��6   $
��Left=�62   $��-109.53,142.43,0,0q�6M   $	��Top=�6i   $��20��6�   $��79��6�   $��8(�6�   $y�Hidden=�6�   �� 5a      $��Stretch=�6   $
��Auto��6:   $!��242.136,27.331,38.92,55.029q�6I   $
��Wrap=�6n   $��	TextBlock��6�   $��Center=�6�   $y�Hidden=�6�   �� 5b      $
��Left=�6   $
��Wrap=�67   $	��Top=�6K   $	��153��6d   $	��242��6q   $��	15,20,0,0q�6}   �� 6�   $���     This Program is designed to combine multiple tools to assist in the secondary structure prediction of Proteins. The tools include,  a hydropathy plot, Chou Fasman Algorithm and Transmembrane Hidden Markov Model TMHMM from:��6�   �� 6�  �� 6�  $-�' http://www.cbs.dtu.dk/services/TMHMM/.��6�  �� 6�  �� 6�  $F�@The program will query the UniProtKB data base for FASTA files.
��6�  6"  5c      � 5f      -   5        $ Results��5f      $��754.462��6$   $
 10��65   $��1225.074��6F   $ 0.5,0.5&�6W   $	 15.851��6w   $
��Left=�6�   $��Center=�6�   $y�Hidden=�6�   .+�5g      �� -   5         ��Name$ DataGridScroller��5g      $
��Left=�69   ��. $
��Auto=�6m   
���   �$
��Auto��6�   $��Visible=�6�   $	��Top=�6�   $��693.738��6�   $��0,50.724,0,0q�6  .��5i      � -   5        $ DataGridWrapper��5i      $��675.989��64   $��10.485��6E   $
��Left=�6T   $	��Top=�6o   .+�5j      � -   5        $ 	GraphGrid��5j      $
��Auto��64   $��	10,10,0,0q�6A   $
��Left=�6T   $	��Top=�6o   $��652.632��6�   � 5k      -   5        $ DataGrid��5k      $	��100��61   $
��Auto��6>   $��10,533.281,0,0q�6K   $
��Left=�6c   $	��Top=�6�   � 5l      -   5        $ ChouFasmanGrid��5l      $
��Left=�67   $��102.27��6R   $	��Top=�6b   $��0.485��6z   $��10,394.479,0,0q�6�   5n      5p      �� 5q      -   5        $ Reset��5q      $��Reset��6(   $
��Left=�6O   $��75��6j   ���	#FF1EB80F$	��Top=�6�   $��0,-24.96,0,0q�6�   � 5r      -   5        $ Results_Title��5r      $��90,-21.249,0,0q�6/   $
��Auto��6G   $	��Top=�6T   $
��Left=�6l   .+�5s      �� -   5         ��Name$ Sequence_Name��5s       $!��98.015,-5.434,29.155,30.521q�6M   ���	#FF1EB80F$��16(�6�   $
��Bold"�6�   ��67   �� *Name+6�   5t      � 5u      -   5        $ DataManipulationGrid��5u      $��77.059,0,0,0q�65   $
��Left=�6K   $	��Top=�6f   $��215.124��6~   $��567.841��6�   .+�5v      9� $��Chou-Fasman Settings��6   $
��Left=�6=   $	��Top=�6X   ���	#FFB3B3B3$
��Bold"�6�   $��189.774,-1.219,0,0q�6�   $��159.486��6�   .��5w      � $��139.002��6   .+�5x      � -   5        $ Chou_Fasman_Settings��5x   #   $��10,0,-2.165,22q�6A   .+�5y   "   �� ���	#FFB3B3B3$
��Left=�6=   $ 10��6Y   $ 10��6f   
���   �$��0��6�   $	��Top=�6�   $��140.664��6�   $��109.192��6�   $��0,4.023,0,0q�6�   �� 5z   "   $��	Show Data��6(   $
��Left=�6<   $��28.294,-0.011,0,0q�6W   $	��Top=�6r   �� 5{   "   
���   �$��1q�6=   $
��Left=�6Q   $��86.917��6l   $��17.342,19.843,0,0q�6|   $	��Top=�6�   $��104.888��6�    �� CornerRadius$ 10��6�   	� 5|   "   -   5         	�Name$ 
AlphaHelix��5|   .   $��Alpha-Helix��6B   $
��Left=�6X   $��20.772,23.132,0,0q�6s   $	��Top=�6�   6�   	� 5}   "   -   5        $ 	BetaSheet��5}   .   $��
Beta-Sheet��6A   $
��Left=�6V   $��20.772,38.092,0,0q�6q   $	��Top=�6�   6�   	� 5~   "   -   5        $ LoopCoil��5~   .   $��	Loop-Coil��6@   $
��Left=�6T   $��20.772,53.052,0,0q�6o   $	��Top=�6�   $y�	Collapsed=�6�   	� 5   "   -   5        $ ChouFasmanShow_All��5   .   $	��All��6J   $
��Left=�6X   $��20.772,68.012,0,0q�6s   $	��Top=�6�   5�   6   	� 5�   "   -   5        $ ChouFasmanHide_All��5�   .   $��Hide All��6J   $
��Left=�6]   $��20.772,82.972,0,0q�6x   $	��Top=�6�   5�   "   5�      5�      5�      9� 5�      $��Hydropathy Plot Settings��6   $
��Left=�6A   $	��Top=�6\   ���	#FFB3B3B3$
��Bold"�6�   $��206.335��6�   $��261.076��6�   $��-76.302,-1.219,0,0q�6�   .��5�      � $��266.162��6    .+�5�      � -   5        $ Hydropathy_Plot_Settings��5�   #   $��10,5.538,0,-14.016q�6E   .+�5�   "   �� ���	#FFB3B3B3$
��Left=�6=   $��178.797��6X   $ 10��6i   $ 10��6v   
���   �$��0��6�   $	��Top=�6�   $��246.885��6�   � 5�   "   -   5        $ 	ScaleGrid��5�   '   $#��13.524,-4.653,132.269,135.425q�6:   .+�5�   &   �� $��Scale��6,   $
��Left=�6<   $��5.083,0,0,35.853q�6W   $��40.067��6q   �� 5�   &   
���   �$��1q�6A   $
��Left=�6U   $��41.368��6p   $��0,19.85,0,0q�6�   $	��Top=�6�   $��110.584��6�   $ 10��6�   	� 5�   &   -   5        $ Kyte_Doolittle��5�   2   $��Kyte-Doolittle��6J   $
��Left=�6c   $��8.035,25.859,0,0q�6~   $	��Top=�6�   6�   	� 5�   &   -   5        $ 
Hopp_Woods��5�   2   $��
Hopp-Woods��6F   $
��Left=�6[   $��8.035,40.819,0,0q�6v   $	��Top=�6�   6�   �� 5�   &   $��Custom Window��6,   $
��Left=�6D   $��120.705,0,-120.649,35.853q�6_   $��110.313��6�   5�   #   � 5�   "   -    5        $ TypeGrid��5�   '   $ ��13.524,56.388,86.297,18.84q�69     � ColumnDefinitions  5�   &   �� 5�   *   $
��167* �6;   �� 5�   *   $	��51* �6;   .+�5�   &   �� $
��Type��6,   $
��Left=�6;   $��5.083,0,0,0q�6V   �� 5�   &   
���   �$��1q�6A   $
��Left=�6U   $��97.146��6p   $��0,19.85,-68.951,-0.411q�6�   $	��Top=�6�   $��225.292��6�   $ 10��6�   $��2��6�   	� 5�   &   -!   5        $ TransMembrane��5�   2   $$��TransMembrane (Window Size 19)��6I   $
��Left=�6r   $��8.035,25.859,-68.951,0q�6�   $	��Top=�6�   $��2��6�   $��217.257��6�   	� 5�   &   -"   5        $ SurfaceRegion��5�   2   $$��Surface Region (Window Size 9)��6I   $
��Left=�6r   $��8.035,40.819,-68.951,0q�6�   $	��Top=�6�   $��2��6�   $��217.257��6�   	� 5�   &   -#   5        $! SurfaceRegion_TransMembrane��5�   2   $$��Both Transmembrane and Surface��6W   $
��Left=�6�   $��8.035,55.779,-68.951,0q�6�   $	��Top=�6�   $��2��6�   $��217.257��6  	� 5�   &   -$   5        $ CustomWindow��5�   2   $��Custom Window��6H   $
��Left=�6`   $��8.035,70.739,-63.83,0q�6{   $	��Top=�6�   $��2��6�   $��212.136��6�   	� 5�   &   -%   5        $ ShowAllHydroPlots��5�   2   $��Show All��6M   $
��Left=�6`   $��8.035,85.699,0,0q�6{   $	��Top=�6�   6�   	� 5�   &   -&   5        $ HideAllHydroPlots��5�   2   $��Hide All��6M   $
��Left=�6`   $��8.035,99.96,0,0q�6{   $	��Top=�6�   6�   5�   #   � 5�   "   -'   5        $ HydroWindowSize��5�   '   $��133.115,21.106,0,116.997q�6@   $
��Left=�6b   $��113.77��6}    5�   &   �� 5�   *   $	��19* �68   �� 5�   *   $��5* �68   �� 5�   *   $	��30* �68     5�   &   �� 5�   *   $��0* �6;   �� 5�   *   $	��65* �6;   �� 5�   *   $	��49* �6;   .+�5�   &   �� 
���   �$��1q�6A   $
��Left=�6U   $��57.697��6p   $��10,-6.011,0,0q�6�   $	��Top=�6�   $��86.063��6�   $ 10��6�   $��3��6�   $��3��6�   �� 5�   &   -(   5        $ CustomWindowButton��5�   -   $��Create��6I   $
��Left=�6Z   $	��Top=�6u   $��54.822��6�   $��26.618,4.235,0,0q�6�   ���	#FF1EB80F$��3��6�   $��2��6  �� 5�   &   $��20.484,1.113,14.625,3.118q�60   $
��Wrap=�6S   
���   �$��Size:��6z   $��2��6�   $��2��6�   �� 5�   &   -)   5        $ HydropathyWindowSize��5�   .   $
��Left=�6L   $��19.775��6g   $��49.973,0,0,0q�6w   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $��28.146��6�   $��3��6�   $��2��6�   5�   #   5�      5�      5�      9� 5�      $��TMHMM Settings��6   $
��Left=�67   $	��Top=�6R   $��361.759,-1.219,0,0q�6j   ���	#FFADADAD$
��Bold"�6�   $��194.115��6�   $��193.082��6�   .��5�      � -*   5        $ TMHMMSettingsGrid��5�      .+�5�      �� ���	#FFB3B3B3$
��Left=�69   $��164.876��6T   $ 10��6e   $ 10��6r   
���   �$��0��6�   $	��Top=�6�   $��186.961��6�   $��14.866,3.337,-10.745,0q�6�   	� 5�      -+   5        $��Inner Regions��5�   *   $
��Left=�6B   $��28.186,59.701,0,0q�6]   $	��Top=�6x   6�   	� 5�      -,   5        $��Outer Regions��5�   *   $
��Left=�6B   $��28.186,74.661,0,0q�6]   $	��Top=�6x   6�   	� 5�      --   5        $��Transmembrane Regions��5�   *   $
��Left=�6J   $��28.186,89.621,0,0q�6e   $	��Top=�6�   6�   	� 5�      -.   5        $ TMHMMShowAll_RadioButton��5�   *   $��Show All��6L   $
��Left=�6_   $��28.186,104.581,0,0q�6z   $	��Top=�6�   6�   	� 5�      -/   5        $��Hide All��5�   *   $
��Left=�6=   $��28.186,119.541,0,0q�6X   $	��Top=�6t   6�   �� 5�      
���   �$��1q�69   $
��Left=�6M   $��81.18��6h   $	��Top=�6w   $��171.082��6�   $��23.34,54.44,-3.34,0q�6�   $ 10��6�   �� 5�      -0   5        $��Delete��5�   %   ���	#FFEA1212$
��Bold"�6M   $!��119.422,140.62,-3.34,10.535q�6_   6�   �� 5�      -1   5        $ TMHMMJobSelectionComboBox��5�   '   $
��Left=�6J   $��31.568,24.821,0,0q�6e   $	��Top=�6�   $��149.514��6�   6�   �� 5�      $��Select TMHMM Job��6$   $
��Left=�6?   $��18.145,3.337,0,0q�6Z   $	��Top=�6t   5�      5�      5�      � 5�      -2   5        $ 	TMHMMGrid��5�      $
��Left=�6*   $��110.177��6E   $	��Top=�6V   $��138.294��6n   $��12.271,209.39,0,0q�6~   $y�Hidden=�6�   .+�5�      �� $��TMHMM Setup��6   $
��Left=�62   $	��Top=�6M   ���	#FFB3B3B3$
��Bold"�6|   $��0,-4.879,0,0q�6�   �� 5�      ���	#FFB3B3B3$
��Left=�61   $��89.096��6L   $ 10��6\   $ 10��6i   
���   �$��0��6�   $	��Top=�6�   $��158.779��6�   $��0,21.081,-20.485,0q�6�   �� 5�      -3   5        $ StartTmhmm_Button��5�      $��Start��68   $
��Left=�6H   $��29.608,80.217,0,0q�6c   $	��Top=�6~   $��75��6�   ���	#FF1EB80F$
��Bold"�6�   6�   �� 5�      $��Sequence Size��6   $
��Left=�64   $	��Top=�6O   $��6.238,18.412,0,0q�6g   $
��Bold"�6�   �� 5�      -4   5        $ seqLengthTmhmm��5�      $��0��64   $
��Left=�6@   $	��Top=�6[   $��98.238,18.412,0,0q�6s   $
��Bold"�6�   $��46.536��6�   � 5�      -5   5        $ TMHMMDataGrid��5�      $
��Left=�62   $	��426��6M   $	��Top=�6Z   $	��370��6r   $%��165.172,42.127,-396.878,-357.95q�6~   $y�Hidden=�6�   .+�5�      �� ���	#FFB3B3B3$
��Left=�65   $��504.601��6P   $ 10��6a   $ 10��6n   
���   �$��0��6�   $	��Top=�6�   $��362.539��6�   $��0,-20.888,0,-57.713q�6�   �� 5�      -6   5        $ SubmitTMHMMDataButton��5�   !   $��Submit��6@   $
��Left=�6Q   $��250.936,440.684,0,-34.644q�6l   $	��Top=�6�   $��75��6�   $
��Bold"�6�   ���	#FF20B5126�   �� 5�      -7   5        $ Cancel_TMHMM��5�   !   $��Cancel��67   $
��Left=�6H   $��30.727,440.684,0,-34.644q�6c   $	��Top=�6�   $��75��6�   $
��Bold"�6�   ���	#FFF01D1D6�   �� 5�      $��Selected Sequence:��6    $
��Left=�6=   $	��Top=�6X   $��23.46,17.456,0,0q�6p   $
��Bold"�6�   �� 5�      -8   5        $ TMHMMSequenceTextBox��5�   "   $
��Left=�6@   $��123.029��6[   $��30.727,48.416,0,0q�6l   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $��295.209��6�   �� 5�      $��Paste Data Here:��6    $
��Left=�6;   $	��Top=�6V   $��23.46,179.815,0,0q�6n   $
��Bold"�6�   $��24.892��6�   �� 5�      -9   5        $ TMHMMDataTextBox��5�   "   $
��Left=�6<   $��225.977��6W   $��30.727,209.707,0,-9.684q�6h   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $��295.209��6�    . 6�   �� 5�      $��TMHMM Interface Window��6    $
��Left=�6A   $	��Top=�6\   $��23.46,-8.504,0,0q�6t   $
��Bold"�6�   �� 5�      -:   5        $ TMHMMHelpButton��5�   !   $
��Help��6:   $
��Left=�6I   $��261.178,-5.504,0,0q�6d   $	��Top=�6�   $��75��6�   ���	#FF1EB80F6�   5�      �� 5�      $��Job ID��6   $
��Left=�6-   $	��Top=�6H   $��6.238,38.127,0,0q�6`   $
��Bold"�6z   �� 5�      -;   5        $ TMHMMJobIDTextBox��5�      $
��Left=�69   $��17.879��6T   $��52.318,42.127,0,0q�6d   $
 Wrap=�6   $��Job 1��6�   $	��Top=�6�   $��70.976��6�   5�      � 5�      -<   5        $ 	webWindow��5�      $
��Left=�6*   $��734.462��6E   $	��Top=�6V   $��600.296��6n   $��614.778,10,0,0q�6~   $y�	Collapsed=�6�   .+�5�      �� $
��Left=�6    $
��Wrap=�6;   $��	TextBlock��6O   $	��Top=�6`   Outside area of frame6�   �� 5�      -=   5        $ CloseWebWindowButton��5�      $��Hide Window��6;   $
��Left=�6Q   $	��Top=�6l   $��102.423��6�   ���	#FFF01D1D6�   5�      5�      5�      5�      5�      