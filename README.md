This is the code for the paper "Behaviour diversity in a walking and climbing centipede-like virtual creature".

An excecutable for the simulator (Builds.zip) can be downloaded at: https://www.mn.uio.no/ifi/personer/vit/emmaste/builds.zip 

To run the code:
- Download Builds.zip and place it in the same folder as the files in this repository
- Unzip Builds.zip
- Install requirements, we used python version 3.9.12
- Run main.py

The Assets and ProjectSettings folder contains files and scripts used to make the simulator executable found in Builds.zip. This includes the files Assets/Scrips/BodyController.cs, Assets/Scrips/LegController.cs, and Assets/Scrips/FootController.cs which contain the logic for the controller described in the paper. To make a new simulator build a Unity project with Unity ML-Agents would have to be set up using these assets. We used Unity version 2021.3.10f1 and Unity ML-Agents version 2.0.1.
