# Simple Math

A VR Project that helps visualize addition and multiplication, so that it is easier to learn.


By Quinton Hoffman


## How to Use

Run the executable file in the build folder. Pick up the blocks and attach them to other blocks.
For addition, attach the blocks end to end. For multiplication, attach the blocks on the side.
Once finished, the total number of blocks is easily visualized and the answer appears on the back wall.
To proceed to the next question, press the 'A' button.

## Goals

The goal of this project is to help children and those who struggle with mathematics 
to have an easy way of visualizing simple addition and multiplication.
The idea is to take an existing visualization method for learning addition and multiplication, and make it more variable and robust. 
The ultimate goal is for this to be included as a novel way to teach kids addition and multiplication, as well as introduce them to VR. 
It could even be used in such a way that the learning is unsupervised which would allow one teacher to handle more students.

## Idea
 
Many kids learn addition and multiplication through the use of blocks like this, 
but there are limits. Traditionally, this must be supervised, so the person learning knows if they are 
right or wrong. Additionally, the blocks come in a couple sizes, usually a unit cube, 5's, and 10's. This increases the complexity of the visualization, and necessitates a teacher or supervisor 
to ensure the child does not swallow the blocks and uses them properly. 
VR simplifies this experience and introduces technology as a method of learning. 
In VR, there is no worry of blocks getting thrown or choked on. The student 
also does not necessarily need to be supervised as they will progress at their 
own pace and will receive feedback. Finally, VR allows for great variability. 
Any number of blocks can be visualized, which means we are not limited by the number of blocks available or physical space. 
Potentially, this could help with learning addition and multiplication of small numbers 
through very large numbers. With a more time, I could implement a feature that colors a group of blocks different colors based on size, or merge 
a group of blocks to be physically larger to denote 10s or 100s. This could even 
help with counting and larger scale addition and multiplication visualization. 
The possibilities are endless! The hope is that VR preserves the interactability enough to 
still help kids learn.

## Motivation

My motivation behind this stems from some volunteer work that I did a couple years ago 
where I taught basic computer science to middle schoolers. From then on, I have 
wanted to find ways to encorporate technology into learning and help kids 
understand the importance of technology in their future. Hopefully, this project 
achieves that goal, by showing that technology can by used to assist learning 
and in turn, the students also learn about the technology itself. I also 
love math and want everyone to share my passion in whatever way possible. A VR 
math game seems like a good way to encourage kids to learn math and be excited about it.

## Issues

The complexity of the project was more than I imagined. It was fairly complex 
to merge two groups of blocks without them jumping all over the place. It was 
even more complex to repeatedly merge groups of blocks and maintain position 
and smoothness. Besides this complexity, there are a couple things to note. 
For some reason, when running the executable file, the blocks fluttered about a lot more and were much more difficult to grab. Grabbing the blocks 
when running in debug mode in the Unity Editor was easy and seamless as seen in demo.mp4. It seemed much more difficult when I tested it in the executable 
and did not run as nicely. Second is the color of the walls in the executable 
are much brighter than in the demo. This is because the light had never finished 
baking for the demo, and the build took nearly 20min, so when it came out 
little bright, I opted not to rebuild. This makes the sense of depth a little off, 
but nevertheless still playable. Lastly, occasionally the blocks can clip through the floor. 
This just seems to be a Unity issue when two blocks run into each other and one 
pushes the other through the floor. This problem was very rare though.
