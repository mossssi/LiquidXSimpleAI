# Development Document

Developing this test project was a fun and challenging task for me to test my abilities in a fast mode. So I want to describe the development process here and add my thoughts about what should be done differently and what can be done next.

## Setup the project

Setting up the project was the easiest part of the development. So I just created a project based on ThirdPerson template of Unity and I created a folder named \_Project to separate my works from the template assets. So I duplicated the Playground scene and removed all the environment assets. But I kept the player and camera for fast testing.

> I think a better way to setup a sandbox environment is to use ProBuilder package and exactly create a mockup environemt for simulating real game senarios. But I used a faster method to make the process faster.

## Preparation

After that, I created a really simple environment to test my guard and I added some obstacle boxes to hide from the patrolling guard. Then I duplicated the player and I removed all the player scripts from that and made it a guard instead. I changed the materials' color to be more distinguishable. This way, I can have the animations almost for free.

> In a real development senario animations can be more accurate. Guards can have acceleration and turings animations and can have taunting and searching animations too.

## State machine

The most important feature that I need for this guard is an AI brain. Depending on the project needs and time, I decided to create a simple state machine. I like behaviour trees for shooter game NPCs, but implementing a good system needs more time than this. Eventually, I created the patrolling state first. Here I felt the need for a path creator system.

> I think a better way to design the AI brain of the shooter game NPCs is too use a Behaviour Tree or and Hierarchical State Machine. But the needs was simple enough to use a simple FSM here. Also I could implement a Utility AI too but it's usually better for RPG games.

## Patrol Path

The path generator can be a spline generator. Because of the absence of the splines in Unity, I decided to write a simple path script which can give us the position of way points and calculate the next index based on if it is a loop or not. I also added visual feedback for designers to know the exact path represented in the editor.

> Having a good Spline generatot in Unity is must for almost all games. I could create this system and use it instead of this gameObject method. So we can restart the patrolling from the colsest position on the path instead of geting back to the closest point.

## NavMeshAgent

I added a nav mesh agent to the guard and baked the default navmesh for obstacles and the ground. The settings are enough for testing purposes but can be modified further for accurate sizing, different agent types, ground types and slopes.

> Nav mesh is good enoug but can be improved by adding some static methods to find random point on the navmesh and make the guard patrol freely from waypoints.

## Patroling

I used a nav mesh agent to tell the agent to move to an exact location. It was simple enough because the system is already there.

> As I mentioned before guard can have many patrolling strategies. For example can patrol in a random fashion or rest sometimes and take break for cigarretes.

## Perception

So I needed a perception system. I created a simple script which checks if the player is in the field of view of the guard and is not blocked by any obstacles. I had some difficulties there because of a wrong statement which took a good time for me to fix. So, every time the guard sees the player, it calls an action which can be listened to anywhere in the game. Now it's the state machine's responsibility to decide what to do.
I also added a delay for losing and not seeing the player because if it decide on oe frame that can lead to many unwanted situations which the agent will lose the player when passing from behind of a net shaped objects, like a fence.

> I wanted to make a abstract class for all the senses like sight and hearing and add them as objects or ScriptableObjects to the AIPerception class. It is a bit more modular and we can have new senses like scent detection. But I thought it is not neccessary for this test.

## Chasing state

Now that I have the player found action, I created the chasing state and it was simple enough thanks to the nav mesh agent. So I found an issue when the guard reaches the player's destination and the player is not walking. In this case, the guard stands at the player's exact location, so the raycast of the perception fails because there is no distance between them. So I decided to calculate a location near the player and a little behind him to prevent colliding with them.
There was another issue here. When a guard stops near the player, the player can go behind the guard and the guard will lose them because of the sight field of view. So I decided to rotate the guard when he is stopping at a range from the player.

> Chasing is an important feature of the guards. There are some features that needed to be added for a better AI. Like stopping the chase after a certain time or distance. Using attacks during the chase and the abilities to climb and drop on uneven surfaces.

## Hearing noises

Hearing is not very different from vision ability. I added a MakeNoise script for the player which makes a noise call when he is running on a regular basis. I implemented an interface so every object that needs to hear noises can implement that and have the data. Then I implemented the interface in the AIPerception script and setup a hearing threshold for controlling the hearing power of different agents.
So, when a noise is heard by a guard, AIPerception fires action calls to make him suspect and pass through the noise location.

> Every movement of the player should generate noises with different amplitudes. Also, strong noises should pass through obstacles, which makes more sense.

## Search state

I created a new state which is responsible for searching for a location whenever a guard is suspected. I could use the chasing state here, but I wanted to set up a different speed and behavior for searching, so I made them separate.

> This feature could be improved a lot. With good animations and moving into a closed area to find the enemy. Check the hiding spots near to the suspect's position.

## Final words

Thank you for reviewing my work. I hope I did my best to show my abilities, but I know that there is a lot more to making a game. It would be really cool if you gave me feedback on what I should do differently and I can develop it further more.

This is my email address: [mostafa.khaleghi@gmail.com](mostafa.khaleghi@gmail.com)
