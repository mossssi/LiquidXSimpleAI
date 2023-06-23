# Simple AI Document

## Guard properties

This Ai agent uses **NavMeshAgent** to navigate through the scene. So for setting some the most impoertant parameters of it you should consider changing **NavMeshAgent** fields.

- **Angular Speed**: How fast should character rotate toward the velocity direction
- **Acceleration**: Defines the time that character reaches the max speed

## State Machine

This state machine is a basic form of FSM systems and it is the brain of the AI desicions. For making new states you should inherit from StateBase class and implement abstratc methods of it. These are the important notes to consider:

- Use namespace **LiquidX.SM.States** for all the new states
- EnterState can be used for setting initial values and checking some references
- ExecuteState is like Unity Update method so it will runs every frames
- Every states quit with ExitState method so you should reset the global variables here
- You can override **OnDrawGizmos** to draw gizmos only when the state is active

### How State Machine Works

In this simple state machine there is only one state active at a time so you should check for any conditions that you want in anywhere in the game. After a condtions is true we Change the State of the SM to a new state. For example when the guard hears a noise so he changes the state machine to run search state via a simple action.

## Patrol Route

We use a GuardPath scripts in this game. So you should do the following to make a new path:

- Create an empty game object
- Reset its transform
- Add **GuardPath** script to the game object
- Create empty child transforms and place them where you want
- Remember order is important
- Use an Icon for the way point objects
- You can assign the child objects to the GuardPath manually to see the gizmos
- Assign the parent object to the **Gaurd** script on the agent

## Perception

**AIPerception** is the senses of the AI agent. You can set the vision and hearing abilities of the agent separately in this scripts. When selecting the guard with the script attached to him you can observe the sight power visually in the scene.

- EyeOffset is the offset of where we want to shoot rays, usually middle of the character is good enough
- ViewRadius is the range of the sight power
- ViewAngle is the angle of the fov
- Player and Obstacle masks are simple to add or remove
- SearchEverySecond is the delay between each detection call
- LostDelay is a time which after that we declare losing the player to ignore small gaps
- HearingThreshold is the value which agent would hear noises with amplitude above that.
  -- So if its really low the agent hearing power is highr

For setting the noise redius you should edit **NoiseMaker** script one the player.

- NoiseListener mask is all the masks that can have a noise listener on the
- ObstacleMask is the mask that noise can not pass through
- NoiseDecay curve is the decay or fade of the noise over max radius
- MaxRadius is the radius that every noise can travel
- NoiseMakingPeriod is the delay between each noise call from running
