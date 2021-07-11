/**
7/8/21  tips for writing with V2 of the story parsing script. 

There is no need to do #nl #wait.  Any #wait will AUTOMATICALLY be accompanied by two \n's.  

Only a single #nl is needed at the end of a line for a new paragraph to be started for the next line, with a blank line in between.  

#refresh tags need to be on the same line as a #wait tag in order to work.  This is good, so that the player has a second to finish reading before the text is deleted. 
**/



-> intro

=== intro ===
"Hey honey, are you almost done packing up our clothes?  I know the suitcase is small, <>  #part
but I've gotten it all to fit at least once before!"  #part

I yawned and glanced at the clock, 10:34 PM.  We were close to being done with packing for tomorrow. #nl  #pl

I looked down at the half-packed suitcase on the floor, then looked up at the video game screen I had quietly turned on. #pl #wait 

I take a gulp of my beer and set it down at my feet. #wait 

...almost finished with this level. 

*	[finish the level]
	"uhh, I'm having some trouble, but I'll get it in minute!" #pl #wait
	
	-> finish_level

*	(suitcase_packed)[pack the suitcase]
	"...yeah, I'm pretty much done!"  I called down, half heartedly putting down my controller and directing my attention to the suitcase. #pl

	**(suitcase_nice)[fold the clothes nicely]
		In a surprising display of self control, I fold all of the clothes as best I can and place them in the suitcase. #pl
		There are a few maternity dresses that take up alot of space, but after a few minutes, the suitcase is neatly packed, and it closes with relative ease.  
		
		*** ["Anything else?"]
			-> anything_else
			
		*** [finish the level]
			-> finish_level
		

	**(suitcase_messy)[jam them in]
		I quickly stuff them in; a few maternity dresses, some stretchy shorts, and a handful of t-shirts. 
		With some effort, the suitcase closes. 
	
		*** [finish the level]
			-> finish_level
		
		*** ["Anything else?"]
			-> anything_else
						
=== finish_level ===
{anything_else.lie_about_phil or anything_else.quickly_text_phil: With not too much excitement, I pick up the controller. }
VAR ammo_capacity_percentage = 50
VAR health_percentage = 40
As I redirect my attention to the screen, my player approaches an enemy encampment.  An imposing boss door can be seen ahead.  
I check, and realize that my health is low, and I've only got one grenade and a smoke bomb left.  I've got a rifle with about 50% ammo. #nl #pl

Downstairs is quiet, my wife appears to be finishing up. #wait

