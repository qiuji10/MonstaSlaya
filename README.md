


![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.001.png)


<br />
Project Name: 	MonstaSlaya
<br /><br />

Course:		XBGT2054 Game Programming

Name: 		Lim Hong Yu

Student ID: 		0130919




<br /><br />










**Introduction**

Hi, my name is Hong Yu. This is a game call MonstaSlaya that I made for Game Programming Assignment 3. MonstaSlaya is a game with genre of roguelike, player have to defeat all the enemies in the dungeon as fast as they can to win. There are 3 normal levels, 1 boss level.


**Control Scheme**

- WASD / Arrow Key – Movement
- Mouse – Control player facing direction
- Mouse Left Click – Attack (for all characters)
- Mouse Left Click Hold – Bow increase accuracy, damage increase if hold time is 80% (only for archer)
- E / Scroll Mouse – Switch Characters
- Mouse Right Click – Enable character’s skill
- Mouse Right Click Hold – Cast skill at mouse position (only for archer)






<br /><br />





**Gameplay**

**Levels**

There are 3 normal levels and 1 boss level. Each level contained these enemies

Level 1 (Normal) – Wolf, Troll

Level 2 (Normal) – Wolf, Troll, Mushroom

Level 3 (Normal) – Wolf, Troll, Mushroom

Level 4 (Boss Level) – Kam the Golem

For each normal level, there will have wave of monsters spawned at random positions once the player steps into the room. The player needs to clear from level 1 to boss level to win the game.

![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.002.png)

**Health System**

The player will start with 7 health and 6 shields. When player was damaged, the damage will deal to the shield first. If there is no shield left, the damage will deal to health. If the shield only takes part of the damage and become 0, the overflow damage will deal to health. 

**Switch Character**

The player able to switch character in any time they wanted. There are 3 characters in the game, knight, archer and assassin. Knight and assassin are melee type while archer is range type. All character’s attack direction can be controlled by mouse positioning. The switching sequence is Knight -> Archer -> Assassin -> Knight.

![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.004.png)                                      ![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.005.png)                         &nbsp;&nbsp;                 ![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.006.png)

&nbsp;**Knight        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;    Archer    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;         Assassin**


**Player Status**

|<p></p><p>Paralyzed</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.007.png)</p><p></p>|<p></p><p></p><p>When player hit by some attack from monster, player will get paralyzed and can’t do anything such as attack, moving or any other moves for 5 seconds.</p>|
| :-: | :- |


**Character Skill**

|Skill|Descriptions|
| :-: | :-: |
|<p></p><p>Knighty Shield</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.008.png)</p><p></p>|<p></p><p>Cooldown: 15s</p><p></p><p>After mouse right click, a circle shield will spawn around knight and remain for 5 seconds. During the 5 seconds, player able to immune from all damage</p>|
|<p></p><p>Archery Boom</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.005.png)</p><p></p><p></p>|<p></p><p>Cooldown: 40s</p><p></p><p>When holding mouse right click, a targeting area will be appeared. Player can use mouse to determine where to cast the skill and release mouse right click to cast it. A total of 5 arrows will be dropped down and each arrow will deal 10 damage to enemy</p><p></p>|
|<p></p><p>Assassinations</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.009.png)</p><p></p><p></p>|<p></p><p>Cooldown: 6s</p><p></p><p>After mouse right click, assassin will rush towards mouse directions. In the middle of rushing, player will immune from all damage.</p>|











**Enemy Types**

|Enemy|Descriptions|
| :- | :- |
|<p></p><p>Wolf</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.010.png)</p><p></p><p></p>|<p></p><p>Type: Melee</p><p></p><p>When the wolf is in attack state, it will rush towards player and perform a slash. If after 4 seconds it still couldn’t chase the player then it will enter rest state.</p>|
|<p></p><p>Troll</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.011.png)</p><p></p>|<p></p><p></p><p>Type: Melee</p><p></p><p>When the troll is in attack state, it will walk towards player and perform a slash. If after 4 seconds it still couldn’t chase the player then it will enter rest state.</p><p></p>|
|<p></p><p>Mushroom</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.011.png)</p><p></p>|<p></p><p></p><p>Type: Range</p><p></p><p>When the mushroom is in attack state, it will retreat if player is close to it. If player is far from it, it will shoot a bullet towards player’s position</p><p></p>|
|<p></p><p>Wizard</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.012.png)</p><p></p>|<p></p><p></p><p>Type: Range</p><p></p><p>When the wizard is in attack state it will retreat if player is close to it. If player is far from it, it will shoot 4 bouncing bullets towards player’s direction</p>|



