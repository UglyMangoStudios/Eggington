# Eggington
Pronounced as: *egg-ing-ton*

## Table Of Contents
* [Overview](##overview)
* [Rules](##rules)
* [Requirements](##requirements)
* [Structure](##structure)
* [Getting Started](##getting-started)
* [Where To Next](##where-to-next)

## Overview
This gracious chicken inspired Discord bot is meant to serve as a public means for education and utility for fellow peeps. 

You, dear reader, will learn the following:
* The language features of C#
* The ideas of encapsulation
* The powers of Git
* Working on a coding project with multiple people
* Data management
* User Experiences

And many other wonderful skills. 

Feel free to add anything to this bot you like (as long as it follows the rules listed below). This bot will be the community pot luck. Some fun features includes:
* Command Interactions
* Music Bot
* AI 
* Image Generation
* User Economy
* Gaming

I, Kyle, have attempted to take most of the burden of starting a new project. Trust me, I totally understand the difficulties of starting a new project with 
foreign langauges, skills, structure, frameworks, and libraries. However, some things can still be confusing, even with all of this documentation. As such,
please don't hesistate to contact me. 

## Rules
Because this an open sourced bot, one that anyone can contribute to, certain guidelines must established to ensure a smooth and respectful experience.
1. Your idea is not always the best way. Be open to constructive criticism, changes, suggestions.
2. Be respectful. This is meant to be fun, and no one likes a sore sport who attempts to ruin everything.
3. Absolutely no commerialization of this project or it's applications. Yes, that's an awful capatilistic decision; however, monetary gain is not the purspose of this project.
4. No references to hatred, sex, profanity, or violence is allowed. This includes comments, API calls, or simple application responses.
5. Destruction of this repository or it's deployments is forbidden. Don't grief folks. 
6. Only permitted users can accept pull requests. Sometimes requests will be denied, but with feedback. Don't take it personally.
7. Have fun. If you're not, you're either not as big of a nerd you thought you were or you/we are doing something wrong.

We have the right to modify these rules at any time. In addition, we reserve the right to forbid or allow any users participating or viewing 
in this project and future projects at our discretion. 

By viewing, contributing to, and cloning this repository, you agree to these terms. Failure to comply will result in disqaulification of participation. 

And we don't want that.

## Requirements
Below are specific software you need to install. Unless specified, the default installation works just fine. Ensure all operating systems are satisfied. 
(e.g. x64 vs x86, Windows vs. Mac vs. Linux). Some systems may be incompatible with this workflow, which I cannot predict, nor can I prevent (sorry).

* [Git](https://git-scm.com/downloads) (default install)
* [Visual Studio](https://visualstudio.microsoft.com/downloads/) (default install). Community is fine.
* [.NET8](https://dotnet.microsoft.com/en-us/download) (default install)

In addition, you must have these Discord components. Don't worry, most will be explained in detail below.
* A Discord account (like duh)
* [Discord Developer Portal](https://discord.com/developers/applications)
* A Discord Bot Token
* A private Discord Server (for testing)

Finally, you must be familiar with these basic concepts:
* Executing commands in the terminal
* Creating and navigating into directories
* Understanding JSON and how it formats
* Googling when you're stuck
* Asking for help

## Structure
This application contains an organized structure so all assets can be found easily. Also, this information will be given relative to this project's root folder.
* `Contents`: this folder holds all of the C# files for this app. 
* `Documents`: this folder contains all of the other documents for your benefit. 
* `appsettings.json`: this json file configures the application's settings.
* `Main.cs`: the main entry point for this project. You don't really need to touch this.
* `README.md`: the current document you're reading. Also is the first document to be seen in Github.

If you are looking in this directory form a file explorer, you'll see some other files and directories. For the most part, you don't need to worry about these.
They serve Visual Studio.

## Getting Started
After you have sucessfully installed the software above, you are ready to begin work on this project. Congrats! I'm excited!

### (A) Getting Discord Ready
1. Have a Discord account (like, duh!) 
2. Create a new Discord Server. This will act as your testing grounds in case things go wrong.

### (B) Getting Your Bot Ready
1. Have a Discord account (again, like, duh!)
2. Open the [Discord Developer Portal](https://discord.com/developers/applications)
3. Click `New Application` on the top right
4. Name your application. This application *will not* hold the official bot. You're creating a test instance, so name with with whatever you'd like. It's not really important.
5. Assign yourself to the *Team*, and accept Discord's terms.
6. On the left, click `Bot`.
7. By default, your bot's name will be the same as your application. Feel free to change it.
8. Click `Reset Token`, accept all warnings, and complete any 2FA if enabled. 
9. Copy this token. **DO NOT SHARE THIS TOKEN WITH ANYONE**. This is very important. Anyone with this token will be able to control your bot. Scary stuff.
10. Store this token in a safe spot. We will use this later. 

### (C) Getting Your Project Ready
1. Clone this repository to your computer. It's okay if you're not familiar with Git. You will be soon enough. If you don't know what this means, don't worry, I got you!
    a. Ensure Git was installed successfully.
    b. Open Git Bash. This terminal will act your friend.
    c. Navigate to a directory in your computer to store this project. A new directory will be automatically created, so don't worry about that.
    d. In this Github Repository, click the green button labelled `Code`
    e. Copy the URL in the text box under *HTTPS*
    f. In your Git Bash terminal, type `git clone <URL>`. Replace *\<URL\>* with the URL you just copied.
    g. Wait for all git actions to complete. A new directory will have been created, and all of these project's files will be in this new directory.
2. Open Visual Studio
3. In the Visual Studio Welcome page, click `Open a project or solution`. A file explorer will open up. Navigate to where you cloned your files. In the directory, 
look for a file with a file extension with `.sln`. Click open.
4. Your project should open up. File a window pane titled `Solution Explorer`. This pane is where you'll find all of your project files.
5. Under the Solution Explorer, right click the project with a green C# log. 
6. Click `Manage User Secrets`. You may be greeted with a prompt asking you to install the necessary files. Click confirm. 
7. A file named `secrets.json` should open. If not, try clicking Manage Secrets again.
8. Paste this line into this file to satisfy standard JSON formatting:
    ```
    "Bot:Token": <TOKEN>
    ```
9. Replace **\<TOKEN\>** with the token you copied under part *(B)*. Don't worry, as the name implied, this is a secret file that will not be shared. Your token is safe here,
and this app will be able to use it safely. 
10. Inspect the file `appsettings.json` under this repository. All values are okay as is, but it's good to know what's going on here. 
11. You should be good! Click the green play button at the top of your screen to run your bot! Note: The solid arrow runs the app in Debug mode, the other one does not. I suggest 
running the bot in Debug mode so you can see any errors that arise.
12. If your project is running smoothly, congrats! You should see a message that says `Your bot is ready! Invite your bot here` with a link. Click this link and you will be navigated
to a Discord page asking you where to invite your bot. Find the server you created under part *(A)*, and grant all permissions.

And that's it! Your project is ready to go!

## Where To Next
If everything seems to run correctly, then you're ready to start adding things to this bot!

Firtly, I HIGHLY suggest explore all of these files. Get a little taste how things work around here. Once you feel comfortable, visit the [Wormhole Center](./Documents/WormholeCenter.md) 
to visit more guides.

Toodles!