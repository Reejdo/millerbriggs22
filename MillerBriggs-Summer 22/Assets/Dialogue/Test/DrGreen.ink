Hello there! #speaker:Dr. Green #portrait:Dr_Green #emotion:1 #layout:left
-> main

=== main ===
How are you feeling today? 
+ [Happy]
    That makes me feel happy as well! #emotion:0
+ [Sad]
    Oh, well that makes me <anim:wave>sad too</anim>. #portrait:Dr_Green #emotion:2
    
- Don't trust him, he's not a real doctor! #speaker:Ms. Yellow #portrait:Ms_Yellow #emotion:1 #layout:right

Well, do you have any more questions? #speaker:Dr. Green #portrait:Dr_Green #emotion:1 #layout:left

+ [Yes]
    -> main
+ [No]
    Goodbye then!
    -> END