<br /><br />
**Boss**

|Boss|Descriptions|
| :- | :- |
|<p></p><p>Kam the Golem</p><p></p><p>![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.013.png)</p>|<p></p><p>Type: Melee/Range</p><p></p><p>Attack Moves: Jump, Rush, Smash, Throw</p><p>Special Moves: Rage</p><p></p><p>Jump</p><p>- In Jump Attack, the boss will jump to the air and a warning area will appear. The warning area will go towards player and jump down to deal damage to surrounding if chased player.  After 3 seconds, if boss couldn’t chase player, it will jump down too.</p><p>- In Rage mode, the speed of chasing player will be increased.</p><p></p><p>Rush</p><p>- In Rush Attack, the boss will become a rock and roll towards player’s position. The boss will deal damage if player collided with it while in Rush moves. After the Rush ends, the boss will immediately perform Smash moves.</p><p>- In Rage mode, the speed of chasing player will be increased.</p><p>Smash</p><p>- In Smash Attack, the boss will smash the floor and deal damage to surrounding. At the same time 4 bouncing bullets will spawned from smash and shoot towards player’s direction for every smash.</p><p>- In Rage mode, the boss will spawn 100 bouncing bullets per smash.</p><p></p><p>Throw</p><p>- In Throw Attack, the boss will throw a rock towards player’s direction. The rock will cause player damaged and enter paralyzed status</p><p>- In Rage mode, the number of rock throwing towards player is 3.</p><p></p><p>Rage Mode</p><p>- When boss health is lower than half, the boss will enter rage mode.</p><p>- In rage mode, every attack moves of bosses will become stronger.</p><p>- Speed and damage will be increased in rage mode</p><p></p><p></p><p></p>|

















<br /><br />


**Additional**

1. There are different win and lose screen background. Each background is determined on which is their last character after defeat boss / die in the gameplay levels. 

Examples for win bg:

![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.014.png)![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.014.png)![](pic/Aspose.Words.8a38efda-59ed-40b4-8ba4-3f49e688918b.014.png)

1. There are stopwatch timer to records player clear time after player win or lose, The best time record of wining levels will displayed on Main Menu.













<br /><br />


**References and Sources**

**Audio**

**BGM**

Menu BGM - https://maou.audio/bgm\_piano39/

Battle BGM - https://freesound.org/people/Sirkoto51/sounds/443128/

Boss Battle BGM - https://freesound.org/people/Sirkoto51/sounds/416632/

Game Over BGM - https://maou.audio/bgm\_piano40/

Win BGM - <https://maou.audio/bgm_piano37/>

**SFX**

Button SFX - <https://maou.audio/se_system49/>

**Knight SFX**

Open shield SFX - <https://maou.audio/se_system03/>

Attack SFX - https://freesound.org/people/Merrick079/sounds/568169/

Attack 2 SFX -  https://freesound.org/people/32cheeseman32/sounds/180828/

Attack 3 SFX - <https://freesound.org/people/wesleyextreme_gamer/sounds/574821/>

**Archer SFX**

shoot skill SFX - https://maou.audio/se\_battle03/

drop skill SFX - <https://maou.audio/se_battle_explosion07/>

Attack SFX - <https://freesound.org/people/Lydmakeren/sounds/511489/> 

Attack 2 SFX - <https://freesound.org/people/jzdnvdoosj/sounds/626262/>

**Assassin SFX**

Assassin rolling SFX - <https://freesound.org/people/MusicLegends/sounds/344310/>

**Golem SFX**

Smash - https://freesound.org/people/studiomandragore/sounds/401628/

Jump Down - https://freesound.org/people/Isaac200000/sounds/181377/

Jump Up - https://freesound.org/people/bevibeldesign/sounds/366091/

Rage - https://freesound.org/people/icyjim/sounds/476083/

**Art Assets**

Enemies and Boss - <https://superdark.itch.io/enchanted-forest-characters>

**Characters Assets**

Characters Art Reference: Soul Knight
