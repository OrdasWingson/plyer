   M S B A M L   `   `   `  ����J  FMassEffectPlyer, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MassEffectPlyerMassEffectPlyer  5   	       MassEffectPlyer.Authorize   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:MassEffectPlyer     5   	   -   5          �Name$  AuthorizeWindow��5   	    � Title$ 	Authorize��5   	   $	��600��6   $	��800��6(    � WindowStyle$
 None=�64    � AllowsTransparency .  ��5	      �� 5
   
    �� ImageSource$ Image/authorize.png��6   .��5      � .+�5   
   (  "System.Windows.Controls.WebBrowser  -   5          Name$ webBrowser1��5      $
��Left=�6*   $	��282��6E   $��
140,58,0,0q�6R   $	��Top=�6f   $	��524��6~    J� RenderTransformOrigin$ 0.508,0.495&�6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.036Q�6&   T� 5       T� Y$ 0.004Q�6)   5      	 J� Clip	 5      � 5      
 � FillRule$
 Nonzero=�6    .��5      ��  �� Rect$ -59,85,190,150�6(    � 	Transform 5      �� 5      $	 -53Q�6.   5      5      5      �� 5    
   -   5         ��Name$ button��5       $��ОТМЕНА��6!   $
��Left=�62   $��48��6M   $��375,360,0,0q�6Y   $	��Top=�6n   $	��297��6�   ���	#FFEDF370���	#01DDDDDD$��24(�6�      DefaultButtonStyle#���   6�   �� 5!   
   -   5         ��Name$ 	textBlock��5!      $
��Left=�6'   $	��100��6B   $��
226,96,0,0q�6O   $
��Wrap=�6c   $|��vИДЕТ ПОДКЛЮЧЕНИЕ С ЦИТАДЕЛЬЮ. ЭТО МОЖЕТ ЗАНЯТЬ НЕСКОЛЬКО СЕКУНД.��6w   $	��Top=�6�   $	��382��6�   $��24(�6�   ���	#FFEDF3706�   5#      5$      