# EvoEngineX
EvoEngineX empowers developers and researchers to run evolutionary algorithms effortlessly. Key features include a user-friendly interface, powerful computing resources, real-time monitoring, customizable settings, seamless collaboration, scalability for large datasets, and robust security. Unlock your algorithms' potential with EvoEngineX.

## ClientAPP 
Here will user upload, managed his own code, runs, watch for the results and profile
He is able to 
1. upload his code
2. run it
3. watch the run status 
4. check the results. 
5. During his run he is able to see his results

He is able to choose which providers he wants to create (AWS, Azure)
He is able to setup a connection to his subscription


## DesktopApp
- same as Client app but it's going to run on desktop (windows and macos)

## Execution Api
1. Api that will control the scalability of the system. So when users says I want to create 10 runs it will create 10 containers/functions but before he will try to build it if doesnt contain errors. If it does it will give the information to the user back to the client app
2. This Api is able to get create whole environment based on configuration 

## EngineCore
This will contain all the triggered function that will create containers, upload it to the cloud or the premise provider as an IAAS

# DBs

## Execution DB
DB for the api communication that will contain information about system configuration

## Result DB
Probably document based non-relational db which will contain information about run results