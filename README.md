#COMP30019 - Project1

Description:

This project uses diamond-square algorithm to generate a random landscape.
We use a grid with size 65x65 points, to store the heights at different point.
Firstly, we initialize the grid with different random values at the four corners.
Then we use the algorithm to keep updating the height values in the grid.
However, since in Unity a mesh is made of multiple triangles, we have a method
called mapGridToArray which is able to map each point in the grid to the correct
order of vertex construction to create the terrain in Unity. The same method is
also used to make an array of normals after we calculated the vertex normal at
each vertex.

Some Implementation Choices:

1. For the colours of the landscape, we decide to make it change at a particular
height: if it is higher than 20, it will has a white colour which represents snow;
if it is between 7.5 and 20, it will has a brown colour which represents soil;
if it is between 0 and 7.5, it will has a green colour which represents grasses;
if it's lower than 0, it will have a color that looks like sand.

2. For the shader, we decided to use a Phong shader which uses the vertex normal
of each vertex. To do so, we firstly calculate the surface normal of each surface,
and add it to its vertices. Then we simply normalize the normal at each vertex.

3. If the camera hits the terrain, we decide that it won't crash but it has to reverse
first to be able to keep moving. It won't get stuck but it will be hard to get out if
it is in a narrow gap.

Run:

There are many preset parameters in the code; for example, maxHeight, minHeight and
noise. They should not be changed because we have chosen the values that makes our
landscape look more realistic. Since the terrain is generated randomly, so there is no
guarantee that the water surface is always visible because the terrain might be too
high. Also, you may not see the change of the colors at different heights if the noise
happens to be small. Both the keyboard and mouse can be used to control the camera, but
we would suggest not to set the mouse sensitivity too high.
