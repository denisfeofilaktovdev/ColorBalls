   M S B A M L   `   `   `  ����E  AColorBalls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ColorBalls
ColorBalls  5   	       ColorBalls.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ColorBalls     5   	     � Title$  
MainWindow��5   	   $��632.7��6   $��944.5��6+   ���	#FF113348.��5	      � -   5         �Name$ MainGrid��5	      $��0,0,2,4q�6   .+�5   
   A� -   5        $
��Left=�5      $	��147��6-   $��
754,31,0,0q�6:   $	��Top=�6N   $	��148��6f   $��0��6r   $��2��6�   $��2��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 0Q�6#      
BlurRadius$ 50Q�63      Opacity$ 0.685Q�6C   ��5      �� 5       �� ImageSource$1 +pack://siteoforigin:,,,/Resources/green.png��6    j� TileMode$ FlipXY=�6W   5      �� 5   
   -   5         ��Name$ 	MainPanel��5      $
��Left=�6$   $	��500��6?   $��	10,10,0,0q�6L   $	��Top=�6_   $	��710��6w   ���	#FF091C28$��2��6�   �� 5   
   -   5        $ NextBallsCanvas��5      $
��Left=�6*   $��80��6E   $��
10,508,0,0q�6Q   $	��Top=�6e   $	��710��6}   ���	#FF02111B6�   �� 5   
   -   5        	 ��Name$	 lblScore��5      $��0��6"   $
��Left=�6.   $��
754,31,0,0q�6I   $	��Top=�6]   $	��147��6u   $	��148��6�   $��72(�6�   $��Center=�6�   $��Center=�6�   ���	#FF113348 5        5      $ 23Q�6#   $ 2Q�63   5      5      5      