{intro.suitcase_nice: I don't have time to go back and get supplies}
{intro.suitcase_messy or not intro.suitcase_packed: I may still have a few extra minutes to get some supplies}

//basically, if the player didn't take the time to pack the suitcase nicely, or didn't pack it at all
*	{intro.suitcase_messy or !intro.suitcase_packed}[get supplies]
	~ammo_capacity_percentage += 30
	~health_percentage += 30
	I decide to play it safe and look around first.  #wait #nl
	#nl
	Luckily I find some ammo, a spike grenade, and a little bit of health. 
	Afterwards, I open the door and head inside. 
	->decide_tactics
*	[proceed]
	I walk in, hoping for the best.
	->decide_tactics
	

=decide_tactics
*	(stealth)[sneak by with stealth]
	I decide to take the slow and stealthy approach.  This will probably be my last playthrough of the day, might as well play the long game.  #pl
	** [cut right]
		I cut to the right and
	** [crouch left]
		I crouch and move left as I 
	--proceed into the room.  -> random_loot
	
*	(up_front)[fight them up front]
	~ ammo_capacity_percentage -= 15
	There isn't enough time to be coy.  This will probably be my last playthrough of the day, after all.  #pl #wait
	I lob what grenades I have left and try to take out as much of the enemies as I can.  #wait
	
	After the dust settles most of them are down, but a few stragglers remain.
	**	[ignore the remaining]
		I figure this is good enough.  As the remaining enemies swoop into my location, I cut to the right and head towards the boss door. #wait
		-> approach_boss_door
	**	[finish off every one]
		~ammo_capacity_percentage -=10
		Gotta be sure.  As the remaining enemies descend on my location, I duck into cover and pick them off.  The room seems to be clear now.  #wait #refresh
		-> approach_boss_door


	
=random_loot	
#nl
Luckily I find a loot chest in the corner, out of enemy view.  I open it and see <>
{~an energy sword and some spike grenades, nice! -> good_loot| a helmet that gives me +2 lightening defense at the cost of -3 Armor.  Who even made this thing? -> bad_loot} #nl


= good_loot
With the energy sword equipped (obviously), I continue up the side.  #wait
->final_enemy_remaining

= bad_loot
I don't even bother equipping it, and continue up the side.  #wait
->final_enemy_remaining

=final_enemy_remaining
Up ahead there is one more enemy.  A raider with a tough looking magma rifle and some twin blades strapped to his back.  He's in the middle of a <>
narrow passage way that leads up to the boss door.  It would be tough to sneak by him. 

*	{good_loot} [sneak up and use energy sword]
	The number one rule of gaming is, of course, if you have an energy sword you must use it. #wait
	So with some cunning, I get in range and easily slice him down with one hit.  #nl
	#nl
	The boss door is now free and clear. #wait
	-> approach_boss_door
	
*	[pick off with rifle]
	~ammo_capacity_percentage -=10
	I let some of my few remaining bullets fly.  The enemy takes a few lucky critical hits and goes down with alot of ammo but surprisingly little effort.  #wait 
	-> approach_boss_door

*	[attempt to sneak by without being seen]
	Without better weapons and hardly any ammo left, my only option is to sneak by.  Here goes nothing...
	//left or right give 1/3 change, up gives 1/2 
	//this logic has later been altered to ALL result in success.  It was causing logical errors because it was possible to get to wife_comes_upstairs without ever being asked about the art supplies, which is necessary
	**	(right)[right]
		I wait until the enemy turns, then quickly dash to the right and past him...
		//{~ ->final_enemy_sneak_fail|->final_enemy_sneak_fail|-> final_enemy_sneak_success}.
	**	(left)[left]
		I wait until the enemy turns, then quickly dash to the left and past him...
		//{~ ->final_enemy_sneak_fail|->final_enemy_sneak_fail|-> final_enemy_sneak_success}.
	**	(up)[up]
		Just before making my move, I realize there are some vines to my right that lead to an upper walkway. <>
		This will probably be my best bet, so I climb up and slowly creep accross, watching the enemy patrol beneath me.  
		//{~ ->final_enemy_sneak_fail|-> final_enemy_sneak_success}.
	-- ->final_enemy_sneak_success
	

=final_enemy_sneak_fail
 #wait #refresh
But...it was to no avail. #nl

{finish_level.right or finish_level.left : The enemy turned earlier than expected, and with my eyes glued on the boss door ahead, I didn't see him take out his twin blades and swing them right at me. }
{finish_level.up: In my hurry, I made too much noise.  The enemy noticed, but with my eyes glued on the boss door ahead, he had plenty of time to aim his rifle and take me out in just a few bullets.  } 

->death


=final_enemy_sneak_success
#nl
Miraculously, I manage to sneak past without the beast noticing.  

*	[approach boss door]
	-> approach_boss_door

= approach_boss_door
I finally make it to the boss door.  But a few moments before entering, my wife calls out.  
"Hey, honey?  Would you mind grabbing my box of art supplies from the bedroom?  I was thinking I might do some painting on our trip.  #part #nl
I tried to get it earlier, but I just don't have the energy these days.  I'll be coming up to bed in a second anyway."

*	(get_art_supplies)[quit game and get box]
	Reluctantly I turn put down my controller and head into our room to gather up my wife's art supplies.  #pl
	I haven't seen these out in a while.  Seems like she wants to put some time into it before the new addition.  #wait
	

	**	{!anything_else} [Alright, anything else?]
		"Alright, I just grabbed it.  Anything else?" #pl
		-> anything_else
	**	{anything_else} [Done!]
		"Alright, I just grabbed that box and put it by the door." I called down the stairs.  <>
		But before I realized it, my wife was already at the top of the stairs looking into the gaming room. #wait #refresh
		-> wife_comes_upstairs
	**	{!finish_level.fight_boss} [return to game]
		-> fight_boss
	
*	[delay and fight boss]
	"Umm yeah, just give me a second!" #pl
	->fight_boss


= fight_boss
I frantically enter the boss room, knowing for certain that this will be my only shot. #nl<>
#nl
He was huge, a slow and lumbering titan with heavy armor and a slew of weaponry. 
*	{finish_level.good_loot} [use energy sword]
	I decided to get straight to the action, so I pulled out my energy sword and ran straight at him. 
	**	(boss_right)[dodge right]
	**	(boss_left)[roll left]
	--As I approached he fired several rounds from his mini gun, but I <>
	{finish_level.boss_right: dodge to the right and continued straight at him.}
	{finish_level.boss_left: made a quick roll to the left and continued straight at him.}
	--Half way there now. 
	**	[Slow down and throw grenades]
		Between waves of bullets, I ducked behind cover and lobbed the last of my grenades. #nl <>
		#nl
		This opened up a moment of vulnerabilty, and I needed to take advantage of it. 
		
	**	[Sprint with sword]
		I wasn't stopping at this point.  
		
	--	Throwing caution to the wind and soaking up damage as I went, I charged forward. 
	**	[strike!]
		At the earliest moment I could, I lunged forward with my energy sword, hoping to cut him down in one shot. #nl #wait
		#nl
		What I didn't see, however, was the energy sledge hammer that he had pulled out.  His swing met mine, and my timing was just barely off. #nl
		#nl
		In one massive swing, I was sent sailing accross the room, smashing into a wall and crumbling onto the ground.  #wait #refresh

		-> death
		
*	[use rifle]
	I laid down as much fire as I could, peppering the boss between his attacks.  
	**	[rifle: flank right]
		My progress was almost nothing, so I decided to duck right as much as I could, and tried to flank him.  
	**	[rifle: get up close]
		I ducked from cover to cover, slowly making my way forwards to get a cleaner shot. 
	
	
*	[use grenades]
	I decided to lay down as much damage as I could at first, so I chucked the remainder of my grenades.  #nl
	#nl
	Good, but not enough. 
	**	[rifle: flank right]
		I flanked right, hoping to get an open opportunity. 
	**	[rifle: get up close]
		I ducked from cover to cover, slowly making my way forwards to get a cleaner shot. 
	
-	Just as I arrived, my moment came.  The behemoth lowered his weapon for a moment, uncertain where to aim. 

*	[aim for headshot]
	I waited for a moment, lining up the perfect shot...<>#wait
	and let it fly. #nl
	
	I never was much of a shot thought, and the bullet went wide to the left. 
	
	
*	[spray and pray]
	There wasn't any time to waste, so I threw finesse out the window and aimed at his core and unloaded all my ammo. #wait
	This choice proved to be my downfall.  His armor soaked up almost all the damage from my little rifle.  

-	At this point, it was pretty much game over.  I was too close, had taken too much damage, and the beast knew right where I was now. 
He quickly pulled out a burst gun and dispatched me with ease. #wait #refresh
-> death

	

= death
- (beer_spilled)As the steeley text "You Lose" appeared on the screeen I sat back in my chair, frustrated. <> 
As I kicked my legs out, I accidentally kicked over the beer bottle at my feet, <>
spilling the contents over the floor. 

*	[clean up]
	I mutter half-curse words under my breath as I sit up, just about to head downstairs to get a towel. 
	However, now that my attention is taken away from my game, I realize that I can hear my wife coming up to the second floor.  #wait #refresh
	-> wife_comes_upstairs


//nothing diverts here for now...need to finish this
//maybe later this option can be used if the player wants to just do NOTHING
=== take_a_nap ===
I layed down quietly on the couch, playstation still on. 
Wife is very mad!
-> END


=== anything_else ===  
"Not really..."  #wait #part
"Wait, how often is Phil going to walkthrough the house when we're gone?  I'm going to leave some beers for him as a thank you." 

I grunted under my breath.  I had totally forgotten to call him.  #pl
	
* 	(lie_about_phil)[lie and text Phil]
	"ahh...I just told him to come by every evening!  I thought that would be best" #pl 
	"Oh, great!  We'll drop off the key tomorrow morning." #part #wait
	My wife said this with a surprising amount of joy, and even a bit of surprise.
	Deflated at my own behavior, I type out a rushed text to my friend, hoping he can help. 
	
		** [ponder]
		-> self_reflection

* 	(truth_about_phil)[truth: I forgot]
	"Shoot...sorry honey...I haven't asked him yet."#nl #pl
	#nl
	It was a little late now to ask him to help us, we were leaving the next day.#nl #pl
	#nl
	"Our house will probably be fine, I'll just make sure to lock all the windows!"   #pl
	Even I knew how lame this was. #wait  
	
	"Can you at least just ask him?  We're going to be gone for a week, it would be good to have someone look through the house once or twice!"#part #wait #nl
	#nl
	The clacking and clinking of dishes seemed to be a bit louder after this. 
	
		**(text_phil_regular)[text Phil]
		I type out a quick text to my friend, hoping he could help us out.  #pl
		He had always been a good friend, and he probably deserved better than another text like this at almost 11:00 at night.   #wait
			*** [ponder]
			-> self_reflection
		
* 	(quickly_text_phil)[dodge and text Phil]
	"Sorry, what was that?  I'm in the bathroom!"
	I immediately text Phil, hoping he can help.   
	He had always been a good friend...and he probably deserved better than another text like this at almost 11:00 at night.   #wait
		** [ponder]
        -> self_reflection
		
		
		
=== self_reflection ===
{anything_else.text_phil_regular or anything_else.quickly_text_phil : I tossed my phone aside and looked at the ceiling for a minute, then let out a sigh.}  #pl
{anything_else.lie_about_phil: "Great..." I said to myself, letting my face fall into my hands.  Why did I do this to myself?} #pl
#nl

-	This was the kind of thing we had talked about.  Growing up.  #nl #pl

Back in college, things seemed so simple. But these days life felt like it was coming at us head first. #pl
Sometimes she said it seemed like I was still in college.  #wait

Honestly, sometimes she was right. 

* 	{!finish_level}[finish level] 
	-> finish_level
*	{finish_level.approach_boss_door}[fight boss]
	I grabbed the controller and looked up for a second.  I'm not really sure why, but unpaused and started playing. #wait <>
	->finish_level.fight_boss
	
/**********	
* 	[wait]
	I looked down at my game controller, but decided just to leave it there.  #pl
	For a few minutes, I just sat there.  #wait
	-> wife_comes_upstairs
**********/



/****************************************************************************************************************************************************/
=== wife_comes_upstairs ===

At that moment, I looked over at my wife who had just made it to the top of the stairs. <> #pl
She was sweating a little bit from cleaning up downstairs.  She had one hand on her back, <>
and the other on her belly, which was a full 7 months big at this point. <>
Under her arm she had a few final clothes to pack.   #wait

She was leaning slightly on the wall, and her face had a slight grimmace of pain on it, which wasn't unusual these last few weeks.  #wait

* 	[Hey!]
	"Hey!"  I said, trying counteract what I knew was probably coming.  #pl #nl

	Sadly, I felt like I was about to play the villian in a script I had written myself. #wait #refresh
	
* 	[Hey...]
	"Hey..." I said.  #pl #nl

	Sadly, I felt like I was about to play the villian in a script I had written myself. #pl #wait #refresh

- "...hey."    She looked at the television which was still on.  #part 



{finish_level: -> wife_speaks_finish_level}
=wife_speaks_finish_level
Normally, my wife gently tolerated my video games.  She would occasionally show interest in them for my sake, but for the most part she left them alone. #pl
At the moment, however, the metallic deformed "you lose" text on the screen, complete with burning <>
wreckage and a crashed helicoptor in the background seemed to downgrade my habit into it's most fantastical, ridiculous form.  #pl


"ugh...seriously?  Video games, now?  I've been downstairs mopping floors, organizing our things, confirming reservations, getting food ready ... #nl #part

and you're just...here."

*	[defend]
	"Look..." I said in a half hearted attempt at self defense. #nl #pl
	
	"Everyone needs a break now and then.  It just...took a little longer than thought" #pl
	
	"A break from what, exactly?  Tomorrow is our vacation...THAT is the break." #nl #part
	
	-> wife_speaks_suitcase
	
*	[admit]
	"Look I...I didn't mean for this to take so long.  I wanted to just give it one more shot before we left."#nl #pl
	#nl
	"It just took longer than I thought I guess." #pl
	-> wife_speaks_suitcase


- -> wife_speaks_suitcase

= wife_speaks_suitcase

"Didn't we say we wanted to go to bed early tonight to catch our flight tomorrow?  It's at 6:10 AM, remember?  #nl #part
We might have been able to do that if you had actually been taking care of some things". 

*	[Excuse]
	"...I just got tied up with a few things..." #pl
	
*	[Sorry]
	"Sorry, I meant to get them done..." #pl
	
-"...yeah."    #part  #wait 
She walked over to the suitcase to put in the last clothes she had brought up. #pl
{intro.suitcase_messy: -> wife_suitcase_messy} 
{intro.suitcase_nice: -> wife_suitcase_nice} 
{not intro.suitcase_packed: -> wife_suitcase_unpacked}

=wife_suitcase_messy
When she looked inside, she paused for a minute to sigh and lay her face in her palm.  #part
"You could have at least tried to do it nicely..."
*	[yeah...sorry]
	Scratching the back of my head for no reason, I awkwardly said #nl #pl
	#nl
	"Yeah...sorry about that." #wait #pl
-> wife_speaks_art_supplies

=wife_suitcase_nice
"Well, this is a surprise." she said, as she saw the neatly packed clothes. #part
Honestly, I can't blame her for saying that, it was pretty unusual even to me.  #pl

*	[hah...thanks]
	"Hah, yeah...thanks"  #wait
-> wife_speaks_art_supplies

= wife_suitcase_unpacked
However, She stopped short when she saw the suitcase still half-packed on the floor.  She shook her head and blinked in disbelief as she started to pack it herself. #pl #nl
"H-Here, I got it."  I said, trying to remedy the situation.  I haphazardly threw the remaining clothes in the suitcase, then grabbed the <>
clothes she brought up and placed them on top.  
-> wife_speaks_art_supplies
	
= wife_speaks_art_supplies
"And where did you put that box of art supplies I asked you about?  I want to make sure I have the right brushes." #part

{finish_level.get_art_supplies: -> art_supplies_done }
{not finish_level.get_art_supplies: -> art_supplies_not_done }

=art_supplies_done
* 	[downstairs!]
Gratefully, I told her that I had moved it downstairs by the door. She thanked me.  #pl
Before she went downstairs to check it <>
{anything_else:  -> phone_rings}
{not anything_else:  -> friend_never_discussed}

=art_supplies_not_done
*	[uhh...about that...]
"Umm...I'm not sure if I..." #pl #wait
And that was pretty much all that need to be said.  
She opened the door to the side room, and we both looked in to see that the box was right where it had been, unmoved.  #wait #nl
Before her frustration is even shown  it <>
{anything_else:  -> phone_rings}
{not anything_else:  it -> friend_never_discussed}

= friend_never_discussed
appears that she remembers one more thing.  Oh joy. #pl

"Did you ever text Phil about walking through the house when we're gone? <> #part
I wanted to leave a few beers for him as a thank you." #part

*	(final_lie_about_phil) [lie and text Phil]
	"ahh...I just told him to come by every evening!  I thought that would be best" #pl 
	"Oh, great!  We'll drop off the key tomorrow morning." #part #wait
	{wife_comes_upstairs.art_supplies_done:  When my wife turned to go downstairs to check, I frantically texted my friend, hoping he could help } #pl
	{wife_comes_upstairs.art_supplies_not_done:  When my frustrated wife turned to check if the unmoved box had what she wanted, I frantically texted my friend, hoping he could help. } #pl


* 	(final_truth_about_phil)[truth: I forgot]
	I half-cursed under my breath before admitting, #nl #pl
	
	"Shoot...sorry honey...I guess I forgot to ask him." #nl #pl
	
	It was a little late now to ask him to help us, we were leaving the next day.#nl #pl
	
	"Our house will probably be fine, I'll just make sure to lock all the windows!"   #pl
	Even I knew how lame this was. #wait  
	
	"Can you at least just text him now?  We're going to be gone for a week, it would be good to have someone look through the house once or twice!"#part #wait #nl
	
	
		**(text_phil_regular)[text Phil]
		After agreeing, I type out a quick text to my friend, hoping he could help us out.  #pl
		He had always been a good friend, and he probably deserved better than another text like this at almost 11:00 at night.  #refresh #wait
 
- I awkwardly sat there as my wife <>
{wife_comes_upstairs.art_supplies_done: goes downstairs to make sure all of her supplies were in order.  A few minutes later she comes back up and to both of our surprise, they were!  Right when she comes back upstairs and crosses in front of the couch -> phone_rings} #nl
{wife_comes_upstairs.art_supplies_not_done: goes into the other room to inspect the box that I should have moved.  After a few minutes of her shuffling things around -> phone_rings}   #nl

=phone_rings

my phone loudly rings with a message. #pl
The phone happens to be face up between us, and without too much effort we can both read it. #nl #wait

* [read message]
I glanced over.  It reads: #nl
#nl  
"Hey man, sorry, I can't help with that.  I'm heading out of town as well.  Maybe you can ask someone else?  Have a good trip!"


{anything_else.lie_about_phil or wife_comes_upstairs.final_lie_about_phil: -> player_lied}
{anything_else.truth_about_phil or wife_comes_upstairs.final_truth_about_phil: -> player_truth}
{anything_else.quickly_text_phil: -> player_dodged}

=player_lied
"Wow...you said he was already coming over.  You made it sound like you already took care of this!   <> #part
Why would you lie about something so small like this?   #part  #wait

Just...what's the point?"

*	[...]
*	[...]	
- I couldn't really give an answer, because there really was no answer.  #pl  #wait #refresh
-> in_bed

=player_truth
"Great..."  my wife groaned.  #part
"Well...I guess we'll just hope that the house will be okay when we're gone."

*	[yeah...]
	"yeah...I'm sure it will be..." I lamely agreed. #pl
	-> in_bed

=player_dodged
"Wow...you only JUST texted him?  Why do you still do stuff like this?  <>   #part
I mean...you could have just told me you didn't do it at least..."

*	[Sorry...]
	"Sorry...I thought I could at least ask him...it just didn't work."
*	[My bad...]
	"My bad...I thought I could just reach out to him real quick.  He's usually good for this kind of stuff."

- "I know...I just wish you had done it sooner."  #part #wait #refresh

- -> in_bed

=== in_bed ===
The rest of the evening passed in that awkward way, when two people have things to say to eachother  <> #pl
but are either too tired or too busy to say them.  #nl

I attempted to help with any remaining tasks, but <>
I think we were both just hoping we would sleep over it, and wake up tomorrow in a better mood, ready for our vacation.  #pl #nl

After floors had been swept, checklists had been marked off, windows had been locked and <> #pl 
(for some reason) the microwave had been cleaned, we both fell, exhausted, into bed. #wait 

"Look...I don't want to be like this.  You deserve to have fun like everyone else, and you make mistakes like everyone else. <> #part
It just feels like sometimes you're stuck where you were 4 or 5 years, not realizing what's changed...#nl

I mean...we're having a KID for gosh sakes!  I can't do all this by myself.  It's time for you to...you know...  #wait

...grow up a little."

*	[ouch...]
	"Ouch..." I said.  #nl #pl
	#nl
	"But...<>
*	[yeah...]
	"yeah...<>   #pl
-	I can see sort of see where you're coming from.  I guess old habits just die hard.  #nl #pl
	It wasn't that long ago that I was eating and cheap beer with at my friend's dorm room, and now..." #nl
	I gestured toward my wife's belly.  
	
-	"I've got alot to be ready for too,"  my wife said with some compassion.  "I just can't do it by myself." #nl #part

"So I really need you to have a little bit of a...sense of urgency here." #part

*	[I'll do it!]
	"I got it."  I said confidently.  "It's just a matter of fighting the lazy voice in my head.  You know... #wait  #pl
	
	sometimes it's just hard not to listen to it."
*	[I'll try...]
	"I'll try."  I said, with a note of hesitency that even I detected.  "It's just that...   #wait #pl

	sometimes that lazy voice in your head is hard not to listen to."

	
- "It definitely is..." my wife said through sleepy eyes.  #nl #part
#nl
"But...you have to try."

*	[I know.]
	"I know."  I said. #pl
	And with that, my wife couldn't stay up any longer and drifted to sleep. #wait
	
	I stayed up for a little longer, but not much.  I was preoccupied with thinking about what happened that night.  #pl
	My anger hadn't completely faded, but in hindsight, I knew she was right - forethought and responsibility were never really my strongsuits.  But realizing it <>
	and changing it were two very different things. #wait 
	
	I reach for my phone to set my alarm.  A 6:10 AM flight means waking up at 4:30 at least.  Luckily <>
	the airport was nearby. #nl
	
	**	[set alarm]
		I search for my phone in the darkness to set my alarm, but give out an exasperated groan when I realize that it's still on the couch <> #pl
		from earlier. 
		
		***	[get up to grab phone]
			I stumble out of bed to get my phone from the next room.  I grab it and it I hit the power button and...nothing.  It's dead.  #pl
			Even IT was exhausted from the night, it seemed. 
			
			****	[wake up without alarm]
					"I'll be fine without it..." I told myself.  "It's important enough...I'll be able to get up." #pl
					#nl
					And with that, I walked back into the room and plopped back on to the bed. #nl #wait
					#nl
					In a few seconds, I was out.  
					-> END
		*** [wake up without alarm]
			"Ehh...I'll be fine without it.  It's important enough...I'll be able to get up." #pl  #wait
			
			And with that, the thoughts of the night floated away.   #wait
			
			In a few seconds, I was out.  
			
			
-> END		
	
	


/**
UPON RETURN:  

art supplies -> anything_else display is wrong.  

**